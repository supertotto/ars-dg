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
        Interromper,      // corta a conjuração do alvo (pega até em imunes a controle)
        // claude: movimento, zonas e companheiro (novos valores sempre no fim)
        Teleportar,       // leva o alvo até o "destino" abaixo
        CriarZona,        // cria a "zona" no ponto mirado (dano/efeitos por tique em área); valor = multiplicador dos tiques
        CompanheiroFica,  // o companheiro do alvo fica parado no ponto de partida
        CompanheiroSegue  // o companheiro do alvo volta a acompanhá-lo
    }

    // claude: para onde vai um efeito de posição (ex.: Teleportar)
    public enum DestinoDoEfeito
    {
        PontoMirado,     // o ponto do chão escolhido na skill
        PontoDePartida,  // onde o lançador estava ao usar a skill
        PontoDoEstado    // o ponto guardado pelo "estado" deste efeito (ex.: janela de volta)
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
        [Tooltip("Estado aplicado (Aplicar Estado) ou de onde vem o ponto (destino Ponto Do Estado).")]
        public EstadoDef estado;
        // claude: destino de efeitos de posição e zona criada
        [Tooltip("Para onde vai (só Teleportar).")]
        public DestinoDoEfeito destino = DestinoDoEfeito.PontoMirado;
        [Tooltip("Zona criada (só Criar Zona).")]
        public ZonaDef zona;
    }
}
