# Modelo de dados

> Proposta inicial dos formatos. Todos os números dos exemplos são **provisórios** (o balanceamento está em aberto). Exemplos completos em [`data/exemplos/`](../../data/exemplos/).

## Entidades

| Entidade | Arquivo de exemplo | O que define |
| --- | --- | --- |
| Herói | `heroi-caleb.json` | Raça, subclasse, nome, **kit próprio**, visual padrão, skins, afinidade |
| Subclasse | `subclasse-mago.json` | Classe, recurso, atributo principal, armas, kit básico do modo customizado, árvore de talentos |
| Habilidade | `habilidade-bola-de-fogo.json` | Custo, recarga, alvo, lista de efeitos |
| Elemento | `elemento-fogo.json` | Efeito aplicado e interações |
| Item | `item-exemplo.json` | Espaço, raridade, atributos, estatísticas, efeitos |
| Árvore de talentos | `talentos-mago.json` | Nós pequenos, médios e grandes marcos (10/20/30) |

## Regras

- **IDs em `kebab-case`**, estáveis (nunca mudam depois de publicados).
- Textos visíveis ficam em **chaves de tradução**, não nos dados (ex.: `"nome": "skill.bola_de_fogo.nome"`).
- Habilidades referenciam **efeitos** por tipo; o código conhece os tipos de efeito, nunca os heróis.
- Herói não guarda números de combate: atributos vêm da subclasse + nível + equipamento + talentos.

## Tipos de efeito (lista inicial)

`dano`, `cura`, `escudo`, `empurrao`, `puxao`, `enraizar`, `atordoar`, `lentidao`, `aplicar_elemento`, `aplicar_tag`, `remover_tag`, `modificar_atributo`, `invocar`, `teleporte`, `gerar_recurso`, `canalizar` (efeito que só acontece se o lançador não for interrompido).

## Alvos

`proprio`, `aliado`, `inimigo`, `area_circulo`, `area_cone`, `linha`, `projetil`, `ponto_no_chao`.
