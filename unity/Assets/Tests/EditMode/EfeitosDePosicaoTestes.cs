// claude: testes de teleporte, ponto guardado em estado, efeitos ao terminar, ordens ao companheiro e zona multiplicada
using System.Collections.Generic;
using ARS.Combat;
using ARS.Data;
using NUnit.Framework;
using UnityEngine;

namespace ARS.Tests
{
    public class EfeitosDePosicaoTestes
    {
        GameObject objeto;
        Combatente combatente;
        readonly List<Object> criados = new List<Object>();

        [SetUp]
        public void Preparar()
        {
            objeto = new GameObject("Alvo de teste");
            combatente = objeto.AddComponent<Combatente>();
            combatente.RestaurarVida();
        }

        [TearDown]
        public void Limpar()
        {
            Object.DestroyImmediate(objeto);
            foreach (var o in criados) if (o != null) Object.DestroyImmediate(o);
            criados.Clear();
        }

        EstadoDef Estado()
        {
            var def = ScriptableObject.CreateInstance<EstadoDef>();
            def.duracaoPadrao = 0f; // até ser removido
            criados.Add(def);
            return def;
        }

        static ContextoDoEfeito Contexto(Vector3 mirado, Vector3 partida) =>
            new ContextoDoEfeito { origem = null, pontoMirado = mirado, pontoDePartida = partida };

        static Efeito Teleporte(DestinoDoEfeito destino, EstadoDef estado = null) =>
            new Efeito { tipo = TipoDeEfeito.Teleportar, valor = 0f, destino = destino, estado = estado };

        [Test]
        public void Teleportar_PontoMirado_MoveEAvisa()
        {
            var mirado = new Vector3(3f, 0f, 4f);
            Vector3? avisado = null;
            combatente.AoTeleportar += p => avisado = p;

            AplicadorDeEfeitos.Aplicar(new[] { Teleporte(DestinoDoEfeito.PontoMirado) }, combatente, Contexto(mirado, Vector3.zero));

            Assert.AreEqual(mirado, objeto.transform.position);
            Assert.AreEqual(mirado, avisado);
        }

        [Test]
        public void Teleportar_PontoDePartida_VaiParaAPartida()
        {
            var partida = new Vector3(-2f, 0f, 1f);

            AplicadorDeEfeitos.Aplicar(new[] { Teleporte(DestinoDoEfeito.PontoDePartida) }, combatente, Contexto(Vector3.one * 9f, partida));

            Assert.AreEqual(partida, objeto.transform.position);
        }

        [Test]
        public void Teleportar_PontoDoEstado_UsaOPontoGuardado()
        {
            var janela = Estado();
            var guardado = new Vector3(5f, 0f, -5f);
            combatente.Estados.Aplicar(janela, 0f, null, guardado);

            AplicadorDeEfeitos.Aplicar(new[] { Teleporte(DestinoDoEfeito.PontoDoEstado, janela) }, combatente, Contexto(Vector3.zero, Vector3.zero));

            Assert.AreEqual(guardado, objeto.transform.position);
        }

        [Test]
        public void Teleportar_PontoDoEstado_SemEstadoAtivo_NaoMove()
        {
            var janela = Estado();
            objeto.transform.position = new Vector3(1f, 0f, 1f);

            AplicadorDeEfeitos.Aplicar(new[] { Teleporte(DestinoDoEfeito.PontoDoEstado, janela) }, combatente, Contexto(Vector3.zero, Vector3.zero));

            Assert.AreEqual(new Vector3(1f, 0f, 1f), objeto.transform.position);
        }

        [Test]
        public void AplicarEstado_GuardaOPontoDePartida()
        {
            var janela = Estado();
            var partida = new Vector3(2f, 0f, 7f);

            AplicadorDeEfeitos.Aplicar(new[] { new Efeito { tipo = TipoDeEfeito.AplicarEstado, valor = 0f, estado = janela } },
                                       combatente, Contexto(Vector3.zero, partida));

            Assert.AreEqual(partida, combatente.Estados.PontoDe(janela));
        }

        [Test]
        public void RemoverEstado_AplicaEfeitosAoTerminarNoDono()
        {
            var janela = Estado();
            janela.efeitosAoTerminar = new List<Efeito>
            {
                new Efeito { tipo = TipoDeEfeito.CompanheiroSegue, valor = 0f },
                Teleporte(DestinoDoEfeito.PontoDePartida)
            };
            var guardado = new Vector3(-4f, 0f, 3f);
            bool seguiu = false;
            combatente.AoMandarCompanheiroSeguir += () => seguiu = true;
            combatente.Estados.Aplicar(janela, 0f, null, guardado);

            combatente.Estados.Remover(janela);

            Assert.IsTrue(seguiu);
            Assert.AreEqual(guardado, objeto.transform.position);
        }

        [Test]
        public void CompanheiroFica_AvisaComOPontoDePartida()
        {
            var partida = new Vector3(6f, 0f, 2f);
            Vector3? ficou = null;
            combatente.AoMandarCompanheiroFicar += p => ficou = p;

            AplicadorDeEfeitos.Aplicar(new[] { new Efeito { tipo = TipoDeEfeito.CompanheiroFica, valor = 0f } },
                                       combatente, Contexto(Vector3.zero, partida));

            Assert.AreEqual(partida, ficou);
        }

        [Test]
        public void Multiplicados_ZonaSemDuracaoUsaADaZonaVezesMultiplicador()
        {
            var zona = ScriptableObject.CreateInstance<ZonaDef>();
            zona.duracao = 4f;
            criados.Add(zona);
            var efeito = new Efeito { tipo = TipoDeEfeito.CriarZona, valor = 1f, duracao = 0f, zona = zona };

            var copia = AplicadorDeEfeitos.Multiplicados(new[] { efeito }, 1.5f);

            Assert.AreEqual(6f, copia[0].duracao, 0.0001f);
            Assert.AreEqual(1.5f, copia[0].valor, 0.0001f);
            Assert.AreSame(zona, copia[0].zona);
            Assert.AreEqual(0f, efeito.duracao);
            Assert.AreEqual(4f, zona.duracao);
        }
    }
}
