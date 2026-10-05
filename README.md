# Desafio Técnico — Target Sistemas

Aplicação de console desenvolvida em C# para resolver três exercícios:
cálculo de comissões, movimentação de estoque e cálculo de juros por atraso.

## Tecnologias

- C#
- .NET 10
- System.Text.Json
- Git

## Como executar

É necessário ter o SDK do .NET 10 instalado.

Na pasta que contém o arquivo desafio-target.csproj, execute:

```bash
dotnet build
dotnet run
```

O programa apresenta o seguinte menu:

- 1 — Calcular comissões
- 2 — Movimentar estoque
- 3 — Calcular juros
- 0 — Sair

## 1. Comissões

As vendas são carregadas do arquivo Dados/vendas.json.

A comissão é calculada individualmente para cada venda:

| Valor da venda | Taxa |
|---|---|
| Abaixo de R$ 100,00 | 0% |
| De R$ 100,00 até menos de R$ 500,00 | 1% |
| A partir de R$ 500,00 | 5% |

Depois, as comissões são somadas por vendedor.

Os cálculos utilizam decimal. O total é formatado com duas casas
decimais na exibição, sem arredondamento individual de cada comissão.

### Resultados dos dados fornecidos

| Vendedor | Comissão |
|---|---|
| João Silva | R$ 495,68 |
| Maria Souza | R$ 465,95 |
| Carlos Oliveira | R$ 379,37 |
| Ana Lima | R$ 404,98 |

## 2. Movimentação de estoque

Os produtos são carregados do arquivo Dados/estoque.json.

Para movimentar um produto, o usuário informa:

- Código do produto
- Tipo: entrada ou saída
- Quantidade
- Descrição da movimentação

Cada movimentação recebe um identificador Guid e registra o produto,
tipo, descrição, quantidade e estoque final.

O programa valida o código, o tipo, a quantidade e a descrição.
Saídas superiores ao saldo disponível são recusadas.

O saldo e o histórico são mantidos apenas na memória durante a execução.
Ao reiniciar o programa, o estoque é carregado novamente do JSON original.

## 3. Juros por atraso

O usuário informa um valor positivo, usando vírgula como separador
decimal, e uma data de vencimento no formato dd/MM/aaaa.

Como o enunciado não especifica capitalização, foi adotado o cálculo
simples de 2,5% por dia de atraso sobre o valor original:

```text
Juros = valor original × 0,025 × dias de atraso
Total = valor original + juros
```

A data atual é obtida do computador. São considerados dias corridos,
sem contar o dia do vencimento.

Vencimentos na data atual ou no futuro não geram juros.
O acréscimo é arredondado para duas casas decimais com
MidpointRounding.AwayFromZero.

## Validação manual

Para conferir o comportamento, execute os seguintes cenários:

- Comissões com valores de 99,99; 100,00; 499,99 e 500,00.
- Entrada de 10 unidades no produto 101: saldo inicial de 150 passa a 160.
- Saída de 20 unidades após essa entrada: saldo passa a 140.
- Tentativa de saída superior ao saldo disponível.
- Código inexistente, quantidade inválida e descrição vazia.
- Valor de 100,00 vencido ontem: juros de 2,50 e total de 102,50.
- Vencimento hoje ou no futuro: juros iguais a zero.
- Valor inválido e data inexistente.

## Autor

Caio Oliveira do Nascimento Silva