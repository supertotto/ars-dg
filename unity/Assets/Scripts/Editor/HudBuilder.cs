// claude: arquivo criado pelo Claude Code — gera o prefab do HUD em uGUI (layout e cores PROVISÓRIOS)
using System.IO;
using ARS.Characters;
using ARS.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ARS.EditorTools
{
    /// <summary>
    /// Menu ARS → Montar HUD: cria Assets/Prefabs/UI/HUD.prefab com barras de vida e recurso,
    /// passiva e os botões Q/W/E/R. Depois de gerado, o design é ajustado direto no prefab.
    /// </summary>
    public static class HudBuilder
    {
        const int CamadaUI = 5;
        const float Botao = 96f, Espaco = 10f, Margem = 20f, Passiva = 72f;

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
            var branco = SpriteBranco();
            var fonte = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            var raiz = new GameObject("HUD", typeof(RectTransform)) { layer = CamadaUI };
            var canvas = raiz.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 10;
            var escala = raiz.AddComponent<CanvasScaler>();
            escala.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            escala.referenceResolution = new Vector2(1920f, 1080f);
            escala.matchWidthOrHeight = 0.5f;
            var hud = raiz.AddComponent<HudDoJogador>();

            // Painel de baixo, no centro
            float largura = Margem + Passiva + Espaco + 4 * Botao + 3 * Espaco + Margem;
            float alturaBotoes = 16f + Botao;
            var painel = Novo("Painel", raiz.transform);
            Posicionar(painel, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 16f), new Vector2(largura, alturaBotoes + 84f));
            Imagem(painel, branco, new Color(0f, 0f, 0f, 0.55f));

            // Barras
            hud.barraDeVida = Barra("Vida", painel, branco, fonte, new Vector2(Margem, alturaBotoes + 38f),
                new Vector2(largura - 2 * Margem, 30f), new Color(0.75f, 0.15f, 0.15f), 18);
            hud.barraDeRecurso = Barra("Recurso", painel, branco, fonte, new Vector2(Margem, alturaBotoes + 12f),
                new Vector2(largura - 2 * Margem, 18f), new Color(0.2f, 0.45f, 0.95f), 14);

            // Passiva
            var passiva = Novo("Passiva", painel);
            CantoInferiorEsquerdo(passiva, new Vector2(Margem, 16f + (Botao - Passiva) / 2f), new Vector2(Passiva, Passiva));
            hud.passivaRaiz = passiva.gameObject;
            hud.passivaMoldura = Imagem(passiva, branco, hud.corDaPassiva);
            Imagem(Esticar(Novo("Fundo", passiva), 3f), branco, new Color(0.08f, 0.08f, 0.1f));
            hud.passivaIcone = Imagem(Esticar(Novo("Icone", passiva), 7f), null, Color.white);
            hud.passivaIcone.preserveAspect = true;
            var textoPassiva = Novo("Contador", passiva);
            Posicionar(textoPassiva, new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(0f, -2f), new Vector2(0f, 24f), new Vector2(0.5f, 0f));
            hud.passivaTexto = Texto(textoPassiva, fonte, 18, TextAnchor.LowerCenter);

            // Botões Q, W, E, R
            hud.botoes = new BotaoDeSkill[4];
            float x = Margem + Passiva + Espaco;
            foreach (Tecla tecla in System.Enum.GetValues(typeof(Tecla)))
            {
                hud.botoes[(int)tecla] = CriarBotao(tecla, painel, branco, fonte, new Vector2(x, 16f));
                x += Botao + Espaco;
            }

            // Mensagens do kit ("sem mana", "fora de alcance"...)
            var mensagem = Novo("Mensagem", raiz.transform);
            Posicionar(mensagem, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 16f + alturaBotoes + 84f + 12f), new Vector2(900f, 40f));
            hud.mensagem = Texto(mensagem, fonte, 26, TextAnchor.MiddleCenter);

            ArsPaths.GarantirPasta(Path.GetDirectoryName(ArsPaths.HudPrefab));
            var prefab = PrefabUtility.SaveAsPrefabAsset(raiz, ArsPaths.HudPrefab);
            Object.DestroyImmediate(raiz);
            Debug.Log($"[ARS] HUD salvo em {ArsPaths.HudPrefab}.");
            return prefab.GetComponent<HudDoJogador>();
        }

        static BotaoDeSkill CriarBotao(Tecla tecla, Transform pai, Sprite branco, Font fonte, Vector2 posicao)
        {
            var raiz = Novo(tecla.ToString(), pai);
            CantoInferiorEsquerdo(raiz, posicao, new Vector2(Botao, Botao));
            var botao = raiz.gameObject.AddComponent<BotaoDeSkill>();
            botao.tecla = tecla;
            botao.moldura = Imagem(raiz, branco, botao.corDaMoldura);
            Imagem(Esticar(Novo("Fundo", raiz), 3f), branco, new Color(0.08f, 0.08f, 0.1f));
            botao.icone = Imagem(Esticar(Novo("Icone", raiz), 9f), null, Color.white);
            botao.icone.preserveAspect = true;

            botao.sombraDeRecarga = Imagem(Esticar(Novo("Recarga", raiz), 3f), branco, new Color(0f, 0f, 0f, 0.7f));
            botao.sombraDeRecarga.type = Image.Type.Filled;
            botao.sombraDeRecarga.fillMethod = Image.FillMethod.Radial360;
            botao.sombraDeRecarga.fillOrigin = (int)Image.Origin360.Top;
            botao.sombraDeRecarga.fillClockwise = false;

            botao.textoDoTempo = Texto(Esticar(Novo("Tempo", raiz), 0f), fonte, 34, TextAnchor.MiddleCenter);

            var rotulo = Novo("Tecla", raiz);
            CantoInferiorEsquerdo(rotulo, new Vector2(6f, 2f), new Vector2(30f, 26f));
            botao.textoDaTecla = Texto(rotulo, fonte, 18, TextAnchor.LowerLeft);
            botao.textoDaTecla.text = tecla.ToString();

            var cargas = Novo("Cargas", raiz);
            Posicionar(cargas, Vector2.one, Vector2.one, new Vector2(-6f, -2f), new Vector2(30f, 26f), Vector2.one);
            botao.textoDasCargas = Texto(cargas, fonte, 18, TextAnchor.UpperRight);
            return botao;
        }

        static BarraDoHud Barra(string nome, Transform pai, Sprite branco, Font fonte, Vector2 posicao, Vector2 tamanho, Color cor, int letra)
        {
            var raiz = Novo(nome, pai);
            CantoInferiorEsquerdo(raiz, posicao, tamanho);
            Imagem(raiz, branco, new Color(0.06f, 0.06f, 0.08f, 0.95f));
            var barra = raiz.gameObject.AddComponent<BarraDoHud>();
            barra.preenchimento = Imagem(Esticar(Novo("Preenchimento", raiz), 2f), branco, cor);
            barra.preenchimento.type = Image.Type.Filled;
            barra.preenchimento.fillMethod = Image.FillMethod.Horizontal;
            barra.preenchimento.fillOrigin = (int)Image.OriginHorizontal.Left;
            barra.texto = Texto(Esticar(Novo("Texto", raiz), 0f), fonte, letra, TextAnchor.MiddleCenter);
            return barra;
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

        /// <summary>Sprite branco 8×8 (base das barras e da sombra de recarga). Cria se não existir.</summary>
        static Sprite SpriteBranco()
        {
            string caminho = ArsPaths.Ui + "/Base/Branco.png";
            if (!File.Exists(caminho))
            {
                ArsPaths.GarantirPasta(ArsPaths.Ui + "/Base");
                var textura = new Texture2D(8, 8, TextureFormat.RGBA32, false);
                var pixels = new Color32[64];
                for (int i = 0; i < pixels.Length; i++) pixels[i] = new Color32(255, 255, 255, 255);
                textura.SetPixels32(pixels);
                File.WriteAllBytes(caminho, textura.EncodeToPNG());
                Object.DestroyImmediate(textura);
                AssetDatabase.ImportAsset(caminho, ImportAssetOptions.ForceSynchronousImport);
            }
            return AssetDatabase.LoadAssetAtPath<Sprite>(caminho);
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
