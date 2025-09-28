namespace HillCipher.Controller;

using Microsoft.AspNetCore.Mvc;
using HillCipher.DataAccess.Postgres.Repositories;
using HillCipher.DataAccess.Postgres.Models;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserRepository> _logger;

    public UsersController(IUserRepository userRepository, ILogger<UserRepository> logger) =>
        (_userRepository, _logger) = (userRepository, logger);

    // GET: /api/users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserEntity>>> GetAllUsers()
    {
        try
        {
            var users = await _userRepository.GetAllAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all users");
            return StatusCode(500, "Internal server error");
        }
    }

    // GET: /api/users/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<UserEntity>> GetUserById(Guid id)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound($"User with id {id} not found");

            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user by id {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}