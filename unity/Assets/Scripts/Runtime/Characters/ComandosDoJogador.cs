// claude: arquivo criado pelo Claude (app claude.ai)
using ARS.Combat;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ARS.Characters
{
    /// <summary>
    /// Ordens do jogador pelo mouse, estilo LoL (provisório):
    /// botão direito no chão = andar; botão direito num inimigo = atacar até outra ordem;
    /// Q, W, E, R = skills no alvo sob o cursor.
    /// </summary>
    [RequireComponent(typeof(Movimento))]
    public class ComandosDoJogador : MonoBehaviour
    {
        [Tooltip("Camadas que respondem ao clique.")]
        public LayerMask clicavel = ~0;

        Movimento movimento;
        AtaqueBasico ataque;
        KitDeSkills kit;
        Combatente eu;

        void Awake()
        {
            movimento = GetComponent<Movimento>();
            ataque = GetComponent<AtaqueBasico>();
            kit = GetComponent<KitDeSkills>();
            eu = GetComponent<Combatente>();
        }

        void Update()
        {
            var mouse = Mouse.current;
            var camera = Camera.main;
            if (mouse == null || camera == null) return;

            Ray raio = camera.ScreenPointToRay(mouse.position.ReadValue());
            var acertos = Physics.RaycastAll(raio, 500f, clicavel, QueryTriggerInteraction.Collide);
            System.Array.Sort(acertos, (a, b) => a.distance.CompareTo(b.distance));

            LerSkills(acertos);

            if (!mouse.rightButton.isPressed) return;

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
                if (kit != null) kit.DescartarGuardada(); // claude: andar desiste da skill guardada
                movimento.MoverPara(acerto.point);
                return;
            }
        }

        void LerSkills(RaycastHit[] acertos)
        {
            var teclado = Keyboard.current;
            if (kit == null || teclado == null) return;

            Tecla? tecla = null;
            if (teclado.qKey.wasPressedThisFrame) tecla = Tecla.Q;
            else if (teclado.wKey.wasPressedThisFrame) tecla = Tecla.W;
            else if (teclado.eKey.wasPressedThisFrame) tecla = Tecla.E;
            else if (teclado.rKey.wasPressedThisFrame) tecla = Tecla.R;
            if (tecla == null) return;

            Combatente sobCursor = null;
            Vector3 pontoNoChao = transform.position;
            bool achouChao = false;
            foreach (var acerto in acertos)
            {
                if (acerto.collider.transform.IsChildOf(transform)) continue;
                var c = acerto.collider.GetComponentInParent<Combatente>();
                if (c != null && sobCursor == null) sobCursor = c;
                if (c == null && !acerto.collider.isTrigger && !achouChao)
                {
                    pontoNoChao = acerto.point;
                    achouChao = true;
                }
            }
            if (sobCursor != null && !achouChao) pontoNoChao = sobCursor.transform.position;

            kit.Usar(tecla.Value, sobCursor, pontoNoChao);
        }
    }
}
