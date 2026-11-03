using UnityEngine;

namespace ARS.Effects
{
    /// <summary>Reinicia as partículas de um efeito de tempos em tempos (usado na vitrine de efeitos).</summary>
    public class RepetirEfeito : MonoBehaviour
    {
        public float intervalo = 2.5f;

        ParticleSystem[] sistemas;
        float proximo;

        void Start()
        {
            sistemas = GetComponentsInChildren<ParticleSystem>(true);
            Reiniciar();
        }

        void Update()
        {
            if (Time.time < proximo) return;
            Reiniciar();
        }

        void Reiniciar()
        {
            proximo = Time.time + intervalo;
            foreach (var s in sistemas)
            {
                if (s == null) continue;
                s.Clear(true);
                s.Play(true);
            }
        }
    }
}
