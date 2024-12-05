using Application.Interfaces;
using Domain.Entities;
using EpicGameWebAppStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace EpicGameWebAppStore.Controllers.Authentication
{
	public class Register : _BaseController
	{
		private readonly IAuthorizationServices _authorizationServices;
		private readonly IAuthenticationServices _authenticationServices;
		private readonly IAccountService _accountService;
		public Register(
			IAuthorizationServices authorizationServices,
			IAuthenticationServices authenticationServices,
			IAccountService accountService) : base(authorizationServices)
		{
			_authorizationServices = authorizationServices;
			_authenticationServices = authenticationServices;
			_accountService = accountService;
		}
		[HttpGet("Register")]
		public async Task<ActionResult> RegisterTask(RegisterViewModel request)
		{
			// Check if the form is valid
			if (!ModelState.IsValid) // Requirement is not satisfied => FAIL
			{
				return BadRequest(new
				{
					registerState = false,
					errors = ModelState.Values
						.SelectMany(v => v.Errors)
						.Select(e => e.ErrorMessage)
				});
			}
			var account = new Account()
			{
				Username = request.Username,
				Password = request.Password
			};
			var (state, message) = await _authenticationServices.RegisterAccount(account, request.ConfirmPassword);
			if (!state)
			{
				return Unauthorized(new
				{
					registerState = state,
					registerMessage = message,
				});
			}
			return Ok(new
			{
				registerState = state,
				registerMessage = message,
			});
		}
	}
}
