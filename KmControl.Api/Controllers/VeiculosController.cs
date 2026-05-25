using KmControl.Api.Data;
using KmControl.Api.Dtos;
using KmControl.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KmControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VeiculosController : ControllerBase
{
    private readonly AppDbContext _context;

    public VeiculosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<VeiculoRespostaDto>>> ListarVeiculos()
    {
        var veiculos = await _context.Veiculos
            .Select(v => new VeiculoRespostaDto
            {
                Id = v.Id,
                Nome = v.Nome,
                Modelo = v.Modelo,
                Ano = v.Ano,
                Tipo = v.Tipo,
                Placa = v.Placa,
                OdometroAtual = v.OdometroAtual,
                DataCadastro = v.DataCadastro
            })
            .ToListAsync();

        return Ok(veiculos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<VeiculoRespostaDto>> BuscarVeiculoPorId(int id)
    {
        var veiculo = await _context.Veiculos.FindAsync(id);

        if (veiculo == null)
        {
            return NotFound("Veículo não encontrado.");
        }

        var resposta = new VeiculoRespostaDto
        {
            Id = veiculo.Id,
            Nome = veiculo.Nome,
            Modelo = veiculo.Modelo,
            Ano = veiculo.Ano,
            Tipo = veiculo.Tipo,
            Placa = veiculo.Placa,
            OdometroAtual = veiculo.OdometroAtual,
            DataCadastro = veiculo.DataCadastro
        };

        return Ok(resposta);
    }

    [HttpPost]
    public async Task<ActionResult<VeiculoRespostaDto>> CadastrarVeiculo(CadastrarVeiculoDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nome))
        {
            return BadRequest("O nome do veículo é obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(dto.Modelo))
        {
            return BadRequest("O modelo do veículo é obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(dto.Tipo))
        {
            return BadRequest("O tipo do veículo é obrigatório.");
        }

        if (dto.Ano <= 0)
        {
            return BadRequest("O ano do veículo deve ser válido.");
        }

        if (dto.OdometroAtual < 0)
        {
            return BadRequest("O odômetro atual não pode ser negativo.");
        }

        var veiculo = new Veiculo
        {
            Nome = dto.Nome,
            Modelo = dto.Modelo,
            Ano = dto.Ano,
            Tipo = dto.Tipo,
            Placa = dto.Placa,
            OdometroAtual = dto.OdometroAtual
        };

        _context.Veiculos.Add(veiculo);
        await _context.SaveChangesAsync();

        var resposta = new VeiculoRespostaDto
        {
            Id = veiculo.Id,
            Nome = veiculo.Nome,
            Modelo = veiculo.Modelo,
            Ano = veiculo.Ano,
            Tipo = veiculo.Tipo,
            Placa = veiculo.Placa,
            OdometroAtual = veiculo.OdometroAtual,
            DataCadastro = veiculo.DataCadastro
        };

        return CreatedAtAction(nameof(BuscarVeiculoPorId), new { id = veiculo.Id }, resposta);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<VeiculoRespostaDto>> AtualizarVeiculo(int id, CadastrarVeiculoDto dto)
    {
        var veiculo = await _context.Veiculos.FindAsync(id);

        if (veiculo == null)
        {
            return NotFound("Veículo não encontrado.");
        }

        if (string.IsNullOrWhiteSpace(dto.Nome))
        {
            return BadRequest("O nome do veículo é obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(dto.Modelo))
        {
            return BadRequest("O modelo do veículo é obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(dto.Tipo))
        {
            return BadRequest("O tipo do veículo é obrigatório.");
        }

        if (dto.Ano <= 0)
        {
            return BadRequest("O ano do veículo deve ser válido.");
        }

        if (dto.OdometroAtual < 0)
        {
            return BadRequest("O odômetro atual não pode ser negativo.");
        }

        veiculo.Nome = dto.Nome;
        veiculo.Modelo = dto.Modelo;
        veiculo.Ano = dto.Ano;
        veiculo.Tipo = dto.Tipo;
        veiculo.Placa = dto.Placa;
        veiculo.OdometroAtual = dto.OdometroAtual;

        await _context.SaveChangesAsync();

        var resposta = new VeiculoRespostaDto
        {
            Id = veiculo.Id,
            Nome = veiculo.Nome,
            Modelo = veiculo.Modelo,
            Ano = veiculo.Ano,
            Tipo = veiculo.Tipo,
            Placa = veiculo.Placa,
            OdometroAtual = veiculo.OdometroAtual,
            DataCadastro = veiculo.DataCadastro
        };

        return Ok(resposta);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> ExcluirVeiculo(int id)
    {
        var veiculo = await _context.Veiculos.FindAsync(id);

        if (veiculo == null)
        {
            return NotFound("Veículo não encontrado.");
        }

        var possuiAbastecimentos = await _context.Abastecimentos
            .AnyAsync(a => a.VeiculoId == id);

        if (possuiAbastecimentos)
        {
            return BadRequest("Não é possível excluir um veículo que possui abastecimentos cadastrados.");
        }

        _context.Veiculos.Remove(veiculo);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}