// claude: arquivo criado pelo Claude (app claude.ai)
using System.Collections.Generic;
using ARS.Combat;
using UnityEngine;

namespace ARS.Data
{
    public enum AlvoDaSkill
    {
        [Tooltip("O inimigo sob o cursor (ou o mais perto dele).")]
        InimigoNoCursor,
        Proprio      // (a fazer: Direcao, PontoNoChao, AreaCirculo...)
    }

    public enum EntregaDaSkill
    {
        Instantanea, // os efeitos acontecem na hora
        Projetil     // os efeitos acontecem quando o projétil acerta
    }

    /// <summary>
    /// Definição de uma skill (dados, não código). Todos os números são PROVISÓRIOS.
    /// A mesma skill pode ser usada por qualquer personagem (base do modo customizado).
    /// </summary>
    [CreateAssetMenu(menuName = "ARS/Skill", fileName = "Skill")]
    public class SkillDef : ScriptableObject
    {
        public string nome = "Nova skill";
        [TextArea] public string descricao;

        [Header("Custo e recarga")]
        public float custo = 20f;
        [Tooltip("Segundos de recarga (por carga, se tiver mais de uma).")]
        public float recarga = 4f;
        [Tooltip("1 = skill normal. Mais de 1 = cargas que voltam uma de cada vez.")]
        [Min(1)] public int cargas = 1;
        // claude: tempo em que o personagem fica ocupado conjurando (sem ataque básico nem outra skill)
        [Tooltip("Segundos entre apertar a tecla e a skill sair. Nesse tempo o personagem não ataca nem usa outra skill.")]
        [Min(0f)] public float tempoDeConjuracao = 0.3f;

        [Header("Alvo")]
        public AlvoDaSkill alvo = AlvoDaSkill.InimigoNoCursor;
        public float alcance = 8f;
        [Tooltip("Se não houver inimigo exatamente sob o cursor, procura o mais perto do cursor dentro deste raio.")]
        public float raioDeBuscaNoCursor = 3f;

        [Header("Como chega")]
        public EntregaDaSkill entrega = EntregaDaSkill.Projetil;
        public float velocidadeProjetil = 16f;
        public GameObject visualProjetil;
        public Color corProjetil = new Color(1f, 0.85f, 0.4f);
        public float tamanhoProjetil = 0.3f;
        public bool rastro = true;
        public PontoDeDisparo pontoDeDisparo = PontoDeDisparo.MaoDireita;
        // claude: lista de aparências sorteadas a cada disparo; vazia = usa o visual único acima
        [Tooltip("Se tiver itens, cada disparo sorteia um deles (pelo peso) no lugar do visual, cor, tamanho e rastro acima.")]
        public List<VisualDeProjetil> variacoesVisuais = new List<VisualDeProjetil>();

        [Header("O que faz")]
        public List<Efeito> efeitos = new List<Efeito> { new Efeito() };
    }
}
