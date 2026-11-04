// claude: arquivo criado pelo Claude Code — inimigo como dado (ARS → Montar inimigos gera o prefab)
using System.Collections.Generic;
using UnityEngine;

namespace ARS.Data
{
    /// <summary>
    /// Definição de um inimigo (dados, não código). <b>ARS → Montar inimigos</b> gera o prefab em
    /// Assets/Prefabs/Inimigos. Todos os números são PROVISÓRIOS.
    /// </summary>
    [CreateAssetMenu(menuName = "ARS/Inimigo", fileName = "Inimigo")]
    public class InimigoDef : ScriptableObject
    {
        public string nome = "Novo inimigo";

        [Header("Visual")]
        [Tooltip("Prefab ou modelo com o visual (sem regra de jogo).")]
        public GameObject modelo;
        [Tooltip("Escala do modelo.")]
        public float escala = 1f;
        [Tooltip("Controlador de animação (parâmetros: Velocidade, Atacar, Morrer).")]
        public RuntimeAnimatorController animacoes;

        [Header("Corpo")]
        public float vidaMaxima = 300f;
        [Tooltip("Altura do centro do corpo (mira de projéteis e números).")]
        public float alturaDoCorpo = 0.9f;
        [Tooltip("Altura e raio da cápsula de colisão.")]
        public float altura = 1.6f;
        public float raio = 0.4f;
        public float velocidade = 3f;
        [Tooltip("Tags que ele sempre tem (ex.: chefes = Imune A Controle).")]
        public TagDeEstado tagsFixas;

        [Header("Combate")]
        public AtaqueBasicoDef ataqueBasico;
        [Tooltip("Skills na ordem Q, W, E, R (a IA usa a primeira pronta que alcança o alvo).")]
        public List<SkillDef> skills = new List<SkillDef>();

        [Header("IA")]
        public float raioDeVisao = 8f;
        public float distanciaMaximaDeCasa = 20f;
        [Tooltip("Segundos até o corpo sumir depois de morrer.")]
        public float sumirEmSegundos = 4f;
    }
}
