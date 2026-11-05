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
> **Os kits por subclasse mais abaixo são da versão anterior.** Todos foram substituídos pelos kits dos heróis; ficam só como registro. Skills deles podem ser reaproveitadas.

## Padrão das teclas (vale para todos os heróis)

Para o jogador não errar a tecla ao trocar de herói (o problema do pulo da Tristana no W e do dash do Lucian no E, no LoL):

| Tecla | Papel |
| --- | --- |
| **Q** | Skill principal: o ataque ou a cura principal do herói |
| **W** | Skill de apoio ou efeito específico do herói |
| **E** | **Deslocamento**, quando o herói tiver um. Herói sem deslocamento usa o E para outra coisa |
| **R** | Ultimate, cada uma do seu jeito |

Heróis podem ter mais ou menos mobilidade, mas, quando houver um deslocamento, ele fica no **E**.

## Kits dos heróis

Valores de dano, custo, recarga, duração e limites ficam para o balanceamento. Raças e feitiços não fazem parte dos kits. **Todo dano elemental é mágico** (uma skill pode misturar dano físico e mágico). **Invisibilidade não dá invulnerabilidade.**

**DPS: os 10 kits fechados** (03/11/2026). Análise comparativa em [analises/comparacao-dps.md](analises/comparacao-dps.md). Tanks e healers: fechados (abaixo). **Os 20 kits estão completos.**

### Ignis — Mago · tema: só bola de fogo

**Recurso:** Mana · **Armas:** cajado e grimório.

| Tecla | Skill | Efeito | Efeitos genéricos usados |
| --- | --- | --- | --- |
| Q | Bola de Fogo | Lança um projétil que explode no impacto, causando dano mágico em área e aplicando queimadura. | Projétil, dano, área circular, aplicar elemento Fogo. |
| W | Órbita de Brasas | Três bolinhas de fogo orbitam ao redor dele e queimam quem encosta. Ao reativar, lança as bolinhas na direção do cursor, com controle manual. | Projéteis em órbita, aplicar elemento Fogo, lançamento direcional. |
| E | Bola Propulsora | Explode uma bola de fogo nos próprios pés, arremessando-se para trás e queimando os inimigos próximos. | Deslocamento próprio, área circular, aplicar elemento Fogo. |
| R | Bola de Fogo Gigante | Marca um local e faz uma bola de fogo gigante cair do céu, causando dano mágico e queimadura na área atingida. | Projétil, dano, área circular, aplicar elemento Fogo. |
| Passiva | Só Sei Essa | Acertos consecutivos de bolas de fogo aumentam o dano das próximas, até um limite. O bônus desaparece após um tempo sem acertar. | Aplicar estado cumulativo, modificar dano, remover estado por inatividade. |

### Caleb — Mago · tema: magia atrapalhada, aprendizado e um gato caçador

O caos do Caleb (feitiços que saem errados) fica **só no visual**: o kit é previsível e o "erro" aparece na aparência.

**Recurso:** Mana · **Armas:** cajado e grimório.

**Cappuccino:** o gato (laranja e branco) acompanha o Caleb andando ao lado dele o tempo todo e não pode ser atacado. Só sai de perto na E (fica no ponto de partida) e na R (caça os sapos).

| Tecla | Skill | Efeito | Efeitos genéricos usados |
| --- | --- | --- | --- |
| Q | Mísseis Mágicos | Possui três cargas disparadas separadamente. Cada ativação seleciona um inimigo pelo cursor, em point-and-click, e lança um míssil perseguidor que causa dano mágico. Pode concentrar as cargas em um alvo ou distribuí-las. **Visual:** cada míssil sai com uma aparência sorteada (sapinho, borboletas, letras do grimório ou o próprio míssil mágico); só muda a aparência. Cada visual tem o seu impacto: o míssil estoura em brilho arcano, o sapinho some num "puf" de fumaça, as borboletas se espalham e as runas estouram em letras. | Projétil perseguidor, dano, aplicar e consumir estado de cargas. |
| W | Página Errada | Transforma temporariamente um inimigo em um pequeno animal, impedindo ataques e habilidades, mas permitindo movimento. Em chefes, só interrompe magias (chefes são imunes a controle). **Visual:** o bichinho é sorteado entre sapinho, galinha, pinguim e cachorro. | Aplicar estado de transformação e bloqueio de ataques e habilidades. |
| E | Esqueci o Gato | Teleporta para o local escolhido, causando uma explosão de dano mágico na chegada. O **Cappuccino** (o gato, que não pode ser atacado) fica no ponto de partida, permitindo reativar para retornar até ele durante uma janela. No ponto da explosão fica um **enxame de abelhas** picando os inimigos por alguns segundos. Se o gato participar da R durante a janela da E, ele sai para caçar mas deixa uma **marca de patinha** no ponto de partida: a janela continua valendo e a volta leva até a patinha. Depois da caçada, o gato volta para a patinha (se a janela ainda estiver aberta) ou para o lado do Caleb. | Teleporte, dano, área circular, invocar enxame (dano ao longo do tempo em área), aplicar estado com posição de retorno. |
| R | Isso Não Estava no Livro | Faz chover pequenos sapos mágicos na área escolhida. Um círculo mágico verde (da cor dos sapos) marca a área. O Cappuccino cresce até ficar maior que o Caleb (como o Mega Gnar) e corre atrás deles, atacando sapos e inimigos da região: alterna botes por cima dos inimigos (mordendo no meio do salto) com corridas atrás dos sapos. Seus ataques contra inimigos causam dano mágico. Ao terminar, retorna ao tamanho normal. | Invocar, área circular, aplicar estado de crescimento, deslocamento, dano. |
| Passiva | Acerto Épico | Depois de um certo número de habilidades que acertam, a próxima sai **épica**: mais forte, com visual enorme, e **não gasta Mana**. **Contagem:** cada uso que acerta conta 1 (cada míssil da Q conta 1, cada ataque básico conta 1); acertar vários inimigos de uma vez conta 1 só; errar não conta; o que a skill deixa no chão ou invoca (abelhas, gato) não conta. | Aplicar e consumir estado cumulativo, modificar dano, anular custo de recurso. |

### Brok — Bárbaro · tema: fúria crescente e agressão constante

**Recurso:** Fúria · **Armas:** dois machados.

