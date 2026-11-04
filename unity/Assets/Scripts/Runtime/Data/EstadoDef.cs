// claude: arquivo criado pelo Claude Code — definição de um estado (dados, não código)
using UnityEngine;

namespace ARS.Data
{
    /// <summary>
    /// Um estado que fica no personagem por um tempo (ex.: transformado em bichinho,
    /// acúmulos de uma passiva). As regras vêm das tags; os números são PROVISÓRIOS.
    /// </summary>
    [CreateAssetMenu(menuName = "ARS/Estado", fileName = "Estado")]
    public class EstadoDef : ScriptableObject
    {
        public string nome = "Novo estado";
        [TextArea] public string descricao;

        [Tooltip("O que o estado faz. As regras do jogo checam estas tags.")]
        public TagDeEstado tags;

        [Tooltip("Segundos que dura, se o efeito que aplica não disser outro valor. 0 = até ser removido.")]
        [Min(0f)] public float duracaoPadrao = 2f;
        [Tooltip("Quantas vezes acumula. Reaplicar soma um acúmulo (até o máximo) e renova a duração.")]
        [Min(1)] public int acumulosMaximos = 1;

        [Header("Visual (sem regra de jogo)")]
        // claude: lista; cada aplicação sorteia um (ex.: sapo ou galinha)
        [Tooltip("Troca o visual do personagem enquanto durar (ex.: o bichinho). Com mais de um, sorteia a cada aplicação. Vazio = mantém o visual.")]
        public GameObject[] visuaisSubstitutos = new GameObject[0];
        [Tooltip("Visual extra preso ao personagem enquanto durar (ex.: aura, fumaça).")]
        public GameObject visualExtra;
    }
}
