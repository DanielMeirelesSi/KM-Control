namespace KmControl.Api.Dtos;

public class AbastecimentoRespostaDto
{
    public int Id { get; set; }

    public int VeiculoId { get; set; }

    public string NomeVeiculo { get; set; } = string.Empty;

    public DateTime Data { get; set; }

    public double KmRodado { get; set; }

    public double LitrosAbastecidos { get; set; }

    public double ValorTotal { get; set; }

    public string Combustivel { get; set; } = string.Empty;

    public double MediaKmPorLitro { get; set; }

    public double ValorPorLitro { get; set; }

    public double CustoPorKm { get; set; }
}