using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities;

public class UserRole
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public ICollection<User> Users { get; set; } = new List<User>();
}