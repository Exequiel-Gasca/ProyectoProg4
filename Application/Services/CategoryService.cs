
using Application.DTOs.Requests.Categories;
using Application.DTOs.Responses.Categories;
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

    public async Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync()
    {
        var categories = await _categoryRepository.GetAllCategoriesAsync();

        return categories.Select(MapToResponse);
    }

    public async Task<CategoryResponse?> GetCategoryByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la categoría no es válido.");

        var category = await _categoryRepository.GetCategoryByIdAsync(id);

        if (category == null)
            return null;

        return MapToResponse(category);
    }

    public async Task AddCategoryAsync(CategoryRequest categoryRequest)
    {
        if (categoryRequest == null)
            throw new ArgumentNullException(nameof(categoryRequest));

        if (string.IsNullOrWhiteSpace(categoryRequest.Name))
            throw new ArgumentException("El nombre de la categoría es obligatorio.");

        var category = new Category
        {
            Name = categoryRequest.Name.Trim(),
            Description = categoryRequest.Description?.Trim() ?? string.Empty,
            IsActive = true
        };

        await _categoryRepository.AddCategoryAsync(category);
    }

    public async Task UpdateCategoryAsync(int id, CategoryRequest categoryRequest)
    {
        if (categoryRequest == null)
            throw new ArgumentNullException(nameof(categoryRequest));

        if (id <= 0)
            throw new ArgumentException("El ID de la categoría no es válido.");

        if (string.IsNullOrWhiteSpace(categoryRequest.Name))
            throw new ArgumentException("El nombre de la categoría es obligatorio.");

        var existingCategory =
            await _categoryRepository.GetCategoryByIdAsync(id);

        if (existingCategory == null)
            throw new KeyNotFoundException("No se encontró la categoría.");

        existingCategory.Name = categoryRequest.Name.Trim();
        existingCategory.Description = categoryRequest.Description?.Trim() ?? string.Empty;
        existingCategory.IsActive = categoryRequest.IsActive;

        await _categoryRepository.UpdateCategoryAsync(existingCategory);
    }

    private static CategoryResponse MapToResponse(Category category)
    {
        return new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            IsActive = category.IsActive
        };
    }
}
