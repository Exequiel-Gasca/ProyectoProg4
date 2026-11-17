namespace Application.DTOs.Responses.Sales;

public class SaleDetailResponseDto
{
    public int Id { get; set; }

    public float Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal Subtotal { get; set; }

    public int IdProduct { get; set; }
}