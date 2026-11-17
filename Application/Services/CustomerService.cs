
using Application.DTOs.Requests.Customers;
using Application.DTOs.Responses.Customers;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<IEnumerable<CustomerResponseDto>> GetAllCustomerAsync()
    {
        var customers = await _customerRepository.GetAllCustomerAsync();

        return customers.Select(MapToResponse);
    }

    public async Task<CustomerResponseDto?> GetCustomerByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del cliente no es válido.");

        var customer = await _customerRepository.GetCustomerByIdAsync(id);

        if (customer == null)
            return null;

        return MapToResponse(customer);
    }

    public async Task AddCustomerAsync(CustomerRequestDto customerRequest)
    {
        if (customerRequest == null)
            throw new ArgumentNullException(nameof(customerRequest));

        if (string.IsNullOrWhiteSpace(customerRequest.FirstName))
            throw new ArgumentException("El nombre es obligatorio.");

        if (string.IsNullOrWhiteSpace(customerRequest.LastName))
            throw new ArgumentException("El apellido es obligatorio.");

        if (string.IsNullOrWhiteSpace(customerRequest.Dni))
            throw new ArgumentException("El DNI es obligatorio.");

        var customer = new Customer
        {
            FirstName = customerRequest.FirstName.Trim(),
            LastName = customerRequest.LastName.Trim(),
            Dni = customerRequest.Dni.Trim(),
            Phone = customerRequest.Phone?.Trim() ?? string.Empty,
            Email = customerRequest.Email?.Trim() ?? string.Empty,
            Address = customerRequest.Address?.Trim() ?? string.Empty,
            IsActive = true
        };

        await _customerRepository.AddCustomerAsync(customer);
    }

    public async Task UpdateCustomerAsync(int id, CustomerRequestDto customerRequest)
    {
        if (customerRequest == null)
            throw new ArgumentNullException(nameof(customerRequest));

        if (id <= 0)
            throw new ArgumentException("El ID del cliente no es válido.");

        if (string.IsNullOrWhiteSpace(customerRequest.FirstName) ||
            string.IsNullOrWhiteSpace(customerRequest.LastName) ||
            string.IsNullOrWhiteSpace(customerRequest.Dni))
        {
            throw new ArgumentException(
                "Nombre, apellido y DNI son obligatorios.");
        }

        var existingCustomer =
            await _customerRepository.GetCustomerByIdAsync(id);

        if (existingCustomer == null)
            throw new KeyNotFoundException("No se encontró el cliente.");

        existingCustomer.FirstName = customerRequest.FirstName.Trim();
        existingCustomer.LastName = customerRequest.LastName.Trim();
        existingCustomer.Dni = customerRequest.Dni.Trim();
        existingCustomer.Phone = customerRequest.Phone?.Trim() ?? string.Empty;
        existingCustomer.Email = customerRequest.Email?.Trim() ?? string.Empty;
        existingCustomer.Address = customerRequest.Address?.Trim() ?? string.Empty;
        existingCustomer.IsActive = customerRequest.IsActive;

        await _customerRepository.UpdateCustomerAsync(existingCustomer);
    }

    public async Task DeleteCustomerAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del cliente no es válido.");

        var customer = await _customerRepository.GetCustomerByIdAsync(id);

        if (customer == null)
            throw new KeyNotFoundException("No se encontró el cliente.");

        await _customerRepository.DeleteCustomerAsync(id);
    }

    private static CustomerResponseDto MapToResponse(Customer customer)
    {
        return new CustomerResponseDto
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Dni = customer.Dni,
            Phone = customer.Phone,
            Email = customer.Email,
            Address = customer.Address,
            IsActive = customer.IsActive
        };
    }
}
