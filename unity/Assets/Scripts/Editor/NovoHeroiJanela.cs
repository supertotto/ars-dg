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
                if (GUILayout.Button("Cancelar", GUILayout.Width(90))) Close();
                GUI.enabled = !string.IsNullOrWhiteSpace(nome);
                if (GUILayout.Button("Criar", GUILayout.Width(90)))
                {
                    HeroBuilder.CriarPastasDoHeroi(nome);
                    Close();
                }
                GUI.enabled = true;
            }
        }
    }
}
