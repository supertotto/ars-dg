// claude: arquivo criado pelo Claude Code — estados ativos de um combatente (tags, duração, acúmulos)
using System;
using System.Collections.Generic;
using ARS.Data;
using UnityEngine;

namespace ARS.Combat
{
    /// <summary>
    /// Estados ativos de um combatente. Responde "tem a tag X?" para as regras do jogo
    /// e cuida de duração, acúmulos e imunidade a controle.
    /// </summary>
    [RequireComponent(typeof(Combatente))]
    public class Estados : MonoBehaviour
    {
        [Tooltip("Tags que o personagem sempre tem (ex.: chefes = Imune A Controle).")]
        public TagDeEstado tagsFixas;

        public class Ativo
        {
            public EstadoDef def;
            public float fim;          // Time.time em que acaba (infinito = até ser removido)
            public int acumulos;
            public Combatente origem;
            public Vector3? ponto;     // claude: ponto guardado (ex.: de onde partiu, para voltar)
            public float Restante => float.IsPositiveInfinity(fim) ? float.PositiveInfinity : Mathf.Max(0f, fim - Time.time);
        }

        readonly List<Ativo> ativos = new List<Ativo>();
        TagDeEstado tagsAtivas;

        public IReadOnlyList<Ativo> Ativos => ativos;
        public event Action<Ativo> AoAplicar;
        public event Action<Ativo> AoRemover;

        /// <summary>Avisado quando um Estados é criado (o visual dos estados se registra aqui).</summary>
        public static event Action<Estados> AoCriar;

        void Awake()
        {
            var combatente = GetComponent<Combatente>();
            combatente.AoMorrer += _ => RemoverTodos();
            AoCriar?.Invoke(this);
        }

        public static Estados De(Combatente combatente)
        {
            if (combatente == null) return null;
            var estados = combatente.GetComponent<Estados>();
            return estados != null ? estados : combatente.gameObject.AddComponent<Estados>();
        }

        /// <summary>true se tem todas as tags pedidas (somando as fixas).</summary>
        public bool Tem(TagDeEstado tags) => (tagsAtivas | tagsFixas).Contem(tags);

        // claude: modificadores dos estados ativos
        /// <summary>Produto dos valores de um tipo de modificador (1 = nenhum).</summary>
        public float Multiplicador(TipoDeModificador tipo)
        {
            float total = 1f;
            foreach (var a in ativos)
                foreach (var m in a.def.modificadores)
                    if (m.tipo == tipo) total *= m.valor;
            return total;
        }

        // claude: primeiro modificador ativo de um tipo (ex.: o visual extra da skill épica), ou null
        public Modificador PrimeiroModificador(TipoDeModificador tipo)
        {
            foreach (var a in ativos)
                foreach (var m in a.def.modificadores)
                    if (m.tipo == tipo) return m;
            return null;
        }

        public bool TemModificador(TipoDeModificador tipo)
        {
            foreach (var a in ativos)
                foreach (var m in a.def.modificadores)
                    if (m.tipo == tipo) return true;
            return false;
        }

        /// <summary>Tira os estados que acabam ao usar uma skill (ex.: "próxima skill épica").</summary>
        public void ConsumirAoUsarSkill()
        {
            for (int i = ativos.Count - 1; i >= 0; i--)
                if (ativos[i].def.consumidoAoUsarSkill) RemoverAtivo(ativos[i]);
        }

        public bool EstaAtivo(EstadoDef def) => Achar(def) != null;

        // claude: ponto guardado por um estado ativo, ou null
        public Vector3? PontoDe(EstadoDef def)
        {
            var ativo = def != null ? Achar(def) : null;
            return ativo != null ? ativo.ponto : null;
        }

        public int Acumulos(EstadoDef def)
        {
            var ativo = Achar(def);
            return ativo != null ? ativo.acumulos : 0;
        }

        /// <summary>
        /// Aplica (ou reaplica) um estado. Estados com a tag Controle não pegam em quem é imune.
        /// "duracao" &lt;= 0 usa a duração padrão do estado; padrão 0 = até ser removido.
        /// Devolve false se não pegou.
        /// </summary>
        public bool Aplicar(EstadoDef def, float duracao, Combatente origem) => Aplicar(def, duracao, origem, null);

        // claude: versão que guarda um ponto no estado (ex.: janela de volta da E do Caleb)
        public bool Aplicar(EstadoDef def, float duracao, Combatente origem, Vector3? ponto)
        {
            if (def == null) return false;
            if (def.tags.Contem(TagDeEstado.Controle) && Tem(TagDeEstado.ImuneAControle)) return false;

            float segundos = duracao > 0f ? duracao : def.duracaoPadrao;
            float fim = segundos > 0f ? Time.time + segundos : float.PositiveInfinity;

            var ativo = Achar(def);
            if (ativo != null)
            {
                ativo.acumulos = Mathf.Min(def.acumulosMaximos, ativo.acumulos + 1);
                ativo.fim = fim;
                ativo.origem = origem;
                if (ponto.HasValue) ativo.ponto = ponto;
                return true;
            }

            ativo = new Ativo { def = def, fim = fim, acumulos = 1, origem = origem, ponto = ponto };
            ativos.Add(ativo);
            RecalcularTags();
            AoAplicar?.Invoke(ativo);
            return true;
        }

        public void Remover(EstadoDef def)
        {
            var ativo = Achar(def);
            if (ativo != null) RemoverAtivo(ativo);
        }

        /// <summary>Tira todos os estados que tenham alguma das tags pedidas.</summary>
        public void RemoverComTag(TagDeEstado tags)
        {
            for (int i = ativos.Count - 1; i >= 0; i--)
                if ((ativos[i].def.tags & tags) != 0) RemoverAtivo(ativos[i]);
        }

        public void RemoverTodos()
        {
            for (int i = ativos.Count - 1; i >= 0; i--) RemoverAtivo(ativos[i]);
        }

        void Update()
        {
            for (int i = ativos.Count - 1; i >= 0; i--)
                if (Time.time >= ativos[i].fim) RemoverAtivo(ativos[i]);
        }

        Ativo Achar(EstadoDef def)
        {
            foreach (var a in ativos) if (a.def == def) return a;
            return null;
        }

        void RemoverAtivo(Ativo ativo)
        {
            if (!ativos.Remove(ativo)) return;
            RecalcularTags();
            AoRemover?.Invoke(ativo);
            // claude: efeitos ao terminar, no dono, com o ponto guardado pelo estado
            if (ativo.def.efeitosAoTerminar != null && ativo.def.efeitosAoTerminar.Count > 0)
            {
                var dono = GetComponent<Combatente>();
                Vector3 p = ativo.ponto ?? transform.position;
                AplicadorDeEfeitos.Aplicar(ativo.def.efeitosAoTerminar, dono,
                    new ContextoDoEfeito { origem = ativo.origem != null ? ativo.origem : dono, pontoMirado = p, pontoDePartida = p });
            }
        }

        void RecalcularTags()
        {
            tagsAtivas = TagDeEstado.Nenhuma;
            foreach (var a in ativos) tagsAtivas |= a.def.tags;
        }
    }
}
