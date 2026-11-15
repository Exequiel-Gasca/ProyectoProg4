
using Domain.Entities;

namespace Domain.Interfaces;

public interface ISaleDetailRepository
{
    Task<SaleDetail?> GetSaleDetailByIdAsync(int id);
    Task<IEnumerable<SaleDetail>> GetAllSaleDetailsAsync();
    Task AddSaleDetailAsync(SaleDetail saleDetail);
    Task UpdateSaleDetailAsync(SaleDetail saleDetail);
    Task DeleteSaleDetailAsync(int id);
}
