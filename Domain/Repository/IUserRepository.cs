using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// Domain
using Domain.Entities;

namespace Domain.Repository
{
    public interface IUserRepository
    {
        Task<Account> GetUserByUserNameAsync(string username);
    }
}
