using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces;

public interface IStockMovementService
{
    Task<StockMovementDto?> GetStockMovementByIdAsync(int id);
    Task<IEnumerable<StockMovementDto>> GetAllStockMovementsAsync();
    Task<StockMovementDto> AddStockMovementAsync(StockMovement stockMovement);
    Task<StockMovementDto> UpdateStockMovementAsync(int id,StockMovement stockMovement);
    Task DeleteStockMovementAsync(int id);
    Task UpdateStock(StockMovement stockMovement);
}