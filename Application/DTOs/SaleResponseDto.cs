using Application.DTOs;
using Domain.Enums;

namespace Application.DTOs.Responses.Sales;

public class SaleResponseDto
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public decimal Total { get; set; }

    public SaleStatus Status { get; set; }

    public int IdUser { get; set; }

    public int? IdCustomer { get; set; }

    public List<SaleDetailResponseDto> Details { get; set; } = new();

    public List<PaymentDto> Payments { get; set; } = new();
}