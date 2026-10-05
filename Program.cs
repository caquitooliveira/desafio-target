using System.Globalization;
using System.Text.Json;
using DesafioTarget.Modelos;

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
            Console.WriteLine("Movimentação de estoque em desenvolvimento.");
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