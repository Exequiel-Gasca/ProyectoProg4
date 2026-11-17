
using System.Security.Claims;
using Application.DTOs.request;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoProg4.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,SuperAdmin")]
public class SaleController : ControllerBase
{
    private readonly ISaleService _saleService;

    public SaleController(ISaleService saleService)
    {
        _saleService = saleService;
    }

    private int CurrentUserId => int.Parse(User.FindFirstValue("sub")!);
    private bool IsAdmin => User.IsInRole("Admin") || User.IsInRole("SuperAdmin");

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _saleService.GetAllSalesAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var sale = await _saleService.GetSaleByIdAsync(id);
        return sale == null ? NotFound() : Ok(sale);
    }

    [HttpGet("by-date")]
    public async Task<IActionResult> GetByDate([FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        try { return Ok(await _saleService.GetSalesByDateRangeAsync(from, to)); }
        catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SaleRequestDto request)
    {
        try
        {
            var sale = await _saleService.CreateSaleAsync(request, CurrentUserId);
            return CreatedAtAction(nameof(GetById), new { id = sale.Id }, sale);
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
    }

    // Confirma la venta: valida y descuenta stock
    [HttpPost("{id:int}/confirm")]
    public async Task<IActionResult> Confirm(int id)
    {
        try
        {
            var sale = await _saleService.ConfirmSaleAsync(id, CurrentUserId);
            return Ok(sale);
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
        catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> Cancel(int id)
    {
        try
        {
            await _saleService.CancelSaleAsync(id, CurrentUserId);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }
}