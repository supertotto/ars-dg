using System;
using UnityEngine;

namespace ARS.Combat
{
    /// <summary>Projétil teleguiado simples: persegue o alvo e causa dano ao chegar.</summary>
    public class Projetil : MonoBehaviour
    {
        Combatente alvo;
        Action<Combatente> aoAcertar;
        float velocidade;
        float vidaRestante = 5f;

        static Material materialBase;

        /// <summary>Projétil que causa dano simples ao acertar (ataque básico).</summary>
        public static void Disparar(Vector3 de, Combatente alvo, Combatente origem, float dano, TipoDeDano tipo,
                                    float velocidade, Color cor, float tamanho = 0.25f, bool rastro = true,
                                    GameObject visual = null)
        {
            Disparar(de, alvo, velocidade, cor, tamanho, rastro, visual,
                     atingido => atingido.ReceberDano(dano, tipo, origem));
        }

        /// <summary>Projétil genérico: ao acertar, chama "aoAcertar" com o alvo (ex.: aplicar efeitos de uma skill).</summary>
        public static void Disparar(Vector3 de, Combatente alvo, float velocidade, Color cor, float tamanho, bool rastro,
                                    GameObject visual, Action<Combatente> aoAcertar)
        {
            GameObject objeto;
            if (visual != null)
            {
                objeto = Instantiate(visual, de, Quaternion.identity);
                objeto.name = "Projetil";
                foreach (var colisor in objeto.GetComponentsInChildren<Collider>()) Destroy(colisor);
                // Quem move o projétil é este script: física do pack (gravidade, empurrão) fica desligada
                foreach (var corpo in objeto.GetComponentsInChildren<Rigidbody>())
                {
                    corpo.isKinematic = true;
                    corpo.useGravity = false;
                }
            }
            else
            {
                objeto = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                objeto.name = "Projetil";
                Destroy(objeto.GetComponent<Collider>());
                objeto.transform.position = de;
                objeto.transform.localScale = Vector3.one * tamanho;

                var render = objeto.GetComponent<MeshRenderer>();
                render.sharedMaterial = MaterialColorido(cor);
                render.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            }

            if (rastro)
            {
                var trilha = objeto.AddComponent<TrailRenderer>();
                trilha.time = 0.25f;
                trilha.startWidth = tamanho * 0.8f;
                trilha.endWidth = 0f;
                trilha.sharedMaterial = MaterialColorido(cor);
                var corFinal = cor;
                corFinal.a = 0f;
                trilha.startColor = cor;
                trilha.endColor = corFinal;
                trilha.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            }

            var projetil = objeto.AddComponent<Projetil>();
            projetil.alvo = alvo;
            projetil.aoAcertar = aoAcertar;
            projetil.velocidade = velocidade;
        }

        static Material MaterialColorido(Color cor)
        {
            if (materialBase == null)
            {
                var shader = Shader.Find("Universal Render Pipeline/Unlit");
                if (shader == null) shader = Shader.Find("Sprites/Default");
                materialBase = new Material(shader);
            }
            var material = new Material(materialBase);
            if (material.HasProperty("_BaseColor")) material.SetColor("_BaseColor", cor);
            if (material.HasProperty("_Color")) material.SetColor("_Color", cor);
            return material;
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
            Vector3 direcao = destino - posicao;
            if (direcao.sqrMagnitude > 0.0001f) transform.rotation = Quaternion.LookRotation(direcao);
            transform.position = posicao;

            if ((destino - posicao).sqrMagnitude < 0.04f)
            {
                aoAcertar?.Invoke(alvo);
                Destroy(gameObject);
            }
        }
    }
}
