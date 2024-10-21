using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NuGet.Common;
using Microsoft.EntityFrameworkCore;

// Domain
using Application.Interfaces;
using Domain.Entities;

namespace EpicGameWebAppStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
	        return View(new Account());
        }
        // TODO: POST: Auth/Register


        // GET: Auth/LoginPage
        [HttpGet("LoginPage")]
        public IActionResult LoginPage()
        {
	        return View();
        }

        // POST: Auth/
        [HttpPost("LoginConfirm")]
        public async Task<IActionResult> LoginConfirm(Account account)
        {
            // Validate if user input is valid
            if (!ModelState.IsValid) // Requirement is not satisfy => FAIL
            {
	            return View("LoginPage", account);
            }

            // check if the user exist in the database
            var existingAccount = await _authenticationServices.GetAccountByUserNameAsync(account.Username);
            if (existingAccount != null) // FOUND! 
            {
                // Get token for the existingAccount
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
                //User does not exist in our database
                ModelState.AddModelError(string.Empty, "User doest not exist");
            }
            
            // Return to the login page with validation errors
			return View("LoginPage", account);
        }
    }
}
