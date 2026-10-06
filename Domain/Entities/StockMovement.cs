using System;
using System.Collections.Generic;
using System.Text;
using Domain.Enums;

namespace Domain.Entities;

public class StockMovement
{
    public int IdStockMovement { get; set; }

    public DateTime Date { get; set; }

    public float Quantity { get; set; }

    public StockMovementType Type { get; set; }

    public string Reason { get; set; } = string.Empty;

    public int IdProduct { get; set; }

    public Product Product { get; set; } = null!;

    public int IdUser { get; set; }

    public User User { get; set; } = null!;
}