| Tecla | Skill | Efeito | Efeitos genéricos usados |
| --- | --- | --- | --- |
| Q | Talho Duplo | Golpeia um alvo com os dois machados, causando dano físico e aplicando sangramento. | Dano, aplicar estado de sangramento com dano ao longo do tempo. |
| W | Arremesso | Arremessa um machado que causa dano físico na ida e na volta. Os inimigos atingidos na volta também recebem lentidão. | Projétil com retorno, dano, lentidão. |
| E | Redemoinho Furioso | Gira enquanto se movimenta livremente, causando dano físico ao redor. Ganha velocidade de movimento e ignora lentidão e enraizamento durante o efeito. | Dano, área circular, modificar velocidade de movimento, aplicar estado de imunidade a lentidão e enraizamento. |
| R | Fúria Total | Aumenta a velocidade de ataque, impede atordoamento e cura com parte do dano causado. | Modificar velocidade de ataque, aplicar estado de imunidade a atordoamento, cura vinculada ao dano. |
| Passiva | Sede de Batalha | Cada golpe aumenta o dano, até um limite. O bônus desaparece após um tempo sem bater. | Aplicar estado cumulativo, modificar dano, remover estado por inatividade. |

### Bella — Bárbara · tema: força bruta e um machado maior que ela

**Recurso:** Fúria · **Arma:** machado gigante de duas mãos.

| Tecla | Skill | Efeito | Efeitos genéricos usados |
| --- | --- | --- | --- |
| Q | Racha-Chão | Desce o machado numa pancada que causa dano físico nos inimigos em uma linha curta à frente. | Dano, área em linha. |
| W | Grito de Desafio | Reduz temporariamente a armadura dos inimigos próximos e fortalece a próxima habilidade de dano, que também aplica lentidão. | Área circular, modificar armadura, aplicar e consumir estado de fortalecimento, lentidão. |
| E | Salto Brutal | Salta até o local escolhido e aterrissa com o machado, causando dano físico em área. | Deslocamento, dano, área circular. |
| R | Partir a Arena | Desfere uma pancada devastadora numa linha larga à frente. Causa dano físico maior quanto mais vida cada inimigo atingido tiver perdido e joga os atingidos para o alto. | Dano condicionado à vida perdida, área em linha, empurrão vertical. |
| Passiva | Quanto Maior, Melhor | Habilidades de dano em área causam mais dano aos inimigos próximos do centro do impacto; nas habilidades em linha, vale o eixo central. Acertar um inimigo nessa região concede um escudo temporário, independentemente da habilidade usada. | Modificar dano conforme posição, escudo condicionado ao acerto. |

### Rex — Ranger · tema: acrobacias e tiros de exibição

**Recurso:** Essência · **Armas:** arco longo e aljava.

| Tecla | Skill | Efeito | Efeitos genéricos usados |
| --- | --- | --- | --- |
| Q | De Tabela | Dispara uma flecha no alvo escolhido, que ricocheteia entre inimigos próximos. Atinge cada inimigo apenas uma vez, sem limite fixo de alvos, causando menos dano físico a cada ricochete. | Projétil com ricochete, dano, redução progressiva de dano. |
| W | Ritmo de Exibição | Aumenta temporariamente a velocidade de ataque. Acertos durante o efeito concedem velocidade de movimento, sem acumular. | Modificar velocidade de ataque, aplicar estado de velocidade de movimento ao acertar. |
| E | Mortal de Exibição | Dá um mortal na direção escolhida e, de cabeça para baixo, dispara uma flecha no inimigo selecionado, causando dano físico. | Deslocamento, projétil, dano. |
| R | Tempestade do Olho de Águia | Dispara continuamente por um período, com todos os tiros críticos e alcance aumentado. | Projéteis, dano, aplicar estado de crítico garantido, modificar alcance. |
| Passiva | Foco | Tiros consecutivos no mesmo alvo aumentam o dano, até um limite. No Q, apenas o primeiro alvo conta; os ricochetes não acumulam nem interrompem o bônus. Os ataques do Rex **não podem ser esquivados** ("o Rex nunca erra"). | Aplicar estado cumulativo por alvo, modificar dano, filtrar acertos secundários, ignorar esquiva. |

### Kael — Ranger · tema: precisão e distância calculada

**Recurso:** Essência · **Armas:** arco longo e aljava.

| Tecla | Skill | Efeito | Efeitos genéricos usados |
| --- | --- | --- | --- |
| Q | Tiro Perfurante Carregado | Carrega e dispara uma flecha que atravessa os inimigos em linha. Quanto mais carregar, maior o dano físico. | Canalizar, projétil perfurante, dano, área em linha. |
| W | Chuva de Flechas | Dispara uma chuva de flechas sobre uma área marcada, causando dano físico aos inimigos atingidos. | Projéteis, dano, área circular. |
| E | Salto Evasivo | Salta para trás e deixa uma armadilha no ponto de partida, que enraíza quem pisar. | Deslocamento, invocar armadilha, enraizar. |
| R | Tiro Impecável | Mira em um inimigo enquanto canaliza e dispara uma flecha de grande dano físico, ignorando parte da armadura do alvo. | Canalizar, projétil, dano, modificar penetração de armadura para o disparo. |
| Passiva | Distância Calculada | Seus disparos causam mais dano quanto mais longe estiver do alvo, até um limite. Também causam dano adicional contra **alvos parados**: enraizados, conjurando ou simplesmente sem se mover, inclusive por controle de aliados. Funciona em chefes. | Modificar dano conforme distância e conforme o alvo estar parado. |

### Mina — Assassina · tema: doçura letal e ataques de surpresa

**Recurso:** Energia · **Armas:** duas adagas.

| Tecla | Skill | Efeito | Efeitos genéricos usados |
| --- | --- | --- | --- |
| Q | Apunhalar | Golpeia um inimigo, causando dano físico elevado e dano adicional se atingir pelas costas. | Dano, modificar dano conforme posição. |
| W | Leque de Lâminas | Lança facas em leque, causando dano físico aos inimigos à frente. | Projéteis, dano, área em cone. |
| E | Bomba de Fumaça | Lança uma bomba e fica invisível. Ao reativar, aparece atrás de um inimigo escolhido. | Aplicar estado de invisibilidade, teleporte. |
| R | Execução | Executa inimigos comuns abaixo de um limiar de vida. Contra chefes, causa dano físico elevado que aumenta conforme a vida perdida, sem execução automática. | Dano letal condicionado à vida e ao tipo de alvo, dano físico condicionado à vida perdida. |
| Passiva | Sangria e Golpe Oculto | Ataques básicos acumulam sangramento. Após ficar invisível ou aparecer atrás de um inimigo, o próximo ataque básico é crítico. O Q não consome o benefício: funcionam tanto AA crítico → Q quanto Q → AA crítico. | Aplicar estado cumulativo de sangramento, dano ao longo do tempo, aplicar e consumir estado de crítico garantido no ataque básico. |

### Grum — Assassino · tema: furtividade barulhenta e emboscadas brutais

