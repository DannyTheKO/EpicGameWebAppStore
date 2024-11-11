using DataAccess.EpicGame;
using Domain.Entities;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class PaymentMethodRepository : IPaymentMethodRepository
{
    private readonly EpicGameDbContext _context;

    public PaymentMethodRepository(EpicGameDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Paymentmethod>> GetAll()
    {
        return await _context.Paymentmethods.ToListAsync();
    }

    public async Task<Paymentmethod> GetById(int id)
    {
        return await _context.Paymentmethods.FindAsync(id);
    }

    public async Task Add(Paymentmethod paymentMethod)
    {
        await _context.Paymentmethods.AddAsync(paymentMethod);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Paymentmethod paymentMethod)
    {
        _context.Paymentmethods.Update(paymentMethod);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var paymentMethod = await _context.Paymentmethods.FindAsync(id);
        if (paymentMethod != null)
        {
            _context.Paymentmethods.Remove(paymentMethod);
            await _context.SaveChangesAsync();
        }
    }
}