
using HillCipher.DataAccess.Postgres;
using HillCipher.DataAccess.Postgres.Models;
using HillCipher.DataAccess.Postgres.Repositories;
using HillCipher.Interfaces;
using HillCipher.Requests;
using HillCipher.Responses;
using HillCipher.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(ConfigureSwagger);
builder.Services.AddControllers();

builder.Services.AddScoped<IHillCipherService, HillCipherService>();
builder.Services.AddScoped<IHillKeyService, HillKeyService>();
builder.Services.AddScoped<IRequestHistoryRepository, RequestHistoryRepository>();
builder.Services.AddScoped<ITextRepository, TextRepository>();

builder.Services.AddDbContext<CipherDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString(nameof(CipherDbContext))));


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var JWTKey = configuration.GetValue<string>("JwtSettings:SecretKey");
    if (string.IsNullOrEmpty(JWTKey))
        throw new InvalidOperationException("JWT secret key not found in configuration.");

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

builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddDebug();
});

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        var errorMessage = "Произошла внутренняя ошибка сервера";
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

#if DEBUG
        var exceptionHandlerFeature = 
            context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        errorMessage = exceptionHandlerFeature?.Error.Message;
        logger.LogError(exceptionHandlerFeature?.Error, "Unhandled exception");
#endif

        var response = ApiResponse<object>.Failure(errorMessage);
        await context.Response.WriteAsJsonAsync(response);
    });
});

app.Use(async (context, next) =>
{
    var historyRepo = context.RequestServices.GetRequiredService<IRequestHistoryRepository>();
    var userIdClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
    var userId = int.TryParse(userIdClaim, out var id) ? id : (int?)null;

    var actionName = $"{context.Request.Method} {context.Request.Path}{context.Request.QueryString}";

    var historyEntry = new RequestHistory
    {
        UserId = userId ?? 0,
        ActionName = actionName,
        Action = context.Request.Path.Value?.Split('/').LastOrDefault() ?? "Unknown",
        Timestamp = DateTime.UtcNow
    };

    try
    {
        await next();

        if (context.Response.StatusCode is >= 200 and < 400)
            historyEntry.Success = true;
        else
        {
            historyEntry.Success = false;
            historyEntry.ErrorMessage = $"Client error: {context.Response.StatusCode}";
        }
    }
    catch (Exception ex)
    {
        historyEntry.Success = false;
        historyEntry.ErrorMessage = ex.Message;

        var error = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"
            ? ex.ToString()
            : "Внутрення ошибка сервера";

        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(ApiResponse<object>.Failure(error));
        return;
    }
    finally
    {
        if (userId.HasValue && userId > 0)
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    await historyRepo.AddAsync(historyEntry);
                }
                catch { }
            });
        }
    }
});

app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.MapGet("/", () => "HillCipher API is running!");

await SetupDatabaseAsync(app);

Console.WriteLine("Application started successfully!");
Console.WriteLine("Endpoints: ");
Console.WriteLine("Swagger UI: https://localhost:7099/swagger");
Console.WriteLine("API: https://localhost:7099/");
Console.WriteLine("Press Ctrl+C to stop");

app.Run();

void ConfigureSwagger(Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions options)
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "HillCipher API",
        Version = "v1"
    });

    options.SchemaFilter<SwaggerSchemaExampleFilter>();

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
}

async Task SetupDatabaseAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
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
