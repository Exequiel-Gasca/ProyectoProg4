using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoProg4.Controllers;

[ApiController]
[Route("api/[controller]")]

public class SaleController : ControllerBase
{
    private readonly ISaleService _saleService;
    public SaleController(ISaleService saleService)
    {
        _saleService = saleService;
    }
    [HttpGet]
    public ActionResult<List<UserDto>> Get()
    {
        var sale = _saleService.GetAllSalesAsync();
        return Ok(sale);
    }
}