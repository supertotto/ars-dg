// claude: arquivo criado pelo Claude Code — o que acontece quando o personagem morre (herói ou inimigo)
using ARS.Combat;
using UnityEngine;

namespace ARS.Characters
{
    /// <summary>
    /// Ao morrer: para de agir (movimento, ataque, skills, comandos, IA) e toca "Morrer" no Animator
    /// (ou deita o modelo, se não houver). Inimigos somem depois de um tempo; heróis podem voltar
    /// sozinhos (provisório, só para testes, até existir o sistema de reviver).
    /// </summary>
    [RequireComponent(typeof(Combatente))]
    public class Abatido : MonoBehaviour
    {
        [Tooltip("Segundos até sumir depois de morrer. 0 = não some.")]
        public float sumirEmSegundos = 0f;
        [Tooltip("Segundos até voltar com a vida cheia. 0 = não volta (PROVISÓRIO, só para testes).")]
        public float reviverEmSegundos = 0f;

        static readonly int ParametroMorrer = Animator.StringToHash("Morrer");

        Combatente eu;
        Animator animator;
        bool temMorrer;
        Quaternion rotacaoDoModelo;
        Transform modelo;

        public bool Morto { get; private set; }

        void Awake()
        {
            eu = GetComponent<Combatente>();
            animator = GetComponentInChildren<Animator>();
            if (animator != null && animator.runtimeAnimatorController != null)
                foreach (var p in animator.parameters) if (p.nameHash == ParametroMorrer) temMorrer = true;
            modelo = animator != null ? animator.transform : null;
            eu.AoMorrer += _ => Morrer();
        }

        void Morrer()
        {
            if (Morto) return;
            Morto = true;
            Ligar(false);
            if (temMorrer && animator.isActiveAndEnabled) animator.SetTrigger(ParametroMorrer);
            else if (modelo != null && modelo != transform)
            {
                rotacaoDoModelo = modelo.localRotation;
                modelo.localRotation = Quaternion.Euler(-90f, 0f, 0f) * rotacaoDoModelo; // deitado (sem animação de morte)
            }
            if (sumirEmSegundos > 0f) Destroy(gameObject, sumirEmSegundos);
            if (reviverEmSegundos > 0f) Invoke(nameof(Reviver), reviverEmSegundos);
        }

        void Reviver()
        {
            Morto = false;
            eu.RestaurarVida();
            if (modelo != null && modelo != transform && !temMorrer) modelo.localRotation = rotacaoDoModelo;
            if (temMorrer && animator != null) animator.Rebind();
            Ligar(true);
        }

        void Ligar(bool ligado)
        {
            var ataque = GetComponent<AtaqueBasico>();
            if (ataque != null) { if (!ligado) ataque.Cancelar(); ataque.enabled = ligado; }
            var movimento = GetComponent<Movimento>();
            if (movimento != null) { if (!ligado) movimento.Parar(); movimento.enabled = ligado; }
            var kit = GetComponent<KitDeSkills>();
            if (kit != null) kit.enabled = ligado;
            var comandos = GetComponent<ComandosDoJogador>();
            if (comandos != null) comandos.enabled = ligado;
            // a IA mora em outro módulo: desliga qualquer componente marcado como cérebro
            foreach (var cerebro in GetComponents<ICerebro>()) cerebro.Ligado = ligado;
        }
    }

    /// <summary>Quem decide sozinho o que o personagem faz (IA). Abatido desliga ao morrer.</summary>
    public interface ICerebro
    {
        bool Ligado { get; set; }
    }
}
