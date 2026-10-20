using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoProg4.Controllers;

[ApiController]
[Route("api/[controller]")]

public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;
    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }
    [HttpGet]
    public ActionResult<List<UserDto>> Get()
    {
        var customer = _customerService.GetAllCustomerAsync();
        return Ok(customer);
    }
}