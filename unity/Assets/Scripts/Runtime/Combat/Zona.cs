// claude: arquivo criado pelo Claude Code — zona no chão que aplica efeitos por tique nos inimigos
using System.Collections.Generic;
using ARS.Data;
using UnityEngine;

namespace ARS.Combat
{
    /// <summary>
    /// Zona ativa no chão (ex.: enxame de abelhas). A cada tique aplica os efeitos da
    /// <see cref="ZonaDef"/> nos inimigos de quem a criou dentro do raio. Não registra acerto
    /// (não alimenta passivas): quem conta é a skill que criou a zona.
    /// </summary>
    public class Zona : MonoBehaviour
    {
        ZonaDef def;
        Combatente origem;
        float multiplicador = 1f;
        float fim;
        float proximoTique;
        IList<Efeito> efeitosDoTique;

        /// <summary>Cria a zona. "multiplicador" escala os números dos tiques; "duracao" &gt; 0 troca a duração da zona.</summary>
        public static Zona Criar(ZonaDef def, Vector3 ponto, Combatente origem, float multiplicador = 1f, float duracao = 0f)
        {
            var objeto = new GameObject("Zona " + def.nome);
            objeto.transform.position = ponto;
            var zona = objeto.AddComponent<Zona>();
            zona.def = def;
            zona.origem = origem;
            zona.multiplicador = multiplicador;
            zona.fim = Time.time + (duracao > 0f ? duracao : def.duracao);
            zona.proximoTique = Time.time + def.intervalo;
            zona.efeitosDoTique = AplicadorDeEfeitos.Multiplicados(def.efeitosPorTique, multiplicador);
            if (def.visual != null)
            {
                var visual = Instantiate(def.visual, ponto, Quaternion.identity, objeto.transform);
                // o visual é feito para o raio da zona; épico/multiplicado não muda o tamanho aqui
                visual.name = "Visual";
            }
            return zona;
        }

        public float Raio => def != null ? def.raio : 0f;

        void Update()
        {
            if (def == null || Time.time >= fim)
            {
                Destroy(gameObject);
                return;
            }
            if (Time.time < proximoTique) return;
            proximoTique += Mathf.Max(0.05f, def.intervalo);

            var contexto = new ContextoDoEfeito { origem = origem, pontoMirado = transform.position, pontoDePartida = transform.position };
            var lista = Combatente.Todos;
            for (int i = lista.Count - 1; i >= 0; i--)
            {
                var alvo = lista[i];
                if (alvo == null || !alvo.Vivo) continue;
                if (origem != null && !origem.EhInimigoDe(alvo)) continue;
                Vector3 d = alvo.transform.position - transform.position;
                d.y = 0f;
                if (d.sqrMagnitude > def.raio * def.raio) continue;
                AplicadorDeEfeitos.Aplicar(efeitosDoTique, alvo, contexto);
            }
        }
    }
}
