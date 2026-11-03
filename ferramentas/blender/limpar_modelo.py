"""
Limpa modelos 3D gerados por IA (TRELLIS, TripoSR, Stable Fast 3D, Meshy...)
e deixa prontos para o Mixamo e o Unity.

O que faz, para cada arquivo da pasta de entrada (.glb, .gltf, .obj, .fbx):
  1. Importa e junta todas as partes numa malha só
  2. Remove vértices duplicados e pedacinhos soltos (lixo flutuando)
  3. Recalcula as normais
  4. Reduz a quantidade de triângulos para um tamanho de jogo
  5. Cria um mapa de textura (UV) limpo
  6. Transfere a textura original para o mapa novo (bake)
  7. Centraliza, coloca os pés no chão e ajusta a altura
  8. Exporta em FBX com a textura embutida (pronto para o Mixamo)

Uso (pelo terminal):
  blender -b -P limpar_modelo.py -- --entrada PASTA --saida PASTA [opções]

Opções:
  --tris N          Triângulos por modelo (padrão 15000)
  --altura M        Altura em metros (padrão 1.8)
  --textura PX      Tamanho da textura (padrão 2048)
  --config ARQUIVO  JSON com valores por modelo (ver config-exemplo.json)
  --sem-bake        Pula a transferência de textura (mais rápido, para testar)

Todos os valores padrão são PROVISÓRIOS (ver docs/pendencias.md).
"""

import argparse
import json
import math
import os
import sys

import bpy  # bpy antes do bmesh (necessário fora do Blender)
import bmesh

EXTENSOES = (".glb", ".gltf", ".obj", ".fbx")

PADRAO = {
    "tris": 15000,
    "altura": 1.8,
    "textura": 2048,
    # Pedaços com menos que esta fração dos vértices são considerados lixo
    "fracao_lixo": 0.002,
}


# --------------------------------------------------------------------------
# Argumentos
# --------------------------------------------------------------------------

def ler_argumentos():
    argv = sys.argv
    argv = argv[argv.index("--") + 1:] if "--" in argv else []
    p = argparse.ArgumentParser(description="Limpa modelos 3D gerados por IA.")
    p.add_argument("--entrada", required=True)
    p.add_argument("--saida", required=True)
    p.add_argument("--tris", type=int, default=PADRAO["tris"])
    p.add_argument("--altura", type=float, default=PADRAO["altura"])
    p.add_argument("--textura", type=int, default=PADRAO["textura"])
    p.add_argument("--config", default=None)
    p.add_argument("--sem-bake", action="store_true")
    return p.parse_args(argv)


def opcoes_do_modelo(nome, args, config):
    op = {
        "tris": args.tris,
        "altura": args.altura,
        "textura": args.textura,
        "fracao_lixo": PADRAO["fracao_lixo"],
    }
    op.update(config.get(nome, {}))
    return op


# --------------------------------------------------------------------------
# Cena
# --------------------------------------------------------------------------

def limpar_cena():
    bpy.ops.wm.read_factory_settings(use_empty=True)


def importar(caminho):
    ext = os.path.splitext(caminho)[1].lower()
    if ext in (".glb", ".gltf"):
        bpy.ops.import_scene.gltf(filepath=caminho)
    elif ext == ".obj":
        bpy.ops.wm.obj_import(filepath=caminho)
    elif ext == ".fbx":
        bpy.ops.import_scene.fbx(filepath=caminho)
    malhas = [o for o in bpy.context.scene.objects if o.type == "MESH"]
    if not malhas:
        raise RuntimeError("nenhuma malha encontrada no arquivo")
    return malhas


def selecionar(objetos, ativo):
    bpy.ops.object.select_all(action="DESELECT")
    for o in objetos:
        o.select_set(True)
    bpy.context.view_layer.objects.active = ativo


def juntar(malhas):
    # Tira hierarquias (o glTF costuma vir com nós vazios) mantendo a posição
    for o in malhas:
        if o.parent:
            matriz = o.matrix_world.copy()
            o.parent = None
            o.matrix_world = matriz
    selecionar(malhas, malhas[0])
    if len(malhas) > 1:
        bpy.ops.object.join()
    obj = bpy.context.view_layer.objects.active
    bpy.ops.object.transform_apply(location=True, rotation=True, scale=True)
    # Remove o que sobrou (vazios, câmeras, luzes)
    for o in list(bpy.context.scene.objects):
        if o != obj:
            bpy.data.objects.remove(o, do_unlink=True)
    return obj


