using Domain.Entities;

namespace Application.Interfaces;

public interface ISaleService
{
    Task<Sale?> GetSaleByIdAsync(int id);
    Task<IEnumerable<Sale>> GetAllSalesAsync();

    Task<IEnumerable<Sale>> GetSalesByDateRangeAsync(
        DateTime startDate,
        DateTime endDate);

    Task<Sale> CreateSaleAsync(Sale sale);
    Task CancelSaleAsync(int id);
}