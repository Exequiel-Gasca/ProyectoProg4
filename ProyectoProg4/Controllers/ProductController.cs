using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoProg4.Controllers;

[ApiController]
[Route("api/[controller]")]

public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    public ProductController(IProductService productService)
    {
        _productService = productService;
    }
    [HttpGet]
    public ActionResult<List<UserDto>> Get()
    {
        var users = _productService.GetAllProductsAsync();
        return Ok(users);
    }
}