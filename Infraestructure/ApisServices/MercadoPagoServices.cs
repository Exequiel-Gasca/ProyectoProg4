using Application.DTO_s.MercadoPago;
using Application.Interfaces;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using Infraestructure;
using Microsoft.Extensions.Logging;

namespace Infrastructure.ApisServices
{
    public class MercadoPagoServices : IMercadoPagoService
    {
        private readonly HttpClient _client;
        private readonly string _userId;
        private readonly string _webhookSecret;
        private readonly IPaymentRepository _paymentRepository;
        private readonly ISaleRepository _saleRepository;
        private readonly ISaleService _saleService;
        private readonly ILogger<MercadoPagoServices> _logger;

        public MercadoPagoServices(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IPaymentRepository paymentRepository,
            ISaleRepository saleRepository,
            ISaleService saleService,
            ILogger<MercadoPagoServices> logger)
        {
            _client = httpClientFactory.CreateClient(HttpClientNames.MercadoPago);
            _userId = configuration["ExternalApis:MercadoPago:UserId"] ?? string.Empty;
            _webhookSecret = configuration["ExternalApis:MercadoPago:WebhookSecret"] ?? string.Empty;
            _paymentRepository = paymentRepository;
            _saleRepository = saleRepository;
            _saleService = saleService;
            _logger = logger;
        }

        public async Task<List<SucursalResponse>> GetAllSucursalesAsync()
        {
            string url = $"users/{_userId}/stores/search";
            var result = await _client.GetFromJsonAsync<MercadoPagoListResponse<SucursalResponse>>(url);
            return result?.Results ?? new List<SucursalResponse>();
        }

        public async Task<SucursalResponse?> GetSucursalesByIdAsync(long Id)
        {
            string url = $"stores/{Id}";
            return await _client.GetFromJsonAsync<SucursalResponse>(url);
        }

        public async Task<SucursalResponse?> CreateSucursalesAsync(SucursalRequest sucursalRequest)
        {
            string url = $"users/{_userId}/stores";
            HttpResponseMessage response = await _client.PostAsJsonAsync(url, sucursalRequest);
            return await response.Content.ReadFromJsonAsync<SucursalResponse>();
        }

        public async Task<SucursalResponse?> UpdateSucursalesAsync(long Id, SucursalRequest sucursalRequest)
        {
            string url = $"users/{_userId}/stores/{Id}";
            HttpResponseMessage response = await _client.PutAsJsonAsync(url, sucursalRequest);
            return await response.Content.ReadFromJsonAsync<SucursalResponse>();
        }

        public async Task DeleteSucursalesAsync(long Id)
        {
            string url = $"users/{_userId}/stores/{Id}";
            await _client.DeleteAsync(url);
        }

        public async Task<List<CajaResponse>> GetAllCajasAsync()
        {
            string url = "pos";
            var result = await _client.GetFromJsonAsync<MercadoPagoListResponse<CajaResponse>>(url);
            return result?.Results ?? new List<CajaResponse>();
        }
        public async Task<CajaResponse?> GetCajasByIdAsync(long Id)
        {
            string url = $"pos/{Id}";
            return await _client.GetFromJsonAsync<CajaResponse>(url);
        }

        public async Task<CajaResponse?> CreateCajasAsync(CajaRequest cajaRequest)
        {
            string url = "pos";
            var response = await _client.PostAsJsonAsync(url, cajaRequest);
            return await response.Content.ReadFromJsonAsync<CajaResponse>();
        }

        public async Task<CajaResponse?> UpdateCajasAsync(long Id, CajaRequest cajaRequest)
        {
            string url = $"pos/{Id}";
            var response = await _client.PutAsJsonAsync(url, cajaRequest);
            return await response.Content.ReadFromJsonAsync<CajaResponse>();
        }

        public async Task DeleteCajasAsync(long Id)
        {
            string url = $"pos/{Id}";
            await _client.DeleteAsync(url);
        }

        // ORDERS (Procesamiento de pagos QR)

        public async Task<OrderResponse?> CreateOrderAsync(OrderRequest request)
        {
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "v1/orders");
            httpRequest.Headers.Add("X-Idempotency-Key", Guid.NewGuid().ToString());
            httpRequest.Content = JsonContent.Create(request);

            var response = await _client.SendAsync(httpRequest);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error al crear la orden ({(int)response.StatusCode}): {error}");
            }

