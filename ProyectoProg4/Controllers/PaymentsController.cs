using Application.DTOs;
using Application.Interfaces;
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
    [HttpGet]
    public ActionResult<List<UserDto>> Get()
    {
        var payment = _paymentService.GetAllPaymentsAsync();
        return Ok(payment);
    }
}