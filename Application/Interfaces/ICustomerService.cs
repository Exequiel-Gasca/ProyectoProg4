
using Application.DTOs.Requests.Customers;
using Application.DTOs.Responses.Customers;

namespace Application.Interfaces;

public interface ICustomerService
{
    Task<IEnumerable<CustomerResponseDto>> GetAllCustomerAsync();

    Task<CustomerResponseDto?> GetCustomerByIdAsync(int id);

    Task AddCustomerAsync(CustomerRequestDto customerRequest);

    Task UpdateCustomerAsync(int id, CustomerRequestDto customerRequest);

    Task DeleteCustomerAsync(int id);
}
