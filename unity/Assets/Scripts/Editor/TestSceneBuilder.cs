// claude: arquivo criado pelo Claude (app claude.ai)
using System.Linq;
using ARS.CameraSystem;
using ARS.Characters;
using ARS.Combat;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace ARS.EditorTools
{
    /// <summary>
    /// Cria a cena de teste: chão, luz, câmera isométrica e os heróis montados.
    /// O primeiro herói é o controlado (botão direito do mouse para andar).
    /// Menu: ARS → Criar cena de teste.
    /// </summary>
    public static class TestSceneBuilder
    {
        const string CaminhoCena = ArsPaths.Cenas + "/Teste.unity";
        const string CaminhoMaterialChao = "Assets/Materials/Chao_Teste.mat";
        const string CaminhoMaterialBloco = "Assets/Materials/Bloco_Teste.mat";

        [MenuItem("ARS/Criar cena de teste", priority = 40)]
        public static void Criar()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

            var cena = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

            var materialChao = CriarMaterial(CaminhoMaterialChao, new Color(0.35f, 0.42f, 0.33f));
            var materialBloco = CriarMaterial(CaminhoMaterialBloco, new Color(0.55f, 0.5f, 0.45f));

            // Chão de 60 x 60 m
            var chao = GameObject.CreatePrimitive(PrimitiveType.Plane);
            chao.name = "Chao";
            chao.transform.localScale = new Vector3(6f, 1f, 6f);
            chao.GetComponent<Renderer>().sharedMaterial = materialChao;

            // Alguns blocos para dar noção de escala e de movimento
            var blocos = new GameObject("Blocos");
            Vector3[] posicoes =
            {
                new Vector3(6, 0.5f, 4), new Vector3(-7, 0.5f, 6), new Vector3(4, 0.5f, -8),
                new Vector3(-5, 0.5f, -5), new Vector3(10, 1f, -2), new Vector3(-11, 1f, -1)
            };
            foreach (var p in posicoes)
            {
                var bloco = GameObject.CreatePrimitive(PrimitiveType.Cube);
                bloco.transform.SetParent(blocos.transform);
                bloco.transform.position = p;
                bloco.transform.localScale = Vector3.one * (p.y * 2f);
                bloco.GetComponent<Renderer>().sharedMaterial = materialBloco;
            }

            // Heróis montados (ARS → Montar heróis); se não houver, um boneco provisório
            var prefabs = AssetDatabase.FindAssets("t:Prefab", new[] { ArsPaths.Prefabs })
                .Select(g => AssetDatabase.GUIDToAssetPath(g))
                .Select(c => AssetDatabase.LoadAssetAtPath<GameObject>(c))
                .Where(p => p != null)
                .ToList();

            GameObject controlado = null;
            for (int i = 0; i < prefabs.Count; i++)
            {
                var heroi = (GameObject)PrefabUtility.InstantiatePrefab(prefabs[i]);
                heroi.transform.position = new Vector3(i * 2f, 0f, 0f);
                if (i == 0) controlado = heroi;
                else if (heroi.TryGetComponent(out ComandosDoJogador comandos)) comandos.enabled = false;
            }

            if (controlado == null)
            {
                controlado = BonecoProvisorio();
                Debug.Log("[ARS] Nenhum herói montado ainda: usando um boneco provisório.");
            }

            // Bonecos de treino para testar o ataque básico (botão direito em cima deles)
            CriarBonecoDeTreino(new Vector3(4f, 0f, 5f));
            CriarBonecoDeTreino(new Vector3(-3f, 0f, 7f));

            // Câmera isométrica seguindo o herói controlado
            var camera = Camera.main;
            if (camera != null)
            {
                var iso = camera.gameObject.AddComponent<IsometricCamera>();
                iso.alvo = controlado.transform;
                iso.Encaixar();
                camera.backgroundColor = new Color(0.12f, 0.12f, 0.15f);
            }

            // Luz do sol vinda de cima, levemente inclinada
            var luz = Object.FindAnyObjectByType<Light>();
            if (luz != null) luz.transform.rotation = Quaternion.Euler(55f, -30f, 0f);

            ArsPaths.GarantirPasta(ArsPaths.Cenas);
            EditorSceneManager.SaveScene(cena, CaminhoCena);
            AdicionarNoBuild(CaminhoCena);
            Selection.activeGameObject = controlado;

            Debug.Log($"[ARS] Cena de teste salva em {CaminhoCena}. Aperte Play e use o botão direito do mouse para andar; a rodinha dá zoom.");
        }

        static GameObject BonecoProvisorio()
        {
            var boneco = new GameObject("Boneco provisório");
            var corpo = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            corpo.name = "Visual";
            corpo.transform.SetParent(boneco.transform);
            corpo.transform.localPosition = new Vector3(0f, 1f, 0f);
            Object.DestroyImmediate(corpo.GetComponent<Collider>());

            var nariz = GameObject.CreatePrimitive(PrimitiveType.Cube);
            nariz.name = "Frente";
            nariz.transform.SetParent(corpo.transform);
            nariz.transform.localPosition = new Vector3(0f, 0.4f, 0.5f);
            nariz.transform.localScale = new Vector3(0.3f, 0.15f, 0.3f);
            Object.DestroyImmediate(nariz.GetComponent<Collider>());

            var controle = boneco.AddComponent<CharacterController>();
            controle.height = 2f;
            controle.radius = 0.5f;
            controle.center = new Vector3(0f, 1f, 0f);
            boneco.AddComponent<Movimento>();
            boneco.AddComponent<Combatente>().equipe = Equipe.Jogadores;
            var ataque = boneco.AddComponent<AtaqueBasico>();
            ataque.definicao = HeroBuilder.DefinicaoDeAtaque("Provisorio");
            boneco.AddComponent<Recurso>();
            boneco.AddComponent<KitDeSkills>();
            boneco.AddComponent<ComandosDoJogador>();
            boneco.AddComponent<ARS.UI.HudProvisorio>();
            return boneco;
        }

        static void CriarBonecoDeTreino(Vector3 posicao)
        {
            var boneco = new GameObject("Boneco de treino");
            boneco.transform.position = posicao;
            var corpo = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            corpo.name = "Corpo";
            corpo.transform.SetParent(boneco.transform, false);
            corpo.transform.localPosition = new Vector3(0f, 1f, 0f);
            corpo.GetComponent<Renderer>().sharedMaterial = CriarMaterial("Assets/Materials/Boneco_Treino.mat", new Color(0.75f, 0.25f, 0.2f));

            var combatente = boneco.AddComponent<Combatente>();
            combatente.equipe = Equipe.Inimigos;
            combatente.vidaMaxima = 1000f;
            combatente.alturaDoCorpo = 1.1f;
            boneco.AddComponent<BonecoDeTreino>();
        }

        static Material CriarMaterial(string caminho, Color cor)
        {
            var material = AssetDatabase.LoadAssetAtPath<Material>(caminho);
            if (material != null) return material;
            var shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) shader = Shader.Find("Standard");
            material = new Material(shader);
            if (material.HasProperty("_BaseColor")) material.SetColor("_BaseColor", cor);
            if (material.HasProperty("_Color")) material.SetColor("_Color", cor);
            ArsPaths.GarantirPasta("Assets/Materials");
            AssetDatabase.CreateAsset(material, caminho);
            return material;
        }

        static void AdicionarNoBuild(string caminho)
        {
            var cenas = EditorBuildSettings.scenes.ToList();
            if (cenas.Any(c => c.path == caminho)) return;
            cenas.Insert(0, new EditorBuildSettingsScene(caminho, true));
            EditorBuildSettings.scenes = cenas.ToArray();
        }
    }
}
