using System.Globalization;
using System.Text.Json;
using DesafioTarget.Modelos;

var caminhoEstoque = Path.Combine("Dados", "estoque.json");
var jsonEstoque = File.ReadAllText(caminhoEstoque);

var opcoesEstoque = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
};

var dadosEstoque = JsonSerializer.Deserialize<DadosEstoque>(
    jsonEstoque,
    opcoesEstoque
);

if (dadosEstoque is null)
{
    Console.WriteLine("Não foi possível carregar o estoque.");
    return;
}

var movimentacoes = new List<Movimentacao>();

while (true)
{
    Console.WriteLine();
    Console.WriteLine("DESAFIO TARGET");
    Console.WriteLine("1 - Calcular comissões");
    Console.WriteLine("2 - Movimentar estoque");
    Console.WriteLine("0 - Sair");
    Console.Write("Escolha uma opção: ");

    var opcao = Console.ReadLine();

    if (opcao is null || opcao == "0")
    {
        break;
    }

    switch (opcao)
    {
        case "1":
            ExibirComissoes();
            break;

        case "2":
            MovimentarEstoque();
            break;

        default:
            Console.WriteLine("Opção inválida.");
            break;
    }
}

void ExibirComissoes()
{
    var caminho = Path.Combine("Dados", "vendas.json");
    var json = File.ReadAllText(caminho);

    var opcoes = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    var dados = JsonSerializer.Deserialize<DadosVendas>(json, opcoes);

    if (dados is null)
    {
        Console.WriteLine("Não foi possível carregar os dados de vendas.");
        return;
    }

    var cultura = new CultureInfo("pt-BR");
    var comissoes = new Dictionary<string, decimal>();

    foreach (var venda in dados.Vendas)
    {
        decimal taxa;

        if (venda.Valor < 100m)
        {
            taxa = 0m;
        }
        else if (venda.Valor < 500m)
        {
            taxa = 0.01m;
        }
        else
        {
            taxa = 0.05m;
        }

        decimal comissao = venda.Valor * taxa;

        if (!comissoes.ContainsKey(venda.Vendedor))
        {
            comissoes[venda.Vendedor] = 0m;
        }

        comissoes[venda.Vendedor] += comissao;
    }

    Console.WriteLine("COMISSÕES POR VENDEDOR");

    foreach (var resultado in comissoes)
    {
        Console.WriteLine(
            $"{resultado.Key}: {resultado.Value.ToString("C2", cultura)}"
        );
    }
}

void MovimentarEstoque()
{
    Console.WriteLine();
    Console.WriteLine("PRODUTOS");

    foreach (var item in dadosEstoque.Estoque)
    {
        Console.WriteLine(
            $"{item.CodigoProduto} - {item.DescricaoProduto} " +
            $"| Estoque: {item.Estoque}"
        );
    }

    Console.Write("Código do produto: ");

    if (!int.TryParse(Console.ReadLine(), out int codigo))
    {
        Console.WriteLine("Código inválido.");
        return;
    }

    var produto = dadosEstoque.Estoque.Find(
        item => item.CodigoProduto == codigo
    );

    if (produto is null)
    {
        Console.WriteLine("Produto não encontrado.");
        return;
    }

    Console.Write("Tipo (1 - Entrada / 2 - Saída): ");
    var tipo = Console.ReadLine();

    if (tipo != "1" && tipo != "2")
    {
        Console.WriteLine("Tipo inválido.");
        return;
    }

    Console.Write("Quantidade: ");

    if (!int.TryParse(Console.ReadLine(), out int quantidade)
        || quantidade <= 0)
    {
        Console.WriteLine("Informe uma quantidade inteira maior que zero.");
        return;
    }

    if (tipo == "2" && quantidade > produto.Estoque)
    {
        Console.WriteLine("Estoque insuficiente para essa saída.");
        return;
    }

    if (tipo == "1" && quantidade > int.MaxValue - produto.Estoque)
    {
        Console.WriteLine("Quantidade acima do limite permitido.");
        return;
    }

    Console.Write("Descrição da movimentação: ");
    var descricao = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(descricao))
    {
        Console.WriteLine("A descrição é obrigatória.");
        return;
    }

    if (tipo == "1")
    {
        produto.Estoque += quantidade;
    }
    else
    {
        produto.Estoque -= quantidade;
    }

    var movimentacao = new Movimentacao
    {
        CodigoProduto = produto.CodigoProduto,
        Tipo = tipo == "1" ? "Entrada" : "Saída",
        Descricao = descricao.Trim(),
        Quantidade = quantidade,
        EstoqueFinal = produto.Estoque
    };

    movimentacoes.Add(movimentacao);

    Console.WriteLine();
    Console.WriteLine("Movimentação registrada!");
    Console.WriteLine($"Identificador: {movimentacao.Id}");
    Console.WriteLine($"Produto: {produto.DescricaoProduto}");
    Console.WriteLine($"Tipo: {movimentacao.Tipo}");
    Console.WriteLine($"Descrição: {movimentacao.Descricao}");
    Console.WriteLine($"Quantidade: {movimentacao.Quantidade}");
    Console.WriteLine($"Estoque final: {movimentacao.EstoqueFinal}");
}