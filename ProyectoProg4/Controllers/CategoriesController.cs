
using Application.DTOs.Requests.Categories;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoProg4.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // GET: api/Categories
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        return Ok(categories);
    }

    // GET: api/Categories/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);

        if (category == null)
            return NotFound();

        return Ok(category);
    }

    // POST: api/Categories
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CategoryRequestDto categoryRequest)
    {
        try
        {
            await _categoryService.AddCategoryAsync(categoryRequest);

            return Ok("Categoría creada correctamente.");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // PUT: api/Categories/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] CategoryRequestDto categoryRequest)
    {
        try
        {
            await _categoryService.UpdateCategoryAsync(id, categoryRequest);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
