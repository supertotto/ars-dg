// claude: arquivo criado pelo Claude Code — gera os prefabs dos inimigos a partir dos dados (InimigoDef)
using System.Linq;
using ARS.AI;
using ARS.Characters;
using ARS.Combat;
using ARS.Data;
using ARS.UI;
using UnityEditor;
using UnityEngine;

namespace ARS.EditorTools
{
    /// <summary>
    /// ARS → Montar inimigos: para cada InimigoDef em Assets/Data/Inimigos, gera
    /// Assets/Prefabs/Inimigos/&lt;nome do arquivo&gt;.prefab com os componentes de combate e a IA.
    /// </summary>
    public static class InimigoBuilder
    {
        public const string PastaDados = ArsPaths.Dados + "/Inimigos";
        public const string PastaPrefabs = "Assets/Prefabs/Inimigos";

        [MenuItem("ARS/Montar inimigos", priority = 21)]
        public static void MontarTodos()
        {
            if (!AssetDatabase.IsValidFolder(PastaDados))
            {
                Debug.LogWarning($"[ARS] Nenhum inimigo: crie InimigoDef em {PastaDados}.");
                return;
            }
            ArsPaths.GarantirPasta(PastaPrefabs);
            var defs = AssetDatabase.FindAssets("t:InimigoDef", new[] { PastaDados })
                .Select(g => AssetDatabase.LoadAssetAtPath<InimigoDef>(AssetDatabase.GUIDToAssetPath(g)))
                .Where(d => d != null).ToList();
            foreach (var def in defs) Montar(def);
            Debug.Log($"[ARS] {defs.Count} inimigo(s) montado(s) em {PastaPrefabs}.");
        }

        public static GameObject Montar(InimigoDef def)
        {
            var raiz = new GameObject(def.name);

            if (def.modelo != null)
            {
                var modelo = (GameObject)PrefabUtility.InstantiatePrefab(def.modelo);
                modelo.name = "Modelo";
                modelo.transform.SetParent(raiz.transform, false);
                modelo.transform.localScale = Vector3.one * def.escala;
                var animator = modelo.GetComponentInChildren<Animator>();
                if (animator == null) animator = modelo.AddComponent<Animator>();
                if (def.animacoes != null) animator.runtimeAnimatorController = def.animacoes;
                animator.applyRootMotion = false;
                foreach (var c in modelo.GetComponentsInChildren<Collider>()) Object.DestroyImmediate(c);
            }

            var controle = raiz.AddComponent<CharacterController>();
            controle.height = def.altura;
            controle.radius = def.raio;
            controle.center = new Vector3(0f, def.altura / 2f, 0f);

            var combatente = raiz.AddComponent<Combatente>();
            combatente.equipe = Equipe.Inimigos;
            combatente.vidaMaxima = def.vidaMaxima;
            combatente.alturaDoCorpo = def.alturaDoCorpo;
            raiz.AddComponent<Estados>().tagsFixas = def.tagsFixas;

            raiz.AddComponent<Movimento>().velocidade = def.velocidade;
            raiz.AddComponent<AtaqueBasico>().definicao = def.ataqueBasico;
            var kit = raiz.AddComponent<KitDeSkills>();
            SkillDef Skill(int i) => i < def.skills.Count ? def.skills[i] : null;
            kit.q = Skill(0); kit.w = Skill(1); kit.e = Skill(2); kit.r = Skill(3);

            raiz.AddComponent<Ameaca>();
            var cerebro = raiz.AddComponent<CerebroDoInimigo>();
            cerebro.raioDeVisao = def.raioDeVisao;
            cerebro.distanciaMaximaDeCasa = def.distanciaMaximaDeCasa;
            raiz.AddComponent<Abatido>().sumirEmSegundos = def.sumirEmSegundos;
            raiz.AddComponent<PlacaDeVida>();

            string caminho = $"{PastaPrefabs}/{def.name}.prefab";
            var prefab = PrefabUtility.SaveAsPrefabAsset(raiz, caminho);
            Object.DestroyImmediate(raiz);
            return prefab;
        }
    }
}
