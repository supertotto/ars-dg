// claude: arquivo criado pelo Claude Code — modificadores que um estado aplica enquanto dura
using System;
using UnityEngine;

namespace ARS.Data
{
    /// <summary>
    /// O que um estado muda enquanto está ativo. Modificador novo = valor novo aqui
    /// + a regra que o lê (nunca por nome de herói, skill ou estado).
    /// </summary>
    public enum TipoDeModificador
    {
        MultiplicarNumerosDaSkill,  // dano, cura e duração dos efeitos da próxima skill
        MultiplicarVisualDaSkill,   // tamanho do visual (projétil, impacto) da próxima skill
        SkillSemCusto               // a próxima skill não gasta recurso
    }

    [Serializable]
    public class Modificador
    {
        public TipoDeModificador tipo;
        [Tooltip("Multiplicador (para os tipos \"Multiplicar\"). Ignorado em Skill Sem Custo.")]
        public float valor = 1f;
    }
}
