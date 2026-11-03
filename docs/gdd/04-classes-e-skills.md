# 04 — Classes e skills

São **5 classes base**, cada uma com **3 subclasses de função fixa** (tank, healer, DPS) no lançamento — 15 subclasses.

| Classe | Recurso | Tank | Healer | DPS |
| --- | --- | --- | --- | --- |
| Guerreiro | Fúria | Cavaleiro | Lanceiro | Bárbaro |
| Arcano | Mana | Bruxo | Invocador | Mago |
| Sacerdotes | Fé | Paladino | Clérigo | Inquisidor |
| Ladinos | Energia | Espadachim | Bardo | Assassino |
| Naturalistas | Essência | Guardião da Floresta | Druida | Ranger |

> **Decisão (03/11/2026): cada herói fixo tem o seu próprio kit** (3 skills + ultimate + passiva), especializado na personalidade e na história dele (ex.: o Ignis só usa bola de fogo, então o kit dele é todo de fogo). Os heróis já começam com o kit próprio.
>
> A **subclasse** continua definindo a função (tank, healer, DPS), o recurso, o atributo principal, a arma e o acessório.
>
> **Os kits por subclasse abaixo são da versão anterior** e serão refeitos herói por herói (em andamento). Skills deles podem ser reaproveitadas nos kits novos.

## Padrão das teclas (vale para todos os heróis)

Para o jogador não errar a tecla ao trocar de herói (o problema do pulo da Tristana no W e do dash do Lucian no E, no LoL):

| Tecla | Papel |
| --- | --- |
| **Q** | Skill principal: o ataque ou a cura principal do herói |
| **W** | Skill de apoio ou efeito específico do herói |
| **E** | **Deslocamento**, quando o herói tiver um. Herói sem deslocamento usa o E para outra coisa |
| **R** | Ultimate, cada uma do seu jeito |

Heróis podem ter mais ou menos mobilidade, mas, quando houver um deslocamento, ele fica no **E**.

## Kits por herói (em andamento)

### Ignis — Mago · tema: só bola de fogo

| Tecla | Skill | Efeito |
| --- | --- | --- |
| Q | Bola de Fogo | Projétil que explode no impacto e deixa queimadura |
| W | Órbita de Brasas | Três bolinhas de fogo giram em volta dele e queimam quem encosta; usar o W de novo lança as bolinhas na direção do mouse |
| E | Bola Propulsora | Explode uma bola nos próprios pés: ele é arremessado para trás e queima quem estava perto |
| R | Bola de fogo gigante | Em aberto: carregada, caindo do céu ou rolando pela arena |
| Passiva | — | Em aberto |

### Caleb — Mago · tema: aprendiz atrapalhado

- **Decisão:** o caos do Caleb (feitiços que saem errados) fica **só no visual**: o kit é previsível, e o resultado aparece na aparência (míssil em forma de sapo, explosão que solta borboletas).
- Kit em revisão (proposta vinda do ChatGPT): Q mísseis perseguidores com 3 cargas · W transforma um inimigo em animal · E teleporte com dano e volta ao ponto inicial · R chuva de sapinhos e gato gigante · passiva de skill fortalecida + ataque básico que recupera Mana.
- Pontos a resolver: E muito parecido com o W da LeBlanc (LoL); passiva com funções demais; regra da transformação em chefes.

## Modo customizado (futuro)

- O personagem customizado começa com um **kit básico da subclasse** (ex.: o Mago começa com o Q *Disparo Místico*).
- Ao atingir certos níveis (ex.: nível 3), ele **escolhe uma das versões dos heróis daquela subclasse** para evoluir aquela skill (ex.: o Q vira *Mísseis Mágicos*, do Caleb, ou *Bola de Fogo*, do Ignis).
- O elenco fixo vira a **biblioteca de skills** do modo customizado: cada herói novo aumenta as opções.
- Em aberto: os níveis de escolha, se a ultimate e a passiva também entram nisso, e os kits básicos de cada subclasse.

## Armas e acessórios

> **Decisão (03/11/2026):** a subclasse define a **categoria** da arma; cada herói tem a **forma** dele (ex.: Bárbaro = machados → a Bella usa um machado grande de duas mãos, o Brok usa dois machados). No modo customizado, o jogador escolhe a subclasse e a forma da arma.
>
> **A forma muda o ataque básico e dá um bônus pequeno:** arma de duas mãos bate mais forte e mais devagar (ex.: bônus de penetração de armadura); arma dupla bate mais rápido e mais fraco (ex.: bônus de chance de crítico). As skills não mudam. Os bônus de cada forma estão em aberto.
>
> **Arma de duas mãos** ocupa os dois espaços (arma e acessório) e soma o efeito do acessório na própria arma.

