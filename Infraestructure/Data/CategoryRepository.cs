using Domain.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await _context.Category
            .ToListAsync();
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        return await _context.Category
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddCategoryAsync(Category category)
    {
        await _context.Category.AddAsync(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCategoryAsync(Category category)
    {
        _context.Category.Update(category);
        await _context.SaveChangesAsync();
    }
    public async Task<bool> HasProductsAsync(int id)
    {
        return await _context.Product.AnyAsync(p => p.CategoryId == id);
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var category = await _context.Category.FindAsync(id);

        if (category != null)
        {
            _context.Category.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}