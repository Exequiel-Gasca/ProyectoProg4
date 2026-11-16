using System.Text.Json.Serialization;

namespace Application.DTO_s.MercadoPago
{
    public class OrderRequest
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "qr";

        [JsonPropertyName("total_amount")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TotalAmount { get; set; }

        [JsonPropertyName("description")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Description { get; set; }

        [JsonPropertyName("external_reference")]
        public string ExternalReference { get; set; } = string.Empty;

        [JsonPropertyName("expiration_time")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ExpirationTime { get; set; }

        [JsonPropertyName("config")]
        public required OrderConfig Config { get; set; }

        [JsonPropertyName("transactions")]
        public required OrderTransactionsRequest Transactions { get; set; }

        [JsonPropertyName("items")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<OrderItemDto>? Items { get; set; }
    }

    public class OrderConfig
    {
        [JsonPropertyName("qr")]
        public required OrderQrConfig Qr { get; set; }
    }

    public class OrderQrConfig
    {
        [JsonPropertyName("external_pos_id")]
        public string ExternalPosId { get; set; } = string.Empty;

        // Valores posibles: "static", "dynamic", "hybrid"
        [JsonPropertyName("mode")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Mode { get; set; }
    }

    public class OrderTransactionsRequest
    {
        [JsonPropertyName("payments")]
        public List<OrderPaymentRequest> Payments { get; set; } = new();
    }

    public class OrderPaymentRequest
    {
        // Formato esperado: "50.00" (CultureInfo.InvariantCulture)
        [JsonPropertyName("amount")]
        public string Amount { get; set; } = string.Empty;
    }

    public class OrderItemDto
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("unit_price")]
        public string UnitPrice { get; set; } = string.Empty;

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("unit_measure")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? UnitMeasure { get; set; }

        [JsonPropertyName("external_code")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ExternalCode { get; set; }
    }
}