// gpt: testa os casos determinísticos do sorteio ponderado de visuais.
using System.Collections.Generic;
using ARS.Data;
using NUnit.Framework;
using UnityEngine;

namespace ARS.Tests
{
    public class VisualDeProjetilTestes
    {
        Random.State sorteioAnterior;

        // gpt: preserva o estado global do sorteio para isolar os testes.
        [SetUp]
        public void Preparar()
        {
            sorteioAnterior = Random.state;
        }

        // gpt: restaura o sorteio após cada teste; estes testes não criam objetos Unity.
        [TearDown]
        public void Limpar()
        {
            Random.state = sorteioAnterior;
        }

        // gpt: verifica a ausência de resultado para lista nula.
        [Test]
        public void Sortear_ComListaNulaDevolveNull()
        {
            Assert.IsNull(VisualDeProjetil.Sortear(null));
        }

        // gpt: verifica a ausência de resultado para lista vazia.
        [Test]
        public void Sortear_ComListaVaziaDevolveNull()
        {
            Assert.IsNull(VisualDeProjetil.Sortear(new List<VisualDeProjetil>()));
        }

        // gpt: verifica a ausência de resultado quando nenhum visual tem peso.
        [Test]
        public void Sortear_ComTodosOsPesosZeroDevolveNull()
        {
            var visuais = new List<VisualDeProjetil>
            {
                new VisualDeProjetil { peso = 0f },
                new VisualDeProjetil { peso = 0f }
            };

            Assert.IsNull(VisualDeProjetil.Sortear(visuais));
        }

        // gpt: verifica repetidamente o único candidato com peso positivo, com e sem alternativas sem peso.
        [TestCase(false)]
        [TestCase(true)]
        public void Sortear_ComUmUnicoPesoPositivoSempreDevolveEsseItem(bool incluirSemPeso)
        {
            var escolhido = new VisualDeProjetil { peso = 2f };
            var visuais = new List<VisualDeProjetil> { escolhido };
            if (incluirSemPeso)
            {
                visuais.Add(new VisualDeProjetil { peso = 0f });
                visuais.Add(new VisualDeProjetil { peso = 0f });
            }
            Random.InitState(12345);

            for (int tentativa = 0; tentativa < 100; tentativa++)
                Assert.AreSame(escolhido, VisualDeProjetil.Sortear(visuais));
        }
    }
}