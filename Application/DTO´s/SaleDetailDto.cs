using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs;

public class SaleDetailDto
{
    public int IdSaleDetail { get; set; }

    public float Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal Subtotal { get; set; }

    public int IdProduct { get; set; }
}