def contar_tris(obj):
    return sum(len(p.vertices) - 2 for p in obj.data.polygons)


# --------------------------------------------------------------------------
# Limpeza da malha
# --------------------------------------------------------------------------

def limpar_malha(obj, fracao_lixo):
    bm = bmesh.new()
    bm.from_mesh(obj.data)

    bmesh.ops.remove_doubles(bm, verts=bm.verts, dist=0.0001)

    # Separa a malha em ilhas e apaga as muito pequenas
    bm.verts.ensure_lookup_table()
    visitado = set()
    ilhas = []
    for v in bm.verts:
        if v.index in visitado:
            continue
        ilha = []
        pilha = [v]
        visitado.add(v.index)
        while pilha:
            atual = pilha.pop()
            ilha.append(atual)
            for aresta in atual.link_edges:
                outro = aresta.other_vert(atual)
                if outro.index not in visitado:
                    visitado.add(outro.index)
                    pilha.append(outro)
        ilhas.append(ilha)

    total = len(bm.verts)
    minimo = max(8, int(total * fracao_lixo))
    lixo = [v for ilha in ilhas if len(ilha) < minimo for v in ilha]
    if lixo and len(lixo) < total:
        bmesh.ops.delete(bm, geom=lixo, context="VERTS")

    bmesh.ops.recalc_face_normals(bm, faces=bm.faces)
    bm.to_mesh(obj.data)
    bm.free()
    obj.data.update()
    return len(ilhas), len(lixo)


def reduzir(obj, alvo_tris):
    atual = contar_tris(obj)
    if atual <= alvo_tris:
        return atual
    mod = obj.modifiers.new("Reduzir", "DECIMATE")
    mod.decimate_type = "COLLAPSE"
    mod.ratio = alvo_tris / atual
    mod.use_collapse_triangulate = True
    selecionar([obj], obj)
    bpy.ops.object.modifier_apply(modifier=mod.name)
    return contar_tris(obj)


UV_JOGO = "UV_Jogo"


def ativar_uv(obj):
    # Referências a camadas de UV ficam inválidas ao trocar de modo; busca pelo nome
    uv = obj.data.uv_layers[UV_JOGO]
    obj.data.uv_layers.active = uv
    uv.active_render = True


def criar_uv(obj):
    selecionar([obj], obj)
    uvs = obj.data.uv_layers
    nova = uvs.new(name=UV_JOGO)
    uvs.active = nova
    bpy.ops.object.mode_set(mode="EDIT")
    bpy.ops.mesh.select_all(action="SELECT")
    bpy.ops.uv.smart_project(angle_limit=math.radians(66), island_margin=0.01)
    bpy.ops.object.mode_set(mode="OBJECT")
    ativar_uv(obj)


# --------------------------------------------------------------------------
# Transferência de textura
# --------------------------------------------------------------------------

def preparar_material_baixo(obj, nome, tamanho):
    imagem = bpy.data.images.new(f"{nome}_cor", tamanho, tamanho, alpha=False)
    mat = bpy.data.materials.new(f"{nome}_mat")
    if not mat.node_tree:  # Blender 4.x; no 5+ os nós já vêm ligados
        mat.use_nodes = True
    nos = mat.node_tree.nodes
    tex = nos.new("ShaderNodeTexImage")
    tex.image = imagem
    # Fica desligado durante o bake (senão a imagem lê dela mesma); liga depois
    nos.active = tex
    obj.data.materials.clear()
    obj.data.materials.append(mat)
    return imagem


