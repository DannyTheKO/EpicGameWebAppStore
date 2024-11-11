using System.Security.Claims;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repository;

namespace Application.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    #region == Basic CRUB Function ==

    public async Task<Account> AddAccount(Account account)
    {
        await _accountRepository.Add(account);
        return account;
    }

    // SELECT: Get All User
    public async Task<IEnumerable<Account>> GetAllAccounts()
    {
        return await _accountRepository.GetAll();
    }

    // ACTION: Update User
    public async Task<Account> UpdateAccount(Account account)
    {
        if (account == null) // NOT FOUND!
            throw new ArgumentNullException(nameof(account), "Account cannot be null");

        // Retrieve the existing account from the database
        var existingAccount = await _accountRepository.GetId(account.AccountId);
        if (existingAccount == null) throw new Exception("Account not found");

        // Update the account details
        existingAccount.Username = account.Username;
        existingAccount.Role.RoleId = account.Role.RoleId;
        existingAccount.IsActive = account.IsActive;
        existingAccount.Password = account.Password; // Consider hashing the password
        existingAccount.Email = account.Email;

        // Save the updated account to the database
        await _accountRepository.Update(existingAccount);

        return existingAccount;
    }

    // ACTION: Delete User
    public async Task DeleteAccount(int accountId)
    {
        var account = await _accountRepository.GetId(accountId);
        if (account != null) // FOUND!
            await _accountRepository.Delete(accountId);
    }

    #endregion

    #region == Select operation ==

    // SELECT: Get Account by ID
    public async Task<Account> GetAccountById(int accountId)
    {
        return await _accountRepository.GetId(accountId);
    }

    // SELECT: Get Account by Username
    public async Task<Account> GetAccountByUsername(string username)
    {
        return await _accountRepository.GetUsername(username);
    }

    // SELECT: Get Account by Email
    public async Task<Account> GetAccountByEmail(string email)
    {
        return await _accountRepository.GetEmail(email);
    }

    // SELECT: Get current login in Account
    public int GetLoginAccountId(ClaimsPrincipal User)
    {
        return int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "-1");
    }

    #endregion
}