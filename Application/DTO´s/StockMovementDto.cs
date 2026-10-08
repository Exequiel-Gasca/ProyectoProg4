using System;
using System.Collections.Generic;
using System.Text;
using Domain.Enums;

namespace Application.DTOs;

public class StockMovementDto
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public float Quantity { get; set; }

    public StockMovementType Type { get; set; }

    public string Reason { get; set; } = string.Empty;

    public int IdProduct { get; set; }

    public int IdUser { get; set; }
}