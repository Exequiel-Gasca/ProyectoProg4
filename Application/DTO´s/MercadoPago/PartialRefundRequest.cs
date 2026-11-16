using System.Text.Json.Serialization;

namespace Application.DTO_s.MercadoPago
{
    public class PartialRefundRequest
    {
        [JsonPropertyName("transactions")]
        public List<PartialRefundTransactionRequest> Transactions { get; set; } = new();
    }

    public class PartialRefundTransactionRequest
    {
        // ID del pago (PAY...), obtenido en transactions.payments[0].id
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        // Monto a reembolsar en formato "24.90"
        [JsonPropertyName("amount")]
        public string Amount { get; set; } = string.Empty;
    }
}