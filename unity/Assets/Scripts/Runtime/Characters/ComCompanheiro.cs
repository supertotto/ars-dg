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
        }

        void OnDestroy()
        {
            if (Companheiro != null) Destroy(Companheiro.gameObject);
        }
    }
}
