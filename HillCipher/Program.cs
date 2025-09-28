using HillCipher.DataAccess.Postgres;
using HillCipher.DataAccess.Postgres.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<CipherDbContext>(
    options =>
    {
        options.UseNpgsql(configuration.GetConnectionString(nameof(CipherDbContext)));
    });

builder.Services.AddScoped<ITextRepository, TextRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.MapGet("/", () => "HillCipher API is running!");

Console.WriteLine("🚀 Application started successfully!");
Console.WriteLine("Endpoints: ");
Console.WriteLine("📋 Swagger UI: https://localhost:7099/swagger");
Console.WriteLine("🌐 API: https://localhost:7099/");
Console.WriteLine("⏹️  Press Ctrl+C to stop");

await app.RunAsync();
