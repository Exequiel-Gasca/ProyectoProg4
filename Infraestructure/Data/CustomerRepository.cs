using Domain.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _context;

    public CustomerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Customer>> GetAllCustomerAsync()
    {
        return await _context.Customer
            .ToListAsync();
    }

    public async Task<Customer?> GetCustomerByIdAsync(int id)
    {
        return await _context.Customer
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddCustomerAsync(Customer customer)
    {
        await _context.Customer.AddAsync(customer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCustomerAsync(Customer customer)
    {
        _context.Customer.Update(customer);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCustomerAsync(int id)
    {
        var customer = await _context.Customer.FindAsync(id);

        if (customer != null)
        {
            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();
        }
    }
}