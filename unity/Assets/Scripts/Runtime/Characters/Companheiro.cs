// claude: arquivo criado pelo Claude Code — bicho/companheiro que acompanha o dono (ex.: o gato do Caleb)
using System.Collections.Generic;
using ARS.Combat;
using ARS.Data;
using UnityEngine;

namespace ARS.Characters
{
    /// <summary>
    /// Acompanha o dono andando ao lado dele. Skills podem mandá-lo para outro lugar
    /// (<see cref="IrPara"/> / <see cref="FicarEm"/>), caçar numa área (<see cref="Cacar"/>)
    /// e depois devolvê-lo (<see cref="Seguir"/>).
    /// Não é combatente: não pode ser atacado e o que ele causa não conta como acerto do dono.
    /// Números PROVISÓRIOS.
    /// </summary>
    public class Companheiro : MonoBehaviour
    {
        [Tooltip("Quem ele acompanha (preenchido ao ser criado pelo dono).")]
        public Transform dono;
        [Tooltip("Onde fica em relação ao dono: x = de lado, z = à frente (negativo = atrás).")]
        public Vector3 posicaoAoLado = new Vector3(0.9f, 0f, -0.5f);
        [Tooltip("Velocidade andando (m/s).")]
        public float velocidadeAndando = 2f;
        [Tooltip("Velocidade correndo, quando ficou para trás ou caçando (m/s).")]
        public float velocidadeCorrendo = 5f;
        [Tooltip("A partir desta distância do lugar dele, corre.")]
        public float distanciaParaCorrer = 2.5f;
        [Tooltip("A partir desta distância, aparece direto ao lado do dono.")]
        public float distanciaParaTeleportar = 15f;
        [Tooltip("Considera que chegou a esta distância.")]
        public float distanciaParada = 0.25f;
        public float velocidadeGiro = 540f;

        // claude: caçada
        [Header("Caçada")]
        [Tooltip("Quanto cresce enquanto caça (1 = tamanho normal).")]
        public float escalaNaCaca = 2f;
        [Tooltip("Distância para conseguir atacar um inimigo.")]
        public float alcanceDoAtaque = 1f;
        [Tooltip("Marca deixada no lugar onde estava parado ao sair para caçar (ex.: patinha). Some quando ele volta ou passa a seguir o dono.")]
        public GameObject marcaAoSairDoPosto;

        static readonly int ParametroVelocidade = Animator.StringToHash("Velocidade");

        enum Modo { Seguindo, IndoPara, Parado, Cacando }
        Modo modo = Modo.Seguindo;
        Vector3 destino;
        Animator animator;
        Vector3 escalaNormal;

        // caçada em andamento
        Vector3 centroDaCaca;
        ZonaDef zonaDaCaca;
        IList<Efeito> efeitosDoAtaque;
        Combatente donoCombatente;
        float fimDaCaca, proximoAtaque;
        Combatente presa;
        Vector3 pontoDoSapo;
        // para onde vai depois da caçada
        bool depoisVoltaAoPosto;
        Vector3 posto;
        GameObject marca;

        /// <summary>true enquanto acompanha o dono (não foi mandado para outro lugar).</summary>
        public bool Seguindo => modo == Modo.Seguindo;
        public bool Cacando => modo == Modo.Cacando;

        void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            escalaNormal = transform.localScale;
        }

        /// <summary>Vai andando/correndo até um ponto e fica lá até mandarem seguir de novo.</summary>
        public void IrPara(Vector3 ponto)
        {
            if (modo == Modo.Cacando) { depoisVoltaAoPosto = true; posto = ponto; return; }
            destino = ponto; modo = Modo.IndoPara;
        }

        /// <summary>Aparece num ponto e fica parado lá (ex.: o dono teleportou e o deixou para trás).</summary>
        public void FicarEm(Vector3 ponto)
        {
            if (modo == Modo.Cacando)
            {
                // claude: mandado ficar no meio da caçada: marca o posto agora e volta para ele depois
                depoisVoltaAoPosto = true; posto = ponto;
                TirarMarca();
                if (marcaAoSairDoPosto != null) marca = Instantiate(marcaAoSairDoPosto, posto, Quaternion.identity);
                return;
            }
            transform.position = ponto; destino = ponto; modo = Modo.Parado;
        }

        /// <summary>Volta a acompanhar o dono (no meio da caçada, só depois que ela acabar).</summary>
        public void Seguir()
        {
            if (modo == Modo.Cacando) { depoisVoltaAoPosto = false; return; }
            TirarMarca();
            modo = Modo.Seguindo;
        }

