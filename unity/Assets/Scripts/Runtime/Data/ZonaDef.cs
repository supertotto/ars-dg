// claude: arquivo criado pelo Claude Code — zona no chão que aplica efeitos por tique (ex.: enxame de abelhas)
using System.Collections.Generic;
using UnityEngine;

namespace ARS.Data
{
    /// <summary>
    /// Uma área que fica no chão por um tempo aplicando efeitos nos inimigos de quem a criou,
    /// a cada tique. Os tiques NÃO contam como acerto de skill (não alimentam passivas).
    /// Números PROVISÓRIOS.
    /// </summary>
    [CreateAssetMenu(menuName = "ARS/Zona", fileName = "Zona")]
    public class ZonaDef : ScriptableObject
    {
        public string nome = "Nova zona";
        [Tooltip("Raio em metros.")]
        public float raio = 2.5f;
        [Tooltip("Segundos que dura.")]
        public float duracao = 4f;
        [Tooltip("Segundos entre um tique e outro.")]
        public float intervalo = 0.5f;
        [Tooltip("Efeitos em cada inimigo dentro do raio, a cada tique.")]
        public List<Efeito> efeitosPorTique = new List<Efeito>();
        [Tooltip("Visual da zona enquanto dura (sem regra de jogo).")]
        public GameObject visual;
    }
}
