
using Application.DTOs.Requests.Categories;
using Application.DTOs.Responses.Categories;

namespace Application.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync();

    Task<CategoryResponse?> GetCategoryByIdAsync(int id);

    Task AddCategoryAsync(CategoryRequest categoryRequest);

    Task UpdateCategoryAsync(int id, CategoryRequest categoryRequest);
}
