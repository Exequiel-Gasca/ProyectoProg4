
namespace Application.DTOs.Requests.Categories;

public class CategoryRequestDto
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}
