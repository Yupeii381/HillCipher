using HillCipher.Requests;
using HillCipher.Responses;
using HillCipher.DataAccess.Postgres;
using HillCipher.DataAccess.Postgres.Models;
using HillCipher.DataAccess.Postgres.Repositories;
//using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HillCipher.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly CipherDbContext _dbContext;
    private readonly IConfiguration _config;
    private readonly IRequestHistoryRepository _historyRepository;

    public AuthController(CipherDbContext dbContext, IConfiguration config, IRequestHistoryRepository historyRepository)
    {
        _dbContext = dbContext;
        _config = config;
        _historyRepository = historyRepository;
    }


    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Register(RegisterRequest request)
    {
        if (await _dbContext.Users.AnyAsync(u => u.Username == request.Username))
            return BadRequest(ApiResponse<string>.Failure("Пользователь с таким именем уже существует."));

        var user = new UserEntity
        {
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        var token = GenerateJwtToken(user);

        await _historyRepository.AddAsync(new RequestHistory
        {
            UserId = user.Id,
            Action = "Succesfully registered"
        });

        var response = new AuthResponse(token, user.Username);

        return Ok(ApiResponse<AuthResponse>.Success(response, "Регистрация прошла успешно"));
    }

    [HttpPost("Login")]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Login(LoginRequest request)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return BadRequest(ApiResponse<AuthResponse>.Failure("Неверное имя пользователя или пароль"));

        var token = GenerateJwtToken(user);

        await _historyRepository.AddAsync(new RequestHistory
        {
            UserId = user.Id,
            Action = "Succesfully logged in"
        });

        var response = new AuthResponse(token, user.Username);

        return Ok(ApiResponse<AuthResponse>.Success(response, "Вход выполнен"));
    }

    [HttpPatch("change-password")]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> ChangePassword(ChangePasswordRequest request)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString))
            return Unauthorized(ApiResponse<AuthResponse>.Failure("Требуется авторизация"));

        if (!int.TryParse(userIdString, out var userId))
            return BadRequest(ApiResponse<AuthResponse>.Failure("Некорректный идентификатор пользователя"));

        var user = await _dbContext.Users.FindAsync(userId);
        if (user == null)
            return NotFound(ApiResponse<AuthResponse>.Failure("Пользователь не найден"));

        if (!BCrypt.Net.BCrypt.Verify(request.OldPassword, user.PasswordHash))
            return BadRequest(ApiResponse<AuthResponse>.Failure("Неверный старый пароль"));

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        user.TokenVersion++;

        await _historyRepository.AddAsync(new RequestHistory
        {
            UserId = user.Id,
            Action = "Пароль успешно изменен"
        });

        await _dbContext.SaveChangesAsync();

        var newToken = GenerateJwtToken(user);
        var response = new AuthResponse(newToken, user.Username);

        return Ok(ApiResponse<AuthResponse>.Success(response, "Пароль успешно изменён"));
    }


    private string GenerateJwtToken(UserEntity user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Version, user.TokenVersion.ToString())
            };

        var token = new JwtSecurityToken(
            issuer: _config["JwtSettings:Issuer"],
            audience: _config["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}