# 03 — Sistemas de jogo

## Grupo e modos

- **Grupo de 6** (decisão de 05/11/2026): 1 tank, 1 healer, 3 DPS e **1 controlador**.
- **Solo:** o jogador controla um herói; os outros 5 são heróis controlados pela IA, como os seguidores do WoW. O jogo sorteia heróis para preencher as funções que faltam.
- **Co-op:** jogadores reais ocupam as vagas; a IA completa o que faltar (exceto no Mítico).
- **Controlador** (inspirado no papel de Controlador do D&D 4e): ocupa uma vaga própria. É um herói **misturado**, que causa dano e controla a luta. Cada controlador puxa mais para um lado:
  - **zonas e terreno:** áreas que punem quem atravessa e decidem onde vale a pena o inimigo ficar;
  - **enfraquecer:** o inimigo causa menos dano ou recebe mais dano (não é controle, então funciona em chefes);
  - **adds:** limpar e prender os inimigos que o chefe invoca;
  - **interromper** magias interrompíveis.
- A regra **chefes imunes a controle continua**: contra chefes, o controlador trabalha com zonas, enfraquecimento, adds e interrupção.
- Os outros heróis continuam tendo controle como função secundária, em maior ou menor grau.
- Com heróis fixos e tudo em dados, novos heróis e classes podem entrar no futuro.

## Kit do personagem

Cada herói tem **8 ações ativas** (uma por botão) e **2 passivas**.

| Origem | Ativas | Passivas |
| --- | --- | --- |
| Herói (kit próprio) | 3 skills + 1 ultimate | 1 |
| Raça | 1 | 1 |
| Feitiços | 2, escolhidos antes da dungeon | — |
| Consumível | 1 atalho | — |

- **Ataque básico:** botão direito, **automático, estilo LoL** — um clique no inimigo e o personagem segue atacando até receber outra ordem. Passivas que contam golpes contam os ataques básicos.
- **Feitiços:** livres para qualquer classe, como os feitiços de invocador do LoL, com recarga longa. Ideias (a confirmar): Lampejo, Barreira, Purificar, Fôlego, Grito de Fuga, Exaustão, Brasa, Olho do Caçador. Algumas dungeons podem dar um feitiço temático que só funciona lá dentro.

## Controles

| Plataforma | Esquema |
| --- | --- |
| PC | Clique para mover, skills no QWER, ataque básico automático no botão direito |
| Console | Esquema próprio com analógico e 8 botões (4 frontais + 4 de ombro e gatilho) — a desenhar na fase de console |

## Dano e elementos

O dano é só **físico** ou **mágico**. Os elementos não são tipos de dano: cada um aplica um **efeito próprio**, sem combinações entre eles. **Todo dano elemental conta como mágico**; uma mesma skill pode misturar dano físico e mágico.

| Elemento | Efeito |
| --- | --- |
| Fogo | Queimadura: dano ao longo do tempo |
| Gelo | Lentidão, que vira congelamento se acumular |
| Raio | Choque: atordoamento curto |
| Água | Encharcado: ataques mais lentos e recargas mais longas (curto e com valor pequeno) |
| Ar | Rajada: empurra para longe ou joga para o alto |
| Terra | Enraizar: prende no lugar |
| Veneno | Dano ao longo do tempo e menos cura recebida |
| Sombra | Silêncio: impede usar skills por um instante |
| Sagrado | Purifica aliados ou enfraquece os profanos |

## Atributos

Cinco atributos base; cada um gera estatísticas. Roubo de vida, redução de recarga e tenacidade **não são estatísticas**: entram como efeito de item, raça ou skill.

| Atributo | Gera |
| --- | --- |
| Força | Dano físico e dano do ataque básico |
| Agilidade | Velocidade de movimento, velocidade de ataque, esquiva e crítico |
| Vigor | Vida e armadura |
| Inteligência | Dano mágico e resistência mágica |
| Sabedoria | Poder de cura e regeneração de recurso |

> **Atenção no balanceamento:** a Agilidade gera quatro estatísticas, então o valor por ponto precisa ser menor.

**Atributo principal por subclasse:**

| Principal | Subclasses |
| --- | --- |
| Vigor | Cavaleiro, Paladino, Guardião da Floresta |
| Agilidade | Espadachim, Assassino, Ranger |
| Inteligência | Bruxo, Mago |
| Sabedoria | Lanceiro, Invocador, Clérigo, Bardo, Druida |
| Força | Bárbaro, Inquisidor |

- **Crescimento:** fixo por nível, como no Dota — cada subclasse tem a própria curva. O jogador personaliza pelo equipamento e pelos talentos.
- **Raça:** cada raça ganha um pouco num atributo e perde em outro (ver [Raças](05-racas.md)).

## Recursos

| Classe | Recurso | Como funciona |
| --- | --- | --- |
| Guerreiro | Fúria | Enche batendo e apanhando; esvazia fora de combate |
| Arcano | Mana | Começa cheia, é gasta e recupera devagar |
| Sacerdotes | Fé | **5 cargas**, como contas de oração; voltam uma a uma com o tempo, e mais rápido ao acertar inimigos (principalmente profanos) e ao receber dano |
| Ladinos | Energia | Regenera rápido, mas o máximo é pequeno |
| Naturalistas | Essência | Enche com o tempo, mais rápido em movimento |

