// claude: arquivo criado pelo Claude Code — barra do HUD (vida, recurso): preenchimento + texto
using UnityEngine;
using UnityEngine.UI;

namespace ARS.UI
{
    /// <summary>Barra horizontal do HUD. Só mostra o que recebe; não sabe de onde vem o valor.</summary>
    public class BarraDoHud : MonoBehaviour
    {
        [Tooltip("Imagem com Image Type = Filled (horizontal).")]
        public Image preenchimento;
        public Text texto;

        public void Mostrar(float atual, float maximo)
        {
            float fracao = maximo > 0f ? Mathf.Clamp01(atual / maximo) : 0f;
            if (preenchimento != null) preenchimento.fillAmount = fracao;
            if (texto != null) texto.text = $"{Mathf.CeilToInt(atual)} / {Mathf.CeilToInt(maximo)}";
        }
    }
}
