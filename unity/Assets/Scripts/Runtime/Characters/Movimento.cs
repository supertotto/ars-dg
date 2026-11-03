using UnityEngine;

namespace ARS.Characters
{
    /// <summary>
    /// Move o personagem até um ponto no chão (sem ler o mouse: quem dá as ordens é
    /// <see cref="ComandosDoJogador"/> ou a IA). Atualiza o parâmetro "Velocidade" do Animator.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class Movimento : MonoBehaviour
    {
        [Tooltip("Velocidade de movimento em metros por segundo (provisório).")]
        public float velocidade = 3.5f;
        [Tooltip("Velocidade de giro em graus por segundo.")]
        public float velocidadeGiro = 720f;
        [Tooltip("Distância em que considera que chegou.")]
        public float distanciaParada = 0.15f;

        static readonly int ParametroVelocidade = Animator.StringToHash("Velocidade");

        CharacterController controle;
        Animator animator;
        Vector3? destino;
        float paradaAtual;
        float velocidadeVertical;

        public bool EmMovimento => destino.HasValue;

        void Awake()
        {
            controle = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();
        }

        /// <summary>Anda até o ponto. "parada" = a que distância pode parar (ex.: alcance do ataque).</summary>
        public void MoverPara(Vector3 ponto, float parada = -1f)
        {
            destino = ponto;
            paradaAtual = parada >= 0f ? parada : distanciaParada;
        }

        public void Parar() => destino = null;

        /// <summary>Gira na hora para olhar um ponto (usado ao atacar).</summary>
        public void OlharPara(Vector3 ponto)
        {
            Vector3 direcao = ponto - transform.position;
            direcao.y = 0f;
            if (direcao.sqrMagnitude > 0.0001f)
                transform.rotation = Quaternion.LookRotation(direcao, Vector3.up);
        }

        void Update()
        {
            Vector3 deslocamento = Vector3.zero;

            if (destino.HasValue)
            {
                Vector3 direcao = destino.Value - transform.position;
                direcao.y = 0f;
                float restante = direcao.magnitude;

                if (restante <= paradaAtual)
                {
                    destino = null;
                }
                else
                {
                    direcao /= restante;
                    float passo = Mathf.Min(velocidade * Time.deltaTime, restante - paradaAtual);
                    deslocamento = direcao * passo;
                    Quaternion alvo = Quaternion.LookRotation(direcao, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, alvo, velocidadeGiro * Time.deltaTime);
                }
            }

            // Gravidade simples para manter o personagem no chão
            velocidadeVertical = controle.isGrounded ? -1f : velocidadeVertical + Physics.gravity.y * Time.deltaTime;
            deslocamento.y = velocidadeVertical * Time.deltaTime;

            controle.Move(deslocamento);

            if (animator != null && Time.deltaTime > 0f)
            {
                float velocidadeAtual = new Vector3(deslocamento.x, 0f, deslocamento.z).magnitude / Time.deltaTime;
                animator.SetFloat(ParametroVelocidade, velocidadeAtual, 0.1f, Time.deltaTime);
            }
        }
    }
}
