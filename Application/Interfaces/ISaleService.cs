using Application.DTOs.request;
using Application.DTOs.Responses.Sales;

namespace Application.Interfaces;

public interface ISaleService
{
    Task<SaleResponseDto?> GetSaleByIdAsync(int id);
    Task<IEnumerable<SaleResponseDto>> GetAllSalesAsync();
    Task<IEnumerable<SaleResponseDto>> GetSalesByDateRangeAsync(DateTime startDate, DateTime endDate);

    Task<SaleResponseDto> CreateSaleAsync(SaleRequestDto saleRequest, int userId);
    Task<SaleResponseDto> ConfirmSaleAsync(int saleId, int userId);
    Task CancelSaleAsync(int saleId, int userId);
}