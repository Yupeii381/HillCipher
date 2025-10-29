using HillCipher.DataAccess.Postgres;
using HillCipher.DataAccess.Postgres.Models;
using HillCipher.Requests;
using HillCipher.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HillCipher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    public class HillCipherController : ControllerBase
    {
        private readonly CipherDbContext _dbContext;
        private readonly IHillCipherService _hillCipherService;
        public HillCipherController(CipherDbContext dbContext, IHillCipherService hillCipherService)
        {
            _dbContext = dbContext;
            _hillCipherService = hillCipherService;
        }

        [HttpPost("encrypt")]
        public async Task<IActionResult> Encrypt([FromBody] CipherRequest request)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var ciphertext = _hillCipherService.Encrypt(request.Text, request.Key, request.Alphabet);

            var userText = new TextEntity
            {
                Plaintext = request.Text,
                Ciphertext = ciphertext,
                UserId = userId
            };
            _dbContext.Texts.Add(userText);
            await _dbContext.SaveChangesAsync();

            return Ok(new { Ciphertext = ciphertext });
        }

        [HttpPost("decrypt")]
        public IActionResult Decrypt([FromBody] CipherRequest request)
        {
            var plaintext = _hillCipherService.Decrypt(request.Text, request.Key, request.Alphabet);
            return Ok(new { Plaintext = plaintext });
        }
    }
}
