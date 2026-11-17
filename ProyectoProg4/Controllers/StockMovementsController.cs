using System.Security.Claims;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoProg4.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,SuperAdmin")]
public class StockMovementController : ControllerBase
{
    private readonly IStockMovementService _stockMovementService;

    public StockMovementController(IStockMovementService stockMovementService)
    {
        _stockMovementService = stockMovementService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StockMovementDto>>> GetAll()
        => Ok(await _stockMovementService.GetAllStockMovementsAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<StockMovementDto>> GetById(int id)
    {
        var movement = await _stockMovementService.GetStockMovementByIdAsync(id);
        return movement == null ? NotFound() : Ok(movement);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] StockMovementRequestDto request)
    {
        try
        {
            var userId = int.Parse(User.FindFirstValue("sub")!);

            await _stockMovementService.RegisterMovementAsync(
                request.ProductId,
                request.Quantity,
                request.Type,
                request.Reason,
                userId);

            return StatusCode(StatusCodes.Status201Created,
                new { message = "Movimiento de stock registrado." });
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
        catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
    }
}