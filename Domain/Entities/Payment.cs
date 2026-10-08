using System;
using System.Collections.Generic;
using System.Text;
using Domain.Enums;

namespace Domain.Entities;

public class Payment
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public decimal Amount { get; set; }

    public PaymentStatus Status { get; set; }

    public PaymentMethod PaymentMethod { get; set; }

    public int IdSale { get; set; }

    public Sale Sale { get; set; } = null!;
}