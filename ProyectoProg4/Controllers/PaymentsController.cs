
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoProg4.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    // GET: api/Payments
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var payments = await _paymentService.GetAllPaymentsAsync();
        return Ok(payments);
    }

    // GET: api/Payments/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);

            if (payment == null)
                return NotFound();

            return Ok(payment);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // POST: api/Payments
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Payment payment)
    {
        try
        {
            await _paymentService.AddPaymentAsync(payment);

            return CreatedAtAction(
                nameof(GetById),
                new { id = payment.Id },
                payment
            );
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

    // PUT: api/Payments/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Payment payment)
    {
        if (id != payment.Id)
            return BadRequest("El ID de la ruta no coincide con el ID del pago.");

        try
        {
            await _paymentService.UpdatePaymentAsync(payment);
            return NoContent();
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

    // DELETE: api/Payments/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _paymentService.DeletePaymentAsync(id);
            return NoContent();
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
}
