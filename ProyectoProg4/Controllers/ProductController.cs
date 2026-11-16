using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoProg4.Controllers;

[Authorize]
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
    public async Task<ActionResult<List<ProductDto>>> Get()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(products.Select(ToDto).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDto>> GetById(int id)
    {
        try
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
                return NotFound(new { error = $"No se encontró el producto con ID {id}." });

            return Ok(ToDto(product));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("barcode/{barcode}")]
    public async Task<ActionResult<ProductDto>> GetByBarcode(string barcode)
    {
        try
        {
            var product = await _productService.GetByBarcodeAsync(barcode);

            if (product == null)
                return NotFound(new { error = "No se encontró un producto con ese código de barras." });

            return Ok(ToDto(product));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [Authorize(Roles = "Admin,SuperAdmin")]
    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create(CreateProductDto dto)
    {
        try
        {
            var product = new Product
            {
                Barcode = dto.Barcode,
                Name = dto.Name,
                Description = dto.Description,
                SalePrice = dto.SalePrice,
                Cost = dto.Cost,
                CurrentStock = dto.CurrentStock,
                MeasurementUnit = dto.MeasurementUnit,
                MinimumStock = dto.MinimumStock,
                CategoryId = dto.CategoryId
            };

            await _productService.AddProductAsync(product);

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, ToDto(product));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [Authorize(Roles = "Admin,SuperAdmin")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateProductDto dto)
    {
        try
        {
            var product = new Product
            {
                Id = id,
                Barcode = dto.Barcode,
                Name = dto.Name,
                Description = dto.Description,
                SalePrice = dto.SalePrice,
                Cost = dto.Cost,
                MeasurementUnit = dto.MeasurementUnit,
                MinimumStock = dto.MinimumStock,
                IsActive = dto.IsActive,
                CategoryId = dto.CategoryId
            };

            await _productService.UpdateProductAsync(product);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [Authorize(Roles = "Admin,SuperAdmin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Deactivate(int id)
    {
        try
        {
            await _productService.DeactivateProductAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    private static ProductDto ToDto(Product p) => new()
    {
        Id = p.Id,
        Barcode = p.Barcode,
        Name = p.Name,
        Description = p.Description,
        SalePrice = p.SalePrice,
        CurrentStock = p.CurrentStock,
        MeasurementUnit = p.MeasurementUnit,
        MinimumStock = p.MinimumStock,
        IsActive = p.IsActive,
        CategoryId = p.CategoryId
    };
}