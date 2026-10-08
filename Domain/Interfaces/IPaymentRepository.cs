using Domain.Entities;

namespace Domain.Interfaces;

public interface IPaymentRepository
{
    Task<Payment> GetPaymentByIdAsync(int id);
    Task<IEnumerable<Payment>> GetAllPaymentsAsync();
    Task AddPaymentAsync(Payment payment);
    Task UpdatePaymentAsync(Payment payment);
    Task DeletePaymentAsync(int id);
}