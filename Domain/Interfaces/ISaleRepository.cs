using Domain.Entities;

namespace Domain.Interfaces;

public interface ISaleRepository
{
    Task<Sale?> GetSaleByIdAsync(int id);
    Task<IEnumerable<Sale>> GetAllSalesAsync();
    Task<IEnumerable<Sale>> GetSalesByDateRangeAsync(
        DateTime startDate,
        DateTime endDate);
    Task AddSaleAsync(Sale sale);
    Task UpdateSaleAsync(Sale sale);
}