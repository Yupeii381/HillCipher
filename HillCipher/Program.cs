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

