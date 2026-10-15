using System;
using System.Collections.Generic;
using System.Text;
using Domain.Enums;

namespace Domain.Entities;

public class StockMovement
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public float Quantity { get; set; }

    public StockMovementType Type { get; set; }

    public string Reason { get; set; } = string.Empty;

    public int ProductId { get; set; }

    public required Product Product { get; set; }

    public int UserId { get; set; }

    public required User User { get; set; }
}