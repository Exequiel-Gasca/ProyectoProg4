using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Domain.Entities.MercadoPagoEntities
{
    public class PostSucursal
    {
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }
        public string name { get; set; } = string.Empty;
        
        [JsonPropertyName("business_hours")]
        public BusinessHours? BusinessHours { get; set; }
        [JsonPropertyName("external_id")]
        public string ExternalId { get; set; } = string.Empty;
        
        public required Location Location { get; set; }
    }




    public class BusinessHours
    {
        public List<BusinessHour>? Monday   { get; set; }
        public List<BusinessHour>? Tuesday { get; set; }
        public List<BusinessHour>? Wednesday { get; set; }
        public List<BusinessHour>? Thursday { get; set; }
        public List<BusinessHour>? Friday { get; set; }
        public List<BusinessHour>? Saturday { get; set; }
        public List<BusinessHour>? Sunday { get; set; }
    }

    public class BusinessHour
    {
        public string Open { get; set; } = string.Empty;
        public string Close { get; set; } = string.Empty;
    }

    public class Location
    {
        [JsonPropertyName("street_number")]
        public int StreetNumber { get; set; }
        [JsonPropertyName("street_name")]
        public string StreetName { get; set; } = string.Empty;
        [JsonPropertyName("city_name")]
        public string CityName { get; set; } = string.Empty;
        [JsonPropertyName("state_name")]
        public string StateName { get; set; } = string.Empty;
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string Reference { get; set; } = string.Empty;
    }
}
