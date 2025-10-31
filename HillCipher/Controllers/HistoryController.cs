using HillCipher.DataAccess.Postgres.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace HillCipher.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class HistoryController : ControllerBase
    {
        private readonly IRequestHistoryRepository _historyRepo;
        public HistoryController(IRequestHistoryRepository hisoryRepo) => _historyRepo = hisoryRepo;

        private int GetUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new UnauthorizedAccessException("Пользователя не существует");
            }
            return int.Parse(userIdClaim);
        }

        [HttpGet]
        public async Task<IActionResult> GetHistory()
        {
            var userId = GetUserId();
            var history = await _historyRepo.GetByUserIdAsync(userId);
            return Ok(history);
        }

        [HttpDelete]
        public async Task<IActionResult> ClearHistory()
        {
            var userId = GetUserId();
            await _historyRepo.DeleteAllByUserIdAsync(userId);
            return NoContent();
        }
    }
}
