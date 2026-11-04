// claude: arquivo criado pelo Claude Code — passiva como dado (contador de acertos que vira um estado)
using UnityEngine;

namespace ARS.Data
{
    /// <summary>O que alimenta o contador da passiva.</summary>
    public enum GatilhoDaPassiva
    {
        AcertoDeSkillOuAtaqueBasico,
        AcertoDeSkill,
        AcertoDeAtaqueBasico
    }

    /// <summary>
    /// Passiva de contador: cada acerto soma um acúmulo no estado "contador"; quando chega
    /// ao máximo dele, o contador zera e o personagem ganha o estado "ao completar".
    /// Os números ficam nos estados (PROVISÓRIOS).
    /// </summary>
    [CreateAssetMenu(menuName = "ARS/Passiva", fileName = "Passiva")]
    public class PassivaDef : ScriptableObject
    {
        public string nome = "Nova passiva";
        [TextArea] public string descricao;

        public GatilhoDaPassiva gatilho = GatilhoDaPassiva.AcertoDeSkillOuAtaqueBasico;
        [Tooltip("Estado que conta os acertos. Os acúmulos máximos dele = quantos acertos para completar.")]
        public EstadoDef contador;
        [Tooltip("Estado ganho ao completar o contador (ex.: próxima skill épica).")]
        public EstadoDef aoCompletar;
        [Tooltip("Enquanto tiver o estado \"ao completar\", os acertos não contam.")]
        public bool pausarEnquantoCompleto = true;
    }
}
