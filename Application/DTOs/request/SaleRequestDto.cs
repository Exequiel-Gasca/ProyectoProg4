namespace Application.DTOs.request;

public class SaleRequestDto
{
    public int? IdCustomer { get; set; }

    public List<SaleDetailRequestDto> Details { get; set; } = new();
}