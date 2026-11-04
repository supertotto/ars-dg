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

## Mapa do código

- **Projeto Unity em `unity/`:** Unity 6 (6000.6.7f1), URP 17.6, Input System. Detalhes de pastas, menu **ARS** e importação de FBX em [Projeto Unity](docs/tecnico/unity.md).
- **Assemblies:** `ARS.Runtime` (namespace raiz `ARS`, em `Assets/Scripts/Runtime/`) e `ARS.Editor` (namespace `ARS.EditorTools`, só editor, depende do Runtime). Pastas do Runtime: `Data`, `Combat`, `Characters`, `Camera` (`ARS.CameraSystem`), `Effects`, `UI`.
- **Fluxo de uma skill:** `SkillDef` / `AtaqueBasicoDef` (ScriptableObjects em `Assets/Data/`) → `KitDeSkills` / `AtaqueBasico` → na hora ou via `Projetil` → `AplicadorDeEfeitos.Aplicar` (um `switch` por `TipoDeEfeito`) → `Combatente.ReceberDano` / `Curar`.
- **Efeito genérico novo:** valor novo em `TipoDeEfeito` ([Efeito.cs](unity/Assets/Scripts/Runtime/Data/Efeito.cs)) + `case` em [AplicadorDeEfeitos.cs](unity/Assets/Scripts/Runtime/Combat/AplicadorDeEfeitos.cs). Hoje só **Dano** e **Cura** têm código; os outros estão marcados "(a fazer)" e só geram aviso.
- **Tecla da skill vem do nome do asset:** `<Herói>_Q_<Nome>` (Q/W/E/R). **ARS → Montar heróis** liga as skills e o ataque básico ao prefab.
- **Ferramentas de editor** (`Assets/Scripts/Editor/`): `HeroBuilder` (pastas, montar heróis), `HeroModelImporter` (Humanoid, loop, Bake Into Pose na importação), `TestSceneBuilder` (gera `Scenes/Teste.unity`), `VitrineDeEfeitos` (cena `Vitrine.unity` com os VFX de uma pasta), `ArsPaths` (caminhos fixos).
- **Formato de dados de referência:** `data/exemplos/*.json`, descrito em [Modelo de dados](docs/tecnico/modelo-de-dados.md).
- **Pipeline de arte:** `ferramentas/blender/` limpa modelos gerados por IA antes do Mixamo (ver o README da pasta).

### Estado atual (diferenças em relação à arquitetura)

- Os módulos da [Arquitetura](docs/tecnico/arquitetura.md) (`Core`, `Abilities`, `AI`, `Dungeons`, `Net`) **ainda não existem**; tudo está num assembly só. `Data` e `Combat` se referenciam (ex.: `TipoDeDano` está em `Combat`), o que precisa ser resolvido antes de separar assemblies.
- **Testes:** EditMode (Unity Test Framework) no asmdef `ARS.Tests.EditMode`, em `Assets/Tests/EditMode/`. Cobrem estados, efeitos, sorteio de visuais e passiva. Ainda faltam testes por skill (cenário lançador + alvo), como pede a regra 5. Rodar: **Window → General → Test Runner**.
- Tags e modificadores: `TagDeEstado`, `EstadoDef` e `Modificador` em `Data`, `Estados` em `Combat` (ver [Projeto Unity](docs/tecnico/unity.md)).
- Todos os números atuais (skills, ataque básico, câmera, velocidade) são **provisórios**.

### Cuidados

- Packs de terceiros em `Assets/` (`Eric VFX Studio`, `GabrielAguiarProductions`, `Hovl Studio`, `ithappy`, `JMO Assets`, `Quaternius`, `Vampire`): não editar, só usar (exceto converter materiais para URP).
- Arquivos de arte e áudio grandes vão pelo **Git LFS** (ver `.gitattributes`).
- Nunca apague nem edite `.meta` à mão; o Unity os gera e eles guardam as referências.
- **MCP for Unity** (pacote `com.coplaydev.unity-mcp`, fixado no `manifest.json`): com o Unity aberto, **Window → MCP for Unity → Start Server**. O Claude Code acessa por `http://127.0.0.1:8080/mcp` (servidor `UnityMCP`).

## Trabalho em dupla com o Claude Code

- Mais de um agente mexe neste repositório. Antes de começar: `git pull`. Ao terminar: commit pequeno com mensagem clara.
- Nunca trabalhem os dois ao mesmo tempo nos mesmos arquivos.

## Quem fez o quê (Claude e GPT)

- **No código:** todo trecho criado ou alterado leva um comentário dizendo quem fez e o quê, no começo do trecho (classe, método, bloco ou linha):
  - `// claude: o que foi feito`
  - `// gpt: o que foi feito`
  - Arquivos sem comentário (JSON, assets e cenas do Unity, Markdown) ficam registrados só no commit.
- **Nos commits:** a mensagem começa com `[claude]` ou `[gpt]`, conforme quem executou.
- **Delegação:** o Claude Code pode passar tarefas braçais para o GPT pelo terminal, com `codex exec "<tarefa>"`. O Claude revisa o resultado antes de commitar, e o commit sai como `[gpt]`.

## Documentação

- Escreva a documentação em português, direta e curta.
- Quando uma decisão for tomada, atualize o documento do GDD e tire o item de `docs/pendencias.md`.
