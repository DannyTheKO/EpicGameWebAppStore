using Application.Interfaces;
using Domain.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EpicGameWebAppStore.Controllers;

[Authorize(Roles = "Admin, Moderator")]
[Route("Admin")]
public class AdminController : _BaseController
{
	private readonly IAuthenticationServices _authenticationServices;
	private readonly IAuthorizationServices _authorizationServices;

	public AdminController(
		IAuthorizationServices authorizationServices, IAuthenticationServices authenticationServices, IAccountRepository accountRepository) 
		: base(authenticationServices, authorizationServices)
	{
		_authorizationServices = authorizationServices;
		_authenticationServices = authenticationServices;
	}

	[HttpGet("Index")]
	public async Task<IActionResult> Index()
	{
		var account = await _authenticationServices.GetAllUser();
		return View(account);
	}
}