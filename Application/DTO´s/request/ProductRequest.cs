namespace Application.DTOs.Requests.Products;

public class ProductRequest
{
    public string Barcode { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal SalePrice { get; set; }

    public decimal Cost { get; set; }

    public float CurrentStock { get; set; }

    public int MeasurementUnit { get; set; }

    public float MinimumStock { get; set; }

    public bool IsActive { get; set; }

    public int CategoryId { get; set; }
}