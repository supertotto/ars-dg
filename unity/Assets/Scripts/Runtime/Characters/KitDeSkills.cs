// claude: arquivo criado pelo Claude (app claude.ai)
using ARS.Combat;
using ARS.Data;
using UnityEngine;

namespace ARS.Characters
{
    public enum Tecla { Q, W, E, R }

    /// <summary>
    /// As 4 skills do personagem (Q, W, E, R) com recarga, cargas e custo.
    /// As skills são dados (<see cref="SkillDef"/>); este componente só executa.
    /// </summary>
    [RequireComponent(typeof(Combatente))]
    public class KitDeSkills : MonoBehaviour
    {
        public SkillDef q, w, e, r;

        /// <summary>Estado de uma tecla: cargas disponíveis e quando volta a próxima.</summary>
        public class Estado
        {
            public SkillDef skill;
            public int cargas;
            public float proximaCargaEm;
        }

        readonly Estado[] estados = new Estado[4];
        Combatente eu;
        Recurso recurso;
        Movimento movimento; // claude: para virar o personagem para o alvo da skill
        Animator animator;
        TravaDeAcao trava; // claude: uma ação por vez (sem skill no meio do ataque básico)

        // claude: skill apertada enquanto ocupado; a última apertada vale
        class Guardada { public Tecla tecla; public Combatente alvoSobCursor; public Vector3 cursor; }
        Guardada guardada;

        /// <summary>Esquece a skill guardada (ex.: o jogador mandou andar).</summary>
        public void DescartarGuardada()
        {
            guardada = null;
            if (trava != null) trava.Reservado = false;
        }
        static readonly int ParametroAtacar = Animator.StringToHash("Atacar");

        public string UltimaMensagem { get; private set; }
        float mensagemAte;

        void Awake()
        {
            eu = GetComponent<Combatente>();
            recurso = GetComponent<Recurso>();
            movimento = GetComponent<Movimento>(); // claude
            animator = GetComponentInChildren<Animator>();
            trava = TravaDeAcao.De(gameObject); // claude
            eu.AoSerInterrompido += Interrompido; // claude
            var skills = new[] { q, w, e, r };
            for (int i = 0; i < 4; i++)
                estados[i] = new Estado { skill = skills[i], cargas = skills[i] != null ? skills[i].cargas : 0 };
        }

        public Estado EstadoDe(Tecla tecla) => estados[(int)tecla];

        /// <summary>Segundos até a próxima carga (0 = cheia).</summary>
        public float RecargaRestante(Tecla tecla)
        {
            var estado = estados[(int)tecla];
            if (estado == null || estado.skill == null || estado.cargas >= estado.skill.cargas) return 0f; // claude: nulo após recompilar no Play
            return Mathf.Max(0f, estado.proximaCargaEm - Time.time);
        }

        void Update()
        {
            foreach (var estado in estados)
            {
                // claude: estado nulo acontece ao recompilar com o Play ligado (o Unity não guarda esta lista)
                if (estado == null || estado.skill == null || estado.cargas >= estado.skill.cargas) continue;
                if (Time.time >= estado.proximaCargaEm)
                {
                    estado.cargas++;
                    if (estado.cargas < estado.skill.cargas)
                        estado.proximaCargaEm = Time.time + estado.skill.recarga;
                }
            }
            if (Time.time > mensagemAte) UltimaMensagem = null;

            // claude: trava liberou: solta a skill guardada (as checagens de alvo, alcance e custo valem de novo)
            if (guardada != null && !trava.Ocupado)
            {
                var g = guardada;
                DescartarGuardada();
                Usar(g.tecla, g.alvoSobCursor, g.cursor);
            }
        }

        /// <summary>Tenta usar a skill da tecla. "cursor" = ponto do chão sob o mouse.</summary>
        public bool Usar(Tecla tecla, Combatente alvoSobCursor, Vector3 cursor)
        {
            var estado = estados[(int)tecla];
            if (estado == null) return false; // claude: recompilado com o Play ligado; reinicie o Play
            var skill = estado.skill;
            if (skill == null) return Avisar($"{tecla}: sem skill");
            // claude: estados com a tag Sem Skill (ex.: transformado) bloqueiam skills
            if (eu.Estados.Tem(TagDeEstado.SemSkill)) return Avisar("Não pode usar skills agora");
            if (estado.cargas <= 0) return Avisar($"{skill.nome}: recarregando");
            // claude: modificador "Skill Sem Custo" (ex.: skill épica) zera o custo
            float custo = eu.Estados.TemModificador(TipoDeModificador.SkillSemCusto) ? 0f : skill.custo;
            if (recurso != null && !recurso.TemPelomenos(custo)) return Avisar($"Sem {recurso.tipo} suficiente");
            // claude: no meio do ataque básico (ou de outra skill) a skill fica guardada e sai quando a trava liberar
            if (trava.Ocupado)
            {
                guardada = new Guardada { tecla = tecla, alvoSobCursor = alvoSobCursor, cursor = cursor };
                trava.Reservado = true;
                return false;
            }

            Combatente alvo = null;
            if (skill.alvo == AlvoDaSkill.Proprio)
            {
                alvo = eu;
            }
            else
            {
                alvo = alvoSobCursor != null && eu.EhInimigoDe(alvoSobCursor) && alvoSobCursor.Vivo
                    ? alvoSobCursor
                    : InimigoMaisPerto(cursor, skill.raioDeBuscaNoCursor);
                if (alvo == null) return Avisar("Sem alvo");
                if (DistanciaPlana(transform.position, alvo.transform.position) > skill.alcance)
                    return Avisar("Fora de alcance");
            }

            // Pagou: gasta recurso e carga
            if (recurso != null) recurso.Gastar(custo);
            if (estado.cargas == skill.cargas) estado.proximaCargaEm = Time.time + skill.recarga;
            estado.cargas--;

            // claude: lê os modificadores (ex.: skill épica) e gasta os estados que acabam ao usar skill
            var potencia = new Potencia
            {
                numeros = eu.Estados.Multiplicador(TipoDeModificador.MultiplicarNumerosDaSkill),
                visual = eu.Estados.Multiplicador(TipoDeModificador.MultiplicarVisualDaSkill),
                extra = eu.Estados.PrimeiroModificador(TipoDeModificador.AdicionarVisualNaSkill) // claude: camada visual extra
            };
            eu.Estados.ConsumirAoUsarSkill();

            if (animator != null && animator.runtimeAnimatorController != null && animator.isActiveAndEnabled)
                animator.SetTrigger(ParametroAtacar);

            // claude: vira para o alvo ao conjurar (como o ataque básico)
            if (movimento != null && alvo != eu) movimento.OlharPara(alvo.transform.position);

            // claude: fica ocupado conjurando; a skill sai no fim da conjuração
            trava.Ocupar(skill.tempoDeConjuracao);
            if (skill.tempoDeConjuracao > 0f) conjuracao = StartCoroutine(ExecutarDepois(skill, alvo, skill.tempoDeConjuracao, potencia));
            else Executar(skill, alvo, potencia);
            return true;
        }

