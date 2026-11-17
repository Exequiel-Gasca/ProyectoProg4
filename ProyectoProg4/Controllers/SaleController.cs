
using Application.DTOs.request;
using Application.DTOs.Responses.Sales;
using Application.Interfaces;
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

    // GET: api/Sale
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SaleResponseDto>>> Get()
    {
        var sales = await _saleService.GetAllSalesAsync();

        return Ok(sales);
    }

    // GET: api/Sale/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var sale = await _saleService.GetSaleByIdAsync(id);

        if (sale == null)
            return NotFound();

        return Ok(sale);
    }

    // POST: api/Sale
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] SaleRequestDto saleRequest)
    {
        try
        {
            var sale = await _saleService.CreateSaleAsync(saleRequest);

            return CreatedAtAction(
                nameof(GetById),
                new { id = sale.Id },
                sale);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // DELETE: api/Sale/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Cancel(int id)
    {
        try
        {
            await _saleService.CancelSaleAsync(id);

            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