A tabela abaixo é da versão anterior (arma por subclasse) e vale como **categoria** e referência.

A segunda mão é um **acessório próprio da subclasse**. (versão anterior)

| Subclasse | Arma principal | Acessório |
| --- | --- | --- |
| Cavaleiro | Espada longa | Escudo pesado |
| Lanceiro | Lança | Estandarte de guerra |
| Bárbaro | Machado de lâmina dupla | Segundo machado |
| Bruxo | Foice | Coração de demônio num frasco |
| Invocador | Cetro | Orbe de invocação |
| Mago | Cajado | Grimório |
| Paladino | Martelo de guerra | Égide de luz |
| Clérigo | Mangual | Relicário |
| Inquisidor | Maça | Besta de mão |
| Espadachim | Rapieira | Adaga de aparar |
| Bardo | Sabre | Alaúde |
| Assassino | Adaga | Segunda adaga, idêntica |
| Guardião da Floresta | Clava de madeira viva | Escudo de casca |
| Druida | Cajado vivo | Totem natural |
| Ranger | Arco longo | Aljava |

---

## Guerreiro (Fúria)

### Cavaleiro — tank do bloqueio · principal: Vigor

| Espaço | Skill | Efeito |
| --- | --- | --- |
| Skill 1 | Golpe de Escudo | Dano, provoca o alvo e o deixa mais lento |
| Skill 2 | Muralha | Bloqueia quase todo o dano frontal por alguns segundos, andando devagar |
| Skill 3 | Interceptar | Corre até um aliado e fica entre ele e o perigo |
| Ultimate | Bastião | Finca a espada no chão e cria uma área em que os aliados recebem muito menos dano |
| Passiva | Escudo Erguido | Bloqueia automaticamente parte dos ataques que vêm de frente |

### Lanceiro — healer de posicionamento · principal: Sabedoria

| Espaço | Skill | Efeito |
| --- | --- | --- |
| Skill 1 | Estocada Revigorante | Acerta o inimigo e cura o aliado mais ferido por perto |
| Skill 2 | Plantar Estandarte | Zona que cura quem estiver dentro |
| Skill 3 | Chamado ao Estandarte | Puxa um aliado para dentro da zona |
| Ultimate | Avante! | O estandarte cura o grupo inteiro de uma vez, onde quer que esteja |
| Passiva | Vigor | Cada golpe deixa a cura do estandarte mais forte, até um limite |

### Bárbaro — DPS de fúria corpo a corpo · principal: Força

| Espaço | Skill | Efeito |
| --- | --- | --- |
| Skill 1 | Talho Duplo | Os dois machados juntos, num dano forte num alvo |
| Skill 2 | Arremesso | Joga um machado, que vai e volta acertando quem estiver no caminho |
| Skill 3 | Redemoinho Furioso | Gira por alguns segundos, andando livremente, ignorando lentidão e enraizamento e acertando todos em volta (como o giro do Garen) |
| Ultimate | Fúria Total | Ataca muito mais rápido, não pode ser atordoado e se cura com parte do dano causado |
| Passiva | Sede de Batalha | Cada golpe aumenta um pouco o dano, até um limite; zera se ficar muito tempo sem bater |

## Arcano (Mana)

### Bruxo — tank pelo demônio · principal: Inteligência

O demônio invocado tanka; o Bruxo reforça de trás.

| Espaço | Com o demônio | Sem o demônio |
| --- | --- | --- |
| Skill 1 | Ordem de Provocar: o rugido do demônio provoca os inimigos em volta | Fuga: um salto para escapar |
| Skill 2 | Pacto de Sangue: o Bruxo gasta a própria vida para curar o demônio | Contrato de Emergência: um escudo no próprio Bruxo |
| Skill 3 | Ceifada: gira a foice e dispara uma lâmina de ar à distância que reduz o dano dos inimigos atingidos | Ceifada, com recarga menor |
| Ultimate | Contrato Infernal: liga os inimigos próximos ao demônio, e o dano que ele recebe é dividido entre eles | Indisponível até o demônio voltar |
| Passiva | **Estilo Kled:** quando a vida do demônio acaba, ele some e o Bruxo luta sozinho, mais fraco; enquanto luta, enche uma barra (causando dano ou com o tempo) que, completa, invoca o demônio de volta | — |

### Invocador — healer de espíritos · principal: Sabedoria

