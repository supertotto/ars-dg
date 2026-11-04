// claude: arquivo criado pelo Claude Code — tabela de ameaça (aggro) de um inimigo, estilo WoW
using System.Collections.Generic;
using UnityEngine;

namespace ARS.Combat
{
    /// <summary>
    /// Quanto cada atacante "irritou" este combatente. Dano recebido soma ameaça de quem causou.
    /// A IA ataca quem tiver mais ameaça. Números PROVISÓRIOS.
    /// </summary>
    [RequireComponent(typeof(Combatente))]
    public class Ameaca : MonoBehaviour
    {
        [Tooltip("Ameaça por ponto de dano recebido.")]
        public float ameacaPorDano = 1f;

        readonly Dictionary<Combatente, float> tabela = new Dictionary<Combatente, float>();
        Combatente eu;

        /// <summary>Enquanto true, não ganha ameaça (ex.: voltando para casa).</summary>
        public bool Ignorando { get; set; }

        void Awake()
        {
            eu = GetComponent<Combatente>();
            eu.AoReceberDano += (quantidade, _, origem) => Adicionar(origem, quantidade * ameacaPorDano);
        }

        public void Adicionar(Combatente quem, float quanto)
        {
            if (Ignorando || quem == null || quem == eu || !quem.Vivo) return;
            tabela.TryGetValue(quem, out float atual);
            tabela[quem] = atual + Mathf.Max(0f, quanto);
        }

        public float De(Combatente quem) => quem != null && tabela.TryGetValue(quem, out float v) ? v : 0f;

        public bool Vazia
        {
            get { LimparMortos(); return tabela.Count == 0; }
        }

        /// <summary>Quem tem mais ameaça (vivo e inimigo), ou null.</summary>
        public Combatente Maior()
        {
            LimparMortos();
            Combatente melhor = null;
            float maior = float.MinValue;
            foreach (var par in tabela)
            {
                if (eu != null && !eu.EhInimigoDe(par.Key)) continue;
                if (par.Value > maior) { maior = par.Value; melhor = par.Key; }
            }
            return melhor;
        }

        public void Limpar() => tabela.Clear();

        static readonly List<Combatente> remover = new List<Combatente>();
        void LimparMortos()
        {
            remover.Clear();
            foreach (var par in tabela) if (par.Key == null || !par.Key.Vivo) remover.Add(par.Key);
            foreach (var c in remover) tabela.Remove(c);
        }
    }
}
