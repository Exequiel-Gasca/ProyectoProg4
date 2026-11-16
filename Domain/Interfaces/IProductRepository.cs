using Domain.Entities;

public interface IProductRepository
{
    Task<Product?> GetProductByIdAsync(int id);
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<Product?> GetByBarcodeAsync(string barcode);
    Task AddProductAsync(Product product);
    Task UpdateProductAsync(Product product);
}