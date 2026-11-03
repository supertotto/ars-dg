# Arquitetura técnica

> Proposta inicial. Os pontos marcados **em aberto** ainda não foram decididos.

## Objetivo principal: evitar o "código espaguete"

O problema que jogos como o LoL enfrentam depois de anos não é o servidor — é a **lógica de gameplay**: cada campeão com regras próprias escritas à mão, exceções em cima de exceções, e mexer em um quebra outro. Isso se resolve com a forma como as habilidades são construídas, não com microsserviços.

### Regras de ouro

1. **Gameplay orientado a dados.** Heróis, subclasses, habilidades, itens, talentos, inimigos e dungeons são **dados** (ScriptableObjects no Unity, exportáveis para JSON), não classes de código.
2. **Habilidades compostas por efeitos genéricos.** Uma habilidade é uma lista de efeitos reutilizáveis: dano, cura, escudo, empurrão, puxão, enraizar, atordoar, lentidão, aplicar elemento, invocar, teleporte, buff/debuff de atributo. Uma skill nova é uma combinação nova, não código novo.
3. **Sistema de tags e modificadores** (inspirado no Gameplay Ability System da Unreal): estados como `Atordoado`, `EmChamas`, `Invisivel`, `Furia.Ativa` são tags; regras checam tags, nunca o nome do herói.
4. **Nenhum caso especial por herói.** Proibido `if (heroi == "Caleb")`. Se uma subclasse precisa de algo novo (por exemplo, o demônio invocado do Bruxo), cria-se um **efeito genérico novo** que qualquer habilidade poderia usar.
5. **Testes automáticos por habilidade.** Cada skill tem um teste que monta um cenário (lançador, alvo, posição) e confere o resultado. Mudou um efeito? Os testes mostram o que quebrou.
6. **Números fora do código.** Balanceamento em tabelas de dados, ajustável sem recompilar.

## Cliente (Unity)

Módulos separados por responsabilidade, com dependências em uma direção só:

| Módulo | Responsabilidade |
| --- | --- |
| `Core` | Tags, atributos, modificadores, eventos |
| `Abilities` | Execução de habilidades, efeitos, recargas, recursos |
| `Combat` | Dano, elementos, ameaça, morte e reviver |
| `Characters` | Heróis, nível, talentos, equipamento |
| `AI` | Inimigos, chefes, fases, mecânicas |
| `Dungeons` | Salas, pontos seguros, chaves, modificadores, cronômetro |
| `UI` | Interface, sem regra de jogo dentro |
| `Net` | Sincronização multiplayer |
| `Data` | Definições (ScriptableObjects) e carregamento |

## Backend

Módulos com fronteiras claras, cada um com seus próprios dados:

- Contas e autenticação (conta única entre plataformas)
- Heróis e progresso (nível, afinidade, talentos)
- Inventário e equipamento
- Casa de leilões / quadro de trocas
- Guildas e social
- Buscador de grupo
- Progresso do Conclave (patente, campanha)
- Loja (ouro e moeda premium, separados)

### Monólito modular ou microsserviços — **em aberto**

| Opção | Prós | Contras |
| --- | --- | --- |
| **A) Monólito modular** (recomendação) | Um deploy, simples de operar e barato no começo; módulos separados no código, cada um com seu banco/esquema; dá para extrair um serviço depois | Escala tudo junto até a extração |
| **B) Microsserviços desde o início** | Cada parte escala e é publicada sozinha | Muito mais infraestrutura, custo e complexidade para uma equipe pequena; não resolve o espaguete de gameplay |

Também vale avaliar plataformas prontas antes de construir tudo: **Nakama**, **PlayFab** ou **Unity Gaming Services**.

## Preparado para o modo customizado

Lançamento com heróis fixos, mas a base já permite personalização depois:

- Poucos esqueletos padrão, compartilhados entre raças.
- Equipamento separado do visual (equipamento dá atributos, skins dão aparência).
- Herói = dados (raça + subclasse + aparência + kit), não um objeto feito à mão.

## Ferramentas de desenvolvimento

- **Unity** + **Claude Code** com o **MCP for Unity** (CoplayDev), para o Claude ler e mexer na cena, nos assets e nos scripts.
- **Codex** como apoio.
- Arte: ChatGPT/Gemini para conceitos, Meshy/Tripo para 3D (ver [Direção de arte](../arte/direcao-de-arte.md)).
