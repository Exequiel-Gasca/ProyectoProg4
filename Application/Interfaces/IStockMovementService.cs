using Application.DTOs;
using Domain.Enums;

namespace Application.Interfaces;

public interface IStockMovementService
{
    Task<StockMovementDto?> GetStockMovementByIdAsync(int id);
    Task<IEnumerable<StockMovementDto>> GetAllStockMovementsAsync();

    Task RegisterMovementAsync(
        int productId,
        float quantity,
        StockMovementType type,
        string reason,
        int userId,
        bool allowNegativeStock = false);
}