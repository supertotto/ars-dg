# Pendências (decisões em aberto)

> Nada aqui está decidido. Tudo que for sugestão precisa da aprovação do Totto antes de virar regra.

## Identidade

- [ ] Nome do jogo
- [ ] Nome do mundo
- [ ] Nomes próprios: worgen, draconato, tiefling, deuses inspirados na mitologia asteca (Pântano Sombrio)

## História

- [ ] Ponto de vista do jogador: com heróis fixos, quem é o jogador na história (antes era um protagonista silencioso)
- [ ] Arco do Sir Quixote (Cavaleiro)
- [ ] Escrever os ~100 capítulos de afinidade (5 por herói)
- [ ] Escrever as cenas do salão entre missões

## Raças e heróis

- [ ] Como as raciais se aplicam aos heróis fixos
- [ ] Nefilim e raças de DLC (worgen, draconato, minotauro): viram heróis novos?
- [ ] Morto-vivo e tiefling jogáveis?
- [ ] Sub-raças
- [ ] Subclasses híbridas

## Sistemas

- [ ] Controladores: quais heróis ocupam a vaga (heróis novos, DPS reaproveitados ou os dois) e os kits deles
- [ ] Controlador na estrutura de classes: vira uma 4ª subclasse em cada classe?
- [ ] Revisar com o grupo de 6: buscador de grupo, Mítico (só jogadores reais), IA aliada e leitura visual na câmera isométrica
- [ ] Healers: as curas descritas como "os cinco membros do grupo" passam a valer para os 6
- [ ] Revisar os nomes de todas as skills antes da versão final (inclui os nomes ainda abertos de Wix & Abraxas, o "Crescendo" do Pip, igual ao da Sona, e mais personalidade nos nomes da Nana)
- [ ] Autocura dos tanks x importância do healer: ajustar nos números
- [ ] Cada healer curando o grupo sozinho em todos os cenários; bônus de dano do Pip sem virar obrigatório; Lyra sem ser sempre a melhor escolha
- [ ] Florescimento Vital (passiva da Nana): como interage tecnicamente com a Nuvem de Primavera (W)
- [ ] Ameaça (aggro): coeficientes, prioridade de alvo e regras de renovação do Taunt
- [ ] IA aliada jogando os heróis mecânicos (Maximilian, Wix & Abraxas, Bella, Kael)
- [ ] Quais acertos alimentam passivas, acúmulos, escudos e recuperação de recurso (acertos múltiplos, invocações, dano ao longo do tempo)
- [ ] Regras de canalização e carregamento: andar, cancelar e ser interrompido
- [ ] Como reduções de armadura se acumulam
- [ ] Customizado: alcance e arma do Q do Inquisidor (Trix à distância, Darien corpo a corpo) e compatibilidade de arma com skill
- [ ] Modo customizado: kit básico de cada subclasse, níveis de escolha, se ultimate e passiva entram na escolha
- [ ] Árvore de talentos: por subclasse ou por herói, agora que os kits são por herói
- [ ] Forma da arma: valores dos bônus de cada forma
- [ ] Refazer a folha da Bella (Bárbara) com um machado grande de duas mãos
- [ ] Fala dos "dois Redemoinhos" (Brok e Bella): ajustar se só o Brok girar no kit novo
- [ ] Animações de personalidade por herói (parado, andar, comemoração) e animações de skill por herói
- [ ] Equipamento por herói ou compartilhado entre eles
- [ ] Nível máximo e faixas de nível das dungeons
- [ ] Lista final de feitiços
- [ ] Todos os números e o balanceamento
- [ ] Abominação: mecânica da corrente de pontos
- [ ] Voadora: mecânica
- [ ] Esquema de controle no console
- [ ] Interações entre elementos
- [ ] Altura de cada herói e orçamento de triângulos e textura dos modelos (o script de limpeza usa 1,8 m, 15.000 triângulos e 2048 px, provisórios)

- [ ] IA de inimigo: visão 8 m, desiste se for puxado a mais de 20 m de casa (volta e se cura), ameaça = 1 por ponto de dano, decide a cada 0,25 s: provisórios
- [ ] Vampirinho de teste (modelo do Lorde, Skin 2, 70% do tamanho): 300 de vida, 3 m/s, golpe corpo a corpo de 15 a cada 1,4 s, corpo some 4 s depois de morrer: provisórios. Ainda não está decidido se os mobs usam o modelo do Lorde
- [ ] Herói abatido volta sozinho depois de 5 s com a vida cheia: só para testes, até existir o sistema de reviver

## Conteúdo

- [ ] Hub social
- [ ] Zonas de caça
- [ ] Picos Congelados e Torre do Arquimago (pós-lançamento)

## Monetização

- [ ] Bônus de pré-venda que continua exclusivo depois que o nefilim e a Ilha Flutuante viram DLC
- [ ] Preços

## Arte

- [ ] Trocar ou manter os pentagramas do Wix e do Abraxas (classificação etária)
- [ ] Membrana das asas do modelo do Lorde Vampiro (pack `Vampire`): a malha `Glass1` (material `Mat_Vampyr_Glass`) não aparece no URP (face virada para trás); com os dois lados ligados vira placas brancas translúcidas. Decidir: refazer o material, esconder ou manter só os ossos das asas

