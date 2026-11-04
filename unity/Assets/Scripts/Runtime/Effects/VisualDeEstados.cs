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
        readonly HashSet<Estados.Ativo> substituidos = new HashSet<Estados.Ativo>(); // claude

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

            var substituto = Sortear(def.visuaisSubstitutos);
            if (substituto != null)
            {
                if (substituicoes++ == 0) EsconderVisualProprio();
                lista.Add(Instantiate(substituto, transform.position, transform.rotation, transform));
                substituidos.Add(ativo);
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
            if (substituidos.Remove(ativo) && --substituicoes == 0) MostrarVisualProprio();
        }

        static GameObject Sortear(GameObject[] opcoes)
        {
            if (opcoes == null || opcoes.Length == 0) return null;
            int validos = 0;
            foreach (var o in opcoes) if (o != null) validos++;
            if (validos == 0) return null;
            int escolha = Random.Range(0, validos);
            foreach (var o in opcoes) if (o != null && escolha-- == 0) return o;
            return null;
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
