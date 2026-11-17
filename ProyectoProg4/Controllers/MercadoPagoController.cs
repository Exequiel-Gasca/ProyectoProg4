using System.Globalization;
using Application.DTO_s.MercadoPago;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoProg4.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MercadoPagoController : ControllerBase
{
    private readonly IMercadoPagoService _mercadoPagoService;
    private readonly IPaymentService _paymentService;
    private readonly ISaleService _saleService;

    public MercadoPagoController(
        IMercadoPagoService mercadoPagoService,
        IPaymentService paymentService,
        ISaleService saleService)
    {
        _mercadoPagoService = mercadoPagoService;
        _paymentService = paymentService;
        _saleService = saleService;
    }

    // POST: api/MercadoPago/orders
    [Authorize]
    [HttpPost("orders")]
    public async Task<IActionResult> CreateOrder([FromBody] OrderRequest request)
    {
        try
        {
            var order = await _mercadoPagoService.CreateOrderAsync(request);
            if (order == null)
                return BadRequest(new { error = "No se pudo crear la orden en Mercado Pago." });

            // Si external_reference coincide con el ID de una venta existente, registramos el pago pendiente
            if (int.TryParse(order.ExternalReference, out int saleId))
            {
                var sale = await _saleService.GetSaleByIdAsync(saleId);
                if (sale != null)
                {
                    var amount = decimal.TryParse(
                        order.TotalAmount,
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture,
                        out var parsedAmount)
                        ? parsedAmount
                        : sale.Total;

                    var payment = new Payment
                    {
                        SaleId = sale.Id,
                        Sale = null!,
                        Amount = amount,
                        Status = PaymentStatus.Pending,
                        PaymentMethod = PaymentMethod.MercadoPagoQr,
                        ExternalOrderId = order.Id,
                        ExternalPaymentId = order.Transactions?.Payments?.FirstOrDefault()?.Id
                    };

                    await _paymentService.AddPaymentAsync(payment);
                }
            }

            return Ok(order);
        }
        catch (HttpRequestException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    // GET: api/MercadoPago/orders/{orderId}
    [Authorize]
    [HttpGet("orders/{orderId}")]
    public async Task<IActionResult> GetOrder(string orderId)
    {
        try
        {
            var order = await _mercadoPagoService.GetOrderByIdAsync(orderId);
            if (order == null)
                return NotFound(new { error = $"No se encontró la orden {orderId}." });

            return Ok(order);
        }
        catch (HttpRequestException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    // POST: api/MercadoPago/orders/{orderId}/cancel
    [Authorize]
    [HttpPost("orders/{orderId}/cancel")]
    public async Task<IActionResult> CancelOrder(string orderId)
    {
        try
        {
            var order = await _mercadoPagoService.CancelOrderAsync(orderId);
            return Ok(order);
        }
        catch (HttpRequestException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    // POST: api/MercadoPago/orders/{orderId}/refund
    [Authorize]
    [HttpPost("orders/{orderId}/refund")]
    public async Task<IActionResult> RefundOrderTotal(string orderId)
    {
        try
        {
            var order = await _mercadoPagoService.RefundOrderTotalAsync(orderId);
            return Ok(order);
        }
        catch (HttpRequestException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    // POST: api/MercadoPago/orders/{orderId}/refund/partial
    [Authorize]
    [HttpPost("orders/{orderId}/refund/partial")]
    public async Task<IActionResult> RefundOrderPartial(string orderId, [FromBody] PartialRefundRequest request)
    {
        try
        {
            var order = await _mercadoPagoService.RefundOrderPartialAsync(orderId, request);
            return Ok(order);
        }
        catch (HttpRequestException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    // POST: api/MercadoPago/webhook
    [AllowAnonymous]
    [HttpPost("webhook")]
    public async Task<IActionResult> ReceiveWebhook(
        [FromQuery(Name = "data.id")] string? dataId,
        [FromQuery(Name = "type")] string? type,
        [FromBody] WebhookNotificationDto? notification)
    {
        var xSignature = Request.Headers["x-signature"].ToString();
        var xRequestId = Request.Headers["x-request-id"].ToString();

        var effectiveDataId = !string.IsNullOrWhiteSpace(dataId)
            ? dataId
            : notification?.Data?.Id ?? string.Empty;

        if (!_mercadoPagoService.ValidateWebhookSignature(xSignature, xRequestId, effectiveDataId))
            return Unauthorized();

        if (notification != null)
        {
            await _mercadoPagoService.ProcessWebhookAsync(notification);
        }

        return Ok();
    }
    // ==================== SUCURSALES ====================

    // GET: api/MercadoPago/sucursales
    [Authorize]
    [HttpGet("sucursales")]
    public async Task<IActionResult> GetAllSucursales()
    {
        var sucursales = await _mercadoPagoService.GetAllSucursalesAsync();
        return Ok(sucursales);
    }

    // GET: api/MercadoPago/sucursales/{id}
    [Authorize]
    [HttpGet("sucursales/{id:long}")]
    public async Task<IActionResult> GetSucursalById(long id)
    {
        var sucursal = await _mercadoPagoService.GetSucursalesByIdAsync(id);
        if (sucursal == null)
            return NotFound(new { error = $"No se encontró la sucursal con ID {id}." });

        return Ok(sucursal);
    }

    // POST: api/MercadoPago/sucursales
    [Authorize]
    [HttpPost("sucursales")]
    public async Task<IActionResult> CreateSucursal([FromBody] SucursalRequest request)
    {
        var sucursal = await _mercadoPagoService.CreateSucursalesAsync(request);
        if (sucursal == null)
            return BadRequest(new { error = "No se pudo crear la sucursal en Mercado Pago." });

        return CreatedAtAction(nameof(GetSucursalById), new { id = sucursal.Id }, sucursal);
    }

    // PUT: api/MercadoPago/sucursales/{id}
    [Authorize]
    [HttpPut("sucursales/{id:long}")]
    public async Task<IActionResult> UpdateSucursal(long id, [FromBody] SucursalRequest request)
    {
        var sucursal = await _mercadoPagoService.UpdateSucursalesAsync(id, request);
        if (sucursal == null)
            return NotFound(new { error = $"No se pudo actualizar la sucursal con ID {id}." });

        return Ok(sucursal);
    }

    // ==================== CAJAS ====================

    // GET: api/MercadoPago/cajas
    [Authorize]
    [HttpGet("cajas")]
    public async Task<IActionResult> GetAllCajas()
    {
        var cajas = await _mercadoPagoService.GetAllCajasAsync();
        return Ok(cajas);
    }

    // GET: api/MercadoPago/cajas/{id}
    [Authorize]
    [HttpGet("cajas/{id:long}")]
    public async Task<IActionResult> GetCajaById(long id)
    {
        var caja = await _mercadoPagoService.GetCajasByIdAsync(id);
        if (caja == null)
            return NotFound(new { error = $"No se encontró la caja con ID {id}." });

        return Ok(caja);
    }

    // POST: api/MercadoPago/cajas
    [Authorize]
    [HttpPost("cajas")]
    public async Task<IActionResult> CreateCaja([FromBody] CajaRequest request)
    {
        var caja = await _mercadoPagoService.CreateCajasAsync(request);
        if (caja == null)
            return BadRequest(new { error = "No se pudo crear la caja en Mercado Pago." });

        return CreatedAtAction(nameof(GetCajaById), new { id = caja.Id }, caja);
    }

    // PUT: api/MercadoPago/cajas/{id}
    [Authorize]
    [HttpPut("cajas/{id:long}")]
    public async Task<IActionResult> UpdateCaja(long id, [FromBody] CajaRequest request)
    {
        var caja = await _mercadoPagoService.UpdateCajasAsync(id, request);
        if (caja == null)
            return NotFound(new { error = $"No se pudo actualizar la caja con ID {id}." });

        return Ok(caja);
    }
}