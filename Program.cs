
using HillCipher.DataAccess.Postgres.Models;
using HillCipher.DataAccess.Postgres;
using HillCipher.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "HillCipher API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Введите JWT токен, начиная с Bearer и пробела. Пример: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6...",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

builder.Services.AddControllers();

builder.Services.AddScoped<IHillCipherService, HillCipherService>();
builder.Services.AddScoped<IHillKeyService, HillKeyService>();

builder.Services.AddDbContext<CipherDbContext>(
    options =>
    {
        options.UseNpgsql(configuration.GetConnectionString(nameof(CipherDbContext)));
    });


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var JWTKey = configuration.GetValue<string>("JwtSettings:SecretKey");
    if (string.IsNullOrEmpty(JWTKey))
        throw new InvalidOperationException("❌ JWT secret key not found in configuration.");

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTKey!))
    };
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.MapGet("/", () => "HillCipher API is running!");

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CipherDbContext>();

    bool exists = await context.Database.CanConnectAsync();
    Console.WriteLine($"Database exists: {exists}");

    if (!exists)
    {
        Console.WriteLine("Creating database...");
        await context.Database.EnsureCreatedAsync();
        Console.WriteLine("Database created successfully!");
    }
    else
    {
        Console.WriteLine("Database already exists");
    }
}

Console.WriteLine("🚀 Application started successfully!");
Console.WriteLine("Endpoints: ");
Console.WriteLine("📋 Swagger UI: https://localhost:7099/swagger");
Console.WriteLine("🌐 API: https://localhost:7099/");
Console.WriteLine("⏹️  Press Ctrl+C to stop");

await app.RunAsync();