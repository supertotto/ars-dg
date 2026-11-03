using UnityEngine;

namespace ARS.Combat
{
    /// <summary>Projétil teleguiado simples: persegue o alvo e causa dano ao chegar.</summary>
    public class Projetil : MonoBehaviour
    {
        Combatente alvo;
        Combatente origem;
        float dano;
        TipoDeDano tipo;
        float velocidade;
        float vidaRestante = 5f;

        static Material materialBase;

        public static void Disparar(Vector3 de, Combatente alvo, Combatente origem, float dano, TipoDeDano tipo,
                                    float velocidade, Color cor)
        {
            var objeto = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            objeto.name = "Projetil";
            Destroy(objeto.GetComponent<Collider>());
            objeto.transform.position = de;
            objeto.transform.localScale = Vector3.one * 0.25f;

            var render = objeto.GetComponent<MeshRenderer>();
            if (materialBase == null)
            {
                var shader = Shader.Find("Universal Render Pipeline/Unlit");
                if (shader != null) materialBase = new Material(shader);
            }
            if (materialBase != null)
            {
                var material = new Material(materialBase);
                material.SetColor("_BaseColor", cor);
                render.sharedMaterial = material;
            }
            render.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            var projetil = objeto.AddComponent<Projetil>();
            projetil.alvo = alvo;
            projetil.origem = origem;
            projetil.dano = dano;
            projetil.tipo = tipo;
            projetil.velocidade = velocidade;
        }

        void Update()
        {
            vidaRestante -= Time.deltaTime;
            if (alvo == null || !alvo.Vivo || vidaRestante <= 0f)
            {
                Destroy(gameObject);
                return;
            }

            Vector3 destino = alvo.Centro;
            Vector3 posicao = Vector3.MoveTowards(transform.position, destino, velocidade * Time.deltaTime);
            transform.position = posicao;

            if ((destino - posicao).sqrMagnitude < 0.04f)
            {
                alvo.ReceberDano(dano, tipo, origem);
                Destroy(gameObject);
            }
        }
    }
}
