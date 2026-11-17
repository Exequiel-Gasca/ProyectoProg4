namespace Application.DTOs.request;

public class SaleRequestDto
{
    public int IdUser { get; set; }

    public int? IdCustomer { get; set; }

    public List<SaleDetailRequestDto> Details { get; set; } = new();
}