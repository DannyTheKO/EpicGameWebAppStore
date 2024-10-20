﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Domain
using Domain.Authentication;
using Domain.Entities;

namespace Application.Services
{
    public class AuthenticationServices : IAuthenticationServices
    {
        private readonly IUserRepository _userRepository;

        public AuthenticationServices(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> ValidateUserCredentialAsync(string username, string password)
        {
            var account = await _userRepository.GetUserByUserNameAsync(username);
            return account != null && account.Password == password;
        }

        public async Task<string> GenerateTokenAsync(string username)
        {
            // Implement logic to generate a JWT or other token
            return await Task.FromResult("generated_token");
        }

        public async Task<Account> GetAccountByUserNameAsync(string username)
        {
            return await _userRepository.GetUserByUserNameAsync(username);
        }
    }
}
