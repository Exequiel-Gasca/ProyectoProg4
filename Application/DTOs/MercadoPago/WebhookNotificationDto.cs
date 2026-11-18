using System.Text.Json.Serialization;

namespace Application.DTO_s.MercadoPago
{
    public class WebhookNotificationDto
    {
        [JsonPropertyName("action")]
        public string Action { get; set; } = string.Empty;

        [JsonPropertyName("api_version")]
        public string ApiVersion { get; set; } = string.Empty;

        [JsonPropertyName("application_id")]
        public string ApplicationId { get; set; } = string.Empty;

        [JsonPropertyName("data")]
        public WebhookDataDto? Data { get; set; }

        [JsonPropertyName("date_created")]
        public DateTime DateCreated { get; set; }

        [JsonPropertyName("live_mode")]
        public bool LiveMode { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("user_id")]
        public string UserId { get; set; } = string.Empty;
    }

    public class WebhookDataDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("external_reference")]
        public string? ExternalReference { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("status_detail")]
        public string? StatusDetail { get; set; }

        [JsonPropertyName("total_amount")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public decimal? TotalAmount { get; set; }

        [JsonPropertyName("total_paid_amount")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public decimal? TotalPaidAmount { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("version")]
        public int? Version { get; set; }

        [JsonPropertyName("transactions")]
        public WebhookTransactionsDto? Transactions { get; set; }
    }

    public class WebhookTransactionsDto
    {
        [JsonPropertyName("payments")]
        public List<WebhookPaymentDto>? Payments { get; set; }
    }

    public class WebhookPaymentDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("amount")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public decimal? Amount { get; set; }

        [JsonPropertyName("paid_amount")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public decimal? PaidAmount { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("status_detail")]
        public string? StatusDetail { get; set; }
    }
}