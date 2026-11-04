// claude: primeiro teste (modelo para os próximos) — estados, tags e imunidade a controle
// gpt: suporte à limpeza das definições criadas nos testes.
using System.Collections.Generic;
using ARS.Combat;
using ARS.Data;
using NUnit.Framework;
using UnityEngine;

namespace ARS.Tests
{
    public class EstadosTestes
    {
        // gpt: registra estados temporários para destruí-los após cada teste.
        readonly List<EstadoDef> definicoes = new List<EstadoDef>();
        GameObject objeto;
        Combatente combatente;

        [SetUp]
        public void Preparar()
        {
            objeto = new GameObject("Alvo de teste");
            combatente = objeto.AddComponent<Combatente>();
        }

        // gpt: limpa o alvo e todos os ScriptableObjects temporários.
        [TearDown]
        public void Limpar()
        {
            Object.DestroyImmediate(objeto);
            foreach (var definicao in definicoes) Object.DestroyImmediate(definicao);
            definicoes.Clear();
        }

        // gpt: registra cada estado criado e mantém duração independente do tempo.
        EstadoDef Estado(TagDeEstado tags, int acumulos = 1)
        {
            var def = ScriptableObject.CreateInstance<EstadoDef>();
            definicoes.Add(def);
            def.tags = tags;
            def.duracaoPadrao = 0f; // até ser removido (não depende do tempo)
            def.acumulosMaximos = acumulos;
            return def;
        }

        [Test]
        public void AplicarEstado_ColocaAsTags()
        {
            var transformado = Estado(TagDeEstado.Transformado | TagDeEstado.SemSkill);

            combatente.Estados.Aplicar(transformado, 0f, null);

            Assert.IsTrue(combatente.Estados.Tem(TagDeEstado.Transformado));
            Assert.IsTrue(combatente.Estados.Tem(TagDeEstado.SemSkill));
            Assert.IsFalse(combatente.Estados.Tem(TagDeEstado.SemAtaque));
        }

        [Test]
        public void Controle_NaoPegaEmImune()
        {
            var controle = Estado(TagDeEstado.Controle | TagDeEstado.SemAtaque);
            combatente.Estados.tagsFixas = TagDeEstado.ImuneAControle;

            bool pegou = combatente.Estados.Aplicar(controle, 0f, null);

            Assert.IsFalse(pegou);
            Assert.IsFalse(combatente.Estados.Tem(TagDeEstado.SemAtaque));
        }

        // gpt: verifica o aumento gradual e o limite sem duplicar o estado.
        [Test]
        public void Reaplicar_SomaAcumulosAteOMaximo()
        {
            var estado = Estado(TagDeEstado.Nenhuma, 3);
            for (int aplicacao = 1; aplicacao <= 5; aplicacao++)
            {
                combatente.Estados.Aplicar(estado, 0f, null);
                Assert.AreEqual(Mathf.Min(aplicacao, 3), combatente.Estados.Acumulos(estado));
                Assert.AreEqual(1, combatente.Estados.Ativos.Count);
            }
        }

        // gpt: verifica a remoção do estado, dos acúmulos e das suas tags.
        [Test]
        public void Remover_TiraOEstadoEAsTags()
        {
            var estado = Estado(TagDeEstado.Transformado | TagDeEstado.SemSkill);
            combatente.Estados.Aplicar(estado, 0f, null);

            combatente.Estados.Remover(estado);

            Assert.IsFalse(combatente.Estados.EstaAtivo(estado));
            Assert.AreEqual(0, combatente.Estados.Acumulos(estado));
            Assert.IsFalse(combatente.Estados.Tem(TagDeEstado.Transformado));
            Assert.IsFalse(combatente.Estados.Tem(TagDeEstado.SemSkill));
            Assert.IsEmpty(combatente.Estados.Ativos);
        }

        // gpt: remove estados com qualquer tag pedida e preserva os demais.
        [Test]
        public void RemoverComTag_RemoveSomenteEstadosComAlgumaTagPedida()
        {
            var primeiro = Estado(TagDeEstado.SemAtaque);
            var segundo = Estado(TagDeEstado.SemSkill | TagDeEstado.Controle);
            var mantido = Estado(TagDeEstado.SemMovimento);
            combatente.Estados.Aplicar(primeiro, 0f, null);
            combatente.Estados.Aplicar(segundo, 0f, null);
            combatente.Estados.Aplicar(mantido, 0f, null);

            combatente.Estados.RemoverComTag(TagDeEstado.SemAtaque | TagDeEstado.SemSkill);

            Assert.IsFalse(combatente.Estados.EstaAtivo(primeiro));
            Assert.IsFalse(combatente.Estados.EstaAtivo(segundo));
            Assert.IsTrue(combatente.Estados.EstaAtivo(mantido));
            Assert.IsFalse(combatente.Estados.Tem(TagDeEstado.SemAtaque));
            Assert.IsFalse(combatente.Estados.Tem(TagDeEstado.SemSkill));
            Assert.IsFalse(combatente.Estados.Tem(TagDeEstado.Controle));
            Assert.IsTrue(combatente.Estados.Tem(TagDeEstado.SemMovimento));
            Assert.AreEqual(1, combatente.Estados.Ativos.Count);
        }

