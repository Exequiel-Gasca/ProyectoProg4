using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoProg4.Controllers;

[ApiController]
[Route("api/[controller]")]

public class StockMovementController : ControllerBase
{
    private readonly IStockMovementService _stockmovementService;
    public StockMovementController(IStockMovementService stockmovementService)
    {
        _stockmovementService = stockmovementService;
    }
    [HttpGet]
    public ActionResult<List<UserDto>> Get()
    {
        var StockMovement = _stockmovementService.GetAllStockMovementsAsync();
        return Ok(StockMovement);
    }
}