- [ ] HUD: arte gerada por IA aplicada (painel escuro com ouro). Layout (retrato à esquerda, passiva, Q/W/E/R, barras embaixo das skills), tamanhos, cores das barras (vida vermelha, mana azul) e fonte: provisórios

## Técnico

- [ ] Testes por skill (regra do CLAUDE.md): cenário com lançador e alvo para cada skill (a estrutura de testes e os testes de estados, efeitos e passiva já existem)
- [ ] Números do Q do Caleb (20 de Mana, 4 s por carga, 3 cargas, 8 m, 35 de dano, 0,3 s de conjuração): provisórios
- [ ] Uma ação por vez: se o alvo morrer durante a conjuração, a carga e a Mana se perdem (provisório)
- [ ] Chance de cada visual dos Mísseis Mágicos (sapinho, borboletas, letras, míssil): provisório, todos com peso 1
- [ ] Sapinho dos Mísseis Mágicos: tamanho (0,2 do modelo, ~0,3 m) e sem rastro, provisórios
- [ ] Borboletas dos Mísseis Mágicos: forma desenhada por código, cores (rosa, azul, amarelo), 6 por disparo e tamanho, provisórios
- [ ] Letras do grimório dos Mísseis Mágicos: runas inventadas geradas por código (8 símbolos), cor azul do míssil, 3 runas girando + rastro, provisórios
- [ ] W do Caleb (Página Errada): 30 de Mana, 10 s de recarga, 7 m, 2 s de transformação, sai na hora (sem projétil), bichinhos com ~0,5 m (pinguim e cachorro com ~0,8 m): provisórios
- [ ] Passiva do Caleb (Acerto Épico): 5 acertos para encher, skill épica com números ×1,5 e visual ×2, sem custo; acertos não contam enquanto a épica está pronta; o acerto da própria skill épica já conta para a próxima: provisórios
- [ ] Visual épico: camada dourada (faíscas do pacote Hovl) no projétil e no impacto, tamanhos provisórios; vale para a skill épica de qualquer herói
- [ ] Cappuccino: modelo do gato do pacote ithappy pintado de laranja (laranja do tigre); fica 0,9 m ao lado e 0,5 m atrás, anda a 2 m/s, corre a 5 m/s se ficar 2,5 m para trás, aparece ao lado se ficar a 15 m: provisórios
- [ ] E do Caleb (Esqueci o Gato): 40 de Mana, 12 s de recarga, 7 m (mirar mais longe teleporta até o limite), explosão de 60 de dano em 2,5 m, janela de volta de 4 s, abelhas: 4 s, 8 de dano a cada 0,5 s em 2,5 m: provisórios
- [ ] Visual da E: chegada com o "puf" mágico grande (pacote JMO) e saída sem efeito; abelhas desenhadas por código (2 quadros): provisórios
- [ ] R do Caleb (Isso Não Estava no Livro): 100 de Mana, 60 s de recarga, 9 m, 0,5 s de conjuração, área de 3,5 m por 6 s, gato 6× maior (~2,2 m, maior que o Caleb), caçada em ciclo bote → sapo: bote a partir de 3 m, salto de 1,2 m de altura em 0,45 s passando 2 m do inimigo, mordida de 25 de dano no meio do salto, no máximo um bote a cada 0,6 s: provisórios. A R não conta acerto para a passiva (quem acerta é o gato)
- [ ] Visual da R: círculo de runas verde (pacote Hovl, pintado da cor dos sapos) do tamanho da área, chuva de sapinhos (modelo da Q, em verde-claro pelas cores gravadas nos vértices) e patinha laranja desenhada por código: provisórios
- [ ] HUD usa o `Text` antigo da uGUI com a fonte padrão; trocar por TextMeshPro quando a fonte do jogo for escolhida
- [ ] Apagar pelo Unity o que não é mais usado: `HudProvisorio.cs`, os ícones de `Assets/UI/Icones/game-icons/` e os sprites gerados `Assets/UI/Base/Preenchimento.png` e `BrilhoQuadrado.png` (a arte da IA entrou no lugar)
- [ ] Recurso por classe: hoje todo herói recebe Mana (300, +4/s) até existir a ficha do herói nos dados
- [ ] Números do ataque básico (alcance 6 m, 1 ataque/s, 20 de dano, projétil): provisórios, iguais para todos por enquanto
- [ ] Visual do ataque básico do Caleb: bolinha dourada de 0,15 m com rastro roxo de 0,15 s (cores e tamanhos provisórios)
- [ ] Controles e câmera: botão direito anda (provisório, estilo LoL); velocidade 3,5 m/s; câmera com inclinação 50°, rotação 45° e distância 16 m (zoom de 4 a 26 m)
- [ ] Backend: monólito modular ou microsserviços (ver [Arquitetura](tecnico/arquitetura.md))
- [ ] Usar plataforma pronta (Nakama, PlayFab, Unity Gaming Services) ou backend próprio
- [ ] Modelo de rede do multiplayer
