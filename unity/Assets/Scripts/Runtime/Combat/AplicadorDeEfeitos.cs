// claude: arquivo criado pelo Claude (app claude.ai)
using System.Collections.Generic;
using ARS.Data;
using UnityEngine;

namespace ARS.Combat
{
    // claude: onde a skill mirou e de onde o lançador saiu (efeitos de posição, zonas, pontos guardados)
    public struct ContextoDoEfeito
    {
        public Combatente origem;
        public Vector3 pontoMirado;
        public Vector3 pontoDePartida;
    }

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
                // claude: zona sem duração própria usa a da zona: multiplica essa
                if (duracao <= 0f && e.tipo == TipoDeEfeito.CriarZona && e.zona != null)
                    duracao = e.zona.duracao;
                copia.Add(new Efeito
                {
                    tipo = e.tipo,
                    valor = e.valor * multiplicador,
                    tipoDeDano = e.tipoDeDano,
                    duracao = duracao * multiplicador,
                    estado = e.estado,
                    destino = e.destino, // claude
                    zona = e.zona
                });
            }
            return copia;
        }

        public static void Aplicar(IList<Efeito> efeitos, Combatente alvo, Combatente origem)
        {
            // claude: sem ponto mirado, usa a posição do alvo
            Vector3 p = alvo != null ? alvo.transform.position : Vector3.zero;
            Aplicar(efeitos, alvo, new ContextoDoEfeito { origem = origem, pontoMirado = p, pontoDePartida = p });
        }

        // claude: versão com contexto (onde a skill mirou e de onde o lançador saiu)
        public static void Aplicar(IList<Efeito> efeitos, Combatente alvo, ContextoDoEfeito contexto)
        {
            var origem = contexto.origem;
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
                        else alvo.Estados.Aplicar(efeito.estado, efeito.duracao, origem, contexto.pontoDePartida);
                        break;
                    case TipoDeEfeito.Interromper:
                        alvo.Interromper();
                        break;
                    // claude: movimento, zona e companheiro
                    case TipoDeEfeito.Teleportar:
                    {
                        Vector3? destino = efeito.destino == DestinoDoEfeito.PontoMirado ? contexto.pontoMirado
                                         : efeito.destino == DestinoDoEfeito.PontoDePartida ? contexto.pontoDePartida
                                         : alvo.Estados.PontoDe(efeito.estado);
                        if (destino.HasValue) alvo.Teleportar(destino.Value);
                        break;
                    }
                    case TipoDeEfeito.CriarZona:
                        if (efeito.zona == null) Debug.LogWarning("[ARS] Criar Zona sem zona definida.");
                        else Zona.Criar(efeito.zona, contexto.pontoMirado, origem != null ? origem : alvo,
                                        efeito.valor > 0f ? efeito.valor : 1f, efeito.duracao);
                        break;
                    case TipoDeEfeito.CompanheiroFica:
                        alvo.MandarCompanheiroFicar(contexto.pontoDePartida);
                        break;
                    case TipoDeEfeito.CompanheiroSegue:
                        alvo.MandarCompanheiroSeguir();
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
