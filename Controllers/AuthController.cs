using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NuGet.Common;
using Microsoft.EntityFrameworkCore;

// Domain
using Domain.Entities;

// Application
using Application.Interfaces;

namespace EpicGameWebAppStore.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthenticationService _authenticationServices;

        public AuthController(IAuthenticationService authenticationServices)
        {
            _authenticationServices = authenticationServices;
        }

        // TODO: GET: Auth/Register
        [HttpGet("RegisterPage")]
        public IActionResult RegisterPage()
        {
	        return View();
        }
		// TODO: POST: Auth/Register
		public async Task<IActionResult> RegisterConfirm(Account account)
		{
			// Validate if user input is valid
			if (!ModelState.IsValid)
			{
				return View("RegisterPage", account);
			}

			// Check if the username already exists
			var existingAccount = await _authenticationServices.GetAccountByUserNameAsync(account.Username);
			if (existingAccount != null)
			{
				ModelState.AddModelError(string.Empty, "Username already exists");
				return View("RegisterPage", account);
			}

			// Create a new account
			account.CreatedOn = DateTime.UtcNow;
			// TODO: Hash the password before saving it
			// account.Password = HashPassword(account.Password);

			// Save the account to the database
			await _authenticationServices.AddUserAsync(account);

			// Redirect to login page or return a success message
			return RedirectToAction("LoginPage");
		}


        // GET: Auth/LoginPage
        [HttpGet("LoginPage")]
        public IActionResult LoginPage()
        {
	        return View();
        }

		// POST: Auth/LoginConfirm
		[HttpPost("LoginConfirm")]
        public async Task<IActionResult> LoginConfirm(Account account)
        {
	        // Validate if user input is valid
	        if (!ModelState.IsValid) // Requirement is not satisfied => FAIL
	        {
		        return View("LoginPage", account);
	        }

	        // Check if the user exists in the database
	        var existingAccount = await _authenticationServices.GetAccountByUserNameAsync(account.Username);
	        if (existingAccount != null) // FOUND!
	        {
		        // Validate user credentials
		        if (await _authenticationServices.ValidateUserCredentialAsync(account.Username, account.Password)) // Success
		        {
			        var token = await _authenticationServices.GenerateTokenAsync(account.Username);
			        return Ok(new { Token = token });
		        }
		        else
		        {
			        // Password is incorrect
			        ModelState.AddModelError(string.Empty, "Incorrect Password");
		        }
	        }
	        else
	        {
		        // User does not exist in our database
		        ModelState.AddModelError(string.Empty, "User does not exist");
	        }

	        // Return to the login page with validation errors
	        return View("LoginPage", account);
        }
	}
}
