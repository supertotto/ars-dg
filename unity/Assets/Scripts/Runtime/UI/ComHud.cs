// claude: arquivo criado pelo Claude Code — põe o HUD na tela quando o herói é o controlado pelo jogador
using ARS.Characters;
using UnityEngine;

namespace ARS.UI
{
    /// <summary>
    /// Fica no herói. Enquanto ele estiver sob o controle do jogador (ComandosDoJogador ligado),
    /// cria e mostra o HUD; senão, esconde.
    /// </summary>
    public class ComHud : MonoBehaviour
    {
        [Tooltip("Prefab gerado por ARS → Montar HUD.")]
        public HudDoJogador prefabDoHud;
        [Tooltip("Foto do herói no HUD (Assets/UI/Retratos/<Herói>.png).")]
        public Sprite retrato; // claude: provisório aqui até existir a ficha do herói nos dados

        ComandosDoJogador comandos;
        HudDoJogador hud;

        void Awake() => comandos = GetComponent<ComandosDoJogador>();

        void Update()
        {
            bool controlado = comandos != null && comandos.enabled;
            if (controlado && hud == null && prefabDoHud != null)
            {
                hud = Instantiate(prefabDoHud);
                hud.name = prefabDoHud.name;
                hud.Ligar(gameObject);
            }
            if (hud != null && hud.gameObject.activeSelf != controlado) hud.gameObject.SetActive(controlado);
        }

        void OnDestroy()
        {
            if (hud != null) Destroy(hud.gameObject);
        }
    }
}
