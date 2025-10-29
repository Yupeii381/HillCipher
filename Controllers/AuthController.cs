using HillCipher.DataAccess.Postgres;
using HillCipher.DataAccess.Postgres.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HillCipher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly CipherDbContext _dbContext;
        private readonly IConfiguration _config;

        public AuthController(CipherDbContext dbContext, IConfiguration config)
        {
            _dbContext = dbContext;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (await _dbContext.Users.AnyAsync(u => u.Username == request.Username))
                return BadRequest("Пользователь уже существует");

            var user = new UserEntity
            {
                Username = request.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return Ok("Регистрация успешна");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            UserEntity user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Unauthorized("Неверный логин или пароль");

            var token = GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(UserEntity user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
