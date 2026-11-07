using Domain.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly ApplicationDbContext _context;

    public SaleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Sale?> GetSaleByIdAsync(int id)
    {
        return await _context.Sale
            .Include(s => s.Detail)
                .ThenInclude(d => d.Product)
            .Include(s => s.Payment)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Sale>> GetAllSalesAsync()
    {
        return await _context.Sale
            .Include(s => s.Detail)
                .ThenInclude(d => d.Product)
            .Include(s => s.Payment)
            .ToListAsync();
    }

    public async Task<IEnumerable<Sale>> GetSalesByDateRangeAsync(
        DateTime startDate,
        DateTime endDate)
    {
        return await _context.Sale
            .Include(s => s.Detail)
                .ThenInclude(d => d.Product)
            .Include(s => s.Payment)
            .Where(s => s.Date >= startDate && s.Date <= endDate)
            .ToListAsync();
    }

    public async Task AddSaleAsync(Sale sale)
    {
        await _context.Sale.AddAsync(sale);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateSaleAsync(Sale sale)
    {
        _context.Sale.Update(sale);
        await _context.SaveChangesAsync();
    }
}