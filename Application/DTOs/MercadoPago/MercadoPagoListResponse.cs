using System.Text.Json.Serialization;

namespace Application.DTO_s.MercadoPago
{
    public class MercadoPagoListResponse<T>
    {
        [JsonPropertyName("results")]
        public List<T> Results { get; set; } = new();
    }
}