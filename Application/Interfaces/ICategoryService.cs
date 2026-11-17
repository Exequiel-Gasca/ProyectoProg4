
using Application.DTOs.Requests.Categories;
using Application.DTOs.Responses.Categories;

namespace Application.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync();

    Task<CategoryResponseDto?> GetCategoryByIdAsync(int id);

    Task AddCategoryAsync(CategoryRequestDto categoryRequest);

    Task UpdateCategoryAsync(int id, CategoryRequestDto categoryRequest);

    Task DeactivateCategoryAsync(int id);
    Task DeleteCategoryAsync(int id);
}
