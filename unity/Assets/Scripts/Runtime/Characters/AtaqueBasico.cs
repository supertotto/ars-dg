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

        Combatente alvo;
        float proximoAtaque;
        float golpeSaiEm = -1f;

        public Combatente Alvo => alvo;

        void Awake()
        {
            movimento = GetComponent<Movimento>();
            eu = GetComponent<Combatente>();
            animator = GetComponentInChildren<Animator>();
            if (animator != null)
                foreach (var p in animator.parameters)
                    if (p.nameHash == ParametroAtacar) animatorTemAtaque = true;
        }

        public void Atacar(Combatente novoAlvo)
        {
            if (novoAlvo == null || !novoAlvo.Vivo || !eu.EhInimigoDe(novoAlvo)) return;
            alvo = novoAlvo;
        }

        public void Cancelar()
        {
            alvo = null;
            golpeSaiEm = -1f;
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

            if (Time.time >= proximoAtaque)
            {
                proximoAtaque = Time.time + definicao.intervalo;
                golpeSaiEm = Time.time + definicao.tempoDoGolpe;
                if (animatorTemAtaque) animator.SetTrigger(ParametroAtacar);
            }
        }

        void SoltarGolpe()
        {
            if (alvo == null || !alvo.Vivo) return;

            if (definicao.usaProjetil)
            {
                Vector3 saida = transform.position + Vector3.up * definicao.alturaDeDisparo + transform.forward * 0.4f;
                Projetil.Disparar(saida, alvo, eu, definicao.dano, definicao.tipo,
                                  definicao.velocidadeProjetil, definicao.corProjetil);
            }
            else
            {
                alvo.ReceberDano(definicao.dano, definicao.tipo, eu);
            }
        }

        static float DistanciaPlana(Vector3 a, Vector3 b)
        {
            a.y = 0f;
            b.y = 0f;
            return Vector3.Distance(a, b);
        }
    }
}
