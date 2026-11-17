using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Application.DTO_s.MercadoPago
{
    public class CajaResponse
    {
        public long Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("date_created")]
        public string DateCreated { get; set; } = string.Empty;

        [JsonPropertyName("date_last_updated")]
        public string DateLastUpdated { get; set; } = string.Empty;

        [JsonPropertyName("user_id")]
        public long UserId { get; set; }

        [JsonPropertyName("store_id")]
        public string StoreId { get; set; } = string.Empty;

        [JsonPropertyName("external_store_id")]
        public string ExternalStoreId { get; set; } = string.Empty;

        [JsonPropertyName("external_id")]
        public string ExternalId { get; set; } = string.Empty;

        public Config? Config { get; set; }

        [JsonPropertyName("qr_response")]
        public QrResponse? QrResponse { get; set; }
    }

    public class QrResponse
    {
        public string Uuid { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;

        [JsonPropertyName("template_document")]
        public string TemplateDocument { get; set; } = string.Empty;

        [JsonPropertyName("template_image")]
        public string TemplateImage { get; set; } = string.Empty;

        [JsonPropertyName("qr_code")]
        public string QrCode { get; set; } = string.Empty;
    }
}
