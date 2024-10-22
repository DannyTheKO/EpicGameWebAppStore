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

		// == Basic CRUD operation ==

        // Create User into Database
        Task AddUserAsync(Account account);

		// TODO: Read User From the Database

		// TODO: Update User From the Database

		// TODO: Delete User From the Database


		// == Function operation ==

		// Get "Username" value by specific Account
		Task<Account> GetByUsernameAsync(string username);

		// Get "Email" value by specific Account
		Task<Account> GetByEmailAsync(string email);

    }
}
