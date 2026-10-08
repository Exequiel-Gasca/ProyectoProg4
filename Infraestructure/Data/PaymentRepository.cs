using Domain.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly ApplicationDbContext _context;

    public PaymentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Payment> GetPaymentByIdAsync(int id)
    {
        return await _context.Payment
            .Include(p => p.Sale)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
    {
        return await _context.Payment
            .Include(p => p.Sale)
            .ToListAsync();
    }

    public async Task AddPaymentAsync(Payment payment)
    {
        await _context.Payment.AddAsync(payment);
        await _context.SaveChangesAsync();
    }

    public async Task UpdatePaymentAsync(Payment payment)
    {
        _context.Payment.Update(payment);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePaymentAsync(int id)
    {
        var payment = await _context.Payment.FindAsync(id);

        if (payment != null)
        {
            _context.Payment.Remove(payment);
            await _context.SaveChangesAsync();
        }
    }
}