namespace HillCipher.Controller;

using Microsoft.AspNetCore.Mvc;
using HillCipher.Requests;
using HillCipher.DataAccess.Postgres.Repositories;
using HillCipher.DataAccess.Postgres.Models;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserRepository userRepository, ILogger<UsersController> logger) =>
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

    // GET: /api/users/id/{id}
    [HttpGet("{id:guid}")]
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

    // GET: /api/users/email/{email}
    [HttpGet("email/{email}")]
    public async Task<ActionResult<UserEntity>> GetUserByEmail(string email)
    {
        try
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                return NotFound($"User with email {email} not found");
            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user by email {Email}", email);
            return StatusCode(500, "Internal server error");
        }
    }


    // POST: api/users
    [HttpPost]
    public async Task<ActionResult<UserEntity>> CreateUser([FromBody] CreateUserRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Проверка уникальности email
            if (await _userRepository.EmailExistsAsync(request.Email))
                return Conflict("User with this email already exists");

            var user = new UserEntity
            {
                Username = request.Username,
                Email = request.Email,
            };

            var createdUser = await _userRepository.CreateAsync(user);

            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user");
            return StatusCode(500, "Internal server error");
        }
    }

    // PATCH: api/users/{id}
    [HttpPatch("{id:guid}")]
    public async Task<ActionResult<UserEntity>> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
                return NotFound($"User with id {id} not found");
            // Проверка уникальности email, если он изменился
            if (!string.Equals(existingUser.Email, request.Email, StringComparison.OrdinalIgnoreCase) &&
                await _userRepository.EmailExistsAsync(request.Email))
            {
                return Conflict("User with this email already exists");
            }

            existingUser.Username = request.Username ?? existingUser.Username;
            existingUser.Email = request.Email ?? existingUser.Email;

            var updatedUser = await _userRepository.UpdateAsync(existingUser);
            return Ok(updatedUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user with id {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    // DELETE: api/users/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        try
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
                return NotFound($"User with id {id} not found");

            var deleted = await _userRepository.DeleteAsync(id);

            if (!deleted)
                return StatusCode(500, "Error deleting user");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user with id {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}