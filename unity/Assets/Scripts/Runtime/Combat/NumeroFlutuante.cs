using UnityEngine;

namespace ARS.Combat
{
    /// <summary>Número de dano que sobe e some em cima de quem apanhou.</summary>
    public class NumeroFlutuante : MonoBehaviour
    {
        const float Duracao = 0.9f;
        const float Subida = 1.2f;

        static Font fonte;

        TextMesh texto;
        Color corInicial;
        float tempo;

        public static void Mostrar(Vector3 posicao, float quantidade, TipoDeDano tipo)
        {
            if (fonte == null) fonte = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            var objeto = new GameObject("Dano");
            objeto.transform.position = posicao + Random.insideUnitSphere * 0.25f;

            var numero = objeto.AddComponent<NumeroFlutuante>();
            numero.texto = objeto.AddComponent<TextMesh>();
            numero.texto.font = fonte;
            numero.texto.text = Mathf.RoundToInt(quantidade).ToString();
            numero.texto.fontSize = 64;
            numero.texto.characterSize = 0.05f;
            numero.texto.anchor = TextAnchor.MiddleCenter;
            numero.texto.alignment = TextAlignment.Center;
            numero.corInicial = tipo == TipoDeDano.Magico ? new Color(0.55f, 0.75f, 1f) : new Color(1f, 0.85f, 0.3f);
            numero.texto.color = numero.corInicial;
            objeto.GetComponent<MeshRenderer>().sharedMaterial = fonte.material;
        }

        void LateUpdate()
        {
            tempo += Time.deltaTime;
            transform.position += Vector3.up * (Subida / Duracao) * Time.deltaTime;

            // Sempre virado para a câmera
            var camera = UnityEngine.Camera.main;
            if (camera != null) transform.rotation = camera.transform.rotation;

            var cor = corInicial;
            cor.a = 1f - tempo / Duracao;
            texto.color = cor;

            if (tempo >= Duracao) Destroy(gameObject);
        }
    }
}
