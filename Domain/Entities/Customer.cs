using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities;

public class Customer
{
    public int IdCustomer { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Dni { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public ICollection<Sale> Sales { get; set; } = new List<Sale>();
}