**Efeito da Sabedoria em cada recurso:**

| Recurso | Efeito |
| --- | --- |
| Fúria | Ganha mais por golpe e esvazia mais devagar fora de combate |
| Mana | Regenera mais rápido |
| Fé | As cargas voltam mais rápido |
| Energia | Regenera mais rápido |
| Essência | Enche mais rápido, parado ou em movimento |

## Nível dos heróis e nivelamento

- **Cada herói sobe de nível individualmente.** O desafio de longo prazo é upar todos.
- **Nível máximo:** entre 20 e 30 (número exato em aberto). Progressão lenta.
- **Aliados da IA no solo:** entram no nível do herói que o jogador está usando.
- **Aliados da IA em grupo com jogadores reais:** entram na média do nível dos jogadores.
- **Nivelamento para baixo:** quem está acima da faixa da dungeon tem o nível reduzido até o máximo dela (ex.: um jogador 15 jogando com um jogador 4). **Só os atributos são reduzidos**; skills, melhorias e talentos continuam valendo.

## Árvore de talentos

Cada subclasse tem uma árvore própria, **híbrida**, com **um ponto por nível**.

| Tipo de nó | O que faz |
| --- | --- |
| Pequeno (a maioria) | Aumenta atributos e estatísticas: vida, recurso, penetração, crítico e afins |
| Médio | Melhora uma skill: mais dano, menos recarga ou um efeito extra |
| Grande (marcos, ex.: níveis 10, 20 e 30) | Escolha entre duas opções que muda o jeito de jogar, podendo **trocar uma skill inteira** por uma alternativa |

Com o nivelamento para baixo, os atributos dos nós pequenos são reduzidos, mas melhorias e trocas de skill continuam valendo.

## Regras de combate

- **Invisibilidade** não dá invulnerabilidade: dano em área ainda acerta quem está invisível.
- **Controle em chefes:** chefes são **imunes a controle** (atordoar, enraizar, lentidão, transformar, empurrar…), como no WoW. Funciona **interromper** uma magia que o chefe está conjurando, quando ela for interrompível.
- **Taunt não é controle:** é só a ferramenta do tank para segurar a ameaça (não deixar o dragão soltar o sopro no healer) e **funciona em chefes**.
- **Golpes em área:** os golpes dos chefes acertam uma área (cone na frente, círculo em volta), não só o tank. Quem estiver na área leva dano — posicionamento vale para todos.
- **Morte de um herói:** quem cai **fica caído até o fim da batalha**; ninguém revive durante a luta. Acabou a batalha (contra inimigos comuns ou chefe), os caídos **levantam** e seguem com o grupo. Nenhum herói tem ressurreição no kit.
- **Derrota (estilo WoW):** se o grupo inteiro cair, todos voltam ao último **ponto seguro** (checkpoint) e o chefe reseta, **sem limite de tentativas**. A dungeon nunca falha por derrota.
- **Pontos seguros (checkpoints):** ficam **depois de cada chefe e subchefe** (e, portanto, antes do próximo trecho), com a loja de consumíveis.

## Modos de dificuldade

| Modo | Quando | Como funciona |
| --- | --- | --- |
| Normal | Na subida de nível | A dungeon como desenhada |
| Heroico | No nível máximo | Inimigos mais fortes, mecânicas extras, loot melhor |
| Mítico | No nível máximo | Degraus com cronômetro e modificadores semanais; **só jogadores reais** |

**Mítico:**

- **Chave:** terminar no tempo sobe a chave de degrau; fora do tempo, ela desce.
- **Sem aliados de IA**, desde o primeiro degrau → exige **buscador de grupo** no menu do hub.
- **Modificadores que se somam:**
  - Degrau 2: alterna toda semana entre inimigos comuns reforçados e chefes reforçados
  - Degrau 4: entra um modificador da lista
  - Degrau 7: entra um segundo da lista
  - Degrau 10: entra um modificador de temporada
- **Lista (ideias):** Explosivo (bomba ao morrer), Elemental (elemento da semana em todos os inimigos), Fúria (mais fortes com vida baixa), Sanguinário (poças que curam inimigos), Tormenta (redemoinhos que empurram).
- **Mecânicas pesadas** (como a possessão da Banshee) podem ficar só no Heroico e no Mítico.
- **Desafio final no Mítico:** Montanha e Covil continuam emendados, com cronômetro longo. Libertar todos os anões custa tempo, mas garante o Golem como aliado.

## Hub

- O hub é o **salão da Ordem no Caos**, privado (só o jogador e o grupo), com NPCs.
- Lá o jogador compra, vende, recebe missões e entra nas dungeons.
- Negociação com outros jogadores: **casa de leilões ou quadro de trocas** (exige servidor simples).
- Um hub social, com outros jogadores andando, pode vir numa fase futura.
