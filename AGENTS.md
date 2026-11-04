# Instruções para agentes de código (Codex e outros)

> Mesmo conteúdo do `CLAUDE.md`. Ao mudar um, mude o outro.

## Contexto

Jogo em Unity: dungeon crawler cooperativo isométrico (dungeons estilo WoW + skills estilo MOBA). Toda a documentação está em `docs/`, em português. Comece pelo [README](README.md) e pela [Visão geral](docs/gdd/01-visao-geral.md).

## Antes de mexer em algo

- Leia o documento do GDD relacionado à tarefa.
- Confira [Pendências](docs/pendencias.md): o que está lá **não foi decidido**.

## Decisões criativas

- **Nunca decida sozinho** nada criativo (nomes, história, falas, visual, armas, mecânicas de chefe, números). Pergunte primeiro, com opções numeradas ou por letra, uma pergunta por vez.
- Se precisar de um valor provisório para o código funcionar, marque como provisório e registre em `docs/pendencias.md`.
- Ao falar de um herói, cite sempre a classe/subclasse dele (ex.: "Caleb (Mago)").

## Regras de código (gameplay)

1. **Orientado a dados.** Heróis, subclasses, skills, itens, talentos, inimigos e dungeons são dados (ScriptableObjects/JSON), não classes.
2. **Skills = lista de efeitos genéricos** (dano, cura, escudo, empurrão, enraizar, aplicar elemento, invocar...). Skill nova é combinação nova; se faltar algo, crie um efeito genérico novo.
3. **Tags e modificadores** para estados. As regras checam tags, nunca nomes.
4. **Proibido caso especial por herói** (`if (heroi == "...")`). Cada herói tem kit próprio, mas o kit é só dados: uma lista de skills montadas com efeitos genéricos. Qualquer skill pode ser usada por qualquer personagem (o modo customizado depende disso).
5. **Teste automático para cada skill.**
6. **Números fora do código**, em dados.
7. Módulos com dependência em uma direção (ver [Arquitetura](docs/tecnico/arquitetura.md)). UI sem regra de jogo.
8. Equipamento separado do visual (o visual vem das skins).

## Trabalho em dupla com o Claude Code

- Mais de um agente mexe neste repositório. Antes de começar: `git pull`. Ao terminar: commit pequeno com mensagem clara.
- Nunca trabalhem os dois ao mesmo tempo nos mesmos arquivos.

## Quem fez o quê (Claude e GPT)

- **No código:** todo trecho criado ou alterado leva um comentário dizendo quem fez e o quê, no começo do trecho (classe, método, bloco ou linha):
  - `// claude: o que foi feito`
  - `// gpt: o que foi feito`
  - Arquivos sem comentário (JSON, assets e cenas do Unity, Markdown) ficam registrados só no commit.
- **Nos commits:** a mensagem começa com `[claude]` ou `[gpt]`, conforme quem executou.
- **Delegação:** o Claude Code pode passar tarefas braçais para o GPT pela ferramenta MCP `codex` (configurada em `.mcp.json`). O Claude revisa o resultado antes de commitar, e o commit sai como `[gpt]`.

## Documentação

- Escreva a documentação em português, direta e curta.
- Quando uma decisão for tomada, atualize o documento do GDD e tire o item de `docs/pendencias.md`.
