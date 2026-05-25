namespace KmControl.Api.Dtos;

public class VeiculoRespostaDto
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Modelo { get; set; } = string.Empty;

    public int Ano { get; set; }

    public string Tipo { get; set; } = string.Empty;

    public string? Placa { get; set; }

    public double OdometroAtual { get; set; }

    public DateTime DataCadastro { get; set; }
}