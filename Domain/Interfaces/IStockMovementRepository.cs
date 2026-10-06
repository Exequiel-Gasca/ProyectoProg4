using Domain.Entities;

namespace Application.Interfaces;

public interface IStockMovementRepository
{
    Task<StockMovement> GetStockMovementByIdAsync(int id);
    Task<IEnumerable<StockMovement>> GetAllStockMovementsAsync();
    Task AddStockMovementAsync(StockMovement stockMovement);
    Task UpdateStockMovementAsync(StockMovement stockMovement);
    Task DeleteStockMovementAsync(int id);
}