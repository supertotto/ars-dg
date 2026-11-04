// claude: arquivo criado pelo Claude (app claude.ai)
using System.Collections.Generic;
using ARS.Data;
using UnityEngine;

namespace ARS.Combat
{
    /// <summary>
    /// Aplica efeitos genéricos num alvo. É aqui que cada tipo de efeito vira jogo —
    /// nunca por nome de herói ou de skill.
    /// </summary>
    public static class AplicadorDeEfeitos
    {
        static readonly HashSet<TipoDeEfeito> avisados = new HashSet<TipoDeEfeito>();

        public static void Aplicar(IList<Efeito> efeitos, Combatente alvo, Combatente origem)
        {
            if (efeitos == null || alvo == null) return;
            foreach (var efeito in efeitos)
            {
                switch (efeito.tipo)
                {
                    case TipoDeEfeito.Dano:
                        alvo.ReceberDano(efeito.valor, efeito.tipoDeDano, origem);
                        break;
                    case TipoDeEfeito.Cura:
                        alvo.Curar(efeito.valor);
                        break;
                    default:
                        if (avisados.Add(efeito.tipo))
                            Debug.LogWarning($"[ARS] O efeito \"{efeito.tipo}\" ainda não foi implementado (vai ser ignorado).");
                        break;
                }
            }
        }
    }
}
