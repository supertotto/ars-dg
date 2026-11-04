// claude: arquivo criado pelo Claude (app claude.ai)
using System.Linq;
using ARS.Effects;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace ARS.EditorTools
{
    /// <summary>
    /// Monta uma cena com todos os efeitos (prefabs) de uma pasta lado a lado, com o nome
    /// embaixo de cada um, repetindo sem parar. Serve para escolher efeitos de um pack.
    /// Uso: selecione a pasta do pack na janela Project → ARS → Vitrine de efeitos.
    /// </summary>
    public static class VitrineDeEfeitos
    {
        const float Espaco = 3.5f;
        const int Maximo = 150;

        [MenuItem("ARS/Vitrine de efeitos (pasta selecionada)", priority = 60)]
        public static void Montar()
        {
            string pasta = PastaSelecionada();
            if (pasta == null)
            {
                EditorUtility.DisplayDialog("ARS", "Selecione na janela Project a pasta do pack de efeitos e rode de novo.", "Ok");
                return;
            }

            var prefabs = AssetDatabase.FindAssets("t:Prefab", new[] { pasta })
                .Select(g => AssetDatabase.GUIDToAssetPath(g))
                .OrderBy(p => p)
                .Select(p => AssetDatabase.LoadAssetAtPath<GameObject>(p))
                .Where(p => p != null && p.GetComponentsInChildren<Renderer>(true).Length > 0)
                // Pula os objetos da demonstração do pack (câmeras, "player" de demo etc.)
                .Where(p => !p.name.ToLowerInvariant().Contains("demo"))
                .Take(Maximo)
                .ToList();

            if (prefabs.Count == 0)
            {
                EditorUtility.DisplayDialog("ARS", $"Nenhum prefab de efeito encontrado em {pasta}.", "Ok");
                return;
            }

            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            var cena = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

            var fonte = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            int colunas = Mathf.CeilToInt(Mathf.Sqrt(prefabs.Count));
            var raiz = new GameObject("Efeitos");

            for (int i = 0; i < prefabs.Count; i++)
            {
                int linha = i / colunas, coluna = i % colunas;
                var posicao = new Vector3(coluna * Espaco, 0f, -linha * Espaco);

                var efeito = (GameObject)PrefabUtility.InstantiatePrefab(prefabs[i]);
                efeito.transform.SetParent(raiz.transform);
                efeito.transform.position = posicao + Vector3.up * 1f;
                efeito.AddComponent<RepetirEfeito>();

                var placa = new GameObject($"{i + 1:000} - {prefabs[i].name}");
                placa.transform.SetParent(raiz.transform);
                placa.transform.position = posicao + new Vector3(0f, 0.05f, -1.2f);
                placa.transform.rotation = Quaternion.Euler(60f, 0f, 0f);
                var texto = placa.AddComponent<TextMesh>();
                texto.font = fonte;
                texto.text = $"{i + 1}. {prefabs[i].name}";
                texto.fontSize = 40;
                texto.characterSize = 0.04f;
                texto.anchor = TextAnchor.MiddleCenter;
                texto.color = Color.white;
                placa.GetComponent<MeshRenderer>().sharedMaterial = fonte.material;
            }

            // Câmera de cima vendo a grade inteira
            var camera = Camera.main;
            if (camera != null)
            {
                float largura = (colunas - 1) * Espaco;
                float profundidade = (Mathf.CeilToInt(prefabs.Count / (float)colunas) - 1) * Espaco;
                var centro = new Vector3(largura / 2f, 0f, -profundidade / 2f);
                float distancia = Mathf.Max(largura, profundidade) * 1.1f + 6f;
                camera.transform.rotation = Quaternion.Euler(60f, 0f, 0f);
                camera.transform.position = centro - camera.transform.forward * distancia;
                camera.clearFlags = CameraClearFlags.SolidColor;
                camera.backgroundColor = new Color(0.08f, 0.08f, 0.1f);
            }

            ArsPaths.GarantirPasta(ArsPaths.Cenas);
            EditorSceneManager.SaveScene(cena, ArsPaths.Cenas + "/Vitrine.unity");
            Debug.Log($"[ARS] Vitrine com {prefabs.Count} efeito(s) de {pasta}. Aperte Play. " +
                      "Para ver um de perto: clique duas vezes no nome dele na Hierarchy (dentro de 'Efeitos') com a janela Scene aberta.");
        }

        static string PastaSelecionada()
        {
            var selecionado = Selection.activeObject;
            if (selecionado == null) return null;
            string caminho = AssetDatabase.GetAssetPath(selecionado);
            if (string.IsNullOrEmpty(caminho)) return null;
            if (AssetDatabase.IsValidFolder(caminho)) return caminho;
            return System.IO.Path.GetDirectoryName(caminho)?.Replace('\\', '/');
        }
    }
}
