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
		public async Task<IActionResult> RegisterConfirm(RegisterViewModel registerViewModel, Account account)
		{
			// ==> Validate if user input is valid
			if (!ModelState.IsValid)
			{
				return View("RegisterPage", registerViewModel);
			}

			// Check if the username and email already exists
			var existingAccountUserName = await _authenticationServices.GetAccountByUsername(account.Username);
			var existingAccountEmail = await _authenticationServices.GetAccountByEmail(account.Email);
			if (existingAccountUserName != null && existingAccountEmail != null)
			{
				ModelState.AddModelError(string.Empty, "Username and Email already exists");
				return View("RegisterPage", registerViewModel);
			}

			if (existingAccountEmail != null)
			{
				ModelState.AddModelError(string.Empty, "Email already exists");
				return View("RegisterPage", registerViewModel);
			}

			if (existingAccountUserName != null)
			{
				ModelState.AddModelError(string.Empty, "Username already exists");
				return View("RegisterPage", registerViewModel);
			}

			// Check is the "Password" and the "Confirm Password" is correct
			if (registerViewModel.Password != registerViewModel.ConfirmPassword)
			{
				ModelState.AddModelError(string.Empty, "Password and Confirm Password are not the same");
				return View("RegisterPage", registerViewModel);
			}

			// ==> Create a new account
			account.CreatedOn = DateTime.UtcNow;
			account.Username = registerViewModel.Username;
			account.Password = registerViewModel.Password; // TODO: Hash the password before saving it
			account.Email = registerViewModel.Email;
			account.IsAdmin = "N"; // TODO: Going do some authorization later for this.
								   // account.Password = HashPassword(account.Password);


			// ==> Save the account to the database
			await _authenticationServices.AddUser(account);

			// ==> Redirect to login page or return a success message
			return RedirectToAction("RegisterPage");
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
        public async Task<IActionResult> LoginConfirm(LoginViewModel loginViewModel, Account account)
		{
			// Validate if user input is valid
	        if (!ModelState.IsValid) // Requirement is not satisfied => FAIL
			{
		        return View("LoginPage", loginViewModel);
			}

	        // Check if the user exists in the database
	        var existingAccount = await _authenticationServices.GetAccountByUsername(loginViewModel.Username);
	        if (existingAccount != null) // FOUND!
			{
		        // Validate user credentials
		        if (await _authenticationServices.ValidateUserCredentialAsync(loginViewModel.Username, loginViewModel.Password)) // Success
				{
					var token = await _authenticationServices.GenerateTokenAsync(loginViewModel.Username);
					return Ok(new { Token = token });
				}

			    // Password is incorrect
			    ModelState.AddModelError(string.Empty, "Incorrect Password");
	        }
	        else
	        {
		        // User does not exist in our database
		        ModelState.AddModelError(string.Empty, "User does not exist");
			}

			// Return to the login page with validation errors
			return View("LoginPage", loginViewModel);
		}
	}
}
