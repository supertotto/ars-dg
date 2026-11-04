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

        // claude: cópia dos efeitos com os números multiplicados (dano, cura, duração); 1 = devolve os originais
        public static IList<Efeito> Multiplicados(IList<Efeito> efeitos, float multiplicador)
        {
            if (efeitos == null || Mathf.Approximately(multiplicador, 1f)) return efeitos;
            var copia = new List<Efeito>(efeitos.Count);
            foreach (var e in efeitos)
            {
                float duracao = e.duracao;
                // estado sem duração própria usa a padrão do estado: multiplica essa
                if (duracao <= 0f && e.tipo == TipoDeEfeito.AplicarEstado && e.estado != null && e.estado.duracaoPadrao > 0f)
                    duracao = e.estado.duracaoPadrao;
                copia.Add(new Efeito
                {
                    tipo = e.tipo,
                    valor = e.valor * multiplicador,
                    tipoDeDano = e.tipoDeDano,
                    duracao = duracao * multiplicador,
                    estado = e.estado
                });
            }
            return copia;
        }

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
                    // claude: estados (tags) e interrupção
                    case TipoDeEfeito.AplicarEstado:
                        if (efeito.estado == null) Debug.LogWarning("[ARS] Aplicar Estado sem estado definido.");
                        else alvo.Estados.Aplicar(efeito.estado, efeito.duracao, origem);
                        break;
                    case TipoDeEfeito.Interromper:
                        alvo.Interromper();
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
