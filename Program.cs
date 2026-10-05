using System.Globalization;
using System.Text.Json;
using DesafioTarget.Modelos;

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

foreach (var venda in dados.Vendas)
{
    Console.WriteLine(
        $"{venda.Vendedor}: {venda.Valor.ToString("C2", cultura)}"
    );
}

Console.WriteLine($"Total de vendas carregadas: {dados.Vendas.Count}");