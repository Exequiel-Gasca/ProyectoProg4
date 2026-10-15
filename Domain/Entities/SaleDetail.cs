using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities;

public class SaleDetail
{
    public int Id { get; set; }

    public float Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal Subtotal { get; set; }

    public int IdSale { get; set; }

    public required Sale Sale { get; set; }

    public int ProductId { get; set; }

    public required Product Product { get; set; }
}