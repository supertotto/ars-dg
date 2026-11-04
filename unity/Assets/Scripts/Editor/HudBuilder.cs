// claude: arquivo criado pelo Claude Code — gera o prefab do HUD em uGUI com a arte de Assets/UI/HUD (layout e cores PROVISÓRIOS)
using System;
using System.IO;
using ARS.Characters;
using ARS.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace ARS.EditorTools
{
    /// <summary>
    /// Menu ARS → Montar HUD: cria Assets/Prefabs/UI/HUD.prefab com retrato, passiva, botões Q/W/E/R
    /// e barras de vida e recurso. Depois de gerado, o design é ajustado direto no prefab.
    /// Arte em Assets/UI/HUD (gerada por IA, ver docs/tecnico/unity.md); sem ela, usa caixas lisas.
    /// </summary>
    public static class HudBuilder
    {
        const int CamadaUI = 5;

        // Layout provisório (pixels na resolução de referência 1920×1080)
        const float Margem = 26f, Retrato = 140f, Passiva = 72f, Botao = 92f, Espaco = 8f;
        const float AlturaVida = 24f, AlturaRecurso = 18f, BaseDasBarras = 20f;

        // Peças de arte (nome do arquivo em Assets/UI/HUD) e bordas do 9-slice (esq., baixo, dir., cima, em pixels da imagem)
        const string ArtePainel = "hud_painel", ArteMolduraSkill = "hud_moldura_skill",
            ArteMolduraPassiva = "hud_moldura_passiva", ArteMolduraBarra = "hud_moldura_barra", ArteFundoBarra = "hud_barra_fundo";
        static readonly Vector4 BordaPainel = new Vector4(95f, 66f, 95f, 66f);
        static readonly Vector4 BordaMolduraBarra = new Vector4(85f, 24f, 85f, 24f);

        [MenuItem("ARS/Montar HUD", priority = 22)]
        static void MontarPeloMenu()
        {
            if (File.Exists(ArsPaths.HudPrefab) &&
                !EditorUtility.DisplayDialog("Montar HUD",
                    "O HUD.prefab já existe. Refazer apaga os ajustes de design feitos nele.", "Refazer", "Cancelar"))
                return;
            var hud = Montar();
            Selection.activeObject = hud.gameObject;
        }

        /// <summary>O prefab do HUD; gera se ainda não existir.</summary>
        public static HudDoJogador ObterOuCriar()
        {
            var existente = AssetDatabase.LoadAssetAtPath<HudDoJogador>(ArsPaths.HudPrefab);
            return existente != null ? existente : Montar();
        }

        public static HudDoJogador Montar()
        {
            ConfigurarArte();
            var branco = SpriteGerado("Branco", 8, (u, v) => Color.white);
            var fonte = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            var painelArte = Arte(ArtePainel);
            var molduraSkill = Arte(ArteMolduraSkill);
            var molduraPassiva = Arte(ArteMolduraPassiva);
            var molduraBarra = Arte(ArteMolduraBarra);
            var fundoBarra = Arte(ArteFundoBarra);
            var circulo = SpriteGerado("Circulo", 128, Circulo);
            var preenchimento = SpriteGerado("Preenchimento", 64, Preenchimento);
            var brilhoQuadrado = SpriteGerado("BrilhoQuadrado", 128, BrilhoQuadrado);
            var brilhoRedondo = SpriteGerado("BrilhoRedondo", 128, BrilhoRedondo);

            var raiz = new GameObject("HUD", typeof(RectTransform)) { layer = CamadaUI };
            var canvas = raiz.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 10;
            var escala = raiz.AddComponent<CanvasScaler>();
            escala.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            escala.referenceResolution = new Vector2(1920f, 1080f);
            escala.matchWidthOrHeight = 0.5f;
            var hud = raiz.AddComponent<HudDoJogador>();

            // Medidas: [retrato] [passiva] [Q W E R], barras embaixo das skills
            float xPassiva = Margem + Retrato + 12f;
            float xBotoes = xPassiva + Passiva + 12f;
            float larguraBotoes = 4 * Botao + 3 * Espaco;
            float yBotoes = BaseDasBarras + AlturaRecurso + 6f + AlturaVida + 10f;
            float largura = xBotoes + larguraBotoes + Margem;
            float altura = Mathf.Max(yBotoes + Botao, BaseDasBarras + Retrato) + 18f;

            // Painel de fundo
            var painel = Novo("Painel", raiz.transform);
            Posicionar(painel, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 10f), new Vector2(largura, altura));
            var imagemPainel = Imagem(painel, painelArte ?? branco, painelArte != null ? Color.white : new Color(0f, 0f, 0f, 0.55f));
            Fatiar(imagemPainel, 2f);

            // Retrato do herói (quadrado, com a moldura das skills)
            var retrato = Novo("Retrato", painel);
            CantoInferiorEsquerdo(retrato, new Vector2(Margem, BaseDasBarras - 4f), new Vector2(Retrato, Retrato));
            Imagem(Esticar(Novo("Fundo", retrato), Retrato * 0.09f), branco, new Color(0.06f, 0.06f, 0.08f));
            hud.retrato = Imagem(Esticar(Novo("Foto", retrato), Retrato * 0.1f), null, Color.white);
            hud.retrato.preserveAspect = true;
            Imagem(Esticar(Novo("Moldura", retrato), 0f), molduraSkill ?? branco, Color.white);

            // Passiva (redonda)
            var passiva = Novo("Passiva", painel);
            CantoInferiorEsquerdo(passiva, new Vector2(xPassiva, yBotoes + (Botao - Passiva) / 2f), new Vector2(Passiva, Passiva));
            hud.passivaRaiz = passiva.gameObject;
            hud.passivaBrilho = Imagem(Esticar(Novo("Brilho", passiva), -Passiva * 0.3f), brilhoRedondo, new Color(1f, 0.85f, 0.35f));
            hud.passivaBrilho.enabled = false;
            Imagem(Esticar(Novo("Fundo", passiva), Passiva * 0.16f), circulo, new Color(0.06f, 0.06f, 0.08f));
            hud.passivaIcone = Imagem(Esticar(Novo("Icone", passiva), Passiva * 0.27f), null, Color.white);
            hud.passivaIcone.preserveAspect = true;
            Imagem(Esticar(Novo("Moldura", passiva), 0f), molduraPassiva ?? circulo, Color.white);
            var contador = Novo("Contador", passiva);
            Posicionar(contador, new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(0f, -16f), new Vector2(0f, 20f), new Vector2(0.5f, 0f));
            hud.passivaTexto = Texto(contador, fonte, 16, TextAnchor.LowerCenter);

            // Botões Q, W, E, R
            hud.botoes = new BotaoDeSkill[4];
            float x = xBotoes;
            foreach (Tecla tecla in Enum.GetValues(typeof(Tecla)))
            {
                hud.botoes[(int)tecla] = CriarBotao(tecla, painel, branco, molduraSkill, brilhoQuadrado, fonte, new Vector2(x, yBotoes));
                x += Botao + Espaco;
            }

            // Barras embaixo das skills
            hud.barraDeVida = Barra("Vida", painel, branco, fundoBarra, preenchimento, molduraBarra, fonte,
                new Vector2(xBotoes, BaseDasBarras + AlturaRecurso + 6f), new Vector2(larguraBotoes, AlturaVida), new Color(0.8f, 0.16f, 0.14f), 16);
            hud.barraDeRecurso = Barra("Recurso", painel, branco, fundoBarra, preenchimento, molduraBarra, fonte,
                new Vector2(xBotoes, BaseDasBarras), new Vector2(larguraBotoes, AlturaRecurso), new Color(0.22f, 0.48f, 1f), 14);

            // Mensagens do kit ("sem mana", "fora de alcance"...)
            var mensagem = Novo("Mensagem", raiz.transform);
            Posicionar(mensagem, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 10f + altura + 10f), new Vector2(900f, 40f));
            hud.mensagem = Texto(mensagem, fonte, 24, TextAnchor.MiddleCenter);

            ArsPaths.GarantirPasta(Path.GetDirectoryName(ArsPaths.HudPrefab));
            var prefab = PrefabUtility.SaveAsPrefabAsset(raiz, ArsPaths.HudPrefab);
            Object.DestroyImmediate(raiz);
            Debug.Log($"[ARS] HUD salvo em {ArsPaths.HudPrefab} ({(painelArte != null ? "com a arte de Assets/UI/HUD" : "sem arte: caixas lisas")}).");
            return prefab.GetComponent<HudDoJogador>();
        }

        static BotaoDeSkill CriarBotao(Tecla tecla, Transform pai, Sprite branco, Sprite moldura, Sprite brilho, Font fonte, Vector2 posicao)
        {
            float recuo = Botao * 0.1f; // a moldura cobre ~10% de cada lado
            var raiz = Novo(tecla.ToString(), pai);
            CantoInferiorEsquerdo(raiz, posicao, new Vector2(Botao, Botao));
            var botao = raiz.gameObject.AddComponent<BotaoDeSkill>();
            botao.tecla = tecla;

            botao.brilho = Imagem(Esticar(Novo("Brilho", raiz), -Botao * 0.18f), brilho, new Color(1f, 0.85f, 0.35f));
            botao.brilho.enabled = false;
            Imagem(Esticar(Novo("Fundo", raiz), recuo * 0.9f), branco, new Color(0.06f, 0.06f, 0.08f));
            botao.icone = Imagem(Esticar(Novo("Icone", raiz), recuo), null, Color.white);
            botao.icone.preserveAspect = true;

            botao.sombraDeRecarga = Imagem(Esticar(Novo("Recarga", raiz), recuo), branco, new Color(0f, 0f, 0f, 0.7f));
            botao.sombraDeRecarga.type = Image.Type.Filled;
            botao.sombraDeRecarga.fillMethod = Image.FillMethod.Radial360;
            botao.sombraDeRecarga.fillOrigin = (int)Image.Origin360.Top;
            botao.sombraDeRecarga.fillClockwise = false;

            botao.moldura = Imagem(Esticar(Novo("Moldura", raiz), 0f), moldura ?? branco, moldura != null ? Color.white : new Color(0.35f, 0.35f, 0.4f));

            botao.textoDoTempo = Texto(Esticar(Novo("Tempo", raiz), 0f), fonte, 32, TextAnchor.MiddleCenter);

            var rotulo = Novo("Tecla", raiz);
            CantoInferiorEsquerdo(rotulo, new Vector2(recuo + 4f, recuo + 2f), new Vector2(30f, 24f));
            botao.textoDaTecla = Texto(rotulo, fonte, 18, TextAnchor.LowerLeft);
            botao.textoDaTecla.text = tecla.ToString();

            var cargas = Novo("Cargas", raiz);
            Posicionar(cargas, Vector2.one, Vector2.one, new Vector2(-recuo - 4f, -recuo - 1f), new Vector2(30f, 24f), Vector2.one);
            botao.textoDasCargas = Texto(cargas, fonte, 18, TextAnchor.UpperRight);
            return botao;
        }

        static BarraDoHud Barra(string nome, Transform pai, Sprite branco, Sprite fundo, Sprite preenchimento, Sprite moldura,
            Font fonte, Vector2 posicao, Vector2 tamanho, Color cor, int letra)
        {
            var raiz = Novo(nome, pai);
            CantoInferiorEsquerdo(raiz, posicao, tamanho);
            var barra = raiz.gameObject.AddComponent<BarraDoHud>();
            Imagem(Esticar(Novo("Fundo", raiz), 1f), fundo ?? branco, fundo != null ? Color.white : new Color(0.06f, 0.06f, 0.08f, 0.95f));
            barra.preenchimento = Imagem(Esticar(Novo("Preenchimento", raiz), 2f), preenchimento, cor);
            barra.preenchimento.type = Image.Type.Filled;
            barra.preenchimento.fillMethod = Image.FillMethod.Horizontal;
            barra.preenchimento.fillOrigin = (int)Image.OriginHorizontal.Left;
            if (moldura != null)
            {
                // a moldura passa um pouco das bordas (as pontas ficam para fora)
                var rt = Novo("Moldura", raiz);
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.offsetMin = new Vector2(-9f, -3f);
                rt.offsetMax = new Vector2(9f, 3f);
                Fatiar(Imagem(rt, moldura, Color.white), 8f);
            }
            barra.texto = Texto(Esticar(Novo("Texto", raiz), 0f), fonte, letra, TextAnchor.MiddleCenter);
            return barra;
        }

        // ---------- arte ----------

        static Sprite Arte(string nome) => AssetDatabase.LoadAssetAtPath<Sprite>($"{ArsPaths.Ui}/HUD/{nome}.png");

        /// <summary>Mipmaps nas peças de arte (são maiores que na tela) e bordas do 9-slice, se ainda não tiverem.</summary>
        static void ConfigurarArte()
        {
            foreach (var guid in AssetDatabase.FindAssets("t:Texture2D", new[] { ArsPaths.Ui + "/HUD" }))
            {
                var caminho = AssetDatabase.GUIDToAssetPath(guid);
                if (!(AssetImporter.GetAtPath(caminho) is TextureImporter importador)) continue;
                bool mudou = false;
                if (!importador.mipmapEnabled) { importador.mipmapEnabled = true; mudou = true; }
                string nome = Path.GetFileNameWithoutExtension(caminho);
                Vector4 borda = nome == ArtePainel ? BordaPainel : nome == ArteMolduraBarra ? BordaMolduraBarra : Vector4.zero;
                if (borda != Vector4.zero && importador.spriteBorder == Vector4.zero) { importador.spriteBorder = borda; mudou = true; }
                if (mudou) importador.SaveAndReimport();
            }
        }

        static void Fatiar(Image imagem, float reducaoDaBorda)
        {
            if (imagem.sprite == null || imagem.sprite.border == Vector4.zero) return;
            imagem.type = Image.Type.Sliced;
            imagem.pixelsPerUnitMultiplier = reducaoDaBorda;
        }

        /// <summary>Sprite desenhado por código em Assets/UI/Base (cria se não existir).</summary>
        static Sprite SpriteGerado(string nome, int lado, Func<float, float, Color> cor)
        {
            string caminho = $"{ArsPaths.Ui}/Base/{nome}.png";
            if (!File.Exists(caminho))
            {
                ArsPaths.GarantirPasta(ArsPaths.Ui + "/Base");
                var textura = new Texture2D(lado, lado, TextureFormat.RGBA32, false);
                var pixels = new Color[lado * lado];
                for (int y = 0; y < lado; y++)
                    for (int x = 0; x < lado; x++)
                        pixels[y * lado + x] = cor((x + 0.5f) / lado, (y + 0.5f) / lado);
                textura.SetPixels(pixels);
                File.WriteAllBytes(caminho, textura.EncodeToPNG());
                Object.DestroyImmediate(textura);
                AssetDatabase.ImportAsset(caminho, ImportAssetOptions.ForceSynchronousImport);
            }
            return AssetDatabase.LoadAssetAtPath<Sprite>(caminho);
        }

        static Color Circulo(float u, float v)
        {
            float d = Vector2.Distance(new Vector2(u, v), new Vector2(0.5f, 0.5f));
            return new Color(1f, 1f, 1f, Mathf.Clamp01((0.5f - d) * 128f));
        }

        /// <summary>Branco com faixa de brilho em cima e mais escuro embaixo (a cor vem do Image).</summary>
        static Color Preenchimento(float u, float v)
        {
            float tom = Mathf.Lerp(0.62f, 1f, v) + Mathf.Exp(-Mathf.Pow((v - 0.78f) / 0.08f, 2f)) * 0.35f;
            return new Color(Mathf.Min(tom, 1f), Mathf.Min(tom, 1f), Mathf.Min(tom, 1f), 1f);
        }

        /// <summary>Brilho em volta de um quadrado (centro vazio).</summary>
        static Color BrilhoQuadrado(float u, float v)
        {
            // distância até a borda de um quadrado que ocupa 64% do sprite
            float dx = Mathf.Abs(u - 0.5f) - 0.32f, dy = Mathf.Abs(v - 0.5f) - 0.32f;
            float fora = new Vector2(Mathf.Max(dx, 0f), Mathf.Max(dy, 0f)).magnitude;
            float dentro = Mathf.Min(Mathf.Max(dx, dy), 0f);
            float d = fora + Mathf.Abs(dentro);
            float a = Mathf.Exp(-Mathf.Pow(d / 0.07f, 2f));
            return new Color(1f, 1f, 1f, a);
        }

        /// <summary>Brilho em anel (centro vazio).</summary>
        static Color BrilhoRedondo(float u, float v)
        {
            float d = Mathf.Abs(Vector2.Distance(new Vector2(u, v), new Vector2(0.5f, 0.5f)) - 0.3f);
            return new Color(1f, 1f, 1f, Mathf.Exp(-Mathf.Pow(d / 0.07f, 2f)));
        }

        // ---------- peças básicas ----------

        static RectTransform Novo(string nome, Transform pai)
        {
            var objeto = new GameObject(nome, typeof(RectTransform)) { layer = CamadaUI };
            objeto.transform.SetParent(pai, false);
            return (RectTransform)objeto.transform;
        }

        static void Posicionar(RectTransform rt, Vector2 ancoraMin, Vector2 ancoraMax, Vector2 posicao, Vector2 tamanho, Vector2? pivo = null)
        {
            rt.anchorMin = ancoraMin;
            rt.anchorMax = ancoraMax;
            rt.pivot = pivo ?? new Vector2(0.5f, 0f);
            rt.anchoredPosition = posicao;
            rt.sizeDelta = tamanho;
        }

        static void CantoInferiorEsquerdo(RectTransform rt, Vector2 posicao, Vector2 tamanho) =>
            Posicionar(rt, Vector2.zero, Vector2.zero, posicao, tamanho, Vector2.zero);

        static RectTransform Esticar(RectTransform rt, float recuo)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = new Vector2(recuo, recuo);
            rt.offsetMax = new Vector2(-recuo, -recuo);
            return rt;
        }

        static Image Imagem(RectTransform rt, Sprite sprite, Color cor)
        {
            var imagem = rt.gameObject.AddComponent<Image>();
            imagem.sprite = sprite;
            imagem.color = cor;
            imagem.raycastTarget = false;
            return imagem;
        }

        static Text Texto(RectTransform rt, Font fonte, int tamanho, TextAnchor alinhamento)
        {
            var texto = rt.gameObject.AddComponent<Text>();
            texto.font = fonte;
            texto.fontSize = tamanho;
            texto.fontStyle = FontStyle.Bold;
            texto.alignment = alinhamento;
            texto.color = Color.white;
            texto.horizontalOverflow = HorizontalWrapMode.Overflow;
            texto.verticalOverflow = VerticalWrapMode.Overflow;
            texto.raycastTarget = false;
            var contorno = rt.gameObject.AddComponent<Outline>();
            contorno.effectColor = new Color(0f, 0f, 0f, 0.9f);
            contorno.effectDistance = new Vector2(1.5f, -1.5f);
            return texto;
        }
    }

    /// <summary>Imagens novas em Assets/UI entram como Sprite, sem mipmaps. Só na primeira importação.</summary>
    public class ImportadorDeImagensDaUI : AssetPostprocessor
    {
        void OnPreprocessTexture()
        {
            if (!assetPath.Replace('\\', '/').StartsWith(ArsPaths.Ui + "/") || !assetImporter.importSettingsMissing) return;
            var importador = (TextureImporter)assetImporter;
            importador.textureType = TextureImporterType.Sprite;
            importador.spriteImportMode = SpriteImportMode.Single;
            importador.mipmapEnabled = false;
            importador.alphaIsTransparency = true;
        }
    }
}
