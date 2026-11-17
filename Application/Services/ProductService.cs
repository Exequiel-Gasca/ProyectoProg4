
using Application.DTOs.Requests.Products;
using Application.DTOs.Responses.Products;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductService(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<ProductResponseDto?> GetProductByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del producto no es válido.");

        var product = await _productRepository.GetProductByIdAsync(id);

        if (product == null)
            return null;

        return MapToResponse(product);
    }

    public async Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllProductsAsync();

        return products.Select(MapToResponse);
    }

    public async Task<ProductResponseDto?> GetByBarcodeAsync(string barcode)
    {
        if (string.IsNullOrWhiteSpace(barcode))
            throw new ArgumentException("El código de barras es obligatorio.");

        var product = await _productRepository.GetByBarcodeAsync(barcode.Trim());

        if (product == null)
            return null;

        return MapToResponse(product);
    }

    public async Task AddProductAsync(ProductRequestDto productRequest)
    {
        if (productRequest == null)
            throw new ArgumentNullException(nameof(productRequest));

        if (string.IsNullOrWhiteSpace(productRequest.Name))
            throw new ArgumentException("El nombre del producto es obligatorio.");

        if (string.IsNullOrWhiteSpace(productRequest.Barcode))
            throw new ArgumentException("El código de barras es obligatorio.");

        if (productRequest.SalePrice < 0 || productRequest.Cost < 0)
            throw new ArgumentException("Los precios no pueden ser negativos.");

        if (productRequest.CurrentStock < 0 || productRequest.MinimumStock < 0)
            throw new ArgumentException("El stock no puede ser negativo.");

        var existingProduct =
            await _productRepository.GetByBarcodeAsync(productRequest.Barcode.Trim());

        if (existingProduct != null)
            throw new InvalidOperationException(
                "Ya existe un producto con ese código de barras.");

        var category =
            await _categoryRepository.GetCategoryByIdAsync(productRequest.CategoryId);

        if (category == null || !category.IsActive)
            throw new KeyNotFoundException(
                "La categoría no existe o está inactiva.");

        var product = new Product
        {
            Name = productRequest.Name.Trim(),
            Barcode = productRequest.Barcode.Trim(),
            Description = productRequest.Description?.Trim() ?? string.Empty,
            SalePrice = productRequest.SalePrice,
            Cost = productRequest.Cost,
            CurrentStock = productRequest.CurrentStock,
            MinimumStock = productRequest.MinimumStock,
            MeasurementUnit = productRequest.MeasurementUnit,
            CategoryId = category.Id,
            Category = category,
            IsActive = true
        };

        await _productRepository.AddProductAsync(product);
    }

    public async Task UpdateProductAsync(int id, ProductRequestDto productRequest)
    {
        if (productRequest == null)
            throw new ArgumentNullException(nameof(productRequest));

        if (id <= 0)
            throw new ArgumentException("El ID del producto no es válido.");

        var existingProduct =
            await _productRepository.GetProductByIdAsync(id);

        if (existingProduct == null)
            throw new KeyNotFoundException("No se encontró el producto.");

        if (string.IsNullOrWhiteSpace(productRequest.Name) ||
            string.IsNullOrWhiteSpace(productRequest.Barcode))
        {
            throw new ArgumentException(
                "El nombre y el código de barras son obligatorios.");
        }

        if (productRequest.SalePrice < 0 || productRequest.Cost < 0 ||
            productRequest.CurrentStock < 0 || productRequest.MinimumStock < 0)
        {
            throw new ArgumentException(
                "Los precios y el stock no pueden ser negativos.");
        }

        var barcodeProduct =
            await _productRepository.GetByBarcodeAsync(productRequest.Barcode.Trim());

        if (barcodeProduct != null && barcodeProduct.Id != id)
            throw new InvalidOperationException(
                "Otro producto ya utiliza ese código de barras.");

        var category =
            await _categoryRepository.GetCategoryByIdAsync(productRequest.CategoryId);

        if (category == null || !category.IsActive)
            throw new KeyNotFoundException(
                "La categoría no existe o está inactiva.");

        existingProduct.Name = productRequest.Name.Trim();
        existingProduct.Barcode = productRequest.Barcode.Trim();
        existingProduct.Description = productRequest.Description?.Trim() ?? string.Empty;
        existingProduct.SalePrice = productRequest.SalePrice;
        existingProduct.Cost = productRequest.Cost;
        existingProduct.CurrentStock = productRequest.CurrentStock;
        existingProduct.MinimumStock = productRequest.MinimumStock;
        existingProduct.MeasurementUnit = productRequest.MeasurementUnit;
        existingProduct.IsActive = productRequest.IsActive;
        existingProduct.CategoryId = category.Id;
        existingProduct.Category = category;

        await _productRepository.UpdateProductAsync(existingProduct);
    }

    public async Task DeleteProductAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del producto no es válido.");

        var product = await _productRepository.GetProductByIdAsync(id);

        if (product == null)
            throw new KeyNotFoundException("No se encontró el producto.");

        await _productRepository.DeleteProductAsync(id);
    }

    private static ProductResponseDto MapToResponse(Product product)
    {
        return new ProductResponseDto
        {
            Id = product.Id,
            Barcode = product.Barcode,
            Name = product.Name,
            Description = product.Description,
            SalePrice = product.SalePrice,
            Cost = product.Cost,
            CurrentStock = product.CurrentStock,
            MinimumStock = product.MinimumStock,
            MeasurementUnit = product.MeasurementUnit,
            IsActive = product.IsActive,
            CategoryId = product.CategoryId
        };
    }
}
