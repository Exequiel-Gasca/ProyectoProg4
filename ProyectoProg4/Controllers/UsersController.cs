using System.Security.Claims;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoProg4.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize(Roles = "Admin,SuperAdmin")]
    [HttpGet]
    public async Task<ActionResult<List<UserResponseDto>>> Get()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users.Select(ToDto).ToList());
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserResponseDto>> GetById(int id)
    {
        if (!CanAccess(id))
            return Forbid();

        var user = await _userService.GetUserByIdAsync(id);

        if (user == null)
            return NotFound(new { error = $"No se encontró el usuario con ID {id}." });

        return Ok(ToDto(user));
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<UserResponseDto>> Register(RegisterUserDto dto)
    {
        try
        {
            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email
            };

            var created = await _userService.CreateUserAsync(user, dto.Password);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, ToDto(created));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateUserDto dto)
    {
        if (!CanAccess(id))
            return Forbid();

        if (!int.TryParse(User.FindFirstValue("sub"), out var actingUserId))
            return Unauthorized();

        try
        {
            var user = new User
            {
                Id = id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email
            };

            await _userService.UpdateUserAsync(user, actingUserId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }
    [Authorize(Roles = "SuperAdmin")]
    [HttpPut("{id}/role")]
    public async Task<IActionResult> ChangeRole(int id, ChangeRoleDto dto)
    {

        var claim = User.FindFirstValue("sub");
        if (!int.TryParse(claim, out var actingUserId))
            return Unauthorized();

        try
        {
            await _userService.UpdateUserRoleAsync(id, dto.RoleId, actingUserId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [Authorize(Roles = "Admin,SuperAdmin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Deactivate(int id)
    {
        if (!int.TryParse(User.FindFirstValue("sub"), out var actingUserId))
            return Unauthorized();

        try
        {
            await _userService.DeactivateUserAsync(id, actingUserId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    // true si quien llama es el dueño de la cuenta o es Admin/SuperAdmin
    private bool CanAccess(int targetUserId)
    {
        if (User.IsInRole("Admin") || User.IsInRole("SuperAdmin"))
            return true;

        var claim = User.FindFirstValue("sub");
        return int.TryParse(claim, out var currentId) && currentId == targetUserId;
    }

    private static UserResponseDto ToDto(User u) => new()
    {
        Id = u.Id,
        FirstName = u.FirstName,
        LastName = u.LastName,
        Email = u.Email,
        IsActive = u.IsActive,
        RoleId = u.RoleId,
        RoleName = u.Role?.Name ?? string.Empty
    };
}