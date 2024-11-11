using DataAccess.EpicGame;
using Domain.Entities;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class CartdetailRepository : ICartdetailRepository
{
    private readonly EpicGameDbContext _context;

    public CartdetailRepository(EpicGameDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Cartdetail>> GetAll()
    {
        return await _context.Cartdetails
            .Include(cd => cd.Cart)
            .Include(cd => cd.Game)
            .ToListAsync();
    }

    public async Task<Cartdetail> GetById(int id)
    {
        return await _context.Cartdetails
            .Include(cd => cd.Cart)
            .Include(cd => cd.Game)
            .FirstOrDefaultAsync(cd => cd.CartDetailId == id);
    }

    public async Task Create(Cartdetail cartdetail)
    {
        await _context.Cartdetails.AddAsync(cartdetail);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Cartdetail cartdetail)
    {
        _context.Cartdetails.Update(cartdetail);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var cartdetail = await _context.Cartdetails.FindAsync(id);
        if (cartdetail != null)
        {
            _context.Cartdetails.Remove(cartdetail);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Cartdetail>> GetByCartId(int cartId)
    {
        return await _context.Cartdetails
            .Where(cd => cd.CartId == cartId)
            .Include(cd => cd.Game)
            .ToListAsync();
    }

    public async Task Add(Cartdetail cartdetail)
    {
        await _context.Cartdetails.AddAsync(cartdetail);
        await _context.SaveChangesAsync();
    }
}