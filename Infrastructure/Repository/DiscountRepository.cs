using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

// Domain
using Domain.Entities;
using Domain.Repository;

// Infrastructure
using Infrastructure.DataAccess;

namespace Infrastructure.Repository
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly EpicGameDbContext _context;

        public DiscountRepository(EpicGameDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Discount>> GetAll()
        {
            return await _context.Discounts.Include(d => d.Game).ToListAsync();
        }

        public async Task<Discount> GetById(int id)
        {
            return await _context.Discounts.Include(d => d.Game).FirstOrDefaultAsync(d => d.DiscountId == id);
        }

        public async Task Add(Discount discount)
        {
            await _context.Discounts.AddAsync(discount);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Discount discount)
        {
            _context.Discounts.Update(discount);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var discount = await _context.Discounts.FindAsync(id);
            if (discount != null)
            {
                _context.Discounts.Remove(discount);
                await _context.SaveChangesAsync();
            }
        }

        // Thêm phương thức này nếu bạn muốn lấy các giảm giá theo GameId
        public async Task<IEnumerable<Discount>> GetByGameId(int gameId)
        {
            return await _context.Discounts
                .Where(d => d.GameId == gameId)
                .Include(d => d.Game)
                .ToListAsync();
        }
    }
}
