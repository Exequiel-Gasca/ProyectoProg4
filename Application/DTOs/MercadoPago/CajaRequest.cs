using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Application.DTO_s.MercadoPago
{
    public class CajaRequest
    {
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("store_id")]
        public int StoreId { get; set; }
        [JsonPropertyName("external_id")]
        public string ExternalId { get; set; } = string.Empty;
        public required Config Config { get; set; }

    }

    public class Config
    {
        public required Qr Qr { get; set; }
    }

    public class Qr
    {
        [JsonPropertyName("operating_mode")]
        public string OperatingMode { get; set; } = string.Empty;
    }
}
