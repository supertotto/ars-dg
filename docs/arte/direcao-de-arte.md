# Direção de arte

## Estilo

- **Semi-estilizado pintado**, na linha do Dota 2: proporções um pouco exageradas, cores fortes, textura pintada à mão.
- **Silhueta legível de cima.** A câmera é isométrica, então cada herói precisa ser reconhecido só pela forma e pelas cores dominantes (o chapéu do Caleb, a capa de urso da Bella, o estandarte do Bobo).
- **Referência de estilo:** a folha do Caleb (`referencias/caleb.jpg`) foi a primeira aprovada e serve de base para todas as outras.
- Os heróis definem a linguagem visual do mundo; cenários e inimigos seguem a mesma paleta saturada e a mesma pintura.

## Regras fixas

- O símbolo dos deuses do Conclave é o **sol** — nunca uma cruz (inclusive no tabardo do Maximilian e nas armas do Darien).
- O **brasão da Ordem no Caos** tem cores fixas: escudo azul e prata, asas brancas, espada longa prateada na vertical (estilo da espada do Aragorn, sem copiar a Andúril). O estandarte do Bobo é a referência oficial.
- Cada herói tem **cores dominantes** registradas em [Heróis](../gdd/07-herois.md). **Em aberto:** o quanto uma skin pode mudar a silhueta.
- Nada de copiar personagens de outras obras: as referências citadas (Presto, Dandelion, Yami, mosqueteiros) são de **vibe**, e os elementos marcantes delas ficam de fora.

## Pipeline de arte

1. **Folha de personagem** no ChatGPT (imagem), anexando a folha do Caleb como referência de estilo.
2. **Imagem → 3D** no Meshy ou Tripo, a partir da pose frontal (ou T-pose).
3. **Auto-rig** (Meshy, Tripo ou Mixamo) em um dos esqueletos padrão.
4. **Unity:** importar, ajustar materiais e escala, testar na câmera isométrica.

Notas:
- Simplificar detalhes pequenos antes de gerar o 3D (correntes finas, cordas, objetos soltos viram textura ou somem).
- Elementos que flutuam ou são etéreos (os espíritos da Lyra, a fumaça das mãos do Ignis, a égide do Gunnar) viram **VFX**, não malha.
- Usar **poucos esqueletos padrão** para compartilhar animações (sugestão: humanoide médio, baixo/largo, grande/corcunda — em aberto).

## Modelo de prompt da folha de personagem

> Folha de personagem (character sheet) de [NOME], [RAÇA] [CLASSE/SUBCLASSE], no mesmo estilo da imagem de referência anexada: semi-estilizado, pintado à mão, cores fortes, como Dota 2.
>
> **Layout:**
> - Nome do personagem no canto superior esquerdo
> - Duas poses grandes: uma neutra e uma de ação/combate
> - Vistas: Frente, 3/4, Lado e Costas
> - 5 expressões faciais
> - Props/armas em destaque, separados
> - Paleta de cores
> - Um preview pequeno com o título "Visão Isométrica (Top-Down)"
> - Todos os rótulos em português
>
> **Personagem:** [descrição visual completa da ficha em 07-herois.md]
>
> **Cores:** [cores dominantes]
>
> **Não incluir:** [elementos de outras obras a evitar]

## Ajustes para o 3D (diferenças entre a folha e o modelo final)

| Herói | Ajuste |
| --- | --- |
| Gunnar (Paladino) | Cabelo e barba pretos; adicionar pingente de sol |
| Flora (Druida) | Machado de lenhador quebrado nas costas; detalhes do rosto opcionais |
| Mina (Assassina) | Algodão nos sininhos do tornozelo |
| Maximilian (Duelista) | Escala maior que os outros hobgoblins |
| Bella (Berserker) | Presas visíveis |
| Malk (dono da Guilda) | Duas posições de asa: recolhida (padrão) e aberta |

## Classificação etária

As folhas do Wix e do Abraxas (Bruxo) usam pentagramas. Avaliar trocar por outro símbolo demoníaco próprio do jogo, para não pesar na classificação etária e nas lojas. **Em aberto.**

## Arquivos

- `referencias/` — folhas aprovadas de cada herói e do Malk, com alternativas e versões antigas guardadas.
- `referencias/caleb-estudos/` — estudos de estilo (Gemini e ChatGPT) que levaram à folha do Caleb.
- `referencias/classes-referencia.jpg` — referência geral de classes.
