using ARS.Combat;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ARS.Characters
{
    /// <summary>
    /// Ordens do jogador pelo mouse, estilo LoL (provisório):
    /// botão direito no chão = andar; botão direito num inimigo = atacar até outra ordem.
    /// </summary>
    [RequireComponent(typeof(Movimento))]
    public class ComandosDoJogador : MonoBehaviour
    {
        [Tooltip("Camadas que respondem ao clique.")]
        public LayerMask clicavel = ~0;

        Movimento movimento;
        AtaqueBasico ataque;
        Combatente eu;

        void Awake()
        {
            movimento = GetComponent<Movimento>();
            ataque = GetComponent<AtaqueBasico>();
            eu = GetComponent<Combatente>();
        }

        void Update()
        {
            var mouse = Mouse.current;
            var camera = Camera.main;
            if (mouse == null || camera == null || !mouse.rightButton.isPressed) return;

            Ray raio = camera.ScreenPointToRay(mouse.position.ReadValue());
            var acertos = Physics.RaycastAll(raio, 500f, clicavel, QueryTriggerInteraction.Collide);
            System.Array.Sort(acertos, (a, b) => a.distance.CompareTo(b.distance));

            // Inimigo sob o cursor tem prioridade sobre o chão
            foreach (var acerto in acertos)
            {
                if (acerto.collider.transform.IsChildOf(transform)) continue;
                var alvo = acerto.collider.GetComponentInParent<Combatente>();
                if (alvo != null && ataque != null && eu != null && eu.EhInimigoDe(alvo) && alvo.Vivo)
                {
                    ataque.Atacar(alvo);
                    return;
                }
            }

            // Clique no chão: cancela o ataque e anda até lá
            foreach (var acerto in acertos)
            {
                if (acerto.collider.transform.IsChildOf(transform)) continue;
                if (acerto.collider.GetComponentInParent<Combatente>() != null) continue;
                if (acerto.collider.isTrigger) continue;
                if (ataque != null) ataque.Cancelar();
                movimento.MoverPara(acerto.point);
                return;
            }
        }
    }
}
