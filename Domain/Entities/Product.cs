using System;
using System.Collections.Generic;
using System.Text;
using Domain.Enums;

namespace Domain.Entities;

public class Product
{
    public int IdProduct { get; set; }

    public string Barcode { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal SalePrice { get; set; }

    public decimal Cost { get; set; }

    public float CurrentStock { get; set; }

    public MeasurementUnit MeasurementUnit { get; set; }

    public float MinimumStock { get; set; }

    public bool IsActive { get; set; }

    public int IdCategory { get; set; }

    public Category Category { get; set; } = null!;

    public ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();

    public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
}