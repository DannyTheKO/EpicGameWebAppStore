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
public class AuthController : _BaseController
{
    private readonly IAuthenticationServices _authenticationServices;
    private readonly IAuthorizationServices _authorizationServices;
    private readonly IAccountService _accountService;
    private readonly IRoleService _roleService;

    public AuthController(
        IAuthorizationServices authorizationServices,
        IAuthenticationServices authenticationServices,
        IAccountService accountService,
        IRoleService roleService)
        : base(
            authenticationServices,
            authorizationServices,
            accountService,
            roleService)
    {
        _authenticationServices = authenticationServices;
        _authorizationServices = authorizationServices;
        _accountService = accountService;
        _roleService = roleService;
    }

    [HttpGet("Logout")]
    public async Task<IActionResult> Logout()
    {
	    var checkLoginAccount = GetCurrentLoginAccountId();
	    if (checkLoginAccount == -1) // NOT FOUND
	    {
		    return BadRequest(new
		    {
			    loginStateFlag = false,
			    message = "Current Login Account Not Found!"
		    });
	    }

        await HttpContext.SignOutAsync("CookieAuth");

        return Ok(new
        {
	        loginStateFlag = false,
	        message = "Successfully Logout"
        });
    }

    [HttpGet("AccessDenied")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> AccessDenied()
    {
	    return StatusCode(403, new
	    {
		    accessFlag = false,
		    message = "Access Denied: You don't have permission to access this resource"
	    });
    }

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
		    return BadRequest(new
		    {
                registerState = false,
                errors = ModelState.Values
	                .SelectMany(v => v.Errors)
	                .Select(e => e.ErrorMessage)
		    });
		    //return View("RegisterPage", registerViewModel);
	    }


        var account = new Account
        {
            Username = registerViewModel.Username,
            Password = registerViewModel.Password,
            Email = registerViewModel.Email
        };

        var (registerStage, resultMessage) = await _authenticationServices.RegisterAccount(account, registerViewModel.ConfirmPassword);

        if (!registerStage)
        {
	        return BadRequest(new
	        {
                registerStageFlag = registerStage,
                message = resultMessage
	        });

	        //ModelState.AddModelError(string.Empty, resultMessage);
	        //return View("RegisterPage", registerViewModel);
        }

        return Ok( new
        {
            registerStageFlag = registerStage,
            message = resultMessage
        });
    }

    // GET: Auth/LoginPage
    [HttpGet("LoginPage")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
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
        {
            return BadRequest(new
            {
                loginState = false,
                errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
            });
            
            // return View("LoginPage", loginViewModel);
        }

        var account = new Account
        {
            Username = loginViewModel.Username,
            Password = loginViewModel.Password
        };

        var (loginState, resultMessage, accountId) = await _authenticationServices.LoginAccount(account);

        // If user fail to validate "success" return false
        if (!loginState) // Return false
        {
            //ModelState.AddModelError(string.Empty, result);
            //return View("LoginPage", loginViewModel);

            return BadRequest(
                new { loginStateFlag = loginState, message = resultMessage }
            );
        }

        var principal = _authorizationServices.CreateClaimsPrincipal(accountId);
        await HttpContext.SignInAsync("CookieAuth", await principal);

        //return RedirectToAction("Index", "Home");
        return Ok(new
        {
            loginStateBool = loginState,
            message = resultMessage,
            accountDetail = await _accountService.GetAccountById(accountId),
            role = await _roleService.GetRoleById(accountId)
        });
    }
}