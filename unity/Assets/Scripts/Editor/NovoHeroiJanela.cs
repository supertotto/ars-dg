// claude: arquivo criado pelo Claude (app claude.ai)
using UnityEditor;
using UnityEngine;

namespace ARS.EditorTools
{
    /// <summary>Janelinha que pede o nome do herói e cria as pastas dele.</summary>
    public class NovoHeroiJanela : EditorWindow
    {
        string nome = "";

        public static void Abrir()
        {
            var janela = CreateInstance<NovoHeroiJanela>();
            janela.titleContent = new GUIContent("Novo herói");
            janela.minSize = janela.maxSize = new Vector2(320, 90);
            janela.ShowUtility();
        }

        void OnGUI()
        {
            EditorGUILayout.Space();
            nome = EditorGUILayout.TextField("Nome (ex.: Caleb)", nome);
            EditorGUILayout.Space();
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                bool cancelar = GUILayout.Button("Cancelar", GUILayout.Width(90));
                GUI.enabled = !string.IsNullOrWhiteSpace(nome);
                bool criar = GUILayout.Button("Criar", GUILayout.Width(90));
                GUI.enabled = true;

                if (cancelar || criar)
                {
                    // Fecha e cria as pastas depois de terminar de desenhar a janela
                    string nomeEscolhido = nome;
                    EditorApplication.delayCall += () =>
                    {
                        if (criar) HeroBuilder.CriarPastasDoHeroi(nomeEscolhido);
                    };
                    Close();
                    GUIUtility.ExitGUI();
                }
            }
        }
    }
}
