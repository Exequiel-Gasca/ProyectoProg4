using System;
using System.Collections.Generic;
using System.Text;
using Domain.Enums;

namespace Application.DTOs;

public class PaymentDto
{
    public int IdPayment { get; set; }

    public DateTime Date { get; set; }

    public decimal Amount { get; set; }

    public PaymentStatus Status { get; set; }

    public PaymentMethod PaymentMethod { get; set; }

    public int IdSale { get; set; }
}