        /// <summary>
        /// Caça na área: cresce, corre atrás dos inimigos de quem o mandou dentro do raio
        /// e ataca a cada intervalo (efeitos da zona × multiplicador). Sem inimigos, persegue
        /// pontos aleatórios (os sapos). Ao terminar, volta ao tamanho e ao que estava fazendo.
        /// </summary>
        public void Cacar(Vector3 centro, ZonaDef zona, float multiplicador, float duracao, Combatente quemMandou)
        {
            if (zona == null) return;
            if (modo != Modo.Cacando)
            {
                // estava parado num posto (ex.: deixado pela E): marca o lugar e volta para lá depois
                depoisVoltaAoPosto = modo == Modo.Parado || modo == Modo.IndoPara;
                posto = destino;
                if (depoisVoltaAoPosto && marcaAoSairDoPosto != null && marca == null)
                    marca = Instantiate(marcaAoSairDoPosto, posto, Quaternion.identity);
            }
            modo = Modo.Cacando;
            centroDaCaca = centro;
            zonaDaCaca = zona;
            donoCombatente = quemMandou;
            efeitosDoAtaque = AplicadorDeEfeitos.Multiplicados(zona.efeitosPorTique, multiplicador);
            fimDaCaca = Time.time + (duracao > 0f ? duracao : zona.duracao);
            proximoAtaque = Time.time;
            presa = null;
            pontoDoSapo = PontoAleatorio();
            fase = FaseDaCaca.IndoAoInimigo; // claude
            transform.localScale = escalaNormal * escalaNaCaca;
        }

        void Update()
        {
            if (dono == null) return;
            if (modo == Modo.Cacando) { AtualizarCaca(); return; }

            Vector3 alvo = modo == Modo.Seguindo ? LugarAoLado() : destino;
            Vector3 direcao = alvo - transform.position;
            direcao.y = 0f;
            float distancia = direcao.magnitude;

            if (modo == Modo.Seguindo && distancia > distanciaParaTeleportar)
            {
                transform.position = alvo;
                distancia = 0f;
            }

            float velocidade = 0f;
            if (modo != Modo.Parado && distancia > distanciaParada)
            {
                velocidade = distancia > distanciaParaCorrer ? velocidadeCorrendo : velocidadeAndando;
                Andar(direcao, distancia, velocidade);
            }
            else if (modo == Modo.IndoPara)
            {
                modo = Modo.Parado; // chegou no posto
            }
            else if (modo == Modo.Seguindo)
            {
                // parado ao lado: olha para onde o dono olha
                transform.rotation = Quaternion.RotateTowards(transform.rotation, dono.rotation, velocidadeGiro * 0.5f * Time.deltaTime);
            }

            // fica na altura do chão do dono (dungeon plana por enquanto)
            var p = transform.position;
            p.y = modo == Modo.Seguindo ? dono.position.y : destino.y;
            transform.position = p;

            Animar(velocidade);
        }

        void AtualizarCaca()
        {
            if (Time.time >= fimDaCaca)
            {
                TerminarCaca();
                return;
            }

            // claude: ciclo bote no inimigo → corrida até um sapo → bote... (sem inimigo: só sapos)
            if (fase == FaseDaCaca.Saltando) { Saltar(); return; }

            if (fase == FaseDaCaca.IndoAoInimigo)
            {
                if (presa == null || !presa.Vivo || Fora(presa.transform.position)) presa = InimigoMaisPerto();
                if (presa == null) { fase = FaseDaCaca.IndoAoSapo; }
                else
                {
                    Vector3 ate = presa.transform.position - transform.position; ate.y = 0f;
                    float dist = ate.magnitude;
                    if (dist <= distanciaDoBote && Time.time >= proximoAtaque) { ComecarBote(ate); return; }
                    Andar(ate, Mathf.Max(0f, dist - alcanceDoAtaque), velocidadeCorrendo);
                    NoChao();
                    Animar(velocidadeCorrendo);
                    return;
                }
            }

            // indo ao sapo
            Vector3 direcao = pontoDoSapo - transform.position; direcao.y = 0f;
            float distancia = direcao.magnitude;
            if (distancia <= distanciaParada * escalaNaCaca)
            {
                pontoDoSapo = PontoAleatorio();
                fase = FaseDaCaca.IndoAoInimigo; // pegou o sapo: próximo bote
            }
            else Andar(direcao, distancia, velocidadeCorrendo);
            NoChao();
            Animar(velocidadeCorrendo);
        }

