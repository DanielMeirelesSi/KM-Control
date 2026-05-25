using KmControl.Api.Data;
using KmControl.Api.Dtos;
using KmControl.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KmControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AbastecimentosController : ControllerBase
{
    private readonly AppDbContext _context;

    public AbastecimentosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<AbastecimentoRespostaDto>>> ListarAbastecimentos()
    {
        var abastecimentos = await _context.Abastecimentos
            .Include(a => a.Veiculo)
            .OrderByDescending(a => a.Data)
            .Select(a => new AbastecimentoRespostaDto
            {
                Id = a.Id,
                VeiculoId = a.VeiculoId,
                NomeVeiculo = a.Veiculo != null ? a.Veiculo.Nome : string.Empty,
                Data = a.Data,
                KmRodado = a.KmRodado,
                LitrosAbastecidos = a.LitrosAbastecidos,
                ValorTotal = a.ValorTotal,
                Combustivel = a.Combustivel,
                MediaKmPorLitro = a.MediaKmPorLitro,
                ValorPorLitro = a.ValorPorLitro,
                CustoPorKm = a.CustoPorKm
            })
            .ToListAsync();

        return Ok(abastecimentos);
    }

    [HttpGet("resumo-geral")]
    public async Task<ActionResult> ObterResumoGeral()
    {
        var quantidadeVeiculos = await _context.Veiculos.CountAsync();

        var abastecimentos = await _context.Abastecimentos.ToListAsync();

        if (!abastecimentos.Any())
        {
            return Ok(new
            {
                QuantidadeVeiculos = quantidadeVeiculos,
                QuantidadeAbastecimentos = 0,
                TotalKmRodado = 0,
                TotalLitros = 0,
                TotalGasto = 0,
                MediaGeralKmPorLitro = 0,
                CustoMedioPorKm = 0,
                ValorMedioPorLitro = 0
            });
        }

        var totalKmRodado = abastecimentos.Sum(a => a.KmRodado);
        var totalLitros = abastecimentos.Sum(a => a.LitrosAbastecidos);
        var totalGasto = abastecimentos.Sum(a => a.ValorTotal);

        var resumo = new
        {
            QuantidadeVeiculos = quantidadeVeiculos,
            QuantidadeAbastecimentos = abastecimentos.Count,
            TotalKmRodado = totalKmRodado,
            TotalLitros = totalLitros,
            TotalGasto = totalGasto,
            MediaGeralKmPorLitro = Math.Round(totalKmRodado / totalLitros, 2),
            CustoMedioPorKm = Math.Round(totalGasto / totalKmRodado, 2),
            ValorMedioPorLitro = Math.Round(totalGasto / totalLitros, 2)
        };

        return Ok(resumo);
    }

    [HttpGet("veiculo/{veiculoId}")]
    public async Task<ActionResult<List<AbastecimentoRespostaDto>>> ListarAbastecimentosPorVeiculo(int veiculoId)
    {
        var veiculo = await _context.Veiculos.FindAsync(veiculoId);

        if (veiculo == null)
        {
            return NotFound("Veículo não encontrado.");
        }

        var abastecimentos = await _context.Abastecimentos
            .Where(a => a.VeiculoId == veiculoId)
            .OrderByDescending(a => a.Data)
            .Select(a => new AbastecimentoRespostaDto
            {
                Id = a.Id,
                VeiculoId = a.VeiculoId,
                NomeVeiculo = veiculo.Nome,
                Data = a.Data,
                KmRodado = a.KmRodado,
                LitrosAbastecidos = a.LitrosAbastecidos,
                ValorTotal = a.ValorTotal,
                Combustivel = a.Combustivel,
                MediaKmPorLitro = a.MediaKmPorLitro,
                ValorPorLitro = a.ValorPorLitro,
                CustoPorKm = a.CustoPorKm
            })
            .ToListAsync();

        return Ok(abastecimentos);
    }

    [HttpGet("veiculo/{veiculoId}/resumo")]
    public async Task<ActionResult> ObterResumoPorVeiculo(int veiculoId)
    {
        var veiculoExiste = await _context.Veiculos.AnyAsync(v => v.Id == veiculoId);

        if (!veiculoExiste)
        {
            return NotFound("Veículo não encontrado.");
        }

        var abastecimentos = await _context.Abastecimentos
            .Where(a => a.VeiculoId == veiculoId)
            .ToListAsync();

        if (!abastecimentos.Any())
        {
            return Ok(new
            {
                VeiculoId = veiculoId,
                QuantidadeAbastecimentos = 0,
                TotalKmRodado = 0,
                TotalLitros = 0,
                TotalGasto = 0,
                MediaGeralKmPorLitro = 0,
                MelhorMediaKmPorLitro = 0,
                PiorMediaKmPorLitro = 0,
                CustoMedioPorKm = 0,
                ValorMedioPorLitro = 0
            });
        }

        var totalKmRodado = abastecimentos.Sum(a => a.KmRodado);
        var totalLitros = abastecimentos.Sum(a => a.LitrosAbastecidos);
        var totalGasto = abastecimentos.Sum(a => a.ValorTotal);

        var resumo = new
        {
            VeiculoId = veiculoId,
            QuantidadeAbastecimentos = abastecimentos.Count,
            TotalKmRodado = totalKmRodado,
            TotalLitros = totalLitros,
            TotalGasto = totalGasto,
            MediaGeralKmPorLitro = Math.Round(totalKmRodado / totalLitros, 2),
            MelhorMediaKmPorLitro = Math.Round(abastecimentos.Max(a => a.MediaKmPorLitro), 2),
            PiorMediaKmPorLitro = Math.Round(abastecimentos.Min(a => a.MediaKmPorLitro), 2),
            CustoMedioPorKm = Math.Round(totalGasto / totalKmRodado, 2),
            ValorMedioPorLitro = Math.Round(totalGasto / totalLitros, 2)
        };

        return Ok(resumo);
    }

    [HttpPost]
    public async Task<ActionResult<AbastecimentoRespostaDto>> CadastrarAbastecimento(CadastrarAbastecimentoDto dto)
    {
        var veiculo = await _context.Veiculos.FindAsync(dto.VeiculoId);

        if (veiculo == null)
        {
            return NotFound("Veículo não encontrado.");
        }

        if (dto.KmRodado <= 0)
        {
            return BadRequest("O km rodado deve ser maior que zero.");
        }

        if (dto.LitrosAbastecidos <= 0)
        {
            return BadRequest("A quantidade de litros deve ser maior que zero.");
        }

        if (dto.ValorTotal <= 0)
        {
            return BadRequest("O valor total deve ser maior que zero.");
        }

        if (string.IsNullOrWhiteSpace(dto.Combustivel))
        {
            return BadRequest("O combustível é obrigatório.");
        }

        var abastecimento = new Abastecimento
        {
            VeiculoId = dto.VeiculoId,
            KmRodado = dto.KmRodado,
            LitrosAbastecidos = dto.LitrosAbastecidos,
            ValorTotal = dto.ValorTotal,
            Combustivel = dto.Combustivel,
            MediaKmPorLitro = Math.Round(dto.KmRodado / dto.LitrosAbastecidos, 2),
            ValorPorLitro = Math.Round(dto.ValorTotal / dto.LitrosAbastecidos, 2),
            CustoPorKm = Math.Round(dto.ValorTotal / dto.KmRodado, 2)
        };

        veiculo.OdometroAtual += dto.KmRodado;

        _context.Abastecimentos.Add(abastecimento);
        await _context.SaveChangesAsync();

        var resposta = new AbastecimentoRespostaDto
        {
            Id = abastecimento.Id,
            VeiculoId = abastecimento.VeiculoId,
            NomeVeiculo = veiculo.Nome,
            Data = abastecimento.Data,
            KmRodado = abastecimento.KmRodado,
            LitrosAbastecidos = abastecimento.LitrosAbastecidos,
            ValorTotal = abastecimento.ValorTotal,
            Combustivel = abastecimento.Combustivel,
            MediaKmPorLitro = abastecimento.MediaKmPorLitro,
            ValorPorLitro = abastecimento.ValorPorLitro,
            CustoPorKm = abastecimento.CustoPorKm
        };

        return CreatedAtAction(nameof(ListarAbastecimentos), new { id = abastecimento.Id }, resposta);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AbastecimentoRespostaDto>> AtualizarAbastecimento(int id, CadastrarAbastecimentoDto dto)
    {
        var abastecimento = await _context.Abastecimentos.FindAsync(id);

        if (abastecimento == null)
        {
            return NotFound("Abastecimento não encontrado.");
        }

        var veiculo = await _context.Veiculos.FindAsync(abastecimento.VeiculoId);

        if (veiculo == null)
        {
            return NotFound("Veículo relacionado ao abastecimento não encontrado.");
        }

        if (dto.VeiculoId != abastecimento.VeiculoId)
        {
            return BadRequest("Não é permitido alterar o veículo de um abastecimento já cadastrado.");
        }

        if (dto.KmRodado <= 0)
        {
            return BadRequest("O km rodado deve ser maior que zero.");
        }

        if (dto.LitrosAbastecidos <= 0)
        {
            return BadRequest("A quantidade de litros deve ser maior que zero.");
        }

        if (dto.ValorTotal <= 0)
        {
            return BadRequest("O valor total deve ser maior que zero.");
        }

        if (string.IsNullOrWhiteSpace(dto.Combustivel))
        {
            return BadRequest("O combustível é obrigatório.");
        }

        double diferencaKm = dto.KmRodado - abastecimento.KmRodado;

        abastecimento.KmRodado = dto.KmRodado;
        abastecimento.LitrosAbastecidos = dto.LitrosAbastecidos;
        abastecimento.ValorTotal = dto.ValorTotal;
        abastecimento.Combustivel = dto.Combustivel;
        abastecimento.MediaKmPorLitro = Math.Round(dto.KmRodado / dto.LitrosAbastecidos, 2);
        abastecimento.ValorPorLitro = Math.Round(dto.ValorTotal / dto.LitrosAbastecidos, 2);
        abastecimento.CustoPorKm = Math.Round(dto.ValorTotal / dto.KmRodado, 2);

        veiculo.OdometroAtual += diferencaKm;

        await _context.SaveChangesAsync();

        var resposta = new AbastecimentoRespostaDto
        {
            Id = abastecimento.Id,
            VeiculoId = abastecimento.VeiculoId,
            NomeVeiculo = veiculo.Nome,
            Data = abastecimento.Data,
            KmRodado = abastecimento.KmRodado,
            LitrosAbastecidos = abastecimento.LitrosAbastecidos,
            ValorTotal = abastecimento.ValorTotal,
            Combustivel = abastecimento.Combustivel,
            MediaKmPorLitro = abastecimento.MediaKmPorLitro,
            ValorPorLitro = abastecimento.ValorPorLitro,
            CustoPorKm = abastecimento.CustoPorKm
        };

        return Ok(resposta);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> ExcluirAbastecimento(int id)
    {
        var abastecimento = await _context.Abastecimentos.FindAsync(id);

        if (abastecimento == null)
        {
            return NotFound("Abastecimento não encontrado.");
        }

        var veiculo = await _context.Veiculos.FindAsync(abastecimento.VeiculoId);

        if (veiculo == null)
        {
            return NotFound("Veículo relacionado ao abastecimento não encontrado.");
        }

        veiculo.OdometroAtual -= abastecimento.KmRodado;

        _context.Abastecimentos.Remove(abastecimento);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}