// claude: arquivo criado pelo Claude Code — variação visual de um projétil, sorteada a cada disparo
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ARS.Data
{
    /// <summary>
    /// Uma aparência possível do projétil. Só muda o visual: alvo, velocidade e efeitos
    /// continuam os da skill. Os pesos são PROVISÓRIOS.
    /// </summary>
    [Serializable]
    public class VisualDeProjetil
    {
        [Tooltip("Nome só para organizar no Inspector (ex.: Sapinho).")]
        public string nome = "Variação";
        [Tooltip("Prefab do visual. Vazio = bolinha simples na cor abaixo.")]
        public GameObject visual;
        public Color cor = Color.white;
        public float tamanho = 0.3f;
        public bool rastro = true;
        // claude: efeito no ponto do acerto (vazio = usa o impacto da skill)
        [Tooltip("Efeito que aparece onde o projétil acerta (ex.: puf de fumaça). Vazio = o impacto da skill.")]
        public GameObject impacto;
        [Tooltip("Chance relativa de sair esta variação (2 = o dobro de uma com peso 1).")]
        [Min(0f)] public float peso = 1f;

        /// <summary>Sorteia uma variação pelo peso. Devolve null se a lista estiver vazia ou sem peso.</summary>
        public static VisualDeProjetil Sortear(IReadOnlyList<VisualDeProjetil> opcoes)
        {
            if (opcoes == null || opcoes.Count == 0) return null;
            float total = 0f;
            foreach (var o in opcoes) if (o != null) total += Mathf.Max(0f, o.peso);
            if (total <= 0f) return null;

            float sorteio = UnityEngine.Random.value * total;
            foreach (var o in opcoes)
            {
                if (o == null) continue;
                sorteio -= Mathf.Max(0f, o.peso);
                if (sorteio <= 0f) return o;
            }
            return opcoes[opcoes.Count - 1];
        }
    }
}
