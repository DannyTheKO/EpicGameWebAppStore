using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Domain
using Application.Interfaces;
using Domain.Entities;
using Domain.Repository;

namespace Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;

        public AuthenticationService(IUserRepository userRepository)
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
            // TODO: Implement logic to generate a JWT or other token
            return await Task.FromResult("generated_token");
        }

        public async Task<Account> GetAccountByUserNameAsync(string username)
        {
            return await _userRepository.GetUserByUserNameAsync(username);
        }
    }
}
