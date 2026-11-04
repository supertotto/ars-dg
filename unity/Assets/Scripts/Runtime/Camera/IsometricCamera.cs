// claude: arquivo criado pelo Claude (app claude.ai)
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
        public float distanciaMinima = 4f;
        public float distanciaMaxima = 26f;
        [Tooltip("Metros de zoom por clique da rodinha.")]
        public float passoZoom = 1.5f;

        [Header("Suavização")]
        [Tooltip("Quanto maior, mais rápido a câmera alcança o alvo.")]
        public float suavidade = 10f;

        void LateUpdate()
        {
            if (alvo == null) return;

            // Zoom: rodinha do mouse, ou teclas + e - (também as do teclado numérico)
            float zoom = 0f;
            var mouse = Mouse.current;
            if (mouse != null)
            {
                float rolagem = mouse.scroll.ReadValue().y;
                if (Mathf.Abs(rolagem) > 0.01f) zoom = Mathf.Sign(rolagem); // cada "clique" vale o mesmo em qualquer mouse
            }
            var teclado = Keyboard.current;
            if (teclado != null)
            {
                if (teclado.equalsKey.wasPressedThisFrame || teclado.numpadPlusKey.wasPressedThisFrame) zoom = 1f;
                if (teclado.minusKey.wasPressedThisFrame || teclado.numpadMinusKey.wasPressedThisFrame) zoom = -1f;
            }
            if (zoom != 0f)
                distancia = Mathf.Clamp(distancia - zoom * passoZoom, distanciaMinima, distanciaMaxima);

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
