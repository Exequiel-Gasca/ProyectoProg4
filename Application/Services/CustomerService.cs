
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

    public async Task<IEnumerable<Customer>> GetAllCustomerAsync()
    {
        return await _customerRepository.GetAllCustomerAsync();
    }

    public async Task<Customer?> GetCustomerByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del cliente no es válido.");

        return await _customerRepository.GetCustomerByIdAsync(id);
    }

    public async Task AddCustomerAsync(Customer customer)
    {
        if (customer == null)
            throw new ArgumentNullException(nameof(customer));

        if (string.IsNullOrWhiteSpace(customer.FirstName))
            throw new ArgumentException("El nombre es obligatorio.");

        if (string.IsNullOrWhiteSpace(customer.LastName))
            throw new ArgumentException("El apellido es obligatorio.");

        if (string.IsNullOrWhiteSpace(customer.Dni))
            throw new ArgumentException("El DNI es obligatorio.");

        customer.FirstName = customer.FirstName.Trim();
        customer.LastName = customer.LastName.Trim();
        customer.Dni = customer.Dni.Trim();
        customer.Email = customer.Email?.Trim() ?? string.Empty;
        customer.Phone = customer.Phone?.Trim() ?? string.Empty;
        customer.Address = customer.Address?.Trim() ?? string.Empty;
        customer.IsActive = true;

        await _customerRepository.AddCustomerAsync(customer);
    }

    public async Task UpdateCustomerAsync(Customer customer)
    {
        if (customer == null)
            throw new ArgumentNullException(nameof(customer));

        if (customer.Id <= 0)
            throw new ArgumentException("El ID del cliente no es válido.");

        var existingCustomer =
            await _customerRepository.GetCustomerByIdAsync(customer.Id);

        if (existingCustomer == null)
            throw new KeyNotFoundException("No se encontró el cliente.");

        if (string.IsNullOrWhiteSpace(customer.FirstName) ||
            string.IsNullOrWhiteSpace(customer.LastName) ||
            string.IsNullOrWhiteSpace(customer.Dni))
        {
            throw new ArgumentException(
                "Nombre, apellido y DNI son obligatorios.");
        }

        existingCustomer.FirstName = customer.FirstName.Trim();
        existingCustomer.LastName = customer.LastName.Trim();
        existingCustomer.Dni = customer.Dni.Trim();
        existingCustomer.Phone = customer.Phone?.Trim() ?? string.Empty;
        existingCustomer.Email = customer.Email?.Trim() ?? string.Empty;
        existingCustomer.Address = customer.Address?.Trim() ?? string.Empty;
        existingCustomer.IsActive = customer.IsActive;

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
}
