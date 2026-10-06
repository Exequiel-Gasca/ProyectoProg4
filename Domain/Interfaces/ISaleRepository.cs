using Domain.Entities;

namespace Application.Interfaces;

public interface ISaleRepository
{
    Task<Sale> GetSaleByIdAsync(int id);
    Task<IEnumerable<Sale>> GetAllSalesAsync();
    Task AddSaleAsync(Sale sale);
    Task UpdateSaleAsync(Sale sale);
    Task DeleteSaleAsync(int id);
}