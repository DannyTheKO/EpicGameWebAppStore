using DataAccess.EpicGame;
using Domain.Entities;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class CartRepository : ICartRepository
{
    private readonly EpicGameDbContext _context;

    public CartRepository(EpicGameDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Cart>> GetAll()
    {
        return await _context.Carts
            .Include(c => c.Account)
            .Include(c => c.PaymentMethod)
            .Include(c => c.Cartdetails).ThenInclude(cd => cd.Game)
            .ToListAsync();
    }

    public async Task<Cart> GetById(int id)
    {
	    return await _context.Carts
		    .Include(c => c.Account)
		    .Include(c => c.PaymentMethod)
		    .Include(c => c.Cartdetails).ThenInclude(cd => cd.Game)
		    .FirstOrDefaultAsync(c => c.CartId == id);
    }

    public async Task Add(Cart cart)
    {
        await _context.Carts.AddAsync(cart);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Cart cart)
    {
        _context.Carts.Update(cart);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var cart = await _context.Carts.FindAsync(id);
        if (cart != null)
        {
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Cart>> GetAllCartFromAccountId(int accountId)
    {
        return await _context.Carts
            .Where(c => c.AccountId == accountId)
            .Include(c => c.PaymentMethod)
            .Include(cd => cd.Cartdetails)
            .ToListAsync();
    }

    public async Task<Account> GetAccountByCartId(int accountId)
    {
        return await _context.Accounts.FindAsync(accountId);
    }

    public async Task<Paymentmethod> GetPaymentMethodById(int paymentMethodId)
    {
        return await _context.Paymentmethods.FindAsync(paymentMethodId);
    }
}