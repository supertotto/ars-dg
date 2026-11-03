# 01 — Visão geral

> Nome do jogo e do mundo: **em aberto**. Nome de trabalho do projeto: *RPG Isométrico*.

## O jogo em uma frase

Dungeon crawler cooperativo de alta fantasia, com câmera isométrica 3D, jogável solo ou em grupo de 5, que junta **dungeons no estilo do WoW** (feitas à mão, chefes com mecânica, loot persistente) com **combate de skills no estilo MOBA**.

## Pilares

| Pilar | O que significa |
| --- | --- |
| Dungeons memoráveis | Cada dungeon tem magia, cor, tom e tema próprios; chefes com fases e mecânicas que ensinam algo |
| Elenco com alma | 20 heróis fixos da Guilda **Ordem no Caos**, com personalidade, duplas e histórias |
| Grupo de 5 | Trindade clássica: 1 tank, 1 healer, 3 DPS — com jogadores reais ou heróis controlados pela IA |
| Nada de pay-to-win | Poder só se conquista jogando |

## Referências

- **Dungeons e progressão:** World of Warcraft (dungeons clássicas, Mítico+)
- **Combate e kit:** League of Legends, Dota 2
- **Câmera e loot:** Diablo
- **Heróis e coleção:** Ravenswatch, Hades
- **Guilda e tom:** Fairy Tail, Black Clover, Monster Hunter (a Guilda/Conclave)
- **Chefe final:** Fatalis (Monster Hunter)

## Decisões estruturais

| Tema | Decisão |
| --- | --- |
| Engine | Unity |
| Câmera | Isométrica 3D |
| Mecânica | Própria, pensada para ação em tempo real (não usa as regras do ARS RPG) |
| Mundo | Próprio, de alta fantasia (God's Stones fica reservado para o livro) |
| Personagens | **Lançamento com heróis fixos**. Modo de personagem customizado pode vir no futuro como DLC — a base técnica nasce preparada para isso |
| Modelo de negócio | Jogo pago e barato, com DLCs e skins; sem pay-to-win |
| Plataformas | PC (Steam) primeiro, depois console; mobile talvez, grátis para quem comprou |
| Controles no PC | Clique para mover, skills no QWER, ataque básico automático no botão direito |

## Ferramentas

- **Claude Code:** código e arquitetura, ligado ao editor do Unity via MCP
- **Codex:** revisão e tarefas em paralelo
- **Gemini / ChatGPT:** arte conceitual (folhas de personagem)
- **Meshy / Tripo:** imagem → modelo 3D com auto-rig

## Mapa dos documentos

| Documento | Conteúdo |
| --- | --- |
| [02 — Mundo e história](02-mundo-e-historia.md) | Mundo, Conclave, Guildas, Ordem no Caos, Malk, fio da história |
| [03 — Sistemas de jogo](03-sistemas.md) | Grupo, kit, controles, atributos, recursos, elementos, nível, talentos, dificuldade |
| [04 — Classes e skills](04-classes-e-skills.md) | 5 classes, 15 subclasses, kits completos, armas |
| [05 — Raças](05-racas.md) | Raças, raciais, modificadores de atributo, regras visuais |
| [06 — Equipamento e loot](06-equipamento-e-loot.md) | Espaços, raridade, loot, consumíveis |
| [07 — Heróis](07-herois.md) | Os 20 heróis da Ordem no Caos |
| [08 — Dungeons](08-dungeons.md) | As 8 dungeons + Ilha Flutuante, com todos os chefes |
| [09 — Falas e afinidade](09-falas-e-afinidade.md) | Afinidade, capítulos e as 27 duplas |
| [10 — Monetização e plataformas](10-monetizacao-e-plataformas.md) | Moedas, roleta, DLCs, pré-venda, plataformas |
| [Direção de arte](../arte/direcao-de-arte.md) | Estilo, fichas visuais, prompts, pipeline 3D |
| [Arquitetura](../tecnico/arquitetura.md) | Gameplay orientado a dados, backend |
| [Modelo de dados](../tecnico/modelo-de-dados.md) | Esquemas e exemplos JSON |
| [Pendências](../pendencias.md) | Tudo que ainda está em aberto |