        // gpt: verifica a aplicação de controle quando não existe imunidade.
        [Test]
        public void Controle_PegaEmQuemNaoEImune()
        {
            var controle = Estado(TagDeEstado.Controle | TagDeEstado.SemAtaque);

            bool pegou = combatente.Estados.Aplicar(controle, 0f, null);

            Assert.IsTrue(pegou);
            Assert.IsTrue(combatente.Estados.EstaAtivo(controle));
            Assert.IsTrue(combatente.Estados.Tem(TagDeEstado.Controle | TagDeEstado.SemAtaque));
        }

        // gpt: combina modificadores do mesmo tipo entre estados e ignora outros tipos.
        [Test]
        public void Multiplicador_DevolveOProdutoDosModificadoresDoMesmoTipo()
        {
            var primeiro = Estado(TagDeEstado.Nenhuma);
            primeiro.modificadores.Add(new Modificador { tipo = TipoDeModificador.MultiplicarNumerosDaSkill, valor = 1.5f });
            primeiro.modificadores.Add(new Modificador { tipo = TipoDeModificador.MultiplicarNumerosDaSkill, valor = 2f });
            primeiro.modificadores.Add(new Modificador { tipo = TipoDeModificador.MultiplicarVisualDaSkill, valor = 10f });
            var segundo = Estado(TagDeEstado.Nenhuma);
            segundo.modificadores.Add(new Modificador { tipo = TipoDeModificador.MultiplicarNumerosDaSkill, valor = 0.5f });
            combatente.Estados.Aplicar(primeiro, 0f, null);
            combatente.Estados.Aplicar(segundo, 0f, null);

            Assert.AreEqual(1.5f, combatente.Estados.Multiplicador(TipoDeModificador.MultiplicarNumerosDaSkill), 0.0001f);
        }

        // gpt: verifica o multiplicador neutro sem estados e sem modificadores do tipo.
        [Test]
        public void Multiplicador_SemModificadoresDevolveUm()
        {
            Assert.AreEqual(1f, combatente.Estados.Multiplicador(TipoDeModificador.MultiplicarNumerosDaSkill));
            var estado = Estado(TagDeEstado.Nenhuma);
            combatente.Estados.Aplicar(estado, 0f, null);
            Assert.AreEqual(1f, combatente.Estados.Multiplicador(TipoDeModificador.MultiplicarNumerosDaSkill));
            estado.modificadores.Add(new Modificador { tipo = TipoDeModificador.MultiplicarVisualDaSkill, valor = 2f });
            Assert.AreEqual(1f, combatente.Estados.Multiplicador(TipoDeModificador.MultiplicarNumerosDaSkill));
        }

        // gpt: verifica presença, ausência e remoção de um tipo de modificador.
        [Test]
        public void TemModificador_ConsideraSomenteEstadosAtivosDoTipoPedido()
        {
            var estado = Estado(TagDeEstado.Nenhuma);
            estado.modificadores.Add(new Modificador { tipo = TipoDeModificador.SkillSemCusto });
            Assert.IsFalse(combatente.Estados.TemModificador(TipoDeModificador.SkillSemCusto));

            combatente.Estados.Aplicar(estado, 0f, null);

            Assert.IsTrue(combatente.Estados.TemModificador(TipoDeModificador.SkillSemCusto));
            Assert.IsFalse(combatente.Estados.TemModificador(TipoDeModificador.MultiplicarVisualDaSkill));
            combatente.Estados.Remover(estado);
            Assert.IsFalse(combatente.Estados.TemModificador(TipoDeModificador.SkillSemCusto));
        }

        // gpt: consome todos os estados marcados e mantém os não consumíveis.
        [Test]
        public void ConsumirAoUsarSkill_RemoveSomenteEstadosMarcados()
        {
            var primeiro = Estado(TagDeEstado.SemAtaque);
            primeiro.consumidoAoUsarSkill = true;
            var mantido = Estado(TagDeEstado.SemMovimento);
            var segundo = Estado(TagDeEstado.SemSkill);
            segundo.consumidoAoUsarSkill = true;
            combatente.Estados.Aplicar(primeiro, 0f, null);
            combatente.Estados.Aplicar(mantido, 0f, null);
            combatente.Estados.Aplicar(segundo, 0f, null);

            combatente.Estados.ConsumirAoUsarSkill();

            Assert.IsFalse(combatente.Estados.EstaAtivo(primeiro));
            Assert.IsFalse(combatente.Estados.EstaAtivo(segundo));
            Assert.IsTrue(combatente.Estados.EstaAtivo(mantido));
            Assert.IsFalse(combatente.Estados.Tem(TagDeEstado.SemAtaque));
            Assert.IsFalse(combatente.Estados.Tem(TagDeEstado.SemSkill));
            Assert.IsTrue(combatente.Estados.Tem(TagDeEstado.SemMovimento));
            Assert.AreEqual(1, combatente.Estados.Ativos.Count);
        }
    }
}