            return await response.Content.ReadFromJsonAsync<OrderResponse>();
        }

        public async Task<OrderResponse?> GetOrderByIdAsync(string orderId)
        {
            var response = await _client.GetAsync($"v1/orders/{orderId}");
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error al consultar la orden {orderId} ({(int)response.StatusCode}): {error}");
            }

            return await response.Content.ReadFromJsonAsync<OrderResponse>();
        }

        public async Task<OrderResponse?> CancelOrderAsync(string orderId)
        {
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"v1/orders/{orderId}/cancel");
            httpRequest.Headers.Add("X-Idempotency-Key", Guid.NewGuid().ToString());

            var response = await _client.SendAsync(httpRequest);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error al cancelar la orden {orderId} ({(int)response.StatusCode}): {error}");
            }

            return await response.Content.ReadFromJsonAsync<OrderResponse>();
        }

        public async Task<OrderResponse?> RefundOrderTotalAsync(string orderId)
        {
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"v1/orders/{orderId}/refund");
            httpRequest.Headers.Add("X-Idempotency-Key", Guid.NewGuid().ToString());

            var response = await _client.SendAsync(httpRequest);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error al reembolsar la orden {orderId} ({(int)response.StatusCode}): {error}");
            }

            return await response.Content.ReadFromJsonAsync<OrderResponse>();
        }

        public async Task<OrderResponse?> RefundOrderPartialAsync(string orderId, PartialRefundRequest request)
        {
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"v1/orders/{orderId}/refund");
            httpRequest.Headers.Add("X-Idempotency-Key", Guid.NewGuid().ToString());
            httpRequest.Content = JsonContent.Create(request);

            var response = await _client.SendAsync(httpRequest);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error al reembolsar parcialmente la orden {orderId} ({(int)response.StatusCode}): {error}");
            }

            return await response.Content.ReadFromJsonAsync<OrderResponse>();
        }

        // WEBHOOKS (Notificaciones)

        public bool ValidateWebhookSignature(string xSignature, string xRequestId, string dataId)
        {
            if (string.IsNullOrWhiteSpace(_webhookSecret) || string.IsNullOrWhiteSpace(xSignature))
                return false;

            string? ts = null;
            string? hash = null;

            foreach (var part in xSignature.Split(','))
            {
                var eq = part.IndexOf('=');
                if (eq < 0) continue;

                var key = part[..eq].Trim();
                var val = part[(eq + 1)..].Trim();

                if (key == "ts") ts = val;
                if (key == "v1") hash = val;
            }

            if (string.IsNullOrWhiteSpace(ts) || string.IsNullOrWhiteSpace(hash))
                return false;

            var parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(dataId))
                parts.Add($"id:{dataId.ToLowerInvariant()}");
            if (!string.IsNullOrWhiteSpace(xRequestId))
                parts.Add($"request-id:{xRequestId}");
            parts.Add($"ts:{ts}");

            var manifest = string.Join(";", parts) + ";";

            using var hmacSha = new HMACSHA256(Encoding.UTF8.GetBytes(_webhookSecret));
            var computed = BitConverter
                .ToString(hmacSha.ComputeHash(Encoding.UTF8.GetBytes(manifest)))
                .Replace("-", "")
                .ToLowerInvariant();

            return CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(computed),
                Encoding.UTF8.GetBytes(hash));
        }

        public async Task ProcessWebhookAsync(WebhookNotificationDto notification)
        {
            if (notification.Type != "order" || notification.Data == null || string.IsNullOrWhiteSpace(notification.Data.Id))
                return;

            var payment = await _paymentRepository.GetByExternalOrderIdAsync(notification.Data.Id);
            if (payment == null)
                return;

            var action = notification.Action?.ToLowerInvariant();
            var status = notification.Data.Status?.ToLowerInvariant();
            var sale = payment.Sale;

            if (action == "order.processed" || status == "processed")
            {
                payment.Status = PaymentStatus.Approved;
                var mpPaymentId = notification.Data.Transactions?.Payments?.FirstOrDefault()?.Id;
                if (!string.IsNullOrWhiteSpace(mpPaymentId))
                    payment.ExternalPaymentId = mpPaymentId;

                await _paymentRepository.UpdatePaymentAsync(payment);

                // Confirmar la venta solo si sigue pendiente (un webhook repetido no descuenta dos veces)
                if (sale.Status == SaleStatus.Pending)
                {
                    try
                    {
                        await _saleService.ConfirmSaleAsync(sale.Id, sale.UserId);
                    }
                    catch (InvalidOperationException ex)
                    {
                        // Pago aprobado pero sin stock: la venta queda Pending para revisión
                        _logger.LogWarning(
                            "Pago aprobado pero no se pudo confirmar la venta {SaleId}: {Message}",
                            sale.Id, ex.Message);
                    }
                }
                return;
            }

            if (action == "order.canceled" || action == "order.expired" || status == "canceled" || status == "expired")
            {
                payment.Status = PaymentStatus.Cancelled;
            }
            else if (action == "order.refunded" || status == "refunded")
            {
                payment.Status = PaymentStatus.Refunded;
            }
            else
            {
                return;
            }

            await _paymentRepository.UpdatePaymentAsync(payment);

            // Cancelar la venta (si estaba Approved, devuelve el stock)
            if (sale.Status != SaleStatus.Cancelled)
                await _saleService.CancelSaleAsync(sale.Id, sale.UserId);
        }
    }
}