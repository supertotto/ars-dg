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
        Invocar,          // (a fazer)
        // claude: estados e interrupção (novos valores sempre no fim, para não mudar os dados salvos)
        AplicarEstado,    // aplica o "estado" abaixo (controle não pega em imunes)
        Interromper       // corta a conjuração do alvo (pega até em imunes a controle)
    }

    /// <summary>Um efeito genérico com seus números (todos provisórios).</summary>
    [Serializable]
    public class Efeito
    {
        public TipoDeEfeito tipo = TipoDeEfeito.Dano;
        [Tooltip("Quantidade: dano, cura, escudo, % de lentidão...")]
        public float valor = 30f;
        public TipoDeDano tipoDeDano = TipoDeDano.Magico;
        [Tooltip("Duração em segundos (para efeitos que duram). Em Aplicar Estado, 0 = duração padrão do estado.")]
        public float duracao;
        // claude: estado usado por Aplicar Estado
        [Tooltip("Estado aplicado (só para Aplicar Estado).")]
        public EstadoDef estado;
    }
}
