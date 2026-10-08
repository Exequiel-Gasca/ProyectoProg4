using Domain.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories;

public class StockMovementRepository : IStockMovementRepository
{
    private readonly ApplicationDbContext _context;

    public StockMovementRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StockMovement> GetStockMovementByIdAsync(int id)
    {
        return await _context.StockMovement
            .Include(sm => sm.Product)
            .Include(sm => sm.User)
            .FirstOrDefaultAsync(sm => sm.Id == id);
    }

    public async Task<IEnumerable<StockMovement>> GetAllStockMovementsAsync()
    {
        return await _context.StockMovement
            .Include(sm => sm.Product)
            .Include(sm => sm.User)
            .ToListAsync();
    }

    public async Task AddStockMovementAsync(StockMovement stockMovement)
    {
        await _context.StockMovement.AddAsync(stockMovement);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateStockMovementAsync(StockMovement stockMovement)
    {
        _context.StockMovement.Update(stockMovement);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteStockMovementAsync(int id)
    {
        var stockMovement = await _context.StockMovement.FindAsync(id);

        if (stockMovement != null)
        {
            _context.StockMovement.Remove(stockMovement);
            await _context.SaveChangesAsync();
        }
    }
}