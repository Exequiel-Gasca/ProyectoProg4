using Application.DTOs;
using Application.Interfaces;
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
    [HttpGet]
    public ActionResult<List<UserDto>> Get()
    {
        var users = _userService.GetAllUsersAsync();
        return Ok(users);
    }
}