def preparar_material_alto(alto):
    """
    Liga a cor do modelo original direto numa emissão, para o bake ler a cor pura,
    sem luz e sem depender do tipo de material que o gerador criou (PBR, unlit...).
    Usa a textura de cor se houver; senão, a cor por vértice (TripoSR e afins).
    """
    for i, original in enumerate(alto.data.materials):
        if original is None or not original.node_tree:
            continue
        # Cópia, para não estragar o material original (o de reserva, se o bake falhar)
        mat = original.copy()
        alto.data.materials[i] = mat
        arvore = mat.node_tree
        nos = arvore.nodes
        saida = next((n for n in nos if n.type == "OUTPUT_MATERIAL"), None) or nos.new("ShaderNodeOutputMaterial")

        fonte = None
        bsdf = next((n for n in nos if n.type == "BSDF_PRINCIPLED"), None)
        if bsdf is not None and bsdf.inputs["Base Color"].is_linked:
            fonte = bsdf.inputs["Base Color"].links[0].from_socket
        if fonte is None:
            tex = next((n for n in nos if n.type == "TEX_IMAGE" and n.image is not None), None)
            if tex is not None:
                fonte = tex.outputs["Color"]
        if fonte is None and alto.data.color_attributes:
            atributo = nos.new("ShaderNodeVertexColor")
            atributo.layer_name = alto.data.color_attributes[0].name
            fonte = atributo.outputs["Color"]

        emissao = nos.new("ShaderNodeEmission")
        if fonte is not None:
            arvore.links.new(fonte, emissao.inputs["Color"])
        elif bsdf is not None:
            emissao.inputs["Color"].default_value = bsdf.inputs["Base Color"].default_value
        arvore.links.new(emissao.outputs["Emission"], saida.inputs["Surface"])

    # Modelo sem material nenhum, só com cor por vértice
    if not alto.data.materials and alto.data.color_attributes:
        mat = bpy.data.materials.new("cor_por_vertice")
        if not mat.node_tree:
            mat.use_nodes = True
        alto.data.materials.append(mat)
        preparar_material_alto(alto)


def transferir_textura(alto, baixo, imagem):
    preparar_material_alto(alto)
    cena = bpy.context.scene
    cena.render.engine = "CYCLES"
    cena.cycles.device = "CPU"
    cena.cycles.samples = 1
    bake = cena.render.bake
    bake.use_selected_to_active = True
    bake.use_cage = False
    tamanho = max(baixo.dimensions)
    bake.cage_extrusion = tamanho * 0.02
    bake.max_ray_distance = 0.0  # sem limite: acha a superfície original mesmo longe
    bake.margin = 16
    ativar_uv(baixo)
    selecionar([alto, baixo], baixo)
    bpy.ops.object.bake(type="EMIT")
    imagem.pack()
    mat = baixo.data.materials[0]
    nos = mat.node_tree.nodes
    bsdf = next(n for n in nos if n.type == "BSDF_PRINCIPLED")
    mat.node_tree.links.new(nos.active.outputs["Color"], bsdf.inputs["Base Color"])


def brilho_medio(imagem):
    """Média dos pixels (0 = tudo preto). Serve para detectar bake que falhou."""
    try:
        import numpy as np
        px = np.empty(len(imagem.pixels), dtype=np.float32)
        imagem.pixels.foreach_get(px)
        return float(px.reshape(-1, 4)[:, :3].mean())
    except Exception:  # noqa: BLE001
        amostra = imagem.pixels[: 4 * 4096]
        return sum(v for i, v in enumerate(amostra) if i % 4 != 3) / max(1, len(amostra) * 3 / 4)


def textura_original(obj):
    for mat in obj.data.materials:
        if mat and mat.node_tree:
            for n in mat.node_tree.nodes:
                if n.type == "TEX_IMAGE" and n.image is not None:
                    return n.image
    return None


def remover_uvs_antigas(obj):
    for nome in [uv.name for uv in obj.data.uv_layers if uv.name != UV_JOGO]:
        obj.data.uv_layers.remove(obj.data.uv_layers[nome])
    ativar_uv(obj)


# --------------------------------------------------------------------------
# Posição e escala
# --------------------------------------------------------------------------

def posicionar(obj, altura):
    selecionar([obj], obj)
    bpy.ops.object.origin_set(type="ORIGIN_GEOMETRY", center="BOUNDS")
    obj.location = (0, 0, 0)
    bpy.ops.object.transform_apply(location=True, rotation=False, scale=False)
    atual = obj.dimensions.z
    if atual > 0:
        fator = altura / atual
        obj.scale = (fator, fator, fator)
        bpy.ops.object.transform_apply(location=False, rotation=False, scale=True)
    menor_z = min((obj.matrix_world @ v.co).z for v in obj.data.vertices)
    obj.location.z -= menor_z
    bpy.ops.object.transform_apply(location=True, rotation=False, scale=False)
    # Origem nos pés, como o Unity e o Mixamo esperam
    bpy.context.scene.cursor.location = (0, 0, 0)
    bpy.ops.object.origin_set(type="ORIGIN_CURSOR")


