﻿using System;
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

		// == Basic CRUD operation ==

		// Create User into Database
		public async Task AddUserAsync(Account account)
        {
	        _context.Accounts.Add(account);
	        await _context.SaveChangesAsync();
        }

		// TODO: Read User From the Database

		// TODO: Update User From the Database

		// TODO: Delete User From the Database


		// == Function operation ==

		// Get "Username" from the Account table in the Database
		public async Task<Account> GetByUsernameAsync(string username)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Username == username);
        }

		// Get "Email" from the Account table in the Database
		public async Task<Account> GetByEmailAsync(string email)
        {
	        return await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);
        }

    }
}
