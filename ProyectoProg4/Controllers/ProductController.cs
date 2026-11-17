
using Application.DTOs.Requests.Products;
using Application.DTOs.Responses.Products;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoProg4.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetAll()
    {
        var products = await _productService.GetAllProductsAsync();

        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductResponseDto>> GetById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpGet("barcode/{barcode}")]
    public async Task<ActionResult<ProductResponseDto>> GetByBarcode(string barcode)
    {
        var product = await _productService.GetByBarcodeAsync(barcode);

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductRequestDto productRequest)
    {
        await _productService.AddProductAsync(productRequest);

        return Ok("Producto creado correctamente.");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ProductRequestDto productRequest)
    {
        await _productService.UpdateProductAsync(id, productRequest);

        return Ok("Producto actualizado correctamente.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _productService.DeleteProductAsync(id);

        return Ok("Producto eliminado correctamente.");
    }
}
