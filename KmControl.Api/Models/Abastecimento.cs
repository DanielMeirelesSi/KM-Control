namespace KmControl.Api.Models;

public class Abastecimento
{
    public int Id { get; set; }

    public int VeiculoId { get; set; }

    public Veiculo? Veiculo { get; set; }

    public DateTime Data { get; set; } = DateTime.Now;

    public double KmRodado { get; set; }

    public double LitrosAbastecidos { get; set; }

    public double ValorTotal { get; set; }

    public string Combustivel { get; set; } = string.Empty;

    public double MediaKmPorLitro { get; set; }

    public double ValorPorLitro { get; set; }

    public double CustoPorKm { get; set; }
}