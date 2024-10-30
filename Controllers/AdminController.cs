using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EpicGameWebAppStore.Controllers;

public class AdminController : _BaseController
{
	private readonly IAuthenticationServices _authenticationServices;
	private readonly IAuthorizationServices _authorizationServices;

	public AdminController(
		IAuthorizationServices authorizationServices, IAuthenticationServices authenticationServices) 
		: base(authenticationServices, authorizationServices)
	{
		_authorizationServices = authorizationServices;
		_authenticationServices = authenticationServices;
	}

	public IActionResult Index()
	{
		return View();
	}
}