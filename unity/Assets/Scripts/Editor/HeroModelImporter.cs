// claude: arquivo criado pelo Claude (app claude.ai)
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ARS.EditorTools
{
    /// <summary>
    /// Configura sozinho os FBX do Mixamo quando entram no projeto:
    ///  - Modelos em Assets/Characters/&lt;Herói&gt;/Model → rig Humanoid
    ///  - Animações em Assets/Animations ou Assets/Characters/&lt;Herói&gt;/Animations →
    ///    rig Humanoid, loop nas animações de ciclo e personagem preso no lugar
    /// Só age na primeira importação: ajustes feitos à mão depois não são apagados.
    /// </summary>
    public class HeroModelImporter : AssetPostprocessor
    {
        // Palavras que indicam animação em ciclo (inglês do Mixamo + português)
        static readonly string[] PalavrasDeCiclo =
        {
            "idle", "breathing", "walk", "run", "jog", "sprint", "strafe", "locomotion",
            "parado", "andar", "correr", "andando", "correndo"
        };

        void OnPreprocessModel()
        {
            if (!assetImporter.importSettingsMissing) return;
            ConfigurarModelo((ModelImporter)assetImporter, assetPath);
        }

        void OnPreprocessAnimation()
        {
            if (!assetImporter.importSettingsMissing) return;
            ConfigurarClipes((ModelImporter)assetImporter, assetPath);
        }

        public static bool EhFbxDoProjeto(string caminho) =>
            caminho.ToLowerInvariant().EndsWith(".fbx") &&
            (ArsPaths.EhModeloDeHeroi(caminho) || ArsPaths.EhAnimacao(caminho));

        /// <summary>Rig Humanoid e opções de importação.</summary>
        public static void ConfigurarModelo(ModelImporter importador, string caminho)
        {
            if (!EhFbxDoProjeto(caminho)) return;
            bool modelo = ArsPaths.EhModeloDeHeroi(caminho);

            importador.animationType = ModelImporterAnimationType.Human;
            importador.avatarSetup = ModelImporterAvatarSetup.CreateFromThisModel;
            importador.importAnimation = true;
            importador.importBlendShapes = modelo;
            importador.importCameras = false;
            importador.importLights = false;

            // Arquivo só de animação: não precisa de materiais
            if (!modelo) importador.materialImportMode = ModelImporterMaterialImportMode.None;
        }

        /// <summary>Nome, loop e "Bake Into Pose" de cada clipe.</summary>
        public static void ConfigurarClipes(ModelImporter importador, string caminho)
        {
            if (!EhFbxDoProjeto(caminho)) return;

            var clipes = importador.defaultClipAnimations;
            if (clipes == null || clipes.Length == 0) return;

            string nomeArquivo = System.IO.Path.GetFileNameWithoutExtension(caminho).ToLowerInvariant();
            bool ehModelo = ArsPaths.EhModeloDeHeroi(caminho);
            bool ciclo = PalavrasDeCiclo.Any(p => nomeArquivo.Contains(p));

            foreach (var clipe in clipes)
            {
                // O Mixamo chama todo clipe de "mixamo.com": renomeia para algo legível.
                // No modelo do herói, a animação que vem junto (baixada "With Skin") é o parado.
                if (clipes.Length == 1)
                    clipe.name = ehModelo ? "parado" : nomeArquivo;

                string nomeClipe = (clipe.name ?? "").ToLowerInvariant();
                bool esteEhCiclo = ehModelo || ciclo || PalavrasDeCiclo.Any(p => nomeClipe.Contains(p));

                clipe.loopTime = esteEhCiclo;
                clipe.loopPose = esteEhCiclo;

                // "Bake Into Pose": quem move o herói é o jogo, não a animação
                clipe.lockRootRotation = true;
                clipe.keepOriginalOrientation = true;
                clipe.lockRootHeightY = true;
                clipe.keepOriginalPositionY = true;
                clipe.lockRootPositionXZ = true;
                clipe.keepOriginalPositionXZ = true;
            }

            importador.clipAnimations = clipes;
        }

        /// <summary>
        /// Refaz a configuração de todos os FBX (útil se eles entraram no projeto
        /// antes destas ferramentas, ou se algo foi mexido à mão).
        /// </summary>
        [MenuItem("ARS/Reconfigurar FBX (Humanoid e animações)", priority = 10)]
        public static void ReconfigurarTodos()
        {
            var caminhos = AssetDatabase.FindAssets("t:GameObject", new[] { ArsPaths.Personagens, ArsPaths.Animacoes }
                    .Where(p => AssetDatabase.IsValidFolder(p)).ToArray())
                .Select(g => AssetDatabase.GUIDToAssetPath(g))
                .Where(EhFbxDoProjeto)
                .Distinct()
                .ToList();

            try
            {
                for (int i = 0; i < caminhos.Count; i++)
                {
                    string caminho = caminhos[i];
                    EditorUtility.DisplayProgressBar("ARS", "Reconfigurando " + caminho, (float)i / caminhos.Count);
                    if (!(AssetImporter.GetAtPath(caminho) is ModelImporter importador)) continue;

                    // 1ª passada: rig Humanoid (os clipes só ficam humanoides depois disso)
                    ConfigurarModelo(importador, caminho);
                    importador.clipAnimations = new ModelImporterClipAnimation[0];
                    importador.SaveAndReimport();

                    // 2ª passada: nome, loop e Bake Into Pose
                    ConfigurarClipes(importador, caminho);
                    importador.SaveAndReimport();
                }
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }

            Debug.Log($"[ARS] {caminhos.Count} FBX reconfigurado(s). Agora rode ARS → Montar heróis.");
        }
    }
}
