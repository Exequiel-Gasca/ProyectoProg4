using Domain.Enums;

namespace Application.DTOs.Requests.Products;

public class ProductRequestDto
{
    public string Barcode { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal SalePrice { get; set; }

    public decimal Cost { get; set; }

    public float CurrentStock { get; set; }

    public MeasurementUnit MeasurementUnit { get; set; }

    public float MinimumStock { get; set; }

    public bool IsActive { get; set; }

    public int CategoryId { get; set; }
}