// claude: arquivo criado pelo Claude Code — IA genérica de inimigo (aggro, perseguir, atacar, usar skills, voltar para casa)
using ARS.Characters;
using ARS.Combat;
using UnityEngine;

namespace ARS.AI
{
    /// <summary>
    /// Cérebro genérico de inimigo. Usa os mesmos componentes do herói (Movimento, AtaqueBasico,
    /// KitDeSkills), então controle, transformação e interrupção valem sem código extra.
    /// Ocioso até ver um inimigo (ou apanhar) → combate contra quem tem mais ameaça → se for
    /// puxado longe demais de casa ou ficar sem alvo, volta, se cura e esquece a briga.
    /// Números PROVISÓRIOS.
    /// </summary>
    [RequireComponent(typeof(Combatente), typeof(Ameaca))]
    public class CerebroDoInimigo : MonoBehaviour, ICerebro
    {
        [Tooltip("Distância em que percebe um herói e entra em combate.")]
        public float raioDeVisao = 8f;
        [Tooltip("Se ficar mais longe que isto do ponto onde nasceu, desiste e volta.")]
        public float distanciaMaximaDeCasa = 20f;
        [Tooltip("Segundos entre uma decisão e outra.")]
        public float intervaloDeDecisao = 0.25f;

        public enum Modo { Ocioso, Combate, Voltando }
        public Modo Estado { get; private set; } = Modo.Ocioso;
        public Combatente Alvo { get; private set; }

        public bool Ligado
        {
            get => enabled;
            set { enabled = value; if (!value) Parar(); }
        }

        Combatente eu;
        Ameaca ameaca;
        Movimento movimento;
        AtaqueBasico ataque;
        KitDeSkills kit;
        Vector3 casa;
        float proximaDecisao;

        void Awake()
        {
            eu = GetComponent<Combatente>();
            ameaca = GetComponent<Ameaca>();
            movimento = GetComponent<Movimento>();
            ataque = GetComponent<AtaqueBasico>();
            kit = GetComponent<KitDeSkills>();
        }

        void Start() => casa = transform.position;

        void Update()
        {
            if (!eu.Vivo || Time.time < proximaDecisao) return;
            proximaDecisao = Time.time + intervaloDeDecisao;

            switch (Estado)
            {
                case Modo.Ocioso: Ocioso(); break;
                case Modo.Combate: Combate(); break;
                case Modo.Voltando: Voltando(); break;
            }
        }

        void Ocioso()
        {
            if (ameaca.Vazia)
            {
                var visto = InimigoMaisPerto(raioDeVisao);
                if (visto == null) return;
                ameaca.Adicionar(visto, 1f); // percebeu: começa a briga com ameaça mínima
            }
            Estado = Modo.Combate;
        }

        void Combate()
        {
            if (DistanciaPlana(transform.position, casa) > distanciaMaximaDeCasa) { VoltarParaCasa(); return; }

            Alvo = ameaca.Maior();
            if (Alvo == null) { VoltarParaCasa(); return; }

            if (ataque != null && ataque.Alvo != Alvo) ataque.Atacar(Alvo);
            TentarSkills();
        }

        void Voltando()
        {
            if (DistanciaPlana(transform.position, casa) <= 0.6f)
            {
                eu.RestaurarVida();
                ameaca.Ignorando = false;
                Estado = Modo.Ocioso;
                return;
            }
            if (movimento != null && !movimento.EmMovimento) movimento.MoverPara(casa);
        }

        void VoltarParaCasa()
        {
            Parar();
            ameaca.Limpar();
            ameaca.Ignorando = true;
            Alvo = null;
            Estado = Modo.Voltando;
            if (movimento != null) movimento.MoverPara(casa);
        }

        void Parar()
        {
            if (ataque != null) ataque.Cancelar();
            if (movimento != null) movimento.Parar();
        }

        // usa a primeira skill pronta que alcança o alvo (Q, W, E, R)
        void TentarSkills()
        {
            if (kit == null || Alvo == null) return;
            foreach (Tecla tecla in System.Enum.GetValues(typeof(Tecla)))
            {
                var estado = kit.EstadoDe(tecla);
                if (estado == null || estado.skill == null || estado.cargas <= 0) continue;
                if (DistanciaPlana(transform.position, Alvo.transform.position) > estado.skill.alcance) continue;
                if (kit.Usar(tecla, Alvo, Alvo.transform.position)) return;
            }
        }

        Combatente InimigoMaisPerto(float raio)
        {
            Combatente melhor = null;
            float melhorDist = raio;
            foreach (var c in Combatente.Todos)
            {
                if (c == null || !c.Vivo || !eu.EhInimigoDe(c)) continue;
                float d = DistanciaPlana(transform.position, c.transform.position);
                if (d <= melhorDist) { melhorDist = d; melhor = c; }
            }
            return melhor;
        }

        static float DistanciaPlana(Vector3 a, Vector3 b)
        {
            a.y = 0f;
            b.y = 0f;
            return Vector3.Distance(a, b);
        }
    }
}
