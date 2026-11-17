using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Application.DTO_s.MercadoPago
{
    public class SucursalResponse
    {
        public long Id { get; set; }

        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("date_created")]
        public string DateCreated { get; set; } = string.Empty;

        [JsonPropertyName("business_hours")]
        public BusinessHours? BusinessHours { get; set; }

        public LocationResponse? Location { get; set; }

        [JsonPropertyName("external_id")]
        public string ExternalId { get; set; } = string.Empty;
    }

    public class LocationResponse
    {
        [JsonPropertyName("address_line")]
        public string AddressLine { get; set; } = string.Empty;

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Reference { get; set; } = string.Empty;
    }
}