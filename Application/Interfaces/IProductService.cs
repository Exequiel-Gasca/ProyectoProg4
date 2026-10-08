using Domain.Entities;

namespace Application.Interfaces;

public interface IProductService
{
    Task<Product?> GetProductByIdAsync(int id);
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task AddProductAsync(Product product);
    Task UpdateProductAsync(Product product);
    Task<Product?> GetByBarcodeAsync(string barcode);
    Task DeleteProductAsync(int id);
}