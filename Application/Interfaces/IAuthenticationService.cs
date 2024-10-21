﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Domain
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> ValidateUserCredentialAsync(string username, string password);
        Task<string> GenerateTokenAsync(string username);
        Task<Account> GetAccountByUserNameAsync(string username);
    }
}