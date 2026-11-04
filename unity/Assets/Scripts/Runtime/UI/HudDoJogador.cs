// claude: arquivo criado pelo Claude Code — HUD do jogador em uGUI (substitui o HudProvisorio)
using ARS.Characters;
using ARS.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace ARS.UI
{
    /// <summary>
    /// Raiz do HUD (prefab gerado por ARS → Montar HUD). Lê os componentes do herói ligado
    /// e repassa para as peças. Sem regra de jogo: só mostra.
    /// </summary>
    public class HudDoJogador : MonoBehaviour
    {
        public BarraDoHud barraDeVida;
        public BarraDoHud barraDeRecurso;
        [Tooltip("Um botão por tecla (Q, W, E, R).")]
        public BotaoDeSkill[] botoes;

        [Header("Passiva")]
        public GameObject passivaRaiz;
        public Image passivaMoldura;
        public Image passivaIcone;
        public Text passivaTexto;
        public Color corDaPassiva = new Color(0.35f, 0.35f, 0.4f);
        public Color corDaPassivaPronta = new Color(1f, 0.8f, 0.2f);

        [Header("Mensagens")]
        public Text mensagem;

        Combatente vida;
        Recurso recurso;
        KitDeSkills kit;
        Passiva passiva;

        public GameObject Heroi { get; private set; }

        /// <summary>Liga o HUD a um herói (o controlado pelo jogador).</summary>
        public void Ligar(GameObject heroi)
        {
            Heroi = heroi;
            vida = heroi != null ? heroi.GetComponent<Combatente>() : null;
            recurso = heroi != null ? heroi.GetComponent<Recurso>() : null;
            kit = heroi != null ? heroi.GetComponent<KitDeSkills>() : null;
            passiva = heroi != null ? heroi.GetComponent<Passiva>() : null;

            if (barraDeRecurso != null) barraDeRecurso.gameObject.SetActive(recurso != null);
            bool temPassiva = passiva != null && passiva.definicao != null;
            if (passivaRaiz != null) passivaRaiz.SetActive(temPassiva);
            if (temPassiva && passivaIcone != null)
            {
                passivaIcone.sprite = passiva.definicao.icone;
                passivaIcone.enabled = passiva.definicao.icone != null;
            }
        }

        void LateUpdate()
        {
            if (Heroi == null) return;

            if (barraDeVida != null && vida != null) barraDeVida.Mostrar(vida.Vida, vida.vidaMaxima);
            if (barraDeRecurso != null && recurso != null) barraDeRecurso.Mostrar(recurso.Atual, recurso.maximo);

            if (botoes != null)
                foreach (var botao in botoes)
                    if (botao != null) botao.Atualizar(kit, recurso);

            if (passiva != null && passiva.Progresso(out int atual, out int total, out bool pronta))
            {
                if (passivaTexto != null) passivaTexto.text = pronta ? "!" : $"{atual}/{total}";
                if (passivaMoldura != null) passivaMoldura.color = pronta ? corDaPassivaPronta : corDaPassiva;
            }

            if (mensagem != null) mensagem.text = kit != null ? kit.UltimaMensagem ?? "" : "";
        }
    }
}
