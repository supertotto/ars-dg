// claude: arquivo criado pelo Claude Code — botão de uma skill no HUD (ícone, recarga, cargas, reativação)
using ARS.Characters;
using ARS.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace ARS.UI
{
    /// <summary>
    /// Mostra o estado de uma tecla do <see cref="KitDeSkills"/>. Só lê; quem decide se a skill
    /// pode ser usada continua sendo o kit.
    /// </summary>
    public class BotaoDeSkill : MonoBehaviour
    {
        public Tecla tecla;

        [Header("Peças")]
        public Image moldura;
        public Image icone;
        [Tooltip("Sombra da recarga. Image Type = Filled (Radial 360).")]
        public Image sombraDeRecarga;
        public Text textoDaTecla;
        public Text textoDoTempo;
        public Text textoDasCargas;

        [Header("Cores (provisórias)")]
        public Color corDaMoldura = new Color(0.35f, 0.35f, 0.4f);
        public Color corDaMolduraReativar = new Color(1f, 0.8f, 0.2f);
        public Color corDoIcone = Color.white;
        public Color corSemRecurso = new Color(0.35f, 0.45f, 1f);

        void Awake()
        {
            if (textoDaTecla != null) textoDaTecla.text = tecla.ToString();
        }

        public void Atualizar(KitDeSkills kit, Recurso recurso)
        {
            var estado = kit != null ? kit.EstadoDe(tecla) : null;
            var skill = estado?.skill;

            if (icone != null)
            {
                icone.enabled = skill != null && skill.icone != null;
                icone.sprite = skill != null ? skill.icone : null;
                bool semRecurso = skill != null && recurso != null && !recurso.TemPelomenos(skill.custo);
                icone.color = semRecurso ? corSemRecurso : corDoIcone;
            }

            float falta = skill != null ? kit.RecargaRestante(tecla) : 0f;
            bool semCargas = skill != null && estado.cargas == 0 && falta > 0f;

            if (sombraDeRecarga != null)
            {
                sombraDeRecarga.enabled = skill == null || semCargas;
                sombraDeRecarga.fillAmount = skill == null ? 1f
                    : skill.recarga > 0f ? Mathf.Clamp01(falta / skill.recarga) : 0f;
            }
            if (textoDoTempo != null)
                textoDoTempo.text = semCargas ? (falta >= 1f ? Mathf.CeilToInt(falta).ToString() : falta.ToString("0.0")) : "";
            if (textoDasCargas != null)
                textoDasCargas.text = skill != null && skill.cargas > 1 ? estado.cargas.ToString() : "";
            if (moldura != null)
                moldura.color = skill != null && kit.PodeReativar(tecla) ? corDaMolduraReativar : corDaMoldura;
        }
    }
}
