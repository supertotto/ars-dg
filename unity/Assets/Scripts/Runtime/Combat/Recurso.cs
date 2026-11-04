// claude: arquivo criado pelo Claude (app claude.ai)
using UnityEngine;

namespace ARS.Combat
{
    public enum TipoDeRecurso { Furia, Mana, Fe, Energia, Essencia }

    /// <summary>
    /// Recurso usado pelas skills. Por enquanto só o comportamento da Mana
    /// (começa cheia e recupera devagar). Números PROVISÓRIOS.
    /// </summary>
    public class Recurso : MonoBehaviour
    {
        public TipoDeRecurso tipo = TipoDeRecurso.Mana;
        public float maximo = 300f;
        [Tooltip("Quanto recupera por segundo.")]
        public float regeneracao = 4f;

        public float Atual { get; private set; }

        void Awake() => Atual = maximo;

        void Update() => Atual = Mathf.Min(maximo, Atual + regeneracao * Time.deltaTime);

        public bool TemPelomenos(float quantidade) => Atual >= quantidade;

        public bool Gastar(float quantidade)
        {
            if (Atual < quantidade) return false;
            Atual -= quantidade;
            return true;
        }

        public void Ganhar(float quantidade) => Atual = Mathf.Min(maximo, Atual + quantidade);
    }
}
