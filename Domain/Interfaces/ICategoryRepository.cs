using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;

namespace Domain.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task<Category?> GetCategoryByIdAsync(int id);
    Task AddCategoryAsync(Category category);
    Task UpdateCategoryAsync(Category category);
    Task<bool> HasProductsAsync(int id);
    Task DeleteCategoryAsync(int id);
} 