using System.Text.Json.Serialization;

namespace Application.DTO_s.MercadoPago
{
    public class OrderResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("processing_mode")]
        public string ProcessingMode { get; set; } = string.Empty;

        [JsonPropertyName("external_reference")]
        public string ExternalReference { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("total_amount")]
        public string TotalAmount { get; set; } = string.Empty;

        [JsonPropertyName("expiration_time")]
        public string? ExpirationTime { get; set; }

        [JsonPropertyName("country_code")]
        public string? CountryCode { get; set; }

        [JsonPropertyName("user_id")]
        public string? UserId { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("status_detail")]
        public string StatusDetail { get; set; } = string.Empty;

        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        [JsonPropertyName("created_date")]
        public DateTime CreatedDate { get; set; }

        [JsonPropertyName("last_updated_date")]
        public DateTime LastUpdatedDate { get; set; }

        [JsonPropertyName("config")]
        public OrderConfig? Config { get; set; }

        // Solo viene en modelos "dynamic" y "hybrid"
        [JsonPropertyName("type_response")]
        public OrderTypeResponse? TypeResponse { get; set; }

        [JsonPropertyName("transactions")]
        public OrderTransactionsResponse? Transactions { get; set; }

        [JsonPropertyName("items")]
        public List<OrderItemDto>? Items { get; set; }
    }

    public class OrderTypeResponse
    {
        [JsonPropertyName("qr_data")]
        public string QrData { get; set; } = string.Empty;
    }

    public class OrderTransactionsResponse
    {
        [JsonPropertyName("payments")]
        public List<OrderPaymentResponse>? Payments { get; set; }

        [JsonPropertyName("refunds")]
        public List<OrderRefundResponse>? Refunds { get; set; }
    }

    public class OrderPaymentResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("amount")]
        public string Amount { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("status_detail")]
        public string StatusDetail { get; set; } = string.Empty;
    }

    public class OrderRefundResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("transaction_id")]
        public string TransactionId { get; set; } = string.Empty;

        [JsonPropertyName("reference_id")]
        public string? ReferenceId { get; set; }

        [JsonPropertyName("amount")]
        public string Amount { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
    }
}