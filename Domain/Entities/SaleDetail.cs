using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities;

public class SaleDetail
{
    public int IdSaleDetail { get; set; }

    public float Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal Subtotal { get; set; }

    public int IdSale { get; set; }

    public Sale Sale { get; set; } = null!;

    public int IdProduct { get; set; }

    public Product Product { get; set; } = null!;
}