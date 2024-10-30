using System.Diagnostics;
using Application.Interfaces;
using EpicGameWebAppStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EpicGameWebAppStore.Controllers;

public class HomeController : _BaseController
{
	private readonly IAuthenticationServices _authenticationServices;
	private readonly ILogger<HomeController> _logger;

	public HomeController(ILogger<HomeController> logger, IAuthenticationServices authenticationServices, IAuthorizationServices authorizationServices)
		: base(authenticationServices, authorizationServices)
	{
		_logger = logger;
	}

	public IActionResult Index()
	{
		return View();
	}

	public IActionResult Privacy()
	{
		return View();
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}
