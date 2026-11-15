
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly ISaleRepository _saleRepository;

    public PaymentService(
        IPaymentRepository paymentRepository,
        ISaleRepository saleRepository)
    {
        _paymentRepository = paymentRepository;
        _saleRepository = saleRepository;
    }

    public async Task<Payment?> GetPaymentByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del pago no es válido.");

        return await _paymentRepository.GetPaymentByIdAsync(id);
    }

    public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
    {
        return await _paymentRepository.GetAllPaymentsAsync();
    }

    public async Task AddPaymentAsync(Payment payment)
    {
        if (payment == null)
            throw new ArgumentNullException(nameof(payment));

        if (payment.Amount <= 0)
            throw new ArgumentException("El importe debe ser mayor a cero.");

        if (payment.SaleId <= 0)
            throw new ArgumentException("El pago debe estar asociado a una venta.");

        var sale = await _saleRepository.GetSaleByIdAsync(payment.SaleId);

        if (sale == null)
            throw new KeyNotFoundException("No se encontró la venta asociada.");

        payment.Sale = sale;
        payment.Date = DateTime.Now;

        await _paymentRepository.AddPaymentAsync(payment);
    }

    public async Task UpdatePaymentAsync(Payment payment)
    {
        if (payment == null)
            throw new ArgumentNullException(nameof(payment));

        if (payment.Id <= 0)
            throw new ArgumentException("El ID del pago no es válido.");

        var existingPayment =
            await _paymentRepository.GetPaymentByIdAsync(payment.Id);

        if (existingPayment == null)
            throw new KeyNotFoundException("No se encontró el pago.");

        if (payment.Amount <= 0)
            throw new ArgumentException("El importe debe ser mayor a cero.");

        existingPayment.Amount = payment.Amount;
        existingPayment.Status = payment.Status;
        existingPayment.PaymentMethod = payment.PaymentMethod;

        await _paymentRepository.UpdatePaymentAsync(existingPayment);
    }

    public async Task DeletePaymentAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del pago no es válido.");

        var payment = await _paymentRepository.GetPaymentByIdAsync(id);

        if (payment == null)
            throw new KeyNotFoundException("No se encontró el pago.");

        await _paymentRepository.DeletePaymentAsync(id);
    }
}
