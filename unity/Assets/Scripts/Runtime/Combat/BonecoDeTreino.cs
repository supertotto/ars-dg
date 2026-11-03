using UnityEngine;

namespace ARS.Combat
{
    /// <summary>
    /// Boneco de treino: aguenta tudo, mostra a vida em cima da cabeça e volta à vida cheia
    /// depois de um tempo sem apanhar (ou quando a vida acaba).
    /// </summary>
    [RequireComponent(typeof(Combatente))]
    public class BonecoDeTreino : MonoBehaviour
    {
        public float segundosParaRecuperar = 3f;

        Combatente combatente;
        TextMesh placa;
        float ultimoDano = -999f;

        void Awake()
        {
            combatente = GetComponent<Combatente>();
            combatente.AoReceberDano += (_, __, ___) => ultimoDano = Time.time;
            combatente.AoMorrer += _ => Invoke(nameof(Recuperar), 0.5f);

            var fonte = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            var objeto = new GameObject("Vida");
            objeto.transform.SetParent(transform, false);
            objeto.transform.localPosition = Vector3.up * (combatente.alturaDoCorpo + 1.3f);
            placa = objeto.AddComponent<TextMesh>();
            placa.font = fonte;
            placa.fontSize = 48;
            placa.characterSize = 0.05f;
            placa.anchor = TextAnchor.MiddleCenter;
            placa.color = Color.white;
            objeto.GetComponent<MeshRenderer>().sharedMaterial = fonte.material;
        }

        void Update()
        {
            if (combatente.Vivo && combatente.Vida < combatente.vidaMaxima &&
                Time.time - ultimoDano > segundosParaRecuperar)
                Recuperar();

            placa.text = $"{Mathf.CeilToInt(combatente.Vida)} / {Mathf.CeilToInt(combatente.vidaMaxima)}";
            var camera = UnityEngine.Camera.main;
            if (camera != null) placa.transform.rotation = camera.transform.rotation;
        }

        void Recuperar() => combatente.RestaurarVida();
    }
}
