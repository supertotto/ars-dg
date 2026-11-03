using UnityEngine;
using UnityEngine.InputSystem;

namespace ARS.Characters
{
    /// <summary>
    /// Movimento por clique, estilo LoL: segura ou clica com o botão direito no chão
    /// e o herói anda até lá. Atualiza o parâmetro "Velocidade" do Animator.
    /// Provisório: ainda não ataca nem usa skills.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class ClickToMove : MonoBehaviour
    {
        [Tooltip("Velocidade de movimento em metros por segundo (provisório).")]
        public float velocidade = 3.5f;
        [Tooltip("Velocidade de giro em graus por segundo.")]
        public float velocidadeGiro = 720f;
        [Tooltip("Distância em que o herói considera que chegou.")]
        public float distanciaParada = 0.15f;
        [Tooltip("Camadas que contam como chão para o clique.")]
        public LayerMask chao = ~0;

        static readonly int ParametroVelocidade = Animator.StringToHash("Velocidade");

        CharacterController controle;
        Animator animator;
        Vector3? destino;
        float velocidadeVertical;

        void Awake()
        {
            controle = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();
        }

        void Update()
        {
            LerClique();
            Mover();
        }

        void LerClique()
        {
            var mouse = Mouse.current;
            var camera = Camera.main;
            if (mouse == null || camera == null || !mouse.rightButton.isPressed) return;

            Ray raio = camera.ScreenPointToRay(mouse.position.ReadValue());
            foreach (var acerto in Physics.RaycastAll(raio, 500f, chao, QueryTriggerInteraction.Ignore))
            {
                if (acerto.collider.transform.IsChildOf(transform)) continue;
                destino = acerto.point;
                return;
            }
        }

        void Mover()
        {
            Vector3 deslocamento = Vector3.zero;

            if (destino.HasValue)
            {
                Vector3 direcao = destino.Value - transform.position;
                direcao.y = 0f;
                float restante = direcao.magnitude;

                if (restante <= distanciaParada)
                {
                    destino = null;
                }
                else
                {
                    direcao /= restante;
                    float passo = Mathf.Min(velocidade * Time.deltaTime, restante);
                    deslocamento = direcao * passo;
                    Quaternion alvo = Quaternion.LookRotation(direcao, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, alvo, velocidadeGiro * Time.deltaTime);
                }
            }

            // Gravidade simples para manter o herói no chão
            velocidadeVertical = controle.isGrounded ? -1f : velocidadeVertical + Physics.gravity.y * Time.deltaTime;
            deslocamento.y = velocidadeVertical * Time.deltaTime;

            controle.Move(deslocamento);

            if (animator != null)
            {
                float velocidadeAtual = Time.deltaTime > 0f
                    ? new Vector3(deslocamento.x, 0f, deslocamento.z).magnitude / Time.deltaTime
                    : 0f;
                animator.SetFloat(ParametroVelocidade, velocidadeAtual, 0.1f, Time.deltaTime);
            }
        }
    }
}
