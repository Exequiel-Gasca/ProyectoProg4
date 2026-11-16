
using Application.DTOs.Requests.Products;
using Application.DTOs.Responses.Products;

namespace Application.Interfaces;

public interface IProductService
{
    Task<ProductResponse?> GetProductByIdAsync(int id);

    Task<IEnumerable<ProductResponse>> GetAllProductsAsync();

    Task<ProductResponse?> GetByBarcodeAsync(string barcode);

    Task AddProductAsync(ProductRequest productRequest);

    Task UpdateProductAsync(int id, ProductRequest productRequest);

    Task DeleteProductAsync(int id);
}
