// gpt: testa a passiva de contador por eventos de acerto em EditMode.
using System.Reflection;
using ARS.Characters;
using ARS.Combat;
using ARS.Data;
using NUnit.Framework;
using UnityEngine;

namespace ARS.Tests
{
    public class PassivaTestes
    {
        GameObject objeto;
        Combatente combatente;
        Passiva passiva;
        EstadoDef contador;
        EstadoDef completo;
        PassivaDef definicao;

        // gpt: cria dados temporários sem expiração e assina os acertos pelo Awake explícito.
        [SetUp]
        public void Preparar()
        {
            objeto = new GameObject("Dono da passiva de teste");
            combatente = objeto.AddComponent<Combatente>();
            contador = ScriptableObject.CreateInstance<EstadoDef>();
            contador.duracaoPadrao = 0f;
            contador.acumulosMaximos = 3;
            completo = ScriptableObject.CreateInstance<EstadoDef>();
            completo.duracaoPadrao = 0f;
            definicao = ScriptableObject.CreateInstance<PassivaDef>();
            definicao.contador = contador;
            definicao.aoCompletar = completo;
            definicao.gatilho = GatilhoDaPassiva.AcertoDeSkillOuAtaqueBasico;
            definicao.pausarEnquantoCompleto = false;
            passiva = objeto.AddComponent<Passiva>();
            passiva.definicao = definicao;
            typeof(Passiva).GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(passiva, null);
        }

        // gpt: destrói o dono antes dos dados para permitir a desinscrição da passiva.
        [TearDown]
        public void Limpar()
        {
            Object.DestroyImmediate(objeto);
            Object.DestroyImmediate(definicao);
            Object.DestroyImmediate(contador);
            Object.DestroyImmediate(completo);
        }

        // gpt: verifica que cada evento de acerto soma exatamente um acúmulo.
        [TestCase(OrigemDoAcerto.Skill)]
        [TestCase(OrigemDoAcerto.AtaqueBasico)]
        public void RegistrarAcerto_SobeOContador(OrigemDoAcerto origem)
        {
            Assert.AreEqual(0, combatente.Estados.Acumulos(contador));

            for (int acerto = 1; acerto < contador.acumulosMaximos; acerto++)
            {
                combatente.RegistrarAcerto(combatente, origem);
                Assert.AreEqual(acerto, combatente.Estados.Acumulos(contador));
                Assert.IsFalse(combatente.Estados.EstaAtivo(completo));
            }
        }

        // gpt: verifica o limite, a remoção do contador e a aplicação do estado completo.
        [Test]
        public void RegistrarAcerto_NoMaximoZeraOContadorEAplicaAoCompletar()
        {
            for (int acerto = 0; acerto < contador.acumulosMaximos - 1; acerto++)
                combatente.RegistrarAcerto(combatente, OrigemDoAcerto.Skill);
            Assert.AreEqual(contador.acumulosMaximos - 1, combatente.Estados.Acumulos(contador));
            Assert.IsFalse(combatente.Estados.EstaAtivo(completo));

            combatente.RegistrarAcerto(combatente, OrigemDoAcerto.Skill);

            Assert.AreEqual(0, combatente.Estados.Acumulos(contador));
            Assert.IsFalse(combatente.Estados.EstaAtivo(contador));
            Assert.IsTrue(combatente.Estados.EstaAtivo(completo));
            Assert.AreEqual(1, combatente.Estados.Acumulos(completo));
        }

        // gpt: verifica a pausa enquanto completo e a retomada depois da remoção explícita.
        [Test]
        public void PausarEnquantoCompleto_IgnoraAcertosAteRemoverOEstado()
        {
            definicao.pausarEnquantoCompleto = true;
            for (int acerto = 0; acerto < contador.acumulosMaximos; acerto++)
                combatente.RegistrarAcerto(combatente, OrigemDoAcerto.Skill);
            Assert.IsTrue(combatente.Estados.EstaAtivo(completo));

            for (int acerto = 0; acerto < contador.acumulosMaximos + 1; acerto++)
            {
                combatente.RegistrarAcerto(combatente, OrigemDoAcerto.Skill);
                Assert.AreEqual(0, combatente.Estados.Acumulos(contador));
                Assert.IsFalse(combatente.Estados.EstaAtivo(contador));
                Assert.IsTrue(combatente.Estados.EstaAtivo(completo));
            }

            combatente.Estados.Remover(completo);
            combatente.RegistrarAcerto(combatente, OrigemDoAcerto.Skill);
            Assert.AreEqual(1, combatente.Estados.Acumulos(contador));
            Assert.IsFalse(combatente.Estados.EstaAtivo(completo));
        }

        // gpt: verifica que o gatilho de skill ignora ataques básicos antes e durante a contagem.
        [Test]
        public void AcertoDeSkill_IgnoraAtaqueBasico()
        {
            definicao.gatilho = GatilhoDaPassiva.AcertoDeSkill;

            combatente.RegistrarAcerto(combatente, OrigemDoAcerto.AtaqueBasico);
            Assert.AreEqual(0, combatente.Estados.Acumulos(contador));
            Assert.IsFalse(combatente.Estados.EstaAtivo(contador));
            combatente.RegistrarAcerto(combatente, OrigemDoAcerto.Skill);
            Assert.AreEqual(1, combatente.Estados.Acumulos(contador));
            combatente.RegistrarAcerto(combatente, OrigemDoAcerto.AtaqueBasico);
            Assert.AreEqual(1, combatente.Estados.Acumulos(contador));
            Assert.IsFalse(combatente.Estados.EstaAtivo(completo));
        }
    }
}