# --------------------------------------------------------------------------
# Exportação
# --------------------------------------------------------------------------

def salvar_png(imagem, caminho):
    if imagem.packed_file is None or imagem.source == "GENERATED":
        imagem.filepath_raw = caminho
        imagem.file_format = "PNG"
        imagem.save()
        return
    # Imagem embutida no arquivo original (GLB): copia os pixels para uma imagem nova
    largura, altura = imagem.size
    copia = bpy.data.images.new(imagem.name + "_png", largura, altura, alpha=True)
    px = [0.0] * (largura * altura * 4)
    imagem.pixels.foreach_get(px)
    copia.pixels.foreach_set(px)
    copia.filepath_raw = caminho
    copia.file_format = "PNG"
    copia.save()


def exportar(obj, caminho_fbx, imagem):
    if imagem is not None:
        salvar_png(imagem, os.path.splitext(caminho_fbx)[0] + "_cor.png")
    selecionar([obj], obj)
    bpy.ops.export_scene.fbx(
        filepath=caminho_fbx,
        use_selection=True,
        apply_unit_scale=True,
        apply_scale_options="FBX_SCALE_ALL",
        axis_forward="-Z",
        axis_up="Y",
        mesh_smooth_type="FACE",
        path_mode="COPY",
        embed_textures=True,
    )


# --------------------------------------------------------------------------
# Principal
# --------------------------------------------------------------------------

def processar(caminho, saida, op, sem_bake):
    nome = os.path.splitext(os.path.basename(caminho))[0]
    limpar_cena()
    malhas = importar(caminho)
    alto = juntar(malhas)
    tris_original = contar_tris(alto)

    ilhas, removidos = limpar_malha(alto, op["fracao_lixo"])

    # Posiciona o original antes de copiar, para os dois ficarem alinhados
    posicionar(alto, op["altura"])

    baixo = alto.copy()
    baixo.data = alto.data.copy()
    baixo.name = nome
    bpy.context.collection.objects.link(baixo)
    alto.name = nome + "_original"

    tris_final = reduzir(baixo, op["tris"])

    imagem = None
    if not sem_bake:
        materiais_originais = list(baixo.data.materials)
        criar_uv(baixo)
        imagem = preparar_material_baixo(baixo, nome, op["textura"])
        transferir_textura(alto, baixo, imagem)
        brilho = brilho_medio(imagem)
        if brilho < 0.01:
            # Bake falhou (textura preta): volta para a textura e a UV originais
            print(f"  AVISO: a transferência de textura saiu preta (brilho {brilho:.3f}); usando a textura original")
            baixo.data.materials.clear()
            for m in materiais_originais:
                baixo.data.materials.append(m)
            baixo.data.uv_layers.remove(baixo.data.uv_layers[UV_JOGO])
            bpy.data.images.remove(imagem)
            imagem = textura_original(baixo)
        else:
            remover_uvs_antigas(baixo)
    else:
        imagem = textura_original(baixo)

    bpy.data.objects.remove(alto, do_unlink=True)
    exportar(baixo, os.path.join(saida, nome + ".fbx"), imagem)

    print(
        f"  ok: {tris_original} -> {tris_final} triângulos, "
        f"{removidos} vértices de lixo removidos ({ilhas} pedaços), "
        f"altura {op['altura']} m"
    )


def main():
    args = ler_argumentos()
    config = {}
    if args.config:
        with open(args.config, encoding="utf-8") as f:
            config = json.load(f)
    os.makedirs(args.saida, exist_ok=True)

    arquivos = sorted(
        os.path.join(args.entrada, a)
        for a in os.listdir(args.entrada)
        if a.lower().endswith(EXTENSOES)
    )
    if not arquivos:
        print(f"Nenhum modelo encontrado em {args.entrada}")
        return

    falhas = []
    for caminho in arquivos:
        nome = os.path.splitext(os.path.basename(caminho))[0]
        print(f"Processando {nome}...")
        try:
            processar(caminho, args.saida, opcoes_do_modelo(nome, args, config), args.sem_bake)
        except Exception as erro:  # noqa: BLE001 - continua com os outros modelos
            print(f"  ERRO: {erro}")
            falhas.append(nome)

    print(f"\nPronto: {len(arquivos) - len(falhas)} de {len(arquivos)} modelos em {args.saida}")
    if falhas:
        print("Falharam: " + ", ".join(falhas))


if __name__ == "__main__":
    main()
