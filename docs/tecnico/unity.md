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
- A animação de ataque é escolhida pelo nome do arquivo: `attack`, `atacar`, `ataque`, `cast` ou `spell`. Coloque na pasta `Animations` do herói.
- A cena de teste tem **bonecos de treino**: mostram a vida, os números de dano e voltam à vida cheia depois de 3 segundos sem apanhar.

## Importação automática

Ao arrastar um FBX do Mixamo para as pastas acima, o projeto já configura sozinho (só na primeira importação):

- Rig **Humanoid** (as animações servem para qualquer herói)
- Animações de ciclo (parado, andar, correr…) com **loop**
- Personagem preso no lugar (**Bake Into Pose**): quem move o herói é o jogo
- O clipe "mixamo.com" é renomeado com o nome do arquivo; no modelo do herói, vira "parado"

Os nomes dos arquivos importam: `andar`/`walk`, `correr`/`run`, `parado`/`idle`. Em packs, os nomes com "forward" são preferidos e os com "left/right/back/strafe/turn" ficam de lado.

## Valores provisórios

Velocidade do herói (3,5 m/s), câmera (inclinação 50°, rotação 45°, distância 16 m) e controle (botão direito anda) — ver [Pendências](../pendencias.md).