| Espaço | Skill | Efeito |
| --- | --- | --- |
| Skill 1 | Toque Espiritual | Um espírito voa até o aliado e cura de uma vez |
| Skill 2 | Coro dos Espíritos | Cura em pulsos e reduz um pouco o dano recebido |
| Skill 3 | Passagem Espiritual | Vira espírito por um instante, intangível, e causa dano nos inimigos que atravessa |
| Ultimate | Revoada de Espíritos | Cura enorme; por alguns segundos os aliados não podem cair |
| Passiva | Espíritos Inquietos | Cada cura também dá um pequeno escudo |

### Mago — DPS de magia à distância · principal: Inteligência

| Espaço | Skill | Efeito |
| --- | --- | --- |
| Skill 1 | Bola de Fogo | Explode no impacto, em área pequena, e deixa queimadura |
| Skill 2 | Chuva de Meteoros | Pedras flamejantes caem numa área marcada depois de um instante |
| Skill 3 | Piscar | Teleporte curto |
| Ultimate | Ira dos Céus | Canaliza alguns segundos, parado e vulnerável; um raio cai sobre **todos os inimigos da arena** (inspirada no Zeus do Dota e no Karthus do LoL). Se for interrompido, perde a ultimate |
| Passiva | Acúmulo Arcano | Cada skill usada aumenta o dano da próxima, até um limite |

## Sacerdotes (Fé)

### Paladino — tank que se sustenta com a luz · principal: Vigor

| Espaço | Skill | Efeito |
| --- | --- | --- |
| Skill 1 | Juízo | Bate o martelo no chão e solta uma onda de luz que provoca os inimigos em volta |
| Skill 2 | Égide Radiante | Escudo de luz que absorve dano e, quando quebra, cura o Paladino |
| Skill 3 | Smite | Como o Divine Smite do D&D: o próximo golpe do martelo vem carregado de luz e causa dano extra grande, maior contra mortos-vivos e demônios |
| Ultimate | Julgamento Final | Salta alto e cai com o martelo, abrindo uma cratera de luz que cura aliados e fere inimigos |
| Passiva | Fé Inabalável e Martírio | Recupera parte do dano que causa, e recebe mais cura quanto menos vida tiver |

### Clérigo — healer de cura forte e purificação · principal: Sabedoria

| Espaço | Skill | Efeito |
| --- | --- | --- |
| Skill 1 | Cura Sagrada | Cura forte num alvo, mas demora um pouco para conjurar |
| Skill 2 | Purificar | Remove veneno, maldição e efeitos negativos de um aliado, e cura um pouco |
| Skill 3 | Santuário | Um aliado fica protegido, e os inimigos param de mirar nele por alguns segundos |
| Ultimate | Milagre | Cura totalmente o grupo inteiro de uma vez |
| Passiva | Devoção | Cada cura reduz um pouco a recarga da próxima |

### Inquisidor — caçador do profano · principal: Força

| Espaço | Skill | Efeito |
| --- | --- | --- |
| Skill 1 | Rajada Sagrada | Três virotes em leque; cada inimigo atingido fica marcado e recebe mais dano do grupo |
| Skill 2 | Golpe Purificador | Golpe de maça com dano extra contra profanos |
| Skill 3 | Correntes da Inquisição | Prende o alvo no lugar por alguns segundos |
| Ultimate | A Caçada | Por alguns segundos, todos os ataques causam dano extra e ignoram as defesas |
| Passiva | Marca do Inquisidor | A cada três ataques no mesmo alvo, o próximo causa dano sagrado extra |

## Ladinos (Energia)

### Espadachim — tank de esquiva, estilo Fiora · principal: Agilidade

| Espaço | Skill | Efeito |
| --- | --- | --- |
| Skill 1 | Saudação do Duelista | Saúda os inimigos com a rapieira e provoca todos em volta |
| Skill 2 | Aparar | Bloqueia todo o dano por um instante e contra-ataca; se aparar um golpe forte, atordoa o atacante |
| Skill 3 | Avanço | Avança em linha reta, atravessando os inimigos |
| Ultimate | Dança dos Pontos Vitais | Por alguns segundos, esquiva de todos os ataques; cada esquiva gera um contra-ataque num ponto vital, com dano extra e cura nos aliados próximos |
| Passiva | Ritmo e Pés Ligeiros | Cada esquiva ou golpe aparado reduz a recarga das skills; esquiva mais se tiver se movido recentemente |

### Bardo — healer pela música · principal: Sabedoria

