using System;
using System.Collections.Generic;
using System.Text;
using Domain.Enums;

namespace Domain.Entities;

public class Sale
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public decimal Total { get; set; }

    public SaleStatus Status { get; set; }

    public int UserId { get; set; }

    public required User User { get; set; }

    public int? CustomerId { get; set; }

    public Customer? Customer { get; set; }

    public ICollection<SaleDetail> Detail { get; set; } = new List<SaleDetail>();

    public ICollection<Payment> Payment { get; set; } = new List<Payment>();
}