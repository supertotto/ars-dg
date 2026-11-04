// claude: arquivo criado pelo Claude Code — bicho/companheiro que acompanha o dono (ex.: o gato do Caleb)
using UnityEngine;

namespace ARS.Characters
{
    /// <summary>
    /// Acompanha o dono andando ao lado dele. Skills podem mandá-lo para outro lugar
    /// (<see cref="IrPara"/> / <see cref="FicarEm"/>) e depois devolvê-lo (<see cref="Seguir"/>).
    /// Não é combatente: não pode ser atacado. Números PROVISÓRIOS.
    /// </summary>
    public class Companheiro : MonoBehaviour
    {
        [Tooltip("Quem ele acompanha (preenchido ao ser criado pelo dono).")]
        public Transform dono;
        [Tooltip("Onde fica em relação ao dono: x = de lado, z = à frente (negativo = atrás).")]
        public Vector3 posicaoAoLado = new Vector3(0.9f, 0f, -0.5f);
        [Tooltip("Velocidade andando (m/s).")]
        public float velocidadeAndando = 2f;
        [Tooltip("Velocidade correndo, quando ficou para trás (m/s).")]
        public float velocidadeCorrendo = 5f;
        [Tooltip("A partir desta distância do lugar dele, corre.")]
        public float distanciaParaCorrer = 2.5f;
        [Tooltip("A partir desta distância, aparece direto ao lado do dono.")]
        public float distanciaParaTeleportar = 15f;
        [Tooltip("Considera que chegou a esta distância.")]
        public float distanciaParada = 0.25f;
        public float velocidadeGiro = 540f;

        static readonly int ParametroVelocidade = Animator.StringToHash("Velocidade");

        enum Modo { Seguindo, IndoPara, Parado }
        Modo modo = Modo.Seguindo;
        Vector3 destino;
        Animator animator;

        /// <summary>true enquanto acompanha o dono (não foi mandado para outro lugar).</summary>
        public bool Seguindo => modo == Modo.Seguindo;

        void Awake() => animator = GetComponentInChildren<Animator>();

        /// <summary>Vai andando/correndo até um ponto e fica lá até mandarem seguir de novo.</summary>
        public void IrPara(Vector3 ponto) { destino = ponto; modo = Modo.IndoPara; }

        /// <summary>Aparece num ponto e fica parado lá (ex.: o dono teleportou e o deixou para trás).</summary>
        public void FicarEm(Vector3 ponto) { transform.position = ponto; destino = ponto; modo = Modo.Parado; }

        /// <summary>Volta a acompanhar o dono.</summary>
        public void Seguir() => modo = Modo.Seguindo;

        void Update()
        {
            if (dono == null) return;

            Vector3 alvo = modo == Modo.Seguindo ? LugarAoLado() : destino;
            Vector3 direcao = alvo - transform.position;
            direcao.y = 0f;
            float distancia = direcao.magnitude;

            if (modo == Modo.Seguindo && distancia > distanciaParaTeleportar)
            {
                transform.position = alvo;
                distancia = 0f;
            }

            float velocidade = 0f;
            if (modo != Modo.Parado && distancia > distanciaParada)
            {
                velocidade = distancia > distanciaParaCorrer ? velocidadeCorrendo : velocidadeAndando;
                Vector3 passo = direcao / distancia * Mathf.Min(velocidade * Time.deltaTime, distancia);
                transform.position += passo;
                transform.rotation = Quaternion.RotateTowards(transform.rotation,
                    Quaternion.LookRotation(direcao, Vector3.up), velocidadeGiro * Time.deltaTime);
            }
            else if (modo == Modo.Seguindo)
            {
                // parado ao lado: olha para onde o dono olha
                transform.rotation = Quaternion.RotateTowards(transform.rotation, dono.rotation, velocidadeGiro * 0.5f * Time.deltaTime);
            }

            // fica na altura do chão do dono (dungeon plana por enquanto)
            var p = transform.position;
            p.y = modo == Modo.Seguindo ? dono.position.y : destino.y;
            transform.position = p;

            if (animator != null && animator.isActiveAndEnabled && animator.runtimeAnimatorController != null)
                animator.SetFloat(ParametroVelocidade, velocidade, 0.1f, Time.deltaTime);
        }

        Vector3 LugarAoLado() => dono.TransformPoint(posicaoAoLado);
    }
}
