using Domain.Entities;

// Domain

namespace Domain.Repository;

public interface IAccountRepository
{
	#region == Basic CRUD operation ==

	// ACTION: Create User into Database
	Task Add(Account account);

	// SELECT: Get all user from Database
	Task<IEnumerable<Account>> GetAll();

	// ACTION: Update User From the Database
	Task Update(Account account);

	// ACTION: Delete User From the Database
	Task Delete(int accountId);

	#endregion

	#region == Function operation ==

	// SELECT: Get User by AccountID From the Database
	Task<Account> GetId(int accountId);

	// SELECT: Get "Username" value by specific Account
	Task<Account> GetUsername(string username);

	// SELECT: Get "Email" value by specific Account
	Task<Account> GetEmail(string email);

	// SELECT: Get specific user role by Account id
	Task<string> GetUserRoleAsync(int accountId);

	#endregion
}