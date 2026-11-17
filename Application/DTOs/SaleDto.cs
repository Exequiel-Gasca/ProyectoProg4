using System;
using System.Collections.Generic;
using System.Text;
using Domain.Enums;

namespace Application.DTOs;

public class SaleDto
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public decimal Total { get; set; }

    public SaleStatus Status { get; set; }

    public int IdUser { get; set; }

    public int? IdCustomer { get; set; }

    public List<SaleDetailDto> Details { get; set; } = new();

    public PaymentDto? Payment { get; set; }
}