        // claude: quanto a skill foi potencializada por modificadores (1 = normal)
        struct Potencia { public float numeros; public float visual; public Modificador extra; }

        // claude: conjuração em andamento (para poder interromper)
        Coroutine conjuracao;

        // claude: interrompido: a skill em conjuração não sai (carga e custo já foram gastos) e a guardada é esquecida
        void Interrompido()
        {
            if (conjuracao != null) { StopCoroutine(conjuracao); conjuracao = null; trava.Liberar(); }
            DescartarGuardada();
        }

        // claude: espera a conjuração; se o alvo morrer nesse meio tempo, a skill não sai (carga e custo já foram gastos)
        System.Collections.IEnumerator ExecutarDepois(SkillDef skill, Combatente alvo, float segundos, Potencia potencia)
        {
            yield return new WaitForSeconds(segundos);
            conjuracao = null;
            if (alvo == null || !alvo.Vivo) yield break;
            Executar(skill, alvo, potencia);
        }

        void Executar(SkillDef skill, Combatente alvo, Potencia potencia)
        {
            // claude: números multiplicados (skill épica); 1 = os efeitos originais
            var efeitos = AplicadorDeEfeitos.Multiplicados(skill.efeitos, potencia.numeros);

            if (skill.entrega == EntregaDaSkill.Instantanea || alvo == eu)
            {
                AplicadorDeEfeitos.Aplicar(efeitos, alvo, eu);
                if (alvo != eu) eu.RegistrarAcerto(alvo, OrigemDoAcerto.Skill); // claude: 1 acerto por uso
                Projetil.CriarImpacto(skill.visualImpacto, alvo.Centro, transform.rotation, potencia.visual); // claude
                if (potencia.extra != null) Projetil.CriarImpacto(potencia.extra.visualNoImpacto, alvo.Centro, transform.rotation, potencia.visual);
                return;
            }

            Vector3 saida = PontoDeSaida(skill.pontoDeDisparo);
            // claude: se a skill tiver variações visuais, sorteia uma por disparo (só muda a aparência)
            var variacao = VisualDeProjetil.Sortear(skill.variacoesVisuais);
            Projetil.Disparar(saida, alvo, skill.velocidadeProjetil,
                              variacao != null ? variacao.cor : skill.corProjetil,
                              variacao != null ? variacao.tamanho : skill.tamanhoProjetil,
                              variacao != null ? variacao.rastro : skill.rastro,
                              variacao != null ? variacao.visual : skill.visualProjetil,
                              atingido =>
                              {
                                  AplicadorDeEfeitos.Aplicar(efeitos, atingido, eu);
                                  eu.RegistrarAcerto(atingido, OrigemDoAcerto.Skill); // claude: cada projétil = 1 acerto
                              },
                              potencia.visual,
                              variacao != null && variacao.impacto != null ? variacao.impacto : skill.visualImpacto, // claude
                              potencia.extra != null ? potencia.extra.visualNoProjetil : null,
                              potencia.extra != null ? potencia.extra.visualNoImpacto : null);
        }

        Vector3 PontoDeSaida(PontoDeDisparo ponto)
        {
            Transform osso = null;
            if (animator != null && animator.isHuman)
            {
                if (ponto == PontoDeDisparo.MaoDireita) osso = animator.GetBoneTransform(HumanBodyBones.RightHand);
                else if (ponto == PontoDeDisparo.MaoEsquerda) osso = animator.GetBoneTransform(HumanBodyBones.LeftHand);
            }
            return osso != null ? osso.position : transform.position + Vector3.up * 1.3f + transform.forward * 0.4f;
        }

        Combatente InimigoMaisPerto(Vector3 ponto, float raio)
        {
            Combatente melhor = null;
            float melhorDistancia = raio;
            foreach (var c in Combatente.Todos)
            {
                if (!c.Vivo || !eu.EhInimigoDe(c)) continue;
                float d = DistanciaPlana(ponto, c.transform.position);
                if (d <= melhorDistancia) { melhorDistancia = d; melhor = c; }
            }
            return melhor;
        }

        bool Avisar(string mensagem)
        {
            UltimaMensagem = mensagem;
            mensagemAte = Time.time + 1.2f;
            return false;
        }

        static float DistanciaPlana(Vector3 a, Vector3 b)
        {
            a.y = 0f;
            b.y = 0f;
            return Vector3.Distance(a, b);
        }
    }
}
