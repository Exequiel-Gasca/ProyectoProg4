
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories;

public class SaleDetailRepository : ISaleDetailRepository
{
    private readonly ApplicationDbContext _context;

    public SaleDetailRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SaleDetail?> GetSaleDetailByIdAsync(int id)
    {
        return await _context.SaleDetail
            .Include(sd => sd.Product)
            .Include(sd => sd.Sale)
            .FirstOrDefaultAsync(sd => sd.Id == id);
    }

    public async Task<IEnumerable<SaleDetail>> GetAllSaleDetailsAsync()
    {
        return await _context.SaleDetail
            .Include(sd => sd.Product)
            .Include(sd => sd.Sale)
            .ToListAsync();
    }

    public async Task AddSaleDetailAsync(SaleDetail saleDetail)
    {
        await _context.SaleDetail.AddAsync(saleDetail);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateSaleDetailAsync(SaleDetail saleDetail)
    {
        _context.SaleDetail.Update(saleDetail);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteSaleDetailAsync(int id)
    {
        var saleDetail = await _context.SaleDetail.FindAsync(id);

        if (saleDetail != null)
        {
            _context.SaleDetail.Remove(saleDetail);
            await _context.SaveChangesAsync();
        }
    }
}
