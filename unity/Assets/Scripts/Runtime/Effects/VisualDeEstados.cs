// claude: arquivo criado pelo Claude Code — mostra o visual dos estados (sem regra de jogo)
using System.Collections.Generic;
using ARS.Combat;
using UnityEngine;

namespace ARS.Effects
{
    /// <summary>
    /// Escuta os estados de um combatente e cuida só da aparência: troca o visual
    /// (ex.: virou bichinho) e prende visuais extras enquanto o estado durar.
    /// É colocado sozinho em todo combatente que tiver estados.
    /// </summary>
    public class VisualDeEstados : MonoBehaviour
    {
        Estados estados;
        readonly Dictionary<Estados.Ativo, List<GameObject>> criados = new Dictionary<Estados.Ativo, List<GameObject>>();
        readonly List<Renderer> escondidos = new List<Renderer>();
        int substituicoes;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Registrar()
        {
            Estados.AoCriar -= Anexar;
            Estados.AoCriar += Anexar;
        }

        static void Anexar(Estados estados)
        {
            if (estados.GetComponent<VisualDeEstados>() == null)
                estados.gameObject.AddComponent<VisualDeEstados>().Ligar(estados);
        }

        void Ligar(Estados alvo)
        {
            estados = alvo;
            estados.AoAplicar += Aplicado;
            estados.AoRemover += Removido;
        }

        void OnDestroy()
        {
            if (estados == null) return;
            estados.AoAplicar -= Aplicado;
            estados.AoRemover -= Removido;
        }

        void Aplicado(Estados.Ativo ativo)
        {
            var lista = new List<GameObject>();
            var def = ativo.def;

            if (def.visualSubstituto != null)
            {
                if (substituicoes++ == 0) EsconderVisualProprio();
                lista.Add(Instantiate(def.visualSubstituto, transform.position, transform.rotation, transform));
            }
            if (def.visualExtra != null)
                lista.Add(Instantiate(def.visualExtra, transform.position, transform.rotation, transform));

            if (lista.Count > 0) criados[ativo] = lista;
        }

        void Removido(Estados.Ativo ativo)
        {
            if (criados.TryGetValue(ativo, out var lista))
            {
                foreach (var objeto in lista) if (objeto != null) Destroy(objeto);
                criados.Remove(ativo);
            }
            if (ativo.def.visualSubstituto != null && --substituicoes == 0) MostrarVisualProprio();
        }

        // Esconde os renderers do próprio personagem (não os de visuais criados aqui)
        void EsconderVisualProprio()
        {
            escondidos.Clear();
            foreach (var r in GetComponentsInChildren<Renderer>())
            {
                // mantém partículas e textos (ex.: placa de vida)
                if (!r.enabled || r is ParticleSystemRenderer || r.GetComponent<TextMesh>() != null) continue;
                r.enabled = false;
                escondidos.Add(r);
            }
        }

        void MostrarVisualProprio()
        {
            foreach (var r in escondidos) if (r != null) r.enabled = true;
            escondidos.Clear();
        }
    }
}
