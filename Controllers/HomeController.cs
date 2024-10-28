using System.Diagnostics;
using Application.Interfaces;
using EpicGameWebAppStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace EpicGameWebAppStore.Controllers;

public class HomeController : Controller
{
	private readonly IAuthenticationServices _authenticationServices;
	private readonly ILogger<HomeController> _logger;

	public HomeController(ILogger<HomeController> logger, IAuthenticationServices authenticationServices)
	{
		_logger = logger;
		_authenticationServices = authenticationServices;
	}

	public IActionResult Index()
	{
		// Check if the user is login in the homepage
		var isAuthenticated = User.Identity.IsAuthenticated;
		var accountUsername = User.Identity.Name;
		ViewData["IsAuthenticated"] = isAuthenticated;
		ViewData["Account_Username"] = accountUsername;

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