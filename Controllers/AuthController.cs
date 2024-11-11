﻿using Application.Interfaces;
using Domain.Entities;
using EpicGameWebAppStore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

// Domain

// Application

// Presentation

namespace EpicGameWebAppStore.Controllers;

[Route("[controller]")]
//[ApiController]
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
    public async Task<IActionResult> LoginConfirm(string username, string password)
    {
            Console.WriteLine(username);
        return Ok();

    }
}