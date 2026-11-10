using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Application.DTO_s.MercadoPago
{
    public class SucursalRequest
    {
        public string name { get; set; } = string.Empty;

        [JsonPropertyName("business_hours")]
        public BusinessHours? BusinessHours { get; set; }
        [JsonPropertyName("external_id")]
        public string ExternalId { get; set; } = string.Empty;

        public required LocationRequest Location { get; set; }
    }
    public class BusinessHours
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<BusinessHour>? Monday { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<BusinessHour>? Tuesday { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<BusinessHour>? Wednesday { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<BusinessHour>? Thursday { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<BusinessHour>? Friday { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<BusinessHour>? Saturday { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<BusinessHour>? Sunday { get; set; }
    }

    public class BusinessHour
    {
        public string Open { get; set; } = string.Empty;
        public string Close { get; set; } = string.Empty;
    }

    public class LocationRequest
    {
        [JsonPropertyName("street_number")]
        public string StreetNumber { get; set; } = string.Empty;
        [JsonPropertyName("street_name")]
        public string StreetName { get; set; } = string.Empty;
        [JsonPropertyName("city_name")]
        public string CityName { get; set; } = string.Empty;
        [JsonPropertyName("state_name")]
        public string StateName { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Reference { get; set; } = string.Empty;
    }
}


