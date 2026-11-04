// claude: arquivo criado pelo Claude (app claude.ai)
using System;
using ARS.Combat;
using UnityEngine;

namespace ARS.Data
{
    /// <summary>
    /// Tipos de efeito genéricos. Uma skill é uma lista deles; skill nova = combinação nova.
    /// Os marcados "(a fazer)" já existem nos dados, mas ainda não têm código.
    /// </summary>
    public enum TipoDeEfeito
    {
        Dano,
        Cura,
        Escudo,           // (a fazer)
        Lentidao,         // (a fazer)
        Atordoar,         // (a fazer)
        Enraizar,         // (a fazer)
        Empurrar,         // (a fazer)
        AplicarElemento,  // (a fazer)
        Invocar           // (a fazer)
    }

    /// <summary>Um efeito genérico com seus números (todos provisórios).</summary>
    [Serializable]
    public class Efeito
    {
        public TipoDeEfeito tipo = TipoDeEfeito.Dano;
        [Tooltip("Quantidade: dano, cura, escudo, % de lentidão...")]
        public float valor = 30f;
        public TipoDeDano tipoDeDano = TipoDeDano.Magico;
        [Tooltip("Duração em segundos (para efeitos que duram).")]
        public float duracao;
    }
}