**Recurso:** Energia · **Armas:** duas adagas, com armadura pesada.

| Tecla | Skill | Efeito | Efeitos genéricos usados |
| --- | --- | --- | --- |
| Q | Abre Espaço | Acerta uma ombrada seguida de uma punhalada, causando dano físico e interrompendo a canalização do alvo. | Dano, remover estado de canalização. |
| W | Preparação Suspeita | Fortalece a próxima habilidade de dano para causar dano físico adicional e reduzir temporariamente a armadura dos inimigos atingidos. | Aplicar e consumir estado de fortalecimento, dano, modificar armadura. |
| E | Furtividade Pesada | Fica invisível e ganha velocidade de movimento. Atacar ou usar uma habilidade ofensiva encerra essa invisibilidade; o ataque ou habilidade que a encerrar causa dano físico adicional. Se usar R, esse bônus vale somente para a primeira punhalada. | Invisibilidade, modificar velocidade de movimento, dano adicional condicionado, remover estado. |
| R | Ninguém Viu Nada | Fica invisível durante uma sequência rápida de punhaladas no alvo escolhido. O último golpe causa dano físico adicional conforme a vida perdida do alvo. As punhaladas não interrompem a invisibilidade própria da R, que não concede invulnerabilidade. | Aplicar invisibilidade temporária, sequência de dano, dano condicionado à vida perdida. |
| Passiva | Ninguém Está Olhando | Causa dano adicional aos inimigos que estejam mirando outro personagem. Acertar habilidades em inimigos com armadura reduzida recupera Energia. Na R, somente a primeira punhalada pode ativar essa recuperação, se a condição estiver satisfeita. | Modificar dano conforme alvo do inimigo, verificar armadura reduzida, gerar recurso com restrição de ativação. |

### Trix — Inquisidora · tema: besta de mão e engenhocas antidemônio

**Recurso:** Fé · **Arma principal:** besta de uma mão · **Mão secundária:** armadilha.

| Tecla | Skill | Efeito | Efeitos genéricos usados |
| --- | --- | --- | --- |
| Q | Rajada Sagrada | Dispara virotes em leque, causando dano mágico Sagrado. Marca os inimigos atingidos para receberem mais dano do grupo. | Projéteis, área em cone, dano, aplicar elemento Sagrado, aplicar estado de vulnerabilidade. |
| W | Engenhoca de Contenção | Coloca uma armadilha que, ao ser acionada, cria uma área de lentidão e causa dano mágico Sagrado em pulsos. | Invocar armadilha, área circular, lentidão, dano periódico, aplicar elemento Sagrado. |
| E | Recuo Calculado | Dispara um virote de impacto que causa dano físico e empurra o alvo, enquanto o recuo a lança para trás. | Projétil, dano, empurrão, deslocamento próprio. |
| R | A Caçada | Temporariamente, seus ataques causam dano adicional e ignoram parte da armadura e da resistência mágica dos alvos. Escudos, reduções de dano e invulnerabilidade continuam funcionando normalmente. | Aplicar estado de fortalecimento, dano adicional, modificar penetração de armadura e resistência mágica. |
| Passiva | Marca do Inquisidor | Ataques consecutivos no mesmo alvo preparam um próximo ataque com dano mágico Sagrado adicional. | Aplicar e consumir estado cumulativo, dano adicional, aplicar elemento Sagrado. |

### Darien — Inquisidor · tema: perseguição e correntes sagradas

**Recurso:** Fé · **Arma principal:** maça · **Acessório secundário:** correntes.

| Tecla | Skill | Efeito | Efeitos genéricos usados |
| --- | --- | --- | --- |
| Q | Golpe Purificador | Golpeia um inimigo com a maça, causando dano físico mais uma parcela de dano mágico Sagrado contra qualquer inimigo. Causa dano adicional contra profanos. A parcela Sagrada ativa a cura da passiva. | Dano físico e mágico, aplicar elemento Sagrado, dano adicional condicionado ao tipo de alvo. |
| W | Correntes da Inquisição | Prende um inimigo com correntes, enraizando-o temporariamente. | Aplicar estado de enraizamento. |
| E | Corrente de Aproximação | Lança uma corrente em linha reta. Ao atingir um inimigo, puxa a si mesmo até ele, causando dano físico no impacto. | Projétil, deslocamento próprio até o alvo, dano. |
| R | Correntes do Castigo | Liga os inimigos de uma área. Parte do dano que causa a um dos ligados é reproduzida nos demais como dano mágico Sagrado, sem gerar novas reproduções. Contra um alvo isolado, seus golpes causam dano Sagrado adicional. | Área circular, aplicar estado de ligação, dano condicionado ao dano causado, dano mágico Sagrado adicional. |
| Passiva | Fé pelo Castigo | Ataques básicos contra inimigos sob efeitos de controle aceleram a recuperação de Fé, inclusive quando o controle vem de aliados. Recupera vida com parte do dano mágico Sagrado que causa; o dano reproduzido pelas correntes da R cura com eficiência reduzida. | Verificar estado de controle, acelerar recuperação de recurso, cura vinculada ao dano Sagrado, modificar eficiência de cura conforme origem do dano. |

## Kits dos tanks

**Os 5 kits fechados** (04/11/2026). Análise comparativa em [analises/comparacao-tanks.md](analises/comparacao-tanks.md).

- **Taunt não é controle:** serve só para segurar a ameaça e **funciona em chefes** (ver [Sistemas](03-sistemas.md)).
- A autocura dos tanks **ajuda, mas não substitui o healer**; isso se ajusta nos números.
- Maximilian (Espadachim) e Wix & Abraxas (Bruxo) são os tanks **mecânicos**, para jogadores mais habilidosos. O Maximilian se inspira na Fiora e no Master Yi do LoL; o risco da semelhança foi aceito.
- Os nomes de todas as skills serão revisados antes da versão final.

### Quixote — Cavaleiro protetor

**Classe:** Guerreiro · **Subclasse:** Cavaleiro (Tank) · **Recurso:** Fúria · **Atributo principal:** Vigor.  
**Equipamento:** espada longa e escudo pesado.  
**Identidade:** proteção direta de aliados, bloqueio frontal, interceptação e mitigação coletiva.

#### Q — Golpe de Escudo
- Golpeia o alvo com o escudo, causando **dano físico**.
- Aplica **Taunt** e **lentidão** no inimigo atingido.
- Contra chefes, o Taunt funciona; a lentidão não restringe seu movimento.

#### W — Nem Um Passo Atrás
- Assume uma postura defensiva frontal, **bloqueando/reduzindo fortemente o dano vindo da frente** durante uma janela curta.
- A proteção depende da direção do ataque; não é invulnerabilidade integral.
- Bloqueios bem-sucedidos alimentam a passiva, concedendo um escudo menor ao próprio Quixote.

