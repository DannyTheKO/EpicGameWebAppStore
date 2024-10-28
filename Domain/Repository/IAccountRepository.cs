using Domain.Entities;

// Domain

namespace Domain.Repository;

public interface IAccountRepository
{
	#region == Basic CRUD operation ==

	// ACTION: Create User into Database
	Task AddUserAsync(Account account);

	// SELECT: Get all user from Database
	Task<IEnumerable<Account>> GetAllUserAsync();

	// ACTION: Update User From the Database
	Task UpdateUserAsync(Account account);

	// ACTION: Delete User From the Database
	Task DeleteUserAsync(int accountId);

	#endregion


	#region == Function operation ==

	// SELECT: Get User by AccountID From the Database
	Task<Account> GetUserByIdAsync(int accountId);

	// SELECT: Get "Username" value by specific Account
	Task<Account> GetByUsernameAsync(string username);

	// SELECT: Get "Email" value by specific Account
	Task<Account> GetByEmailAsync(string email);

	#endregion
}