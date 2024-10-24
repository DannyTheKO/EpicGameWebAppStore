using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NuGet.Common;
using Microsoft.EntityFrameworkCore;

// Domain
using Domain.Entities;

// Application
using Application.Interfaces;

// Presentation
using EpicGameWebAppStore.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace EpicGameWebAppStore.Controllers
{
	public class AuthController : Controller
	{
		private readonly IAuthenticationService _authenticationServices;

		public AuthController(IAuthenticationService authenticationServices)
		{
			_authenticationServices = authenticationServices;
		}

		// == Register ==

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
			if (!ModelState.IsValid)
			{
				return View("RegisterPage", registerViewModel);
			}

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


		// == Login ==

		// GET: Auth/LoginPage
		[HttpGet("LoginPage")]
		public IActionResult LoginPage()
		{
			return View();
		}

		// POST: Auth/LoginConfirm
		[HttpPost("LoginConfirm")]
		public async Task<IActionResult> LoginConfirm(LoginViewModel loginViewModel)
		{
			// Validate if user input is valid
			if (!ModelState.IsValid) // Requirement is not satisfied => FAIL
			{
				return View("LoginPage", loginViewModel);
			}

			var account = new Account
			{
				Username = loginViewModel.Username,
				Password = loginViewModel.Password,
			};

			var (success, message) = await _authenticationServices.LoginUser(account);

			if (!success)
			{
				ModelState.AddModelError(string.Empty, message);
				return View("LoginPage", loginViewModel);
			}

			return RedirectToAction("Index", "Home");
		}
	}
}