#### E — Interceptar
- Avança até um **aliado selecionado** para interceptar o perigo e protegê-lo.
- A proteção efetivamente fornecida ao aliado contribui para a passiva.
- Ferramenta de resposta a ameaças sobre Healers e DPS, não apenas de deslocamento.

#### R — Bastião
- **Finca a espada no chão**, criando uma **zona defensiva**.
- Aliados dentro da área recebem **redução de dano** enquanto o efeito durar.
- O dano evitado aos aliados pela Ultimate alimenta a passiva de Quixote.

#### Passiva — Em Nome da Honra
- Parte do **dano que Quixote impede de atingir aliados** com E e R é convertida em **escudo para ele mesmo**, com limite.
- Bloqueios bem-sucedidos com W concedem um escudo próprio menor.
- A passiva recompensa proteger companheiros, em vez de apenas receber dano.

**Pontos fortes:** salvar aliados, antecipar dano de grupo, proteger posições.  
**Pontos de atenção:** depende de posicionamento e timing; seus escudos não devem tornar o grupo invulnerável quando combinados ao R.


### Wix & Abraxas — Bruxo e demônio

**Classe:** Arcano · **Subclasse:** Bruxo (Tank) · **Recurso:** Mana.  
**Equipamento:** foice e coração demoníaco em recipiente.  
**Identidade:** duas unidades, maldições, transferência de vida, gerenciamento de ameaça e plano de contingência quando Abraxas cai.

#### Mecânica estrutural
- O kit possui **duas configurações**, conforme **Abraxas esteja ativo ou ausente**.
- Wix e Abraxas precisam ser gerenciados como unidades distintas. A ausência do demônio altera W, E, R e a função da passiva.
- O efeito **Hex** acumula até **5 cargas** nos inimigos por ataques básicos; o Q pode detoná-las.

#### Com Abraxas ativo

##### Q — Foice mágica / detonação de Hex
- Ataque **mágico à distância** com a foice.
- **Detona cargas de Hex** do alvo, causando efeito ofensivo adicional.
- A detonação também **cura Wix**, conectando o ataque ao sustain.

##### W — Sacrifício de Sangue
- Wix **gasta parte da própria vida para curar Abraxas**.
- A transferência gera **crédito de sangue**, limitado, para a economia defensiva do kit.
- O uso exige avaliar a vida dos dois personagens.

##### E — Investida de Abraxas
- Abraxas **avança atravessando inimigos**.
- A colisão pode **interromper conjurações interrompíveis**, inclusive de chefes.
- Ao chegar ao destino, aplica **Taunt em área** e recebe mitigação temporária.
- Wix recebe aumento temporário de velocidade.

##### R — Maldição em Área
- Aplica uma maldição em área que **acelera o acúmulo de Hex**.
- Fortalece as **detonações de Hex pelo Q** durante o efeito.
- É uma Ultimate de pressão e manutenção do ciclo de maldições, não uma invulnerabilidade.

##### Passiva — Hex
- **Ataques básicos aplicam Hex**, até **5 cargas** por inimigo.
- O Q explora as cargas acumuladas para dano e sustentação de Wix.

#### Sem Abraxas

##### Q — Foice mágica / detonação de Hex
- Mantém o ataque mágico à distância e a interação com Hex.

##### W — Escudo de Wix
- Wix utiliza **Mana para criar um escudo próprio**, substituindo a cura do demônio.

##### E — Investida pelo Coração
- Wix executa um **dash impulsionado pelo coração demoníaco**.
- A investida **interrompe canalizações interrompíveis** e aplica **Taunt**, inclusive contra chefes no que diz respeito à provocação.

##### R — Defesa de Emergência
- Concede um **escudo emergencial** a Wix.
- **Acelera o retorno/reconstituição de Abraxas**.

##### Passiva — Sobrevivência sem Abraxas
- Quando o demônio deixa o combate, **a ameaça dele é transferida automaticamente para Wix**.
- Wix recebe **escudo e mitigação** para sobreviver à transição.
- Abraxas passa por um processo de reconstituição para retornar.

**Pontos fortes:** versatilidade, aggro entre duas unidades, interrupção e maldições.  
**Pontos de atenção:** alta complexidade; o demônio não pode equivaler a um segundo Tank completo sem contrapartida; calibrar custo de vida, crédito de sangue e reconstrução.

> **Nota de nomenclatura:** os nomes descritivos das habilidades de Wix & Abraxas acima identificam seus efeitos fechados; a nomenclatura final individual de todas elas não está consolidada neste resumo.


### Gunnar — Paladino do Fogo Sagrado

**Classe:** Sacerdotes · **Subclasse:** Paladino (Tank) · **Recurso:** Fé.  
**Equipamento:** martelo de guerra e égide de luz.  
**Identidade:** resistência pela ofensiva sagrada, queimaduras que geram ameaça e autocura, grande impacto contra demônios e mortos-vivos.

#### Q — Martelada Incandescente
- Desfere um golpe de martelo que causa **dano físico + dano mágico Sagrado**.
- Aplica **Fogo Sagrado**, um efeito de queimadura.
- Ataques básicos **renovam a queimadura** já aplicada.

#### W — Égide Radiante
- Cria uma **égide que absorve dano**.
- Quando a égide é **destruída**, Gunnar recebe **cura própria**.
- A capacidade do escudo e a cura terão limites de balanceamento.

#### E — Martelada Santa
- Gunnar **salta até o local escolhido** e golpeia o chão com o martelo.
- Causa **dano físico e mágico Sagrado em área**.
- **Atordoa inimigos comuns**.
- **Interrompe conjurações interrompíveis**, inclusive de chefes, sem atordoá-los.
- Aplica **Fogo Sagrado em área**.

#### R — Julgamento Final
- Executa um **golpe colossal único** de martelo, no estilo de um *Divine Smite*.
- Causa **dano físico + dano mágico Sagrado**.
- Causa **dano adicional contra demônios e mortos-vivos**.
- **Interrompe canalizações interrompíveis**.
- Gunnar recebe **autocura** e **mitigação durante a animação**.

#### Passiva — Fé Inabalável
- Inimigos afetados por **Fogo Sagrado** são **provocados a atacar Gunnar (Taunt)**.
- Os pulsos de queimadura **curam Gunnar**, com limite para evitar crescimento descontrolado em grupos numerosos.
- **Duração do Taunt e duração da queimadura são independentes**: renovar Fogo Sagrado com ataques básicos não pode manter um chefe permanentemente provocado sem janela de resposta.

**Pontos fortes:** sustentação em combate prolongado, aggro por queimadura, chefes demoníacos ou mortos-vivos.  
**Pontos de atenção:** limitar cura de múltiplas queimaduras; manter janelas de Taunt justas.


### Maximilian — Mestre da Esgrima

**Classe:** Ladinos · **Subclasse:** Espadachim (Tank) · **Recurso:** Energia · **Atributo principal:** Agilidade.  
**Equipamento:** rapieira e adaga de aparar.  
**Identidade:** Tank técnico, alto teto de habilidade, aparadas, esquivas direcionais, ataques posicionais e autocura crescente por Elegância.

#### Q — Dança das Estocadas
- Executa uma **sequência veloz de estocadas** entre os inimigos em um raio, lembrando o ritmo de uma sequência de ataques múltiplos.
- Aplica **dano físico e Taunt a todos os inimigos elegíveis na área**, sem limite fixo de quatro alvos.
- A sequência possui **pelo menos quatro estocadas**: se houver um inimigo, ele recebe quatro; se houver dois, dois golpes cada; se houver quatro, um cada; se houver mais de quatro, cada inimigo é atingido ao menos uma vez.
- **Acertos repetidos no mesmo alvo têm dano reduzido**, para que a habilidade não vire explosão de DPS em um único boss.
- Maximilian fica **brevemente inalvejável durante a execução** e retorna próximo ao alvo inicial.
- A duração total da animação é limitada, ajustando a velocidade visual conforme a quantidade de alvos.
- O Q é uma ferramenta principal de **aggro em área**.

#### W — Aparar
- Maximilian levanta a **adaga de aparar** por uma janela curta, bloqueando o ataque recebido.
- Uma aparada bem-sucedida produz **contra-ataque automático de rapieira**.
- A **aparada perfeita**, feita no momento preciso, fortalece a resposta e pode **interromper canalizações interrompíveis**, inclusive de chefes.
- A janela deve permitir uso funcional por jogadores iniciantes, enquanto o timing perfeito recompensa especialistas.

#### E — Passo de Esgrima
- **Dash curto direcional com duas cargas**, utilizável para frente, trás ou lados.
- Oferece uma breve janela para **esquivar de ataques esquiváveis**.
- **Esquiva perfeita reduz parte da recarga do W**.
- Após o dash, o **próximo ataque básico** é fortalecido **se acertar o flanco ou as costas**: ganha dano físico, ameaça adicional e **autocura**.
- Acerto pelas costas tem benefício superior ao acerto pelo flanco.
- Ataque frontal não recebe o benefício posicional e consome a oportunidade de ataque fortalecido.
- A melhoria não acumula entre dashes; a autocura possui limite e pode considerar a vida perdida de Maximilian.
- Funciona contra chefes, respeitando posicionamento e esquivabilidade.

#### Passiva — Elegância Impecável
- Maximilian **acumula Elegância** por ações de esgrima bem-sucedidas: aparadas, esquivas e ataques posicionais; execuções perfeitas rendem mais.
- **Oito pontos** são a referência conceitual atual para o máximo; a pontuação exata de cada ação fica para balanceamento.
- **Cada ponto acumulado aumenta progressivamente a autocura dos ataques básicos**.
- Ao atingir o máximo, a autocura permanece **no máximo enquanto Elegância estiver cheia**.
- **Ataques básicos NÃO gastam Elegância**: atacam, curam e podem contribuir para acumulá-la.
- O ataque básico fortalecido pelo E mantém sua própria autocura, compatível com o bônus da passiva.
- **Somente a Ultimate consome e zera Elegância.**

#### R — Dança dos Pontos Vitais
- O R fica **bloqueado até Elegância atingir o máximo** (referência: 8 pontos).
- Ao ativar, Maximilian executa uma **rajada espetacular de estocadas** contra inimigos próximos, causando **dano físico** e gerando ameaça adicional.
- Concede **escudo a si mesmo e aos aliados que estiverem próximos no instante da ativação**.
- **Aliados distantes não recebem o escudo**; o efeito não é global.
- **Consome todos os pontos de Elegância e zera a barra**. Consequentemente, a autocura crescente da passiva recomeça do patamar inicial.
- A Ultimate também respeita sua **recarga normal**.
- A mecânica não exige acertar pontos vitais em posições fixas ao redor do alvo: a diferenciação está em construir Elegância por boa execução e decidir quando trocar sustain pessoal por proteção do grupo.

**Pontos fortes:** alta recompensa por domínio mecânico, mobilidade, aggro em área, defesa reativa e escudo coletivo pontual.  
**Pontos de atenção:** garantir que o jogador menos experiente consiga operar o kit; equilibrar o incentivo de guardar Elegância cheia versus ativar o R.


### Flora — Guardiã da Floresta

**Classe:** Naturalistas · **Subclasse:** Guardião da Floresta (Tank) · **Recurso:** Essência · **Atributo principal:** Vigor.  
**Equipamento:** clava de madeira viva e escudo de casca.  
**Identidade:** ex-lenhadora brabona, Tank de iniciação agressiva, escudo regenerativo, controle territorial com risco para aliados e reflexão universal de dano.

#### Q — Sai da Minha Mata!
- Flora **golpeia um inimigo com a Clava Viva**, causando **dano físico no alvo principal**.
- Solta um berro que aplica **Taunt em área** nos inimigos ao redor dela.
- **Enraíza brevemente inimigos comuns**.
- Contra chefes, o **Taunt funciona**, mas o enraizamento não.
- O Taunt em área **não depende de acertar o golpe principal**, para manter a confiabilidade da ferramenta de aggro.

#### W — Pele de Casca
- Reveste o corpo com uma **camada de madeira viva que funciona como escudo**.
- O escudo **se regenera gradualmente** enquanto estiver ativo.
- **Cada ataque básico acertado acelera a regeneração do escudo**.
- A regeneração não supera a capacidade máxima do escudo.
- Se o escudo for **completamente destruído**, a habilidade termina.
- Recompensa uma Tank que **continua atacando** em vez de recuar.

#### E — Investida Selvagem
- Flora **avança na direção escolhida com o escudo à frente**.
- A investida causa **dano físico** e **empurra inimigos comuns** no trajeto.
- **Uma muralha de espinheiros cresce ao longo do trajeto atrás dela**; Flora termina à frente da barreira.
- A muralha permanece por um período e **bloqueia a passagem de TODOS**: inimigos, aliados e da própria Flora.
- Inimigos que encostam na muralha sofrem **dano mágico de Natureza**, com limite de ativações.
- **Chefes não são empurrados nem bloqueados** pela muralha: atravessam-na, recebendo o dano aplicável.
- A barreira é relativamente curta, ligada ao trajeto da investida, e **pode prejudicar o próprio grupo** se mal posicionada. Não existe passagem gratuita para aliados.

