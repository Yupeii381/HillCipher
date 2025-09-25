using HillCipher.DataAccess.Postgres;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CipherDbContext>(
    options =>
    {
        options.UseNpgsql(configuration.GetConnectionString(nameof(CipherDbContext)));
    });

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "HillCipher API is running!");

Console.WriteLine("🚀 Application started successfully!");
Console.WriteLine("📋 Swagger UI: https://localhost:7099/swagger");
Console.WriteLine("🌐 API: https://localhost:7099/");
Console.WriteLine("⏹️  Press Ctrl+C to stop");

app.Run();
