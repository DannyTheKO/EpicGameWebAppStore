using Application.Interfaces;
using Domain.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EpicGameWebAppStore.Controllers;

[Authorize(Roles = "Admin, Moderator")]
[Route("Admin")]
public class DashboardController : _BaseController
{
	private readonly IAuthenticationServices _authenticationServices;
	private readonly IAuthorizationServices _authorizationServices;

	public DashboardController(
		IAuthorizationServices authorizationServices, IAuthenticationServices authenticationServices, IAccountRepository accountRepository) 
		: base(authenticationServices, authorizationServices)
	{
		_authorizationServices = authorizationServices;
		_authenticationServices = authenticationServices;
	}

	[HttpGet("Index")]
	public async Task<IActionResult> Index()
	{
		var account = await _authenticationServices.GetAllAccounts();
		return View(account);
	}
}