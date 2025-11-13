using HillCipher.DataAccess.Postgres.Dtos;
using HillCipher.DataAccess.Postgres.Models;
using HillCipher.DataAccess.Postgres.Repositories;
using HillCipher.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace HillCipher.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class HistoryController : ControllerBase
{
    private readonly IRequestHistoryRepository _historyRepo;
    public HistoryController(IRequestHistoryRepository hisoryRepo) 
        => _historyRepo = hisoryRepo;

    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException());

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<RequestHistoryDto>>>> GetHistory()
    {
        var userId = GetUserId();
        var history = await _historyRepo.GetByUserIdAsync(userId);
        var dtos = history.Select(RequestHistoryDto.FromEntity).ToList();
        return Ok(ApiResponse<List<RequestHistoryDto>>.Success(dtos));
    }

    [HttpDelete]
    public async Task<IActionResult> ClearHistory()
    {
        var userId = GetUserId();
        await _historyRepo.DeleteAllByUserIdAsync(userId);
        return NoContent();
    }
}
