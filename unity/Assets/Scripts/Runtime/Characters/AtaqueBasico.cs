// claude: arquivo criado pelo Claude (app claude.ai)
using ARS.Combat;
using ARS.Data;
using UnityEngine;

namespace ARS.Characters
{
    /// <summary>
    /// Ataque básico automático, estilo LoL: com um alvo definido, o personagem anda até
    /// entrar no alcance e segue atacando até receber outra ordem ou o alvo morrer.
    /// Os números vêm de um <see cref="AtaqueBasicoDef"/> (dados).
    /// </summary>
    [RequireComponent(typeof(Movimento), typeof(Combatente))]
    public class AtaqueBasico : MonoBehaviour
    {
        public AtaqueBasicoDef definicao;

        static readonly int ParametroAtacar = Animator.StringToHash("Atacar");

        Movimento movimento;
        Combatente eu;
        Animator animator;
        bool animatorTemAtaque;
        TravaDeAcao trava; // claude: uma ação por vez (não ataca no meio de uma skill)

        Combatente alvo;
        float proximoAtaque;
        float golpeSaiEm = -1f;

        public Combatente Alvo => alvo;

        void Awake()
        {
            movimento = GetComponent<Movimento>();
            eu = GetComponent<Combatente>();
            animator = GetComponentInChildren<Animator>();
            trava = TravaDeAcao.De(gameObject); // claude
            eu.AoSerInterrompido += CancelarGolpe; // claude: interrompido no balanço = golpe não sai
            if (animator != null && animator.runtimeAnimatorController != null)
                foreach (var p in animator.parameters)
                    if (p.nameHash == ParametroAtacar) animatorTemAtaque = true;
        }

        public void Atacar(Combatente novoAlvo)
        {
            if (novoAlvo == null || !novoAlvo.Vivo || !eu.EhInimigoDe(novoAlvo)) return;
            alvo = novoAlvo;
        }

        // claude: corta só o golpe em andamento; o alvo continua (volta a atacar quando puder)
        void CancelarGolpe()
        {
            if (golpeSaiEm >= 0f) trava.Liberar();
            golpeSaiEm = -1f;
        }

        public void Cancelar()
        {
            alvo = null;
            // claude: cancelar um golpe no balanço libera a trava para a skill
            if (golpeSaiEm >= 0f && trava != null) trava.Liberar();
            golpeSaiEm = -1f;
            if (animatorTemAtaque && animator.isActiveAndEnabled) animator.ResetTrigger(ParametroAtacar);
        }

        void Update()
        {
            if (definicao == null) return;

            // Golpe em andamento: espera o "balanço" e solta o dano
            if (golpeSaiEm >= 0f)
            {
                if (Time.time >= golpeSaiEm)
                {
                    golpeSaiEm = -1f;
                    SoltarGolpe();
                }
                return;
            }

            if (alvo == null) return;
            if (!alvo.Vivo) { Cancelar(); return; }

            float distancia = DistanciaPlana(transform.position, alvo.transform.position);
            if (distancia > definicao.alcance)
            {
                // Persegue até entrar no alcance
                movimento.MoverPara(alvo.transform.position, definicao.alcance * 0.9f);
                return;
            }

            movimento.Parar();
            movimento.OlharPara(alvo.transform.position);

            // claude: no meio de uma skill, espera; o ataque volta sozinho quando ela termina
            // claude: estados com a tag Sem Ataque (ex.: transformado) impedem o ataque básico
            if (Time.time >= proximoAtaque && !trava.Ocupado && !trava.Reservado && !eu.Estados.Tem(TagDeEstado.SemAtaque))
            {
                proximoAtaque = Time.time + definicao.intervalo;
                golpeSaiEm = Time.time + definicao.tempoDoGolpe;
                trava.Ocupar(definicao.tempoDoGolpe);
                if (animatorTemAtaque && animator.isActiveAndEnabled) animator.SetTrigger(ParametroAtacar);
            }
        }

        void SoltarGolpe()
        {
            if (alvo == null || !alvo.Vivo) return;

            if (definicao.usaProjetil)
            {
                Vector3 saida = PontoDeSaida();
                Projetil.Disparar(saida, alvo, eu, definicao.dano, definicao.tipo,
                                  definicao.velocidadeProjetil, definicao.corProjetil,
                                  definicao.tamanhoProjetil, definicao.rastro, definicao.visualProjetil);
            }
            else
            {
                alvo.ReceberDano(definicao.dano, definicao.tipo, eu);
            }
        }

        Vector3 PontoDeSaida()
        {
            Transform osso = null;
            if (animator != null && animator.isHuman)
            {
                if (definicao.pontoDeDisparo == PontoDeDisparo.MaoDireita)
                    osso = animator.GetBoneTransform(HumanBodyBones.RightHand);
                else if (definicao.pontoDeDisparo == PontoDeDisparo.MaoEsquerda)
                    osso = animator.GetBoneTransform(HumanBodyBones.LeftHand);
            }
            if (osso != null) return osso.position + transform.forward * 0.15f;
            return transform.position + Vector3.up * definicao.alturaDeDisparo + transform.forward * 0.4f;
        }

        static float DistanciaPlana(Vector3 a, Vector3 b)
        {
            a.y = 0f;
            b.y = 0f;
            return Vector3.Distance(a, b);
        }
    }
}