#### R — Fúria da Floresta
- Flora **transforma temporariamente a clava e o escudo em versões gigantescas de madeira viva**, com raízes, galhos e espinhos.
- Seus **ataques básicos ganham alcance e acertam múltiplos inimigos em arco frontal**, causando dano físico.
- Cada inimigo atingido gera **ameaça adicional**.
- A regeneração do escudo de **W — Pele de Casca é fortalecida**.
- Flora recebe **redução de dano** durante a transformação.
- Pode continuar usando **E — Investida Selvagem** normalmente.
- Todos os benefícios ofensivos e defensivos funcionam contra chefes, sem depender de controle convencional.
- A transformação tem duração limitada e **não concede invulnerabilidade**.

#### Passiva — Espinhos
- **Sempre que Flora recebe dano de um inimigo**, seus espinhos causam **dano mágico de Natureza ao agressor**.
- A reflexão funciona contra **ataques físicos corpo a corpo, projéteis físicos à distância, magias, projéteis mágicos, habilidades diretas e ataques de chefes**.
- **Dano periódico** (queimaduras, venenos etc.) também pode ativar a reflexão, mas com **limite de frequência**.
- O agressor sofre a reflexão **mesmo que esteja distante** de Flora.
- A passiva funciona **sem exigir W ativo**.
- O dano refletido **gera ameaça** contra o agressor.
- **Dano refletido não ativa outra reflexão**, evitando ciclos infinitos.
- Frequência e escala de dano precisam de limites para impedir abuso contra ataques muito rápidos ou de múltiplos acertos.

**Pontos fortes:** aggro mesmo sob ataques à distância, avanço agressivo, controle de corredores, defesa sustentada por ataques básicos.  
**Pontos de atenção:** a muralha bloqueia aliados; limites de reflexão e regeneração são críticos para balanceamento.

## Kits dos healers

**Os 5 kits fechados** (04/11/2026). Análise comparativa em [analises/comparacao-healers.md](analises/comparacao-healers.md).

- **Cada healer precisa conseguir curar o grupo sozinho** em qualquer dungeon e chefe. O balanceamento iguala a viabilidade, não o estilo.
- Nenhum healer tem ressurreição no kit: quem cai levanta no fim da batalha (ver [Sistemas](03-sistemas.md)).
- Na revisão final de nomes: "Crescendo" (R do Pip) é o mesmo nome da ultimate da Sona, do LoL; os nomes e o visual das skills da Nana podem ganhar mais da personalidade dela (sopa, sapos, vovó brava), sem mudar a mecânica.
- Cuidado de balanceamento: o bônus de dano do Pip não pode torná-lo obrigatório.

### Bobo — Guerreiro curandeiro / Lanceiro

**Identidade:** único Healer corpo a corpo; converte agressividade e posicionamento em sustentação coletiva. Usa lança e estandarte. **Recurso:** Fúria. **Tipo de dano:** físico.

#### Q — Lança da Vida

- Ao atingir **um inimigo em alcance corpo a corpo**, causa dano físico e cura o aliado com menor percentual de vida próximo, incluindo Bobo.
- Pode ser usado **diretamente em um aliado**, sem atingir inimigos, para curá-lo; também permite selecionar o próprio Bobo.
- A utilização ofensiva é mais eficiente que a cura direta, mas a cura direta precisa ser viável mesmo sem inimigos acessíveis.
- **Ajuste aprovado:** curar diretamente um aliado com Q também **acelera o próximo pulso do W**, assim como os ataques básicos. A aceleração respeita um limite.
- Funciona fora de combate para cura direta.

#### W — Estandarte da Ordem

- Bobo planta um estandarte que cria uma **área estacionária de cura**.
- Todos os aliados dentro da área, incluindo Bobo, recebem pulsos periódicos de cura.
- **Ataques básicos de Bobo dentro da área** aceleram o próximo pulso, respeitando um limite.
- **Curas diretas do Q** também aceleram o pulso, permitindo sustentar o grupo durante fases em que Bobo não consegue atacar.
- O estandarte **continua curando normalmente sem ataques ou acelerações**.

#### E — Investida do Porta-Estandarte

- Avanço curto e **direcional**, apontado pelo cursor, sem necessidade de selecionar alvo.
- Causa dano físico aos inimigos atravessados.
- Concede **um pequeno escudo somente ao próprio Bobo**.
- Serve para aproximação, reposicionamento ou fuga; não depende de controle de chefes.

#### R — A Bandeira Não Cai!

- Bobo ergue uma manifestação luminosa do estandarte e recebe um escudo.
- Durante a ultimate, **ataques básicos** de Bobo produzem ondas de cura para **todo o grupo, independentemente da posição na instância**, com limitação de frequência.
- Se o W estiver ativo, continua curando **independentemente**, com seus pulsos normais e aceleração pelos ataques. O R **não multiplica a potência do W**.
- O R continua funcionando mesmo se o W expirar.
- **Limitação importante:** a geração de ondas da ultimate depende de Bobo conseguir atacar.

#### Passiva — Vigor Incansável

- Cada ataque básico recupera uma pequena quantidade da vida de Bobo.
- A cada **terceiro ataque básico**, recebe cura própria adicional e pequena redução da recarga do E.
- **Instinto de Sobrevivência (efeito complementar da passiva):** quando um golpe recebido levaria Bobo abaixo de um limiar crítico de vida, antes de aplicar o dano, Bobo recebe um escudo e o golpe disparador sofre redução de dano. Tem recarga interna longa.
- Valores discutidos como referência, **não finais**: limiar por volta de 20% da vida máxima e redução aproximada de 50% no golpe disparador. O escudo absorve o golpe já reduzido antes de afetar a vida.
- Não neutraliza mecânicas de execução de chefes.

**Forças:** cura enquanto ataca; estandarte persistente; autossustentação; mobilidade de curta distância.

**Fraquezas:** risco de aggro e proximidade do chefe; perde eficiência ofensiva quando não pode atacar; não deve substituir o Tank.


### Lyra — Invocadora espiritual

**Identidade:** prevenção, mitigação, escudos e curas distribuídas por espíritos. **Recurso:** Mana. **Tipo de dano:** mágico.

**Regra central dos espíritos:** todos os espíritos pertencem à **Lyra**, não são unidades independentes permanentes nem ficam vinculados aos aliados indefinidamente. São enviados para realizar ações e retornam a ela.

#### Q — Toque Espiritual

