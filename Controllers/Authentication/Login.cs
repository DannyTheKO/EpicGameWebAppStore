using Application.Interfaces;
using Domain.Entities;
using EpicGameWebAppStore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;

namespace EpicGameWebAppStore.Controllers.Authentication;

public class Login : _BaseController
{
	private readonly IAuthorizationServices _authorizationServices;
	private readonly IAuthenticationServices _authenticationServices;
	private readonly IAccountService _accountService;

	public Login(
		IAuthorizationServices authorizationServices, 
		IAuthenticationServices authenticationServices,
		IAccountService accountService) 
		: base(authorizationServices)
	{
		_authorizationServices = authorizationServices;
		_authenticationServices = authenticationServices;
		_accountService = accountService;
	}

	[HttpGet("Login")]
	public async Task<ActionResult> LoginTask(LoginViewModel request)
	{
		// Check if the form is valid
		if (!ModelState.IsValid) // Requirement is not satisfied => FAIL
		{
			return BadRequest(new
			{
				loginState = false,
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

		var (state, token, message) = await _authenticationServices.LoginAccount(account);

		if (!state)
		{
			return Unauthorized(new
			{
				loginState = state,
				loginMessage = message,
			});
		}

		return Ok(new
		{
			loginState = state,
			loginMessage = message,
			loginToken = token
		});
	}
}
