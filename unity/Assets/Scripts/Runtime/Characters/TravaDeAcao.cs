// claude: arquivo criado pelo Claude Code — trava que impede duas ações ao mesmo tempo (ataque básico x skill)
using UnityEngine;

namespace ARS.Characters
{
    /// <summary>
    /// Uma ação por vez: enquanto o personagem solta um ataque básico ou conjura uma skill,
    /// fica ocupado e a outra ação espera (ataque básico) ou é recusada (skill).
    /// </summary>
    public class TravaDeAcao : MonoBehaviour
    {
        float livreEm;

        public bool Ocupado => Time.time < livreEm;

        // claude: uma skill guardada esperando a trava liberar; o ataque básico não passa na frente dela
        public bool Reservado { get; set; }

        /// <summary>Fica ocupado por "segundos" a partir de agora (nunca encurta uma trava maior).</summary>
        public void Ocupar(float segundos) => livreEm = Mathf.Max(livreEm, Time.time + Mathf.Max(0f, segundos));

        public void Liberar() => livreEm = 0f;

        /// <summary>Pega a trava do personagem, criando se ainda não existir (prefabs antigos).</summary>
        public static TravaDeAcao De(GameObject personagem)
        {
            var trava = personagem.GetComponent<TravaDeAcao>();
            return trava != null ? trava : personagem.AddComponent<TravaDeAcao>();
        }
    }
}
