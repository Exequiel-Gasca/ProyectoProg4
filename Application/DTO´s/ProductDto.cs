using System;
using System.Collections.Generic;
using System.Text;
using Domain.Enums;

namespace Application.DTOs;

public class ProductDto
{
    public int Id { get; set; }

    public string Barcode { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal SalePrice { get; set; }

    public float CurrentStock { get; set; }

    public MeasurementUnit MeasurementUnit { get; set; }

    public float MinimumStock { get; set; }

    public bool IsActive { get; set; }

    public int IdCategory { get; set; }
}