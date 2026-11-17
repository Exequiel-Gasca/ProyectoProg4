
using Application.DTOs.Requests.Customers;
using Application.DTOs.Responses.Customers;

namespace Application.Interfaces;

public interface ICustomerService
{
    Task<IEnumerable<CustomerResponse>> GetAllCustomerAsync();

    Task<CustomerResponse?> GetCustomerByIdAsync(int id);

    Task AddCustomerAsync(CustomerRequest customerRequest);

    Task UpdateCustomerAsync(int id, CustomerRequest customerRequest);

    Task DeleteCustomerAsync(int id);
}
