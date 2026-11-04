// claude: arquivo criado pelo Claude (app claude.ai)
using ARS.Characters;
using ARS.Combat;
using UnityEngine;

namespace ARS.UI
{
    /// <summary>
    /// HUD provisório (letras grandes): vida, recurso e as skills Q/W/E/R com cargas e recarga.
    /// Só aparece no personagem controlado pelo jogador. Será trocado pela interface de verdade.
    /// </summary>
    public class HudProvisorio : MonoBehaviour
    {
        [Tooltip("Tamanho da letra do HUD.")]
        public int tamanhoDaLetra = 26;

        Combatente vida;
        Recurso recurso;
        KitDeSkills kit;
        ComandosDoJogador comandos;
        GUIStyle estilo, estiloCaixa;
        Texture2D fundo;

        void Awake()
        {
            vida = GetComponent<Combatente>();
            recurso = GetComponent<Recurso>();
            kit = GetComponent<KitDeSkills>();
            comandos = GetComponent<ComandosDoJogador>();
        }

        void OnGUI()
        {
            if (comandos == null || !comandos.enabled) return;
            PrepararEstilos();

            float escala = tamanhoDaLetra / 26f;
            float x = 20f, y = Screen.height - 230f * escala;

            if (vida != null)
                Linha(ref x, ref y, $"Vida  {Mathf.CeilToInt(vida.Vida)} / {Mathf.CeilToInt(vida.vidaMaxima)}", escala);
            if (recurso != null)
                Linha(ref x, ref y, $"{recurso.tipo}  {Mathf.FloorToInt(recurso.Atual)} / {Mathf.CeilToInt(recurso.maximo)}", escala);

            if (kit != null)
            {
                float bx = 20f, by = y + 8f * escala, largura = 150f * escala, altura = 70f * escala;
                foreach (Tecla tecla in System.Enum.GetValues(typeof(Tecla)))
                {
                    var estado = kit.EstadoDe(tecla);
                    string texto;
                    if (estado == null || estado.skill == null) texto = $"{tecla}\n—";
                    else
                    {
                        float falta = kit.RecargaRestante(tecla);
                        string cargas = estado.skill.cargas > 1 ? $"  [{estado.cargas}/{estado.skill.cargas}]" : "";
                        string tempo = falta > 0f && estado.cargas == 0 ? $"  {falta:0.0}s" : "";
                        texto = $"{tecla}{cargas}{tempo}\n{estado.skill.nome}";
                    }
                    GUI.Box(new Rect(bx, by, largura, altura), texto, estiloCaixa);
                    bx += largura + 8f * escala;
                }

                if (!string.IsNullOrEmpty(kit.UltimaMensagem))
                    GUI.Label(new Rect(20f, by - 2f * (tamanhoDaLetra + 10f), 800f, tamanhoDaLetra + 12f), kit.UltimaMensagem, estilo);
            }
        }

        void Linha(ref float x, ref float y, string texto, float escala)
        {
            GUI.Label(new Rect(x, y, 600f, tamanhoDaLetra + 10f), texto, estilo);
            y += tamanhoDaLetra + 10f * escala;
        }

        void PrepararEstilos()
        {
            if (estilo != null && estilo.fontSize == tamanhoDaLetra) return;
            fundo = new Texture2D(1, 1);
            fundo.SetPixel(0, 0, new Color(0f, 0f, 0f, 0.6f));
            fundo.Apply();

            estilo = new GUIStyle(GUI.skin.label) { fontSize = tamanhoDaLetra, fontStyle = FontStyle.Bold };
            estilo.normal.textColor = Color.white;

            estiloCaixa = new GUIStyle(GUI.skin.box)
            {
                fontSize = Mathf.RoundToInt(tamanhoDaLetra * 0.7f),
                alignment = TextAnchor.MiddleCenter,
                wordWrap = true
            };
            estiloCaixa.normal.background = fundo;
            estiloCaixa.normal.textColor = Color.white;
        }
    }
}
