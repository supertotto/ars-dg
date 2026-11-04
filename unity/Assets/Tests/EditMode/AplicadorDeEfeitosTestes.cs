// gpt: testes EditMode de aplicação e multiplicação de efeitos genéricos.
using System.Collections.Generic;
using ARS.Combat;
using ARS.Data;
using NUnit.Framework;
using UnityEngine;

namespace ARS.Tests
{
    public class AplicadorDeEfeitosTestes
    {
        GameObject objeto;
        Combatente combatente;
        EstadoDef estado;
        HashSet<NumeroFlutuante> numerosAnteriores;
        Random.State sorteioAnterior;

        // gpt: inicializa a vida explicitamente e registra os visuais preexistentes.
        [SetUp]
        public void Preparar()
        {
            sorteioAnterior = Random.state;
            numerosAnteriores = new HashSet<NumeroFlutuante>(Object.FindObjectsByType<NumeroFlutuante>(FindObjectsSortMode.None));
            objeto = new GameObject("Alvo de teste");
            combatente = objeto.AddComponent<Combatente>();
            combatente.vidaMaxima = 100f;
            combatente.RestaurarVida();
            estado = ScriptableObject.CreateInstance<EstadoDef>();
            estado.duracaoPadrao = 0f;
        }

        // gpt: limpa os recursos e números de dano criados pelo teste, preservando os anteriores.
        [TearDown]
        public void Limpar()
        {
            foreach (var numero in Object.FindObjectsByType<NumeroFlutuante>(FindObjectsSortMode.None))
                if (!numerosAnteriores.Contains(numero)) Object.DestroyImmediate(numero.gameObject);
            Object.DestroyImmediate(objeto);
            Object.DestroyImmediate(estado);
            Random.state = sorteioAnterior;
        }

        // gpt: verifica a redução de vida causada pelo efeito de dano.
        [Test]
        public void Dano_ReduzAVida()
        {
            AplicadorDeEfeitos.Aplicar(new[] { new Efeito { tipo = TipoDeEfeito.Dano, valor = 25f } }, combatente, null);
            Assert.AreEqual(75f, combatente.Vida, 0.0001f);
        }

        // gpt: parte de um alvo ferido para verificar cura e limite de vida.
        [Test]
        public void Cura_NaoPassaDaVidaMaxima()
        {
            AplicadorDeEfeitos.Aplicar(new[] { new Efeito { tipo = TipoDeEfeito.Dano, valor = 25f } }, combatente, null);
            Assert.AreEqual(75f, combatente.Vida, 0.0001f);
            AplicadorDeEfeitos.Aplicar(new[] { new Efeito { tipo = TipoDeEfeito.Cura, valor = 50f } }, combatente, null);
            Assert.AreEqual(combatente.vidaMaxima, combatente.Vida);
        }

        // gpt: verifica estado e tags no alvo após a aplicação do efeito.
        [Test]
        public void AplicarEstado_AplicaOEstadoNoAlvo()
        {
            estado.tags = TagDeEstado.SemSkill;
            AplicadorDeEfeitos.Aplicar(new[] { new Efeito { tipo = TipoDeEfeito.AplicarEstado, estado = estado } }, combatente, null);
            Assert.IsTrue(combatente.Estados.EstaAtivo(estado));
            Assert.IsTrue(combatente.Estados.Tem(TagDeEstado.SemSkill));
        }

        // gpt: verifica que o efeito dispara uma interrupção no alvo.
        [Test]
        public void Interromper_DisparaAoSerInterrompido()
        {
            int interrupcoes = 0;
            combatente.AoSerInterrompido += () => interrupcoes++;
            AplicadorDeEfeitos.Aplicar(new[] { new Efeito { tipo = TipoDeEfeito.Interromper } }, combatente, null);
            Assert.AreEqual(1, interrupcoes);
        }

        // gpt: verifica identidade da lista quando o multiplicador é neutro.
        [Test]
        public void Multiplicados_ComUmDevolveAMesmaLista()
        {
            var efeitos = new List<Efeito> { new Efeito { valor = 20f, duracao = 4f } };
            Assert.AreSame(efeitos, AplicadorDeEfeitos.Multiplicados(efeitos, 1f));
        }

        // gpt: verifica cópias independentes, números multiplicados e preservação dos demais campos.
        [Test]
        public void Multiplicados_ComUmEMeioNaoAlteraOriginais()
        {
            var efeitos = new List<Efeito>
            {
                new Efeito { tipo = TipoDeEfeito.Dano, valor = 20f, duracao = 4f, tipoDeDano = TipoDeDano.Fisico },
                new Efeito { tipo = TipoDeEfeito.AplicarEstado, valor = 10f, duracao = 2f, estado = estado }
            };
            var copia = AplicadorDeEfeitos.Multiplicados(efeitos, 1.5f);
            Assert.AreNotSame(efeitos, copia);
            Assert.AreEqual(efeitos.Count, copia.Count);
            for (int i = 0; i < efeitos.Count; i++)
            {
                Assert.AreNotSame(efeitos[i], copia[i]);
                Assert.AreEqual(efeitos[i].valor * 1.5f, copia[i].valor, 0.0001f);
                Assert.AreEqual(efeitos[i].duracao * 1.5f, copia[i].duracao, 0.0001f);
                Assert.AreEqual(efeitos[i].tipo, copia[i].tipo);
                Assert.AreEqual(efeitos[i].tipoDeDano, copia[i].tipoDeDano);
                Assert.AreSame(efeitos[i].estado, copia[i].estado);
            }
            Assert.AreEqual(20f, efeitos[0].valor);
            Assert.AreEqual(4f, efeitos[0].duracao);
            Assert.AreEqual(10f, efeitos[1].valor);
            Assert.AreEqual(2f, efeitos[1].duracao);
        }

        // gpt: verifica duração padrão multiplicada apenas nos dados, sem esperar o tempo passar.
        [Test]
        public void Multiplicados_EstadoComDuracaoZeroUsaPadraoVezesMultiplicador()
        {
            estado.duracaoPadrao = 4f;
            var efeito = new Efeito { tipo = TipoDeEfeito.AplicarEstado, estado = estado, duracao = 0f };
            var copia = AplicadorDeEfeitos.Multiplicados(new[] { efeito }, 1.5f);
            Assert.AreEqual(6f, copia[0].duracao, 0.0001f);
            Assert.AreSame(estado, copia[0].estado);
            Assert.AreEqual(0f, efeito.duracao);
            Assert.AreEqual(4f, estado.duracaoPadrao);
        }
    }
}
