# Projeto Unity

Projeto em `unity/` (Unity 6, URP, Input System).

## Pastas

```
Assets/
  Characters/<Herói>/
    Model/        FBX do herói baixado do Mixamo ("With Skin")
    Textures/     textura do herói (o _cor.png do script do Blender)
    Animations/   animações só deste herói
    Materials/    criado automaticamente
  Animations/
    Common/       animações usadas por todos (tomar dano, cair…)
    ...           packs do Mixamo ("Without Skin")
  Prefabs/Heroes/ prefabs montados automaticamente
  Scenes/         Teste.unity (cena de teste)
  Scripts/
    Runtime/      código do jogo (ARS.Runtime)
    Editor/       ferramentas do editor (ARS.Editor)
```

## Menu ARS (na barra de cima do Unity)

| Opção | O que faz |
| --- | --- |
| Criar pastas do projeto | Cria a estrutura acima |
| Novo herói (criar pastas)… | Pede o nome e cria `Characters/<Herói>/Model`, `Textures`, `Animations`, `Materials` |
| Reconfigurar FBX (Humanoid e animações) | Refaz a configuração de todos os FBX. Use se as animações não tocarem (por exemplo, se os arquivos entraram antes das ferramentas) |
| Montar heróis | Para cada herói com FBX em `Model/`: cria o material com a textura, o Animator (parado/andar/correr) e o prefab com movimento por clique |
| Criar cena de teste | Chão, blocos, luz, câmera isométrica e os heróis montados. Botão direito anda, rodinha dá zoom |

## Combate (primeira versão)

- **Botão direito no chão:** anda. **Botão direito num inimigo:** ataca sozinho até receber outra ordem (estilo LoL).
- O ataque básico de cada herói é um dado em `Assets/Data/Heroes/<Herói>_AtaqueBasico.asset` (alcance, intervalo, dano, projétil). Criado com valores **provisórios** na primeira montagem; ajuste no Inspector.
- **Uma ação por vez** (`TravaDeAcao`): durante o balanço do ataque básico ou a conjuração de uma skill (`tempoDeConjuracao` no `SkillDef`), o herói não faz outra ação. Skill apertada nesse meio tempo fica guardada e sai assim que a trava libera (vale a última apertada); mandar andar descarta a skill guardada. O ataque básico não passa na frente de uma skill guardada.
- **Estados e passivas:** estados são dados (`EstadoDef`, em `Assets/Data/Estados`) com tags (as regras checam tags) e modificadores (ex.: próxima skill mais forte e sem custo). Efeitos genéricos `AplicarEstado` e `Interromper`. Passiva de contador = `PassivaDef` na pasta de skills do herói com `_P_` no nome (ex.: `Caleb_P_AcertoEpico`); **ARS → Montar heróis** liga. Acerto que alimenta passiva: cada uso que acerta conta 1 (cada projétil = 1; ataque básico = 1).
- **Companheiro** (bicho que acompanha o herói): prefab em `Assets/Prefabs/Companheiros/<Herói>_<Nome>.prefab` com o componente `Companheiro`; **ARS → Montar heróis** liga e o herói o cria ao entrar no jogo. Skills mandam ele sair e voltar com `IrPara`, `FicarEm` e `Seguir`, e caçar numa área com o efeito `CompanheiroCaca` (usa uma `ZonaDef` para raio, duração e ataque; o dano sai em nome do dono e não conta acerto). Se sair de um posto para caçar, deixa a `marcaAoSairDoPosto` e volta para lá depois.
- **Skills no chão, áreas, zonas e reativação:** alvo `PontoNoChao` (limitado ao alcance), entrega `AreaNoPonto` (todos os inimigos no raio; conta 1 acerto), `efeitosNoLancador` (ex.: `Teleportar`, `CriarZona`, `CompanheiroFica`), `ZonaDef` (dano por tique em área, em `Assets/Data/Zonas`; tiques não contam acerto) e reativação (`janelaDeReativacao` + `efeitosAoReativar`; o estado da janela guarda o ponto de partida e tem `efeitosAoTerminar`).
- **Inimigos:** dados em `Assets/Data/Inimigos` (`InimigoDef`: modelo, animações, vida, velocidade, ataque básico, skills, visão); **ARS → Montar inimigos** gera `Assets/Prefabs/Inimigos/<nome>.prefab` com os mesmos componentes do herói (movimento, ataque, skills, estados) + `Ameaca` (aggro por dano), `CerebroDoInimigo` (módulo `AI`: ocioso → combate contra quem tem mais ameaça → volta para casa e se cura se for puxado longe) e `Abatido`. O controlador de animação usa os parâmetros `Velocidade`, `Atacar` e `Morrer`. A cena de teste coloca 2 de cada inimigo a ~12 m do herói.
- A animação de ataque é escolhida pelo nome do arquivo: `attack`, `atacar`, `ataque`, `cast` ou `spell`. Coloque na pasta `Animations` do herói.
- A cena de teste tem **bonecos de treino**: mostram a vida, os números de dano e voltam à vida cheia depois de 3 segundos sem apanhar.

## Skills (primeira versão)

- Cada skill é um dado (`SkillDef`) em `Assets/Data/Skills/<Herói>/`. O nome do arquivo diz a tecla: `Caleb_Q_MisseisMagicos` vai no **Q** (`_W_`, `_E_`, `_R_` para as outras). **ARS → Montar heróis** liga sozinho.
- Uma skill tem custo, recarga, **cargas** (ex.: 3 mísseis que voltam um de cada vez), alvo, alcance, como chega (na hora ou por projétil) e uma **lista de efeitos genéricos**.
- Efeitos com código hoje: **Dano** e **Cura**. Os outros (escudo, lentidão, atordoar, enraizar, empurrar, elemento, invocar) já existem nos dados e serão implementados um a um.
- **Q, W, E, R** usam a skill no inimigo sob o cursor (ou o mais perto do cursor).
- Para criar uma skill nova: botão direito na pasta → **Create → ARS → Skill**.
- HUD provisório com letras grandes: vida, Mana e as 4 teclas com cargas e recarga (o tamanho da letra fica no componente **Hud Provisorio**).

## Importação automática

Ao arrastar um FBX do Mixamo para as pastas acima, o projeto já configura sozinho (só na primeira importação):

- Rig **Humanoid** (as animações servem para qualquer herói)
- Animações de ciclo (parado, andar, correr…) com **loop**
- Personagem preso no lugar (**Bake Into Pose**): quem move o herói é o jogo
- O clipe "mixamo.com" é renomeado com o nome do arquivo; no modelo do herói, vira "parado"

**Animação lançada de lado:** algumas magias do Mixamo giram o corpo (o herói lança para o lado e fica de costas para o alvo). Corrija girando o clipe no Inspector do FBX (**Root Transform Rotation → Offset**) até a mão que lança apontar para a frente. O `standing 1H cast spell 01` usa **+90°**. O menu **ARS → Reconfigurar FBX** mantém esse giro.

Os nomes dos arquivos importam: `andar`/`walk`, `correr`/`run`, `parado`/`idle`. Em packs, os nomes com "forward" são preferidos e os com "left/right/back/strafe/turn" ficam de lado.

## Valores provisórios

Velocidade do herói (3,5 m/s), câmera (inclinação 50°, rotação 45°, distância 16 m) e controle (botão direito anda) — ver [Pendências](../pendencias.md).
