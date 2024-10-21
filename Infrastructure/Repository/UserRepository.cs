using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

// Domain
using Domain.Entities;

// Infrastructure
using DataAccess;
using Domain.Repository;


namespace Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly EpicGameDbContext _context;

        public UserRepository(EpicGameDbContext context)
        {
            _context = context;
        }
        
        public async Task<Account> GetUserByUserNameAsync(string username)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Username == username);
        }
    }
}
