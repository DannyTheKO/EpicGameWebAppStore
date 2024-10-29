using Application.Interfaces;
using Domain.Entities;
using EpicGameWebAppStore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
// Domain

// Application

// Presentation

namespace EpicGameWebAppStore.Controllers;

[Route("Auth")]
public class AuthController : Controller
{
	private readonly IAuthenticationServices _authenticationServices;
	private readonly IAuthorizationServices _authorizationServices;

	public AuthController(IAuthenticationServices authenticationServices, IAuthorizationServices authorizationServices)
	{
		_authenticationServices = authenticationServices;
		_authorizationServices = authorizationServices;
	}

	#region == Logout ==

	[HttpGet("Logout")]
	public async Task<IActionResult> Logout()
	{
		await HttpContext.SignOutAsync("CookieAuth");
		return RedirectToAction("Index", "Home");
	}

	#endregion

	#region == Register ==

	// GET: Auth/Register
	[HttpGet("RegisterPage")]
	public IActionResult RegisterPage()
	{
		return View();
	}

	// POST: Auth/RegisterConfirm
	[HttpPost("RegisterConfirm")]
	public async Task<IActionResult> RegisterConfirm(RegisterViewModel registerViewModel)
	{
		if (!ModelState.IsValid) return View("RegisterPage", registerViewModel);

		var account = new Account
		{
			Username = registerViewModel.Username,
			Password = registerViewModel.Password,
			Email = registerViewModel.Email
		};

		var (success, message) = await _authenticationServices.RegisterUser(account, registerViewModel.ConfirmPassword);

		if (!success)
		{
			ModelState.AddModelError(string.Empty, message);
			return View("RegisterPage", registerViewModel);
		}

		return RedirectToAction("Index", "Home");
	}

	#endregion

	#region == Login ==

	// GET: Auth/LoginPage
	[HttpGet("LoginPage")]
	public IActionResult LoginPage()
	{
		return View();
	}

	// POST: Auth/LoginConfirm
	[HttpPost("LoginConfirm")]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public async Task<IActionResult> LoginConfirm(LoginViewModel loginViewModel)
	{
		// Validate if user input is valid
		if (!ModelState.IsValid) // Requirement is not satisfied => FAIL
			return View("LoginPage", loginViewModel);

		var account = new Account
		{
			Username = loginViewModel.Username,
			Password = loginViewModel.Password
		};

		var (success, result, accountId) = await _authenticationServices.LoginUser(account);

		// If user fail to validate "success" return false
		if (!success) // Return false
		{
			ModelState.AddModelError(string.Empty, result);
			return View("LoginPage", loginViewModel);
		}

		var principal = _authorizationServices.CreateClaimsPrincipal(accountId);
		await HttpContext.SignInAsync("CookieAuth", await principal);

		return RedirectToAction("Index", "Home");
	}

	#endregion
}