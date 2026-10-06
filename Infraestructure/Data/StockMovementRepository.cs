using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories;

public class StockMovementRepository : IStockMovementRepository
{
    private readonly AppDbContext _context;

    public StockMovementRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<StockMovement> GetStockMovementByIdAsync(int id)
    {
        return await _context.StockMovements
            .Include(sm => sm.Product)
            .Include(sm => sm.User)
            .FirstOrDefaultAsync(sm => sm.IdStockMovement == id);
    }

    public async Task<IEnumerable<StockMovement>> GetAllStockMovementsAsync()
    {
        return await _context.StockMovements
            .Include(sm => sm.Product)
            .Include(sm => sm.User)
            .ToListAsync();
    }

    public async Task AddStockMovementAsync(StockMovement stockMovement)
    {
        await _context.StockMovements.AddAsync(stockMovement);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateStockMovementAsync(StockMovement stockMovement)
    {
        _context.StockMovements.Update(stockMovement);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteStockMovementAsync(int id)
    {
        var stockMovement = await _context.StockMovements.FindAsync(id);

        if (stockMovement != null)
        {
            _context.StockMovements.Remove(stockMovement);
            await _context.SaveChangesAsync();
        }
    }
}