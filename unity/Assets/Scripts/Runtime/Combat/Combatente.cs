// claude: arquivo criado pelo Claude (app claude.ai)
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ARS.Combat
{
    public enum Equipe { Jogadores, Inimigos }

    public enum TipoDeDano { Fisico, Magico }

    // claude: de onde veio um acerto (para passivas)
    public enum OrigemDoAcerto { AtaqueBasico, Skill }

    /// <summary>Tudo que causa ou recebe dano: heróis, inimigos, bonecos de treino.</summary>
    public class Combatente : MonoBehaviour
    {
        public Equipe equipe = Equipe.Jogadores;
        [Tooltip("Vida máxima (provisório).")]
        public float vidaMaxima = 500f;
        [Tooltip("Altura do centro do corpo, para mirar projéteis e mostrar números.")]
        public float alturaDoCorpo = 1.1f;

        public float Vida { get; private set; }
        public bool Vivo => Vida > 0f;
        public Vector3 Centro => transform.position + Vector3.up * alturaDoCorpo;

        /// <summary>(quantidade, tipo, quem causou)</summary>
        public event Action<float, TipoDeDano, Combatente> AoReceberDano;
        public event Action<Combatente> AoMorrer;
        // claude: corta a conjuração em andamento (skill ou golpe); quem conjura escuta este evento
        public event Action AoSerInterrompido;

        // claude: estados e tags do combatente (criado na hora, se o prefab não tiver)
        Estados estados;
        public Estados Estados => estados != null ? estados : (estados = Estados.De(this));

        public void Interromper() => AoSerInterrompido?.Invoke();

        // claude: aviso de "acertei" (alimenta passivas). Cada uso que acerta avisa uma vez:
        // um míssil = 1, um ataque básico = 1, uma skill que pega vários inimigos = 1.
        // O que a skill invoca ou deixa no chão (abelhas, gato) não avisa.
        public event Action<Combatente, OrigemDoAcerto> AoAcertar;
        public void RegistrarAcerto(Combatente alvo, OrigemDoAcerto origem) => AoAcertar?.Invoke(alvo, origem);

        // claude: teleporte (desliga o CharacterController para poder mover) e aviso para quem anda parar
        public event Action<Vector3> AoTeleportar;
        public void Teleportar(Vector3 destino)
        {
            var controle = GetComponent<CharacterController>();
            bool ligado = controle != null && controle.enabled;
            if (ligado) controle.enabled = false;
            transform.position = destino;
            if (ligado) controle.enabled = true;
            AoTeleportar?.Invoke(destino);
        }

        // claude: ordens para o companheiro (quem tem companheiro escuta)
        public event Action<Vector3> AoMandarCompanheiroFicar;
        public event Action AoMandarCompanheiroSeguir;
        public void MandarCompanheiroFicar(Vector3 ponto) => AoMandarCompanheiroFicar?.Invoke(ponto);
        public void MandarCompanheiroSeguir() => AoMandarCompanheiroSeguir?.Invoke();
        // claude: caçar numa área (centro, zona com raio/duração/ataques, multiplicador do dano, duração; 0 = a da zona)
        public event Action<Vector3, ARS.Data.ZonaDef, float, float> AoMandarCompanheiroCacar;
        public void MandarCompanheiroCacar(Vector3 centro, ARS.Data.ZonaDef zona, float multiplicador, float duracao) =>
            AoMandarCompanheiroCacar?.Invoke(centro, zona, multiplicador, duracao);

        static readonly List<Combatente> todos = new List<Combatente>();
        /// <summary>Todos os combatentes ativos na cena.</summary>
        public static IReadOnlyList<Combatente> Todos => todos;

        void Awake() => Vida = vidaMaxima;
        void OnEnable() => todos.Add(this);
        void OnDisable() => todos.Remove(this);

        public bool EhInimigoDe(Combatente outro) => outro != null && outro.equipe != equipe;

        public void ReceberDano(float quantidade, TipoDeDano tipo, Combatente origem)
        {
            if (!Vivo || quantidade <= 0f) return;
            Vida = Mathf.Max(0f, Vida - quantidade);
            AoReceberDano?.Invoke(quantidade, tipo, origem);
            NumeroFlutuante.Mostrar(Centro + Vector3.up * 0.8f, quantidade, tipo);
            if (!Vivo) AoMorrer?.Invoke(this);
        }

        public void Curar(float quantidade)
        {
            if (quantidade <= 0f) return;
            Vida = Mathf.Min(vidaMaxima, Vida + quantidade);
        }

        public void RestaurarVida() => Vida = vidaMaxima;
    }
}
