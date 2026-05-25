using KmControl.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Aqui criamos a aplicação web usando as configurações acima.
var app = builder.Build();

app.UseCors("PermitirFrontend");

app.MapControllers();

// Rota simples só para testar se a API está funcionando.
app.MapGet("/", () =>
{
    return "KmControl API está funcionando!";
});

// Inicia a aplicação.
app.Run();