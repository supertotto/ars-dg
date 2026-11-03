# Limpeza de modelos 3D (Blender)

Pega os modelos gerados por IA (TRELLIS, TripoSR, Stable Fast 3D, Meshy...) e deixa prontos para o Mixamo e o Unity.

## O que o script faz

1. Junta todas as partes do modelo numa malha só
2. Remove vértices duplicados e pedacinhos soltos flutuando
3. Reduz os triângulos para um tamanho de jogo (padrão 15.000)
4. Cria um mapa de textura limpo e transfere a textura original para ele
5. Centraliza, põe os pés no chão e ajusta a altura (padrão 1,8 m)
6. Exporta em FBX com a textura embutida, mais um PNG da textura

## Como usar (Windows)

1. Instale o [Blender](https://www.blender.org/download/) (grátis, versão 4.2 ou mais nova).
2. Coloque os modelos gerados (`.glb`, `.gltf`, `.obj` ou `.fbx`) na pasta `entrada`. Use o nome do herói no arquivo: `caleb.glb`.
3. Dê dois cliques em `limpar_modelos.bat`.
4. Os modelos prontos aparecem na pasta `saida`.

Se o Blender não for encontrado, abra o `.bat` no Bloco de Notas e coloque o caminho do `blender.exe` na linha `set "BLENDER="`.

## Ajustes por herói

Copie `config-exemplo.json` para `config.json` e ajuste altura, triângulos e tamanho da textura de cada modelo. Os valores são **provisórios** (ver `docs/pendencias.md`).

## Pelo terminal

```
blender -b -P limpar_modelo.py -- --entrada entrada --saida saida --tris 15000 --altura 1.8 --textura 2048
```

`--sem-bake` pula a transferência de textura (mais rápido, para testar a malha).

## Próximo passo

Suba o FBX da pasta `saida` no [Mixamo](https://www.mixamo.com) para o rig automático e as animações.

## Limites

- O script não tira luz e sombra que a IA "pintou" na textura; isso é ajuste manual.
- O Mixamo precisa do personagem de **braços abertos** (pose em T ou em A). Gere o 3D a partir de uma imagem nessa pose.
- Partes grudadas (arma na mão, capa no corpo) continuam grudadas; separar é manual.
