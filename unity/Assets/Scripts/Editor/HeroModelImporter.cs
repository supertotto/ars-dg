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
            if (!assetPath.ToLowerInvariant().EndsWith(".fbx")) return;
            if (!assetImporter.importSettingsMissing) return;

            bool modelo = ArsPaths.EhModeloDeHeroi(assetPath);
            bool animacao = ArsPaths.EhAnimacao(assetPath);
            if (!modelo && !animacao) return;

            var importador = (ModelImporter)assetImporter;
            importador.animationType = ModelImporterAnimationType.Human;
            importador.avatarSetup = ModelImporterAvatarSetup.CreateFromThisModel;
            importador.importAnimation = true;
            importador.importBlendShapes = modelo;
            importador.importCameras = false;
            importador.importLights = false;

            if (animacao)
            {
                // Arquivo só de animação: não precisa de materiais
                importador.materialImportMode = ModelImporterMaterialImportMode.None;
            }
        }

        void OnPreprocessAnimation()
        {
            if (!assetImporter.importSettingsMissing) return;
            if (!ArsPaths.EhAnimacao(assetPath) && !ArsPaths.EhModeloDeHeroi(assetPath)) return;

            var importador = (ModelImporter)assetImporter;
            var clipes = importador.defaultClipAnimations;
            if (clipes == null || clipes.Length == 0) return;

            string nomeArquivo = System.IO.Path.GetFileNameWithoutExtension(assetPath).ToLowerInvariant();
            bool ehModelo = ArsPaths.EhModeloDeHeroi(assetPath);
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
    }
}
