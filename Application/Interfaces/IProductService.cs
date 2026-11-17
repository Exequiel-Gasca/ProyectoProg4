
using Application.DTOs.Requests.Products;
using Application.DTOs.Responses.Products;

namespace Application.Interfaces;

public interface IProductService
{
    Task<ProductResponseDto?> GetProductByIdAsync(int id);

    Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync();

    Task<ProductResponseDto?> GetByBarcodeAsync(string barcode);

    Task<ProductResponseDto> AddProductAsync(ProductRequestDto productRequest);
    Task DeactivateProductAsync(int id);

    Task UpdateProductAsync(int id, ProductRequestDto productRequest);
}