        // claude: bote = salto em arco por cima do inimigo, mordendo no meio e caindo do outro lado
        [Tooltip("Distância em que começa o bote no inimigo.")]
        public float distanciaDoBote = 3f;
        [Tooltip("Quanto passa do inimigo ao cair do bote.")]
        public float distanciaAlemDoAlvo = 2f;
        public float alturaDoBote = 1.2f;
        public float duracaoDoBote = 0.45f;

        enum FaseDaCaca { IndoAoInimigo, Saltando, IndoAoSapo }
        FaseDaCaca fase;
        Vector3 inicioDoBote, fimDoBote;
        float progressoDoBote;
        bool mordeu;

        void ComecarBote(Vector3 ateOInimigo)
        {
            fase = FaseDaCaca.Saltando;
            inicioDoBote = transform.position;
            fimDoBote = presa.transform.position + ateOInimigo.normalized * distanciaAlemDoAlvo;
            fimDoBote.y = centroDaCaca.y;
            progressoDoBote = 0f;
            mordeu = false;
            proximoAtaque = Time.time + Mathf.Max(0.05f, zonaDaCaca.intervalo);
            transform.rotation = Quaternion.LookRotation(ateOInimigo, Vector3.up);
        }

        void Saltar()
        {
            progressoDoBote += Time.deltaTime / Mathf.Max(0.05f, duracaoDoBote);
            float t = Mathf.Clamp01(progressoDoBote);
            var p = Vector3.Lerp(inicioDoBote, fimDoBote, t);
            p.y = centroDaCaca.y + Mathf.Sin(t * Mathf.PI) * alturaDoBote;
            transform.position = p;
            Animar(velocidadeCorrendo);

            if (!mordeu && t >= 0.5f)
            {
                mordeu = true;
                if (presa != null && presa.Vivo)
                {
                    var contexto = new ContextoDoEfeito { origem = donoCombatente, pontoMirado = presa.transform.position, pontoDePartida = transform.position };
                    AplicadorDeEfeitos.Aplicar(efeitosDoAtaque, presa, contexto);
                }
            }
            if (t >= 1f)
            {
                NoChao();
                pontoDoSapo = PontoAleatorio();
                fase = FaseDaCaca.IndoAoSapo;
            }
        }

        void NoChao()
        {
            var p = transform.position;
            p.y = centroDaCaca.y;
            transform.position = p;
        }

        void TerminarCaca()
        {
            transform.localScale = escalaNormal;
            presa = null;
            if (depoisVoltaAoPosto)
            {
                destino = posto;
                modo = Modo.IndoPara;
            }
            else
            {
                TirarMarca();
                modo = Modo.Seguindo;
            }
        }

        void TirarMarca()
        {
            if (marca != null) Destroy(marca);
            marca = null;
        }

        void Andar(Vector3 direcao, float distancia, float velocidade)
        {
            if (distancia <= 0f) return;
            Vector3 passo = direcao.normalized * Mathf.Min(velocidade * Time.deltaTime, distancia);
            transform.position += passo;
            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                Quaternion.LookRotation(direcao, Vector3.up), velocidadeGiro * Time.deltaTime);
        }

        void Animar(float velocidade)
        {
            if (animator != null && animator.isActiveAndEnabled && animator.runtimeAnimatorController != null)
                animator.SetFloat(ParametroVelocidade, velocidade, 0.1f, Time.deltaTime);
        }

        bool Fora(Vector3 ponto)
        {
            Vector3 d = ponto - centroDaCaca;
            d.y = 0f;
            return d.sqrMagnitude > zonaDaCaca.raio * zonaDaCaca.raio;
        }

        Combatente InimigoMaisPerto()
        {
            Combatente melhor = null;
            float melhorDist = float.MaxValue;
            foreach (var c in Combatente.Todos)
            {
                if (c == null || !c.Vivo || donoCombatente == null || !donoCombatente.EhInimigoDe(c)) continue;
                if (Fora(c.transform.position)) continue;
                float d = (c.transform.position - transform.position).sqrMagnitude;
                if (d < melhorDist) { melhorDist = d; melhor = c; }
            }
            return melhor;
        }

        Vector3 PontoAleatorio()
        {
            Vector2 r = Random.insideUnitCircle * (zonaDaCaca != null ? zonaDaCaca.raio : 1f);
            return centroDaCaca + new Vector3(r.x, 0f, r.y);
        }

        Vector3 LugarAoLado() => dono.TransformPoint(posicaoAoLado);
    }
}