- Lyra envia um espírito até um aliado selecionado.
- O espírito **se desloca fisicamente**, cura o alvo **quando chega** e retorna à Lyra.
- Quando Lyra seleciona a si mesma, a cura é **imediata**.

#### W — Coro dos Espíritos

- Envia um grupo de espíritos para um aliado escolhido, inclusive a própria Lyra.
- Os espíritos acompanham temporariamente esse aliado.
- Durante o efeito, emitem **pulsos de cura** e **reduzem o dano recebido** pelo alvo e por aliados próximos.
- Ao terminar, retornam à Lyra.

#### E — Troca Espiritual

- Lyra escolhe **um ponto no chão** dentro do alcance.
- Um espírito se desloca rapidamente até o local e Lyra **troca instantaneamente de posição** com ele.
- Durante a transição, Lyra fica intangível e causa **dano mágico** aos inimigos atravessados pela trajetória espiritual.
- A intangibilidade termina na chegada. Não precisa de alvo inimigo ou aliado.

#### R — Revoada dos Ancestrais

- Lyra envia espíritos até **cada membro do grupo**.
- Cada espírito produz **uma cura forte ao chegar** ao seu aliado.
- Depois, os espíritos acompanham os respectivos aliados temporariamente e realizam **curas reativas quando eles recebem dano**, sujeitas a intervalo mínimo entre ativações.
- Não concede imunidade à morte nem impede automaticamente dano fatal.
- O W pode continuar ativo, mas **não recebe multiplicador de potência** do R.

#### Passiva — Espíritos Inquietos

- Curas realizadas pelos espíritos de Lyra concedem **um pequeno escudo** ao aliado curado.
- Curas repetidas **renovam** a proteção, mas os escudos não se acumulam indefinidamente.
- Quando um desses escudos é **totalmente quebrado por dano**, ativa uma pequena cura no aliado protegido, com recarga interna por aliado.
- Essa cura reativa **não cria outro escudo**, evitando ciclos infinitos, e não impede dano fatal por si só.

**Forças:** mobilidade, prevenção de dano, proteção distribuída, cobertura de aliados espalhados.

**Fraquezas:** tempo de viagem dos espíritos e necessidade de antecipar picos de dano.


### Orin — Sacerdote / Clérigo clássico

**Identidade:** cura direta poderosa, purificação e proteção emergencial. A personalidade fofoqueira e hipocondríaca **não cria mecânicas de jogo**. **Recurso:** Fé, com **cinco cargas regenerativas**. **Equipamento:** mangual e relicário.

#### Q — Cura Sagrada (com Graça Compartilhada)

- Orin escolhe um aliado ou a si mesmo e inicia uma **curta conjuração**, durante a qual precisa ficar parado.
- Ao concluir, o alvo principal recebe **cura forte e imediata**.
- **Ajuste aprovado, Graça Compartilhada:** os demais aliados feridos recebem **uma pequena parcela da cura**, mesmo afastados dentro da arena.
- Se interrompida antes de terminar, a cura não acontece.
- Pode ser usada fora de combate.
- A cura compartilhada **não cria novas curas em cadeia**.

#### W — Purificação Restauradora

- Remove efeitos negativos **removíveis** de um aliado e aplica uma pequena cura.
- Se remover pelo menos um efeito negativo, a cura é **fortalecida**.
- Continua podendo curar mesmo quando o alvo não possui efeitos negativos removíveis.

#### E — Santuário de Emergência

- Concede **instantaneamente um escudo** a um aliado ou ao próprio Orin.
- Se o alvo estiver com vida crítica, o escudo fica mais forte e concede **uma pequena cura instantânea**.
- Não altera aggro. Funciona contra chefes como proteção de dano.
- Não exige conjuração.

#### R — Milagre

- Orin ergue o relicário e realiza uma **conjuração poderosa**.
- Ao concluí-la, restaura **uma grande quantidade de vida de todos os cinco membros do grupo**, independentemente da distância entre eles na arena.
- A conjuração pode ser interrompida.
- **Não ressuscita**; ressurreição pertence à mecânica global de checkpoints.

#### Passiva — Devoção

- Curar aliados reduz ligeiramente o tempo de recarga das **outras habilidades**.
- **Regra antiexploração:** uma utilização do Q ativa a redução de recarga **uma única vez**, mesmo que a Graça Compartilhada cure vários aliados.
- Valores e habilidades afetadas em detalhe serão definidos no balanceamento.

**Forças:** cura explosiva, recuperação emergencial, purificação e proteção direcionada.

**Fraquezas:** dependência de janelas seguras para conjurar Q e R; pouca mobilidade própria.


### Pip — Bardo

**Identidade:** curandeiro musical e suporte ofensivo. **Equipamento:** sabre e alaúde. **Recurso:** Energia. **Tipo de dano das habilidades sonoras ofensivas:** mágico.

#### Q — Acorde Vibrante

- Pip lança uma **onda sonora em linha reta** na direção do cursor.
- **Aliados atingidos:** recebem cura.
- **Inimigos atingidos:** recebem dano mágico e lentidão, se forem suscetíveis a ela.
- Cada personagem atingido, **aliado ou inimigo**, **prolonga a distância restante** da onda, permitindo atingir novos personagens adiante.
- Se não encontrar outro personagem dentro da distância restante, a onda desaparece.
- Também desaparece ao alcançar sua **distância máxima absoluta** ou ao atingir **parede/obstáculo sólido**; **não atravessa paredes**.
- Cada personagem pode ser afetado **uma única vez por lançamento**.
- Não procura alvos automaticamente nem muda de direção.

#### W — Hino de Batalha

- Pip toca uma melodia que fortalece **aliados próximos, incluindo ele próprio**.
- Concede temporariamente **aumento de dano**, **velocidade de ataque** e **velocidade de conjuração**.
- Os bônus permanecem pela duração prevista mesmo que Pip pare de tocar ou se afaste.
- Não cura diretamente.

#### E — Passo de Dança

- Pip realiza um **salto curto direcional** até o ponto indicado pelo cursor, sem precisar de alvo.
- **Na partida:** produz uma onda que causa dano mágico e lentidão aos inimigos próximos.
- **Na chegada:** produz outra onda que cura os aliados próximos, incluindo Pip.
- Chefes imunes a lentidão recebem apenas o dano.
- Serve para fuga, aproximação e reposicionamento de cura.

#### R — Crescendo

