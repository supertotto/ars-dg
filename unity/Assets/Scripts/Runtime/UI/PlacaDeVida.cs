// claude: arquivo criado pelo Claude Code — vida em cima da cabeça (provisório, só leitura)
using ARS.Combat;
using UnityEngine;

namespace ARS.UI
{
    /// <summary>Mostra "vida / máxima" em cima do combatente, virado para a câmera. Sem regra de jogo.</summary>
    [RequireComponent(typeof(Combatente))]
    public class PlacaDeVida : MonoBehaviour
    {
        [Tooltip("Altura acima do centro do corpo.")]
        public float alturaExtra = 1.2f;

        Combatente combatente;
        TextMesh placa;

        void Awake()
        {
            combatente = GetComponent<Combatente>();
            var fonte = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            var objeto = new GameObject("Vida");
            objeto.transform.SetParent(transform, false);
            objeto.transform.localPosition = Vector3.up * (combatente.alturaDoCorpo + alturaExtra);
            placa = objeto.AddComponent<TextMesh>();
            placa.font = fonte;
            placa.fontSize = 48;
            placa.characterSize = 0.04f;
            placa.anchor = TextAnchor.MiddleCenter;
            placa.color = new Color(1f, 0.55f, 0.5f);
            objeto.GetComponent<MeshRenderer>().sharedMaterial = fonte.material;
        }

        void LateUpdate()
        {
            placa.text = combatente.Vivo ? $"{Mathf.CeilToInt(combatente.Vida)} / {Mathf.CeilToInt(combatente.vidaMaxima)}" : "";
            var camera = UnityEngine.Camera.main;
            if (camera != null) placa.transform.rotation = camera.transform.rotation;
        }
    }
}
