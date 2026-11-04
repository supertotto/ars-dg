// claude: arquivo criado pelo Claude Code — executa uma passiva de contador (os dados vêm da PassivaDef)
using ARS.Combat;
using ARS.Data;
using UnityEngine;

namespace ARS.Characters
{
    /// <summary>
    /// Escuta os acertos do personagem e alimenta o contador da passiva. Quando enche,
    /// zera o contador e aplica o estado "ao completar" (ex.: próxima skill épica).
    /// </summary>
    [RequireComponent(typeof(Combatente))]
    public class Passiva : MonoBehaviour
    {
        public PassivaDef definicao;

        Combatente eu;

        public Combatente Dono => eu;

        void Awake()
        {
            eu = GetComponent<Combatente>();
            eu.AoAcertar += Acertou;
        }

        void OnDestroy()
        {
            if (eu != null) eu.AoAcertar -= Acertou;
        }

        void Acertou(Combatente alvo, OrigemDoAcerto origem)
        {
            if (definicao == null || definicao.contador == null) return;
            if (!Conta(origem)) return;

            var estados = eu.Estados;
            if (definicao.pausarEnquantoCompleto && definicao.aoCompletar != null && estados.EstaAtivo(definicao.aoCompletar))
                return;

            estados.Aplicar(definicao.contador, 0f, eu);
            if (estados.Acumulos(definicao.contador) < definicao.contador.acumulosMaximos) return;

            estados.Remover(definicao.contador);
            if (definicao.aoCompletar != null) estados.Aplicar(definicao.aoCompletar, 0f, eu);
        }

        bool Conta(OrigemDoAcerto origem)
        {
            switch (definicao.gatilho)
            {
                case GatilhoDaPassiva.AcertoDeSkill: return origem == OrigemDoAcerto.Skill;
                case GatilhoDaPassiva.AcertoDeAtaqueBasico: return origem == OrigemDoAcerto.AtaqueBasico;
                default: return true;
            }
        }

        /// <summary>Texto curto para o HUD (ex.: "Acerto Épico 3/5" ou "Acerto Épico: PRONTO").</summary>
        public string Resumo()
        {
            if (definicao == null || eu == null) return null;
            if (definicao.aoCompletar != null && eu.Estados.EstaAtivo(definicao.aoCompletar))
                return $"{definicao.nome}: PRONTO";
            int atual = definicao.contador != null ? eu.Estados.Acumulos(definicao.contador) : 0;
            int total = definicao.contador != null ? definicao.contador.acumulosMaximos : 0;
            return $"{definicao.nome} {atual}/{total}";
        }
    }
}