| Espaço | Skill | Efeito |
| --- | --- | --- |
| Skill 1 | Acorde Vibrante | Nota forte que cura de uma vez os aliados próximos |
| Skill 2 | Hino de Batalha | Aumenta o dano dos aliados próximos por alguns segundos |
| Skill 3 | Nota Dupla | Som estridente atordoa os inimigos à frente, e o eco cura os aliados atrás do Bardo |
| Ultimate | Crescendo | Música que fica mais forte a cada segundo e termina numa explosão que cura todo o grupo |
| Passiva | Inspiração | Cada cura dá ao aliado um pequeno bônus de dano por alguns segundos |

### Assassino — DPS de explosão · principal: Agilidade

| Espaço | Skill | Efeito |
| --- | --- | --- |
| Skill 1 | Apunhalar | Muito mais dano se o ataque vier pelas costas |
| Skill 2 | Leque de Lâminas | Várias facas em leque, acertando quem estiver à frente |
| Skill 3 | Bomba de Fumaça | Joga a bomba e fica invisível; ativando de novo, aparece atrás de um inimigo |
| Ultimate | Execução | Golpe final que executa na hora quem estiver com menos de 5% de vida |
| Passiva | Sangria e Golpe Oculto | Ataques acumulam sangramento; o primeiro ataque depois de ficar invisível ou aparecer atrás do inimigo é sempre crítico |

## Naturalistas (Essência)

### Guardião da Floresta — tank de controle do campo · principal: Vigor

| Espaço | Skill | Efeito |
| --- | --- | --- |
| Skill 1 | Rugido da Mata | Provoca os inimigos em volta e os enraíza por um instante |
| Skill 2 | Pele de Casca | Camada de casca que absorve dano e vai se regenerando |
| Skill 3 | Muralha de Espinheiros | Parede de espinhos que bloqueia a passagem dos inimigos |
| Ultimate | Despertar da Floresta | Árvores e raízes ganham vida e prendem todos os inimigos da área |
| Passiva | Espinhos | Quem o ataca corpo a corpo leva parte do dano de volta |

### Druida — healer de cura contínua · principal: Sabedoria

| Espaço | Skill | Efeito |
| --- | --- | --- |
| Skill 1 | Seiva Curativa | Cura que salta de aliado em aliado, deixando flores que continuam curando |
| Skill 2 | Chuva Primaveril | Chuva leve que cura todos numa área por alguns segundos |
| Skill 3 | Raízes | Prende um inimigo no lugar |
| Ultimate | Sopro da Primavera | Cura forte em pulsos em todo o grupo enquanto o Druida canaliza |
| Passiva | Crescimento | Curas contínuas ficam mais fortes quanto mais tempo duram |

### Ranger — DPS à distância · principal: Agilidade

| Espaço | Skill | Efeito |
| --- | --- | --- |
| Skill 1 | Tiro Perfurante Carregado | Segura e solta: quanto mais carregar, mais dano, atravessando todos os inimigos na linha |
| Skill 2 | Chuva de Flechas | Flechas caem do céu numa área marcada |
| Skill 3 | Salto Evasivo | Salta para trás e deixa uma armadilha onde estava, que prende quem pisar |
| Ultimate | Tempestade do Olho de Águia | Dispara sem parar por alguns segundos, com todos os tiros críticos e mais alcance |
| Passiva | Foco | Cada tiro seguido no mesmo alvo aumenta um pouco o dano |

---

## Subclasses guardadas para o futuro

| Ideia | Família provável | Nota |
| --- | --- | --- |
| Defensor | Guerreiro | Tank de espada e escudo; saiu para não ficar parecido com o Cavaleiro |
| Monge | Guerreiro | Guerreiro desarmado (DPS ou healer). O Totto tem outra ideia para ele |
| Feiticeiro | Arcano | DPS de magia nata |
| Artífice | Arcano | Torretas, golens e engenhocas; combina com gnomos |
| Encantador | Arcano | Controle e suporte |
| Ladrão | Ladinos | DPS ágil de furtividade |
| Mestre das Feras | Naturalistas | Luta com um animal companheiro (tank ou DPS) |
| Druida tank | Naturalistas | Segunda versão do Druida |
| Alquimista | A definir | Poções e venenos, DPS ou healer |
| Necromante | A definir | Pode voltar agora que o mundo é livre |
| Xamã | A definir | Estava na imagem de referência, nunca foi discutido |

**Subclasses híbridas** (ex.: Paladino fazendo tank, healer e DPS) também ficaram guardadas — falta decidir como seria a troca de função.
