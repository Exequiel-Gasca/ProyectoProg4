using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs;

public class StockMovementRequestDto
{
    [Required]
    public int ProductId { get; set; }

    
    [Range(0, float.MaxValue)]
    public float Quantity { get; set; }

    [Required]
    public StockMovementType Type { get; set; }

    [Required, StringLength(200)]
    public string Reason { get; set; } = string.Empty;
}