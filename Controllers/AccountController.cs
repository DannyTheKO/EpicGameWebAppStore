using Application.Interfaces;
using Domain.Entities;
using EpicGameWebAppStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace EpicGameWebAppStore.Controllers;

[Route("[controller]")]
[ApiController]
public class AccountController : Controller
{
	private readonly IAccountService _accountService;
	private readonly IRoleService _roleService;

	public AccountController(IAccountService accountService, IRoleService roleService)
	{
		_accountService = accountService;
		_roleService = roleService;
	}

	// GET: Get all account
	[HttpGet("GetAll")]
	public async Task<ActionResult<IEnumerable<Account>>> GetAll()
	{
		var accountList = await _accountService.GetAllAccounts();
		return Ok(accountList);
	}

	// GET: Get account by accountID
	[HttpGet("GetById/{accountId}")]
	public async Task<ActionResult<Account>> GetAccountById(int accountId)
	{
		// Get Account by accountId
		var account = await _accountService.GetAccountById(accountId);

		// Check if that account is valid
		if (account == null)
		{
			return BadRequest(new
			{
				success = false,
				message = "Request Account is not available"
			});
		}

		return Ok(new
		{
			success = true,
			message = "Account Found", 
			account,
		});
	}

	// PUT: Update specific account base on accountID
	[HttpPut("UpdateAccount/{accountId}")]
	public async Task<ActionResult> UpdateAccount(int accountId, [FromBody] AccountFormModel accountFormModel)
	{
		// Get Account by accountId
		var checkAccount = await _accountService.GetAccountById(accountId);
		if (checkAccount == null) // if that account is empty
		{
			return BadRequest(new
			{
				sucess = false,
				message = "Requested Account do not existed!"
			});
		}

		// Check if the user input is valid
		if (!ModelState.IsValid)
		{
			return BadRequest(new
			{
				success = false,
				errors = ModelState.Values
					.SelectMany(v => v.Errors)
					.Select(e => e.ErrorMessage)
			});
		}

		// Check if username is already existed
		var checkUsername = await _accountService.GetAccountByUsername(accountFormModel.Username);
		if (checkUsername != null && checkUsername.AccountId != accountId)
		{
			return BadRequest(new
			{
				success = false,
				message = "Username is already existed"
			});
		}

		// Check if email is already existed
		var checkEmail = await _accountService.GetAccountByEmail(accountFormModel.Email);
		if (checkEmail != null && checkEmail.AccountId != accountId)
		{
			return BadRequest(new
			{
				success = false,
				message = "Email is already existed"
			});
		}

		// Check if that RoleId is exist
		var checkRole = await _roleService.GetRoleById(accountFormModel.RoleId);
		if (checkRole == null)
		{
			return NotFound(new
			{
				success = false,
				message = "Role not found"
			});
		}

		// Create a new account
		var account = new Account
		{
			AccountId = accountId,
			Username = accountFormModel.Username,
			Password = accountFormModel.Password,
			Email = accountFormModel.Email,
			RoleId = accountFormModel.RoleId,
			Role = await _roleService.GetRoleById(accountFormModel.RoleId),
			IsActive = accountFormModel.IsActive,
			CreatedOn = DateTime.UtcNow
		};

		await _accountService.UpdateAccount(account);
		return Ok(new
		{
			success = true,
			message = "Successfully Updated Account",
			account
		});
	}


	// DELETE: Delete account base on accountID
	[HttpDelete("DeleteAccount")]
	public async Task<ActionResult> DeleteAccount([FromQuery] int accountId)
	{
		var account = await _accountService.GetAccountById(accountId);

		if (account == null)
		{
			return BadRequest(new
			{
				success = false,
				message = "Requested Account do not existed or already deleted"
			});
		}

		await _accountService.DeleteAccount(accountId);
		return Ok(new
		{
			success = true,
			message = "Successfully Deleted Account"
		});
	}
}