using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly AppDbContext _context;

    public SaleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Sale>> GetAllAsync()
    {
        return await _context.Sales
            .Include(s => s.Details)
            .Include(s => s.Payment)
            .ToListAsync();
    }

    public async Task<Sale?> GetByIdAsync(int id)
    {
        return await _context.Sales
            .Include(s => s.Details)
            .Include(s => s.Payment)
            .FirstOrDefaultAsync(s => s.IdSale == id);
    }

    public async Task AddAsync(Sale sale)
    {
        await _context.Sales.AddAsync(sale);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Sale sale)
    {
        _context.Sales.Update(sale);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var sale = await _context.Sales.FindAsync(id);

        if (sale is not null)
        {
            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync();
        }
    }
}