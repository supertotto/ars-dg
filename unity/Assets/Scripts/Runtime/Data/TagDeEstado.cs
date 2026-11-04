// claude: arquivo criado pelo Claude Code — tags de estado (as regras checam tags, nunca nomes)
using System;

namespace ARS.Data
{
    /// <summary>
    /// Tags que um estado coloca no personagem. As regras do jogo perguntam
    /// "tem a tag X?", nunca "é o estado Y?" nem "é o herói Z?".
    /// Tag nova = valor novo aqui (potência de 2) + a regra que a checa.
    /// </summary>
    [Flags]
    public enum TagDeEstado
    {
        Nenhuma = 0,
        SemAtaque = 1 << 0,       // não faz ataque básico
        SemSkill = 1 << 1,        // não usa skills
        SemMovimento = 1 << 2,    // não anda
        Transformado = 1 << 3,    // virou outra coisa (ex.: bichinho)
        Controle = 1 << 4,        // o estado é um controle (chefes e imunes ignoram)
        ImuneAControle = 1 << 5,  // ignora estados com a tag Controle (ex.: chefes)
    }

    public static class TagDeEstadoExt
    {
        /// <summary>true se "tags" tem todas as tags pedidas.</summary>
        public static bool Contem(this TagDeEstado tags, TagDeEstado pedidas) => (tags & pedidas) == pedidas && pedidas != 0;
    }
}
