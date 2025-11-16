using HillCipher.DataAccess.Postgres.Repositories;
using HillCipher.DataAccess.Postgres.Models;
using HillCipher.DataAccess.Postgres.Dtos;
using HillCipher.Requests;
using HillCipher.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HillCipher.Interfaces;
using HillCipher.Responses;

namespace HillCipher.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TextController : ControllerBase
{
    private readonly ITextRepository _textRepo;
    private readonly IHillCipherService _cipherService;
    private readonly IRequestHistoryRepository _historyRepo;

    public TextController(ITextRepository textRepository, IHillCipherService cipherService, IRequestHistoryRepository historyRepo)
    {
        _textRepo = textRepository;
        _cipherService = cipherService;
        _historyRepo = historyRepo;
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim))
        {
            throw new UnauthorizedAccessException("Пользователя не существует");
        }
        return int.Parse(userIdClaim);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<TextResponse>>> AddText([FromBody] AddTextRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetUserId();
        var text = await _textRepo.AddOrGetAsync(request.Content, userId);

        await _historyRepo.AddAsync(new RequestHistory
        {
            UserId = userId,
            Action = $"Added text with id: {text.Id}",
            InputText = request.Content
        });

        var response = new TextResponse(text.Id, text.Content);

        return Ok(ApiResponse<TextResponse>.Success(response));
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<ApiResponse<TextResponse>>> UpdateText(int id, [FromBody] UpdateTextRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetUserId();
        var text = await _textRepo.GetByIdAsync(id, userId);
        if (text == null)
            return NotFound();

        text.Content = request.Content;
        await _textRepo.UpdateAsync(text);

        await _historyRepo.AddAsync(new RequestHistory
        {
            UserId = userId,
            Action = $"Patched text with id: {text.Id}",
            InputText = request.Content
        });

        var response = new TextResponse(text.Id, text.Content);

        return Ok(ApiResponse<TextResponse>.Success(response));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteText(int id)
    {
        var userId = GetUserId();
        await _textRepo.DeleteAsync(id, userId);

        await _historyRepo.AddAsync(new RequestHistory
        {
            UserId = userId,
            Action = $"Deleted text with id: {id}"
        });

        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<TextResponse>>> GetByIdText(int id)
    {
        var userId = GetUserId();
        var text = await _textRepo.GetByIdAsync(id, userId);
        var dto = new TextResponse(text.Id, text.Content);
        return text == null ? NotFound() : Ok(ApiResponse<TextResponse>.Success(dto));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<TextResponse>>>> GetAllText()
    {
        var userId = GetUserId();
        var texts = await _textRepo.GetAllAsync(userId);
        var dtos = texts.Select(t => new TextResponse(t.Id, t.Content)).ToList();
        return Ok(ApiResponse<List<TextResponse>>.Success(dtos));
    }

    [HttpPost("{id}/encrypt")]
    public async Task<ActionResult<ApiResponse<CipherResponse>>> EncryptText(int id, [FromBody] CipherRequest request)
    {
        var userId = GetUserId();
        var text = await _textRepo.GetByIdAsync(id, userId);
        if (text == null)
            return NotFound(ApiResponse<CipherResponse>.Failure("Текст не был найден."));

        var result = _cipherService.Encrypt(text.Content, request.Key, request.Alphabet);

        await _historyRepo.AddAsync(new RequestHistory
        {
            UserId = userId,
            Action = $"Encrypted text with id: {text.Id}",
            InputText = text.Content,
            ResultText = result
        });

        var response = new CipherResponse(result);

        return Ok(ApiResponse<CipherResponse>.Success(response));
    }

    [HttpPost("{id}/decrypt")]
    public async Task<ActionResult<ApiResponse<CipherResponse>>> DecryptText(int id, [FromBody] CipherRequest request)
    {
        var userId = GetUserId();
        var text = await _textRepo.GetByIdAsync(id, userId);
        if (text == null)
            return NotFound(ApiResponse<CipherResponse>.Failure("Текст не был найден"));

        var result = _cipherService.Decrypt(text.Content, request.Key, request.Alphabet);

        await _historyRepo.AddAsync(new RequestHistory
        {
            UserId = userId,
            Action = $"Decrypted text with id: {text.Id}",
            InputText = text.Content,
            ResultText = result
        });

        var response = new CipherResponse(result);

        return Ok(ApiResponse<CipherResponse>.Success(response));
    }
}