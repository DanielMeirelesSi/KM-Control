namespace KmControl.Api.Dtos;

public class CadastrarAbastecimentoDto
{
    public int VeiculoId { get; set; }

    public double KmRodado { get; set; }

    public double LitrosAbastecidos { get; set; }

    public double ValorTotal { get; set; }

    public string Combustivel { get; set; } = string.Empty;
}