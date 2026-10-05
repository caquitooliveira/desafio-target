namespace DesafioTarget.Modelos;

public class Movimentacao
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int CodigoProduto { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public int EstoqueFinal { get; set; }
}