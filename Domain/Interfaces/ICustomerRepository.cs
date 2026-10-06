using Domain.Entities;

namespace Application.Interfaces;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetAllCustomerAsync();
    Task<Customer?> GetCustomerByIdAsync(int id);
    Task AddCustomerAsync(Customer customer);
    Task UpdateCustomerAsync(Customer customer);
    Task DeleteCustomerAsync(int id);
}