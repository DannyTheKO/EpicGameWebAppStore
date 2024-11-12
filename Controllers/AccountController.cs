using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EpicGameWebAppStore.Controllers
{
	public class AccountController : Controller
	{
		private readonly IAccountService _accountService;

		public AccountController(IAccountService accountService)
		{
			_accountService = accountService;
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

			return Ok( new
			{
				success = true,
				accountDetails = account,
			});
		}

		// PUT: Update specific account base on accountID
		[HttpPut("UpdateAccount/{accountId}")]
		public async Task<ActionResult> UpdateAccount(int accountId)
		{
			// Get Account by accountId
			var account = await _accountService.GetAccountById(accountId);

			if (account == null) // if that account is empty
			{
				return BadRequest(new
				{
					sucess = false,
					message = "Requested Account do not existed!"
				}); 
			}

			await _accountService.UpdateAccount(account);
			return Ok(new
			{
				success = true,
				message = "Successfully Updated Account"
			});
		}


		// DELETE: Delete account base on accountID
		[HttpDelete("DeleteAccount/{accountId}")]
		public async Task<ActionResult> DeleteAccount(int accountId)
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
}
