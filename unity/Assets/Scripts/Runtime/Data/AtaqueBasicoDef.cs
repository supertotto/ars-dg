using ARS.Combat;
using UnityEngine;

namespace ARS.Data
{
    /// <summary>
    /// Definição do ataque básico (dados, não código). Cada herói aponta para uma.
    /// Todos os números são PROVISÓRIOS — balanceamento em aberto (docs/pendencias.md).
    /// </summary>
    public enum PontoDeDisparo { MaoDireita, MaoEsquerda, Altura }

    [CreateAssetMenu(menuName = "ARS/Ataque básico", fileName = "AtaqueBasico")]
    public class AtaqueBasicoDef : ScriptableObject
    {
        [Tooltip("Distância máxima para atacar, em metros.")]
        public float alcance = 6f;
        [Tooltip("Segundos entre um ataque e outro.")]
        public float intervalo = 1f;
        [Tooltip("Segundos entre começar o golpe e o dano sair (o \"balanço\" da animação).")]
        public float tempoDoGolpe = 0.3f;
        public float dano = 20f;
        public TipoDeDano tipo = TipoDeDano.Magico;

        [Header("Projétil (ataque à distância)")]
        [Tooltip("Desligado = ataque corpo a corpo (o dano sai na hora).")]
        public bool usaProjetil = true;
        public float velocidadeProjetil = 14f;
        public Color corProjetil = new Color(0.6f, 0.4f, 1f);
        [Tooltip("De onde o projétil sai. Mãos usam o esqueleto Humanoid; quando houver arma, a ponta dela.")]
        public PontoDeDisparo pontoDeDisparo = PontoDeDisparo.MaoDireita;
        [Tooltip("Altura de onde o projétil sai, se o ponto for \"Altura\" ou se a mão não for encontrada.")]
        public float alturaDeDisparo = 1.3f;
    }
}