- Pip inicia uma **apresentação canalizada** com pulsos de cura em área ao seu redor, **progressivamente mais fortes**.
- Ao final, libera uma **grande explosão de cura coletiva**.
- Pip **pode caminhar** enquanto toca, mas **não pode atacar nem utilizar Q, W ou E** durante a apresentação.
- Se interrompido, a apresentação termina antecipadamente e **não produz a cura final**.
- A área de cura **acompanha Pip** e beneficia aliados dentro do raio, incluindo ele próprio.
- **Intenção aprovada:** os pulsos, por si, devem conseguir sustentar o grupo inteiro durante a apresentação, mesmo sob dano intenso, sem depender apenas da cura final. Exige calibração em testes.

#### Passiva — Inspiração

- Sempre que Pip cura um aliado por Q, E ou R, o aliado recebe **um pequeno bônus temporário de dano**.
- Pip pode receber o bônus ao curar a si mesmo.
- Novas curas **renovam a duração**, sem acumular várias cópias do bônus.
- O efeito pode coexistir com o W — Hino de Batalha.

**Forças:** fortalecimento ofensivo do grupo, mobilidade, curas em linha e ultimate de sustentação intensa.

**Fraquezas:** exige alinhamento e proximidade dos aliados; R canalizado pode ser interrompido.


### Nana — Naturalista / Druida

**Identidade:** regeneração ao longo do tempo (HoT), curas em cadeia, proteção de área e gerenciamento da duração dos efeitos. **Equipamento:** cajado vivo e totem. **Recurso:** Essência. **Dano natural/elemental:** mágico.

#### Q — Corrente Vital

- Nana lança uma corrente de energia natural em um aliado selecionado ou em si mesma.
- Cada aliado atingido recebe **cura imediata** e um efeito de **regeneração de vida ao longo do tempo (HoT)**.
- A corrente salta automaticamente para o aliado com **menor percentual de vida** dentro do alcance do próximo salto.
- Cada aliado só pode ser atingido **uma vez por lançamento**.
- Funciona fora de combate.
- Número máximo de saltos e alcance ainda serão balanceados.

#### W — Nuvem de Primavera

- Nana invoca uma **nuvem móvel** sobre um aliado ou sobre si mesma.
- A nuvem **acompanha o alvo** e produz pulsos periódicos de chuva curativa sobre ele e aliados próximos.
- Continua funcionando enquanto Nana usa outras habilidades.
- Nana só pode manter **uma nuvem ativa**.
- **Interação a especificar com a passiva:** a aplicação de regeneração associada ao W deve possuir um ciclo/duração identificável, de modo que completar o efeito possa gerar Florescimento Vital. Pulsos da nuvem não devem produzir flores ilimitadas.

#### E — Raízes Protetoras

- Nana cria raízes em uma **área escolhida no chão**.
- Inimigos comuns na área sofrem dano mágico e **imobilização temporária**.
- Chefes imunes a controle recebem dano, **sem imobilização**.
- Aliados que permanecem dentro da área recebem **redução de dano**.
- A área persiste por alguns segundos.

#### R — Despertar da Árvore da Vida

- Uma **grande Árvore da Vida** surge **no centro da arena** e transforma **toda a arena em um jardim mágico**.
- Durante a duração da ultimate, **todos os aliados recebem regeneração de vida**, independentemente da posição na arena.
- A Árvore da Vida **fortalece as regenerações do Q e do W** de Nana.
- Nana pode **movimentar-se, atacar e utilizar Q, W e E normalmente**.
- A árvore é manifestação mágica: **não pode ser destruída por inimigos, não bloqueia movimentação nem interfere nas mecânicas do chefe**.
- A árvore e o jardim desaparecem ao término.

#### Passiva — Florescimento Vital

- Quando uma **regeneração individual de Nana completa naturalmente sua duração**, uma flor desabrocha e realiza **uma cura instantânea adicional** no aliado afetado.
- **Renovar o efeito antes do fim reinicia a contagem** e impede o florescimento daquele ciclo.
- A flor cura mesmo que Nana esteja distante.
- Cada regeneração segue seu próprio ciclo; **não há ativações em cadeia infinitas**.
- **Regra proposta para balanceamento:** a passiva interage com as regenerações individuais do Q e do W, mas **não com cada pulso coletivo do R**. A interação exata do W requer especificação técnica.

**Forças:** cura prolongada, regeneração coletiva, proteção de área e recompensa por timing.

**Fraquezas:** recuperação de dano explosivo menos imediata que a de Orin; depende de gerenciar ciclos de HoT; mobilidade pessoal limitada.

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

Formas já definidas nos kits dos DPS:

| Herói | Arma principal | Segunda mão |
| --- | --- | --- |
| Brok (Bárbaro) | Machado | Segundo machado |
| Bella (Bárbara) | Machado gigante de duas mãos | — (duas mãos) |
| Ignis e Caleb (Magos) | Cajado | Grimório |
| Rex e Kael (Rangers) | Arco longo | Aljava |
| Mina e Grum (Assassinos) | Adaga | Segunda adaga |
| Trix (Inquisidora) | Besta de uma mão | Armadilha |
| Darien (Inquisidor) | Maça | Correntes |

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

Substituído pelo kit do herói: Quixote.

### Lanceiro — healer de posicionamento · principal: Sabedoria

Substituído pelo kit do herói: Bobo.

### Bárbaro — DPS de fúria corpo a corpo · principal: Força

Substituído pelos kits dos heróis: Brok e Bella.

## Arcano (Mana)

### Bruxo — tank pelo demônio · principal: Inteligência

Substituído pelo kit do herói: Wix & Abraxas.

### Invocador — healer de espíritos · principal: Sabedoria

Substituído pelo kit do herói: Lyra.

### Mago — DPS de magia à distância · principal: Inteligência

Substituído pelos kits dos heróis: Ignis e Caleb.

## Sacerdotes (Fé)

### Paladino — tank que se sustenta com a luz · principal: Vigor

Substituído pelo kit do herói: Gunnar.

### Clérigo — healer de cura forte e purificação · principal: Sabedoria

Substituído pelo kit do herói: Orin.

### Inquisidor — caçador do profano · principal: Força

Substituído pelos kits dos heróis: Trix e Darien.

## Ladinos (Energia)

### Espadachim — tank de esquiva, estilo Fiora · principal: Agilidade

Substituído pelo kit do herói: Maximilian.

### Bardo — healer pela música · principal: Sabedoria

Substituído pelo kit do herói: Pip.

### Assassino — DPS de explosão · principal: Agilidade

Substituído pelos kits dos heróis: Mina e Grum.

## Naturalistas (Essência)

### Guardião da Floresta — tank de controle do campo · principal: Vigor

Substituído pelo kit do herói: Flora.

### Druida — healer de cura contínua · principal: Sabedoria

Substituído pelo kit do herói: Nana.

### Ranger — DPS à distância · principal: Agilidade

Substituído pelos kits dos heróis: Rex e Kael.

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
