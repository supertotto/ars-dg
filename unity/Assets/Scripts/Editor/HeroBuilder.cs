// claude: arquivo criado pelo Claude (app claude.ai)
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ARS.Characters;
using ARS.Combat;
using ARS.Data;
using ARS.UI;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace ARS.EditorTools
{
    /// <summary>
    /// Monta o prefab de cada herói a partir do modelo do Mixamo:
    /// material com a textura, Animator com parado/andar/correr e movimento por clique.
    /// Menu: ARS → Montar heróis.
    /// </summary>
    public static class HeroBuilder
    {
        // Velocidades (m/s) em que cada animação aparece no blend. Provisório.
        const float LimiarAndar = 1.6f;
        const float LimiarCorrer = 3.5f;

        static readonly string[] PalavrasParado = { "parado", "idle", "breathing" };
        static readonly string[] PalavrasAndar = { "andar", "walk" };
        static readonly string[] PalavrasCorrer = { "correr", "run", "jog", "sprint" };
        static readonly string[] PalavrasAtaque = { "atacar", "ataque", "attack", "cast", "spell" };
        static readonly string[] PalavrasExcluidas = { "back", "left", "right", "strafe", "turn", "tras", "esquerda", "direita", "start", "stop" };

        [MenuItem("ARS/Criar pastas do projeto", priority = 0)]
        public static void CriarPastas()
        {
            ArsPaths.GarantirPasta(ArsPaths.Personagens);
            ArsPaths.GarantirPasta(ArsPaths.AnimacoesComuns);
            ArsPaths.GarantirPasta(ArsPaths.Prefabs);
            ArsPaths.GarantirPasta(ArsPaths.Cenas);
            ArsPaths.GarantirPasta(ArsPaths.Dados + "/Heroes");
            ArsPaths.GarantirPasta(ArsPaths.Dados + "/Skills");

            foreach (var heroi in Directory.GetDirectories(ArsPaths.Personagens))
                foreach (var sub in ArsPaths.SubpastasDoHeroi)
                    ArsPaths.GarantirPasta(heroi.Replace('\\', '/') + "/" + sub);

            AssetDatabase.Refresh();
            Debug.Log("[ARS] Pastas prontas.");
        }

        [MenuItem("ARS/Novo herói (criar pastas)...", priority = 1)]
        public static void NovoHeroi()
        {
            NovoHeroiJanela.Abrir();
        }

        public static void CriarPastasDoHeroi(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome)) return;
            nome = nome.Trim();
            foreach (var sub in ArsPaths.SubpastasDoHeroi)
                ArsPaths.GarantirPasta($"{ArsPaths.Personagens}/{nome}/{sub}");
            AssetDatabase.Refresh();
            EditorUtility.RevealInFinder($"{ArsPaths.Personagens}/{nome}/Model");
            Debug.Log($"[ARS] Pastas do {nome} criadas. Coloque o FBX do Mixamo em {ArsPaths.Personagens}/{nome}/Model.");
        }

        [MenuItem("ARS/Montar heróis", priority = 20)]
        public static void MontarTodos()
        {
            if (!AssetDatabase.IsValidFolder(ArsPaths.Personagens))
            {
                EditorUtility.DisplayDialog("ARS", $"A pasta {ArsPaths.Personagens} não existe. Use ARS → Criar pastas do projeto.", "Ok");
                return;
            }

            int montados = 0;
            foreach (var pasta in Directory.GetDirectories(ArsPaths.Personagens))
            {
                string pastaHeroi = pasta.Replace('\\', '/');
                if (!AssetDatabase.IsValidFolder(pastaHeroi + "/Model")) continue;
                string modelo = AssetDatabase.FindAssets("t:GameObject", new[] { pastaHeroi + "/Model" })
                    .Select(g => AssetDatabase.GUIDToAssetPath(g))
                    .FirstOrDefault(p => p.ToLowerInvariant().EndsWith(".fbx"));

                if (modelo == null)
                {
                    Debug.LogWarning($"[ARS] {Path.GetFileName(pastaHeroi)}: nenhum FBX em {pastaHeroi}/Model.");
                    continue;
                }

                if (Montar(Path.GetFileName(pastaHeroi), pastaHeroi, modelo)) montados++;
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"[ARS] {montados} herói(s) montado(s) em {ArsPaths.Prefabs}.");
        }

        static bool Montar(string nome, string pastaHeroi, string caminhoModelo)
        {
            var modelo = AssetDatabase.LoadAssetAtPath<GameObject>(caminhoModelo);
            var avatar = AssetDatabase.LoadAllAssetsAtPath(caminhoModelo).OfType<Avatar>().FirstOrDefault();
            if (modelo == null || avatar == null || !avatar.isHuman)
            {
                Debug.LogError($"[ARS] {nome}: o modelo precisa estar com rig Humanoid. Selecione o FBX, aba Rig, Animation Type = Humanoid, e clique em Apply.");
                return false;
            }

            // Animações: primeiro as do próprio herói, depois as compartilhadas
            var clipes = new List<AnimationClip>();
            clipes.AddRange(ClipesEm(new[] { caminhoModelo }));
            clipes.AddRange(ClipesNaPasta(pastaHeroi + "/Animations"));
            clipes.AddRange(ClipesNaPasta(ArsPaths.Animacoes));

            var naoHumanoides = clipes.Where(c => !c.humanMotion).Select(c => c.name).Distinct().ToList();
            if (naoHumanoides.Count > 0)
                Debug.LogWarning($"[ARS] {nome}: {naoHumanoides.Count} animação(ões) não estão como Humanoid e foram ignoradas " +
                                 $"({string.Join(", ", naoHumanoides.Take(8))}). Use ARS → Reconfigurar FBX.");
            clipes = clipes.Where(c => c.humanMotion).ToList();
            Debug.Log($"[ARS] {nome}: {clipes.Count} animação(ões) humanoide(s) encontrada(s): {string.Join(", ", clipes.Select(c => c.name).Distinct().Take(15))}");

            var parado = Escolher(clipes, PalavrasParado);
            var andar = Escolher(clipes, PalavrasAndar);
            var correr = Escolher(clipes, PalavrasCorrer);
            var atacar = Escolher(clipes, PalavrasAtaque);

            if (parado == null) Debug.LogWarning($"[ARS] {nome}: nenhuma animação de parado (idle) encontrada.");
            if (andar == null) Debug.LogWarning($"[ARS] {nome}: nenhuma animação de andar (walk) encontrada.");

            var controlador = CriarAnimator($"{pastaHeroi}/{nome}.controller", parado, andar, correr, atacar);
            var material = CriarMaterial(nome, pastaHeroi);

            // Monta o objeto na cena temporariamente
            var raiz = new GameObject(nome);
            var visual = (GameObject)PrefabUtility.InstantiatePrefab(modelo);
            visual.name = "Visual";
            visual.transform.SetParent(raiz.transform, false);

            if (!visual.TryGetComponent(out Animator animator)) animator = visual.AddComponent<Animator>();
            animator.avatar = avatar;
            animator.runtimeAnimatorController = controlador;
            animator.applyRootMotion = false;
            animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;

            if (material != null)
                foreach (var r in visual.GetComponentsInChildren<Renderer>())
                    r.sharedMaterials = Enumerable.Repeat(material, r.sharedMaterials.Length).ToArray();

            var limites = CalcularLimites(visual);
            float altura = Mathf.Max(limites.size.y, 0.5f);
            if (altura > 10f || altura < 0.3f)
                Debug.LogWarning($"[ARS] {nome}: altura estranha ({altura:0.00} m). Confira a escala do FBX (Scale Factor na aba Model).");

            var controle = raiz.AddComponent<CharacterController>();
            controle.height = altura;
            controle.radius = Mathf.Clamp(Mathf.Max(limites.size.x, limites.size.z) * 0.25f, 0.25f, 0.6f);
            controle.center = new Vector3(0f, altura / 2f, 0f);
            raiz.AddComponent<Movimento>();
            var combatente = raiz.AddComponent<Combatente>();
            combatente.equipe = Equipe.Jogadores;
            combatente.alturaDoCorpo = altura * 0.6f;
            raiz.AddComponent<AtaqueBasico>().definicao = DefinicaoDeAtaque(nome);
            raiz.AddComponent<Recurso>(); // provisório: Mana para todos até existir a ficha do herói
            var kit = raiz.AddComponent<KitDeSkills>();
            AtribuirSkills(nome, kit);
            AtribuirPassiva(nome, raiz); // claude
            AtribuirCompanheiro(nome, raiz); // claude
            raiz.AddComponent<Abatido>().reviverEmSegundos = 5f; // claude: provisório, só para testes (sem sistema de reviver ainda)
            raiz.AddComponent<ComandosDoJogador>();
            // claude: HUD em uGUI no lugar do HudProvisorio; retrato em Assets/UI/Retratos/<Herói>.png
            var comHud = raiz.AddComponent<ComHud>();
            comHud.prefabDoHud = HudBuilder.ObterOuCriar();
            comHud.retrato = AssetDatabase.LoadAssetAtPath<Sprite>($"{ArsPaths.Ui}/Retratos/{nome}.png");

            ArsPaths.GarantirPasta(ArsPaths.Prefabs);
            string caminhoPrefab = $"{ArsPaths.Prefabs}/{nome}.prefab";
            PrefabUtility.SaveAsPrefabAsset(raiz, caminhoPrefab);
            Object.DestroyImmediate(raiz);

            Debug.Log($"[ARS] {nome}: prefab salvo em {caminhoPrefab} " +
                      $"(parado: {Nome(parado)}, andar: {Nome(andar)}, correr: {Nome(correr)}, atacar: {Nome(atacar)}, textura: {(material != null ? "sim" : "a do FBX")}).");
            return true;
        }

        static string Nome(AnimationClip c) => c != null ? c.name : "—";

        static IEnumerable<AnimationClip> ClipesNaPasta(string pasta)
        {
            if (!AssetDatabase.IsValidFolder(pasta)) return Enumerable.Empty<AnimationClip>();
            var caminhos = AssetDatabase.FindAssets("t:AnimationClip", new[] { pasta })
                .Select(g => AssetDatabase.GUIDToAssetPath(g)).Distinct();
            return ClipesEm(caminhos);
        }

        static IEnumerable<AnimationClip> ClipesEm(IEnumerable<string> caminhos) =>
            caminhos.SelectMany(c => AssetDatabase.LoadAllAssetsAtPath(c))
                .OfType<AnimationClip>()
                .Where(c => !c.name.StartsWith("__preview__"))
                // Ignora "animações" paradas de 1 quadro (a pose do modelo baixado sem animação)
                .Where(c => c.length >= 0.2f);

        static AnimationClip Escolher(List<AnimationClip> clipes, string[] palavras)
        {
            // A lista já vem na ordem de preferência (herói antes das compartilhadas)
            AnimationClip melhor = null;
            int melhorNota = int.MinValue;
            for (int i = 0; i < clipes.Count; i++)
            {
                string n = clipes[i].name.ToLowerInvariant();
                if (!palavras.Any(x => n.Contains(x))) continue;
                int nota = -i;
                if (PalavrasExcluidas.Any(x => n.Contains(x))) nota -= 1000;
                if (n.Contains("forward") || n.Contains("frente")) nota += 100;
                if (nota > melhorNota) { melhorNota = nota; melhor = clipes[i]; }
            }
            return melhor;
        }

        /// <summary>
        /// Liga as skills do herói pela pasta Assets/Data/Skills/&lt;Herói&gt;: o arquivo com "_Q_" no nome
        /// vai no Q, "_W_" no W, e assim por diante (ex.: Caleb_Q_MisseisMagicos).
        /// </summary>
        static void AtribuirSkills(string nome, KitDeSkills kit)
        {
            string pasta = $"{ArsPaths.Dados}/Skills/{nome}";
            if (!AssetDatabase.IsValidFolder(pasta)) return;
            var skills = AssetDatabase.FindAssets("t:SkillDef", new[] { pasta })
                .Select(g => AssetDatabase.GUIDToAssetPath(g))
                .Select(c => AssetDatabase.LoadAssetAtPath<SkillDef>(c))
                .Where(sk => sk != null)
                .ToList();
            SkillDef Achar(string tecla) => skills.FirstOrDefault(sk => sk.name.ToUpperInvariant().Contains($"_{tecla}_"));
            kit.q = Achar("Q");
            kit.w = Achar("W");
            kit.e = Achar("E");
            kit.r = Achar("R");
            Debug.Log($"[ARS] {nome}: skills Q={Nome(kit.q)}, W={Nome(kit.w)}, E={Nome(kit.e)}, R={Nome(kit.r)}");
        }

        static string Nome(SkillDef s) => s != null ? s.nome : "—";

        // claude: companheiro em Assets/Prefabs/Companheiros/<Herói>_<Nome>.prefab (ex.: Caleb_Cappuccino)
        static void AtribuirCompanheiro(string nome, GameObject raiz)
        {
            const string pasta = "Assets/Prefabs/Companheiros";
            if (!AssetDatabase.IsValidFolder(pasta)) return;
            var prefab = AssetDatabase.FindAssets("t:Prefab", new[] { pasta })
                .Select(g => AssetDatabase.GUIDToAssetPath(g))
                .Where(c => System.IO.Path.GetFileName(c).StartsWith(nome + "_"))
                .Select(c => AssetDatabase.LoadAssetAtPath<GameObject>(c))
                .FirstOrDefault(p => p != null && p.GetComponent<Companheiro>() != null);
            if (prefab == null) return;
            raiz.AddComponent<ComCompanheiro>().prefab = prefab;
            Debug.Log($"[ARS] {nome}: companheiro {prefab.name}");
        }

        // claude: a passiva fica na mesma pasta das skills, com "_P_" no nome (ex.: Caleb_P_AcertoEpico)
        static void AtribuirPassiva(string nome, GameObject raiz)
        {
            string pasta = $"{ArsPaths.Dados}/Skills/{nome}";
            if (!AssetDatabase.IsValidFolder(pasta)) return;
            var passiva = AssetDatabase.FindAssets("t:PassivaDef", new[] { pasta })
                .Select(g => AssetDatabase.LoadAssetAtPath<PassivaDef>(AssetDatabase.GUIDToAssetPath(g)))
                .FirstOrDefault(p => p != null && p.name.ToUpperInvariant().Contains("_P_"));
            if (passiva == null) return;
            raiz.AddComponent<Passiva>().definicao = passiva;
            Debug.Log($"[ARS] {nome}: passiva {passiva.nome}");
        }

        /// <summary>Ataque básico do herói em Assets/Data/Heroes (cria com valores provisórios se não existir).</summary>
        public static AtaqueBasicoDef DefinicaoDeAtaque(string nome)
        {
            string pasta = ArsPaths.Dados + "/Heroes";
            ArsPaths.GarantirPasta(pasta);
            string caminho = $"{pasta}/{nome}_AtaqueBasico.asset";
            var def = AssetDatabase.LoadAssetAtPath<AtaqueBasicoDef>(caminho);
            if (def == null)
            {
                def = ScriptableObject.CreateInstance<AtaqueBasicoDef>();
                AssetDatabase.CreateAsset(def, caminho);
                Debug.Log($"[ARS] {nome}: ataque básico criado com valores provisórios em {caminho} (ajuste lá).");
            }
            return def;
        }

        static AnimatorController CriarAnimator(string caminho, AnimationClip parado, AnimationClip andar, AnimationClip correr, AnimationClip atacar)
        {
            // Reaproveita o arquivo existente (mantém o mesmo GUID e as referências dos prefabs e cenas)
            var controlador = AssetDatabase.LoadAssetAtPath<AnimatorController>(caminho);
            if (controlador == null)
            {
                controlador = AnimatorController.CreateAnimatorControllerAtPath(caminho);
            }
            else
            {
                foreach (var p in controlador.parameters.ToArray()) controlador.RemoveParameter(p);
                if (controlador.layers.Length == 0) controlador.AddLayer("Base Layer");
                var antiga = controlador.layers[0].stateMachine;
                foreach (var t in antiga.anyStateTransitions.ToArray()) antiga.RemoveAnyStateTransition(t);
                foreach (var e in antiga.states.ToArray()) antiga.RemoveState(e.state);
                foreach (var arvoreVelha in AssetDatabase.LoadAllAssetsAtPath(caminho).OfType<BlendTree>())
                    Object.DestroyImmediate(arvoreVelha, true);
            }
            controlador.AddParameter("Velocidade", AnimatorControllerParameterType.Float);

            var estado = controlador.CreateBlendTreeInController("Locomocao", out BlendTree arvore);
            arvore.blendType = BlendTreeType.Simple1D;
            arvore.blendParameter = "Velocidade";
            arvore.useAutomaticThresholds = false;

            if (parado != null) arvore.AddChild(parado, 0f);
            if (andar != null) arvore.AddChild(andar, correr != null ? LimiarAndar : LimiarCorrer);
            if (correr != null) arvore.AddChild(correr, LimiarCorrer);

            var maquina = controlador.layers[0].stateMachine;
            maquina.defaultState = estado;

            // Ataque: dispara pelo gatilho "Atacar" e volta para a locomoção no fim
            controlador.AddParameter("Atacar", AnimatorControllerParameterType.Trigger);
            if (atacar != null)
            {
                var estadoAtaque = maquina.AddState("Atacar");
                estadoAtaque.motion = atacar;
                var entrada = maquina.AddAnyStateTransition(estadoAtaque);
                entrada.AddCondition(AnimatorConditionMode.If, 0f, "Atacar");
                entrada.hasExitTime = false;
                entrada.duration = 0.05f;
                entrada.canTransitionToSelf = true;
                var saida = estadoAtaque.AddTransition(estado);
                saida.hasExitTime = true;
                saida.exitTime = 0.9f;
                saida.duration = 0.1f;

                // Se começar a andar no meio do golpe, corta o ataque na hora (evita "deslizar")
                var aoAndar = estadoAtaque.AddTransition(estado);
                aoAndar.AddCondition(AnimatorConditionMode.Greater, 0.3f, "Velocidade");
                aoAndar.hasExitTime = false;
                aoAndar.duration = 0.1f;
                aoAndar.interruptionSource = TransitionInterruptionSource.None;
            }
            EditorUtility.SetDirty(controlador);
            AssetDatabase.SaveAssets();
            return controlador;
        }

        static Material CriarMaterial(string nome, string pastaHeroi)
        {
            var textura = AssetDatabase.FindAssets("t:Texture2D", new[] { pastaHeroi })
                .Select(g => AssetDatabase.GUIDToAssetPath(g))
                .OrderBy(p => p.Contains("/Textures/") ? 0 : 1)
                .Select(c => AssetDatabase.LoadAssetAtPath<Texture2D>(c))
                .FirstOrDefault(t => t != null);
            if (textura == null) return null;

            var shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) return null;

            string caminho = $"{pastaHeroi}/Materials/{nome}.mat";
            ArsPaths.GarantirPasta(pastaHeroi + "/Materials");
            var material = AssetDatabase.LoadAssetAtPath<Material>(caminho);
            if (material == null)
            {
                material = new Material(shader);
                AssetDatabase.CreateAsset(material, caminho);
            }
            material.shader = shader;
            material.SetTexture("_BaseMap", textura);
            material.mainTexture = textura;
            material.SetFloat("_Smoothness", 0.1f);
            EditorUtility.SetDirty(material);
            return material;
        }

        static Bounds CalcularLimites(GameObject objeto)
        {
            var renderizadores = objeto.GetComponentsInChildren<Renderer>();
            if (renderizadores.Length == 0) return new Bounds(objeto.transform.position, Vector3.one);
            var limites = renderizadores[0].bounds;
            foreach (var r in renderizadores.Skip(1)) limites.Encapsulate(r.bounds);
            return limites;
        }
    }
}
