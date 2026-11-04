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
        Proprio,     // (a fazer: Direcao...)
        // claude: um lugar do chão (até o alcance; mais longe = no limite do alcance)
        [Tooltip("Um ponto do chão sob o cursor (limitado ao alcance).")]
        PontoNoChao
    }

    public enum EntregaDaSkill
    {
        Instantanea, // os efeitos acontecem na hora
        Projetil,    // os efeitos acontecem quando o projétil acerta
        AreaNoPonto  // claude: os efeitos acontecem em todos os inimigos no raio em volta do ponto (conta 1 acerto)
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
        // claude: efeito no ponto do acerto
        [Tooltip("Efeito que aparece onde a skill acerta. Vazio = nenhum.")]
        public GameObject visualImpacto;
        // claude: lista de aparências sorteadas a cada disparo; vazia = usa o visual único acima
        [Tooltip("Se tiver itens, cada disparo sorteia um deles (pelo peso) no lugar do visual, cor, tamanho e rastro acima.")]
        public List<VisualDeProjetil> variacoesVisuais = new List<VisualDeProjetil>();

        // claude: raio da área (entrega Área No Ponto)
        [Tooltip("Raio em metros (só para Área No Ponto).")]
        public float raioDaArea = 2.5f;

        [Header("O que faz")]
        public List<Efeito> efeitos = new List<Efeito> { new Efeito() };
        // claude: efeitos no próprio lançador, antes dos efeitos no alvo (ex.: teleporte, marcar ponto de volta)
        [Tooltip("Efeitos aplicados no próprio lançador, na ordem, antes dos efeitos no alvo (ex.: teleportar).")]
        public List<Efeito> efeitosNoLancador = new List<Efeito>();

        // claude: segunda ativação dentro de uma janela (ex.: E do Caleb volta até o gato)
        [Header("Reativação")]
        [Tooltip("Estado que abre a janela de reativação. Enquanto o lançador tiver este estado, apertar a tecla de novo faz a reativação (sem custo e sem gastar carga). Vazio = sem reativação.")]
        public EstadoDef janelaDeReativacao;
        [Tooltip("Efeitos no lançador ao reativar (ex.: teleportar até o ponto marcado pelo estado da janela).")]
        public List<Efeito> efeitosAoReativar = new List<Efeito>();
    }
}
