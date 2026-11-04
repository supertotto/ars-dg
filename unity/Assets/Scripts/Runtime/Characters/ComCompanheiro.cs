// claude: arquivo criado pelo Claude Code — o herói cria o próprio companheiro ao entrar em jogo
using UnityEngine;

namespace ARS.Characters
{
    /// <summary>
    /// Cria o companheiro do herói (ex.: o Cappuccino do Caleb) ao lado dele quando o jogo começa.
    /// Skills acham o companheiro por <see cref="Companheiro"/>.
    /// </summary>
    public class ComCompanheiro : MonoBehaviour
    {
        [Tooltip("Prefab do companheiro (precisa ter o componente Companheiro).")]
        public GameObject prefab;

        public Companheiro Companheiro { get; private set; }

        void Start()
        {
            if (prefab == null) return;
            var objeto = Instantiate(prefab);
            objeto.name = prefab.name;
            Companheiro = objeto.GetComponent<Companheiro>();
            if (Companheiro == null) Companheiro = objeto.AddComponent<Companheiro>();
            Companheiro.dono = transform;
            objeto.transform.SetPositionAndRotation(transform.TransformPoint(Companheiro.posicaoAoLado), transform.rotation);

            // claude: obedece às ordens das skills (efeitos Companheiro Fica / Companheiro Segue)
            var combatente = GetComponent<ARS.Combat.Combatente>();
            if (combatente != null)
            {
                combatente.AoMandarCompanheiroFicar += Ficar;
                combatente.AoMandarCompanheiroSeguir += Seguir;
                combatente.AoMandarCompanheiroCacar += Cacar; // claude
            }
        }

        // claude: caçada (ex.: R do Caleb); o dano sai em nome do dono
        void Cacar(Vector3 centro, ARS.Data.ZonaDef zona, float multiplicador, float duracao)
        {
            if (Companheiro != null) Companheiro.Cacar(centro, zona, multiplicador, duracao, GetComponent<ARS.Combat.Combatente>());
        }

        void Ficar(Vector3 ponto) { if (Companheiro != null) Companheiro.FicarEm(ponto); }
        void Seguir() { if (Companheiro != null) Companheiro.Seguir(); }

        void OnDestroy()
        {
            if (Companheiro != null) Destroy(Companheiro.gameObject);
        }
    }
}
