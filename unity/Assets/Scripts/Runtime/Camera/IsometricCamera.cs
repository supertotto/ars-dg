using UnityEngine;
using UnityEngine.InputSystem;

namespace ARS.CameraSystem
{
    /// <summary>
    /// Câmera isométrica que segue um alvo, com zoom na rodinha do mouse.
    /// Valores provisórios (ver docs/pendencias.md).
    /// </summary>
    public class IsometricCamera : MonoBehaviour
    {
        [Tooltip("Quem a câmera segue (normalmente o herói do jogador).")]
        public Transform alvo;

        [Header("Ângulo")]
        [Tooltip("Inclinação para baixo, em graus.")]
        [Range(20f, 89f)] public float inclinacao = 50f;
        [Tooltip("Rotação em volta do alvo, em graus (45 = visão isométrica clássica).")]
        public float rotacao = 45f;

        [Header("Distância e zoom")]
        public float distancia = 16f;
        public float distanciaMinima = 8f;
        public float distanciaMaxima = 26f;
        public float velocidadeZoom = 0.01f;

        [Header("Suavização")]
        [Tooltip("Quanto maior, mais rápido a câmera alcança o alvo.")]
        public float suavidade = 10f;

        void LateUpdate()
        {
            if (alvo == null) return;

            var mouse = Mouse.current;
            if (mouse != null)
            {
                float rolagem = mouse.scroll.ReadValue().y;
                distancia = Mathf.Clamp(distancia - rolagem * velocidadeZoom, distanciaMinima, distanciaMaxima);
            }

            Quaternion orientacao = Quaternion.Euler(inclinacao, rotacao, 0f);
            Vector3 destino = alvo.position - orientacao * Vector3.forward * distancia;

            transform.position = Vector3.Lerp(transform.position, destino, 1f - Mathf.Exp(-suavidade * Time.deltaTime));
            transform.rotation = orientacao;
        }

        /// <summary>Coloca a câmera na posição final na hora, sem suavizar.</summary>
        public void Encaixar()
        {
            if (alvo == null) return;
            Quaternion orientacao = Quaternion.Euler(inclinacao, rotacao, 0f);
            transform.SetPositionAndRotation(alvo.position - orientacao * Vector3.forward * distancia, orientacao);
        }
    }
}
