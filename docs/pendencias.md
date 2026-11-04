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

- [ ] Kits dos tanks e healers (os 10 DPS estão fechados)
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

## Técnico

- [ ] Testes automáticos das skills (regra do CLAUDE.md): instalar o Unity Test Framework e criar o primeiro teste (Mísseis Mágicos)
- [ ] Números do Q do Caleb (20 de Mana, 4 s por carga, 3 cargas, 8 m, 35 de dano, 0,3 s de conjuração): provisórios
- [ ] Uma ação por vez: se o alvo morrer durante a conjuração, a carga e a Mana se perdem (provisório)
- [ ] Chance de cada visual dos Mísseis Mágicos (sapinho, borboletas, letras, míssil): provisório, todos com peso 1
- [ ] Sapinho dos Mísseis Mágicos: tamanho (0,2 do modelo, ~0,3 m) e sem rastro, provisórios
- [ ] Borboletas dos Mísseis Mágicos: forma desenhada por código, cores (rosa, azul, amarelo), 6 por disparo e tamanho, provisórios
- [ ] Letras do grimório dos Mísseis Mágicos: runas inventadas geradas por código (8 símbolos), cor azul do míssil, 3 runas girando + rastro, provisórios
- [ ] W do Caleb (Página Errada): 30 de Mana, 10 s de recarga, 7 m, 2 s de transformação, sai na hora (sem projétil), bichinhos com ~0,5 m (pinguim e cachorro com ~0,8 m): provisórios
- [ ] Passiva do Caleb (Acerto Épico): 5 acertos para encher, skill épica com números ×1,5 e visual ×2, sem custo; acertos não contam enquanto a épica está pronta; o acerto da própria skill épica já conta para a próxima: provisórios
- [ ] Recurso por classe: hoje todo herói recebe Mana (300, +4/s) até existir a ficha do herói nos dados
- [ ] Números do ataque básico (alcance 6 m, 1 ataque/s, 20 de dano, projétil): provisórios, iguais para todos por enquanto
- [ ] Visual do ataque básico do Caleb: bolinha dourada de 0,15 m com rastro roxo de 0,15 s (cores e tamanhos provisórios)
- [ ] Controles e câmera: botão direito anda (provisório, estilo LoL); velocidade 3,5 m/s; câmera com inclinação 50°, rotação 45° e distância 16 m (zoom de 4 a 26 m)
- [ ] Backend: monólito modular ou microsserviços (ver [Arquitetura](tecnico/arquitetura.md))
- [ ] Usar plataforma pronta (Nakama, PlayFab, Unity Gaming Services) ou backend próprio
- [ ] Modelo de rede do multiplayer
