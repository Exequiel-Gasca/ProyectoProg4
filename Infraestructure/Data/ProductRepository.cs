using Domain.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _context.Product
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _context.Product
            .ToListAsync();
    }

    public async Task AddProductAsync(Product product)
    {
        await _context.Product.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateProductAsync(Product product)
    {
        _context.Product.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task<Product?> GetByBarcodeAsync(string barcode)
    {
        return await _context.Product
            .FirstOrDefaultAsync(p => p.Barcode == barcode);
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await _context.Product.FindAsync(id);

        if (product != null)
        {
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}