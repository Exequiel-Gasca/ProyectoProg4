
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

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del producto no es válido.");

        return await _productRepository.GetProductByIdAsync(id);
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _productRepository.GetAllProductsAsync();
    }

    public async Task<Product?> GetByBarcodeAsync(string barcode)
    {
        if (string.IsNullOrWhiteSpace(barcode))
            throw new ArgumentException("El código de barras es obligatorio.");

        return await _productRepository.GetByBarcodeAsync(barcode.Trim());
    }

    public async Task AddProductAsync(Product product)
    {
        if (product == null)
            throw new ArgumentNullException(nameof(product));

        if (string.IsNullOrWhiteSpace(product.Name))
            throw new ArgumentException("El nombre del producto es obligatorio.");

        if (string.IsNullOrWhiteSpace(product.Barcode))
            throw new ArgumentException("El código de barras es obligatorio.");

        if (product.SalePrice <= 0)
            throw new ArgumentException("El precio de venta debe ser mayor a cero.");

        if (product.Cost < 0)
            throw new ArgumentException("El costo no puede ser negativo.");

        var existingProduct =
            await _productRepository.GetByBarcodeAsync(product.Barcode.Trim());

        if (existingProduct != null)
            throw new InvalidOperationException(
                "Ya existe un producto con ese código de barras.");

        var category =
            await _categoryRepository.GetCategoryByIdAsync(product.CategoryId);

        if (category == null || !category.IsActive)
            throw new KeyNotFoundException(
                "La categoría no existe o está inactiva.");

        product.Name = product.Name.Trim();
        product.Barcode = product.Barcode.Trim();
        product.Description = product.Description?.Trim() ?? string.Empty;
        product.Category = category;
        product.IsActive = true;

        await _productRepository.AddProductAsync(product);
    }

    public async Task UpdateProductAsync(Product product)
    {
        if (product == null)
            throw new ArgumentNullException(nameof(product));

        if (product.Id <= 0)
            throw new ArgumentException("El ID del producto no es válido.");

        var existingProduct =
            await _productRepository.GetProductByIdAsync(product.Id);

        if (existingProduct == null)
            throw new KeyNotFoundException("No se encontró el producto.");

        if (string.IsNullOrWhiteSpace(product.Name) ||
            string.IsNullOrWhiteSpace(product.Barcode))
        {
            throw new ArgumentException(
                "El nombre y el código de barras son obligatorios.");
        }

        if (product.SalePrice <= 0)
            throw new ArgumentException("El precio de venta debe ser mayor a cero.");

        if (product.Cost < 0 || product.MinimumStock < 0)
            throw new ArgumentException("El costo y el stock mínimo no pueden ser negativos.");

        var barcodeProduct =
            await _productRepository.GetByBarcodeAsync(product.Barcode.Trim());

        if (barcodeProduct != null && barcodeProduct.Id != product.Id)
            throw new InvalidOperationException(
                "Otro producto ya utiliza ese código de barras.");

        var category =
            await _categoryRepository.GetCategoryByIdAsync(product.CategoryId);

        if (category == null || !category.IsActive)
            throw new KeyNotFoundException(
                "La categoría no existe o está inactiva.");

        existingProduct.Name = product.Name.Trim();
        existingProduct.Barcode = product.Barcode.Trim();
        existingProduct.Description = product.Description?.Trim() ?? string.Empty;
        existingProduct.SalePrice = product.SalePrice;
        existingProduct.Cost = product.Cost;
        existingProduct.MinimumStock = product.MinimumStock;
        existingProduct.MeasurementUnit = product.MeasurementUnit;
        existingProduct.IsActive = product.IsActive;
        existingProduct.CategoryId = category.Id;
        existingProduct.Category = category;

        await _productRepository.UpdateProductAsync(existingProduct);
    }

    public async Task DeactivateProductAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del producto no es válido.");

        var product = await _productRepository.GetProductByIdAsync(id);

        if (product == null)
            throw new KeyNotFoundException("No se encontró el producto.");

        if (!product.IsActive)
            throw new InvalidOperationException("El producto ya está inactivo.");

        product.IsActive = false;

        await _productRepository.UpdateProductAsync(product);
    }
}