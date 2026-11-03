# RPG Isométrico

> Nome de trabalho. O nome do jogo e do mundo ainda estão em aberto.

Dungeon crawler cooperativo de alta fantasia em **Unity**, com câmera isométrica 3D. Junta **dungeons no estilo do World of Warcraft** com **combate de skills no estilo MOBA**. Solo ou em grupo de 5 (1 tank, 1 healer, 3 DPS), com 20 heróis fixos da Guilda **Ordem no Caos**.

## Documentação

### Game Design (GDD)

| # | Documento | Conteúdo |
| --- | --- | --- |
| 01 | [Visão geral](docs/gdd/01-visao-geral.md) | Pitch, pilares, referências, decisões estruturais |
| 02 | [Mundo e história](docs/gdd/02-mundo-e-historia.md) | Conclave, Guildas, Ordem no Caos, Malk, arcos |
| 03 | [Sistemas](docs/gdd/03-sistemas.md) | Grupo, kit, atributos, recursos, níveis, talentos, dificuldades |
| 04 | [Classes e skills](docs/gdd/04-classes-e-skills.md) | 5 classes, 15 subclasses, armas e kits |
| 05 | [Raças](docs/gdd/05-racas.md) | Raças, raciais, modificadores |
| 06 | [Equipamento e loot](docs/gdd/06-equipamento-e-loot.md) | Espaços, raridades, loot, consumíveis |
| 07 | [Heróis](docs/gdd/07-herois.md) | Os 20 heróis e o Malk |
| 08 | [Dungeons](docs/gdd/08-dungeons.md) | As 8 dungeons, chefes e mecânicas |
| 09 | [Falas e afinidade](docs/gdd/09-falas-e-afinidade.md) | Camadas de fala, afinidade, 27 duplas |
| 10 | [Monetização e plataformas](docs/gdd/10-monetizacao-e-plataformas.md) | Moedas, roleta, skins, DLCs, plataformas |

### Arte e técnico

- [Direção de arte](docs/arte/direcao-de-arte.md) — estilo, regras, pipeline, prompt das folhas
- [Referências visuais](docs/arte/referencias/) — folhas de personagem aprovadas
- [Arquitetura](docs/tecnico/arquitetura.md) — gameplay orientado a dados, módulos, backend
- [Modelo de dados](docs/tecnico/modelo-de-dados.md) — formatos de herói, skill, item, talentos ([exemplos](data/exemplos/))

### [Pendências](docs/pendencias.md)

Tudo que ainda não foi decidido.

## Ferramentas

Unity · Claude Code + MCP for Unity · Codex · ChatGPT/Gemini (conceitos) · Meshy/Tripo (3D e rig)

## Estrutura

```
docs/
  gdd/          Game design (01–10)
  arte/         Direção de arte e referências
  tecnico/      Arquitetura e modelo de dados
  pendencias.md Decisões em aberto
data/
  exemplos/     Exemplos de dados (JSON)
ferramentas/
  blender/      Script de limpeza dos modelos 3D gerados por IA
CLAUDE.md       Instruções para o Claude Code
```
