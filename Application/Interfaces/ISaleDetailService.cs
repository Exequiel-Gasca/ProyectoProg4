using Domain.Entities;

namespace Application.Interfaces;

public interface ISaleDetailService
{
    Task<SaleDetail?> GetSaleDetailByIdAsync(int id);
    Task<IEnumerable<SaleDetail>> GetAllSaleDetailsAsync();
    Task AddSaleDetailAsync(SaleDetail saleDetail);
    Task UpdateSaleDetailAsync(SaleDetail saleDetail);
    Task DeleteSaleDetailAsync(int id);
}