
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await _categoryRepository.GetAllCategoriesAsync();
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la categoría no es válido.");

        return await _categoryRepository.GetCategoryByIdAsync(id);
    }

    public async Task AddCategoryAsync(Category category)
    {
        if (category == null)
            throw new ArgumentNullException(nameof(category));

        if (string.IsNullOrWhiteSpace(category.Name))
            throw new ArgumentException("El nombre de la categoría es obligatorio.");

        category.Name = category.Name.Trim();
        category.Description = category.Description?.Trim() ?? string.Empty;
        category.IsActive = true;

        await _categoryRepository.AddCategoryAsync(category);
    }

    public async Task UpdateCategoryAsync(Category category)
    {
        if (category == null)
            throw new ArgumentNullException(nameof(category));

        if (category.Id <= 0)
            throw new ArgumentException("El ID de la categoría no es válido.");

        if (string.IsNullOrWhiteSpace(category.Name))
            throw new ArgumentException("El nombre de la categoría es obligatorio.");

        var existingCategory =
            await _categoryRepository.GetCategoryByIdAsync(category.Id);

        if (existingCategory == null)
            throw new KeyNotFoundException("No se encontró la categoría.");

        existingCategory.Name = category.Name.Trim();
        existingCategory.Description = category.Description?.Trim() ?? string.Empty;
        existingCategory.IsActive = category.IsActive;

        await _categoryRepository.UpdateCategoryAsync(existingCategory);
    }
}
