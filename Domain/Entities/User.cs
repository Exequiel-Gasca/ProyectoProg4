namespace Domain.Entities;

public class User
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public int IdRole { get; set; }

    public UserRole Role { get; set; } = null!;

    public ICollection<Sale> Sale { get; set; } = new List<Sale>();

    public ICollection<StockMovement> StockMovement { get; set; } = new List<StockMovement>();
}
