using KmControl.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace KmControl.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options)
    {
    }

    public DbSet<Veiculo> Veiculos { get; set; }

    public DbSet<Abastecimento> Abastecimentos { get; set; }
}