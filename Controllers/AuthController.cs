using Application.Interfaces;
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
    public async Task<ActionResult> Logout()
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

        // Sign out of cookie authentication
        await HttpContext.SignOutAsync("CookieAuth");

        // Return instruction to clear token
        return Ok(new
        {
            loginStateFlag = false,
            message = "Successfully Logout",
            action = "CLEAR_TOKEN" // Frontend should handle this to remove token from storage
        });
    }

    [HttpGet("AccessDenied")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public ActionResult AccessDenied()
    {
        return StatusCode(403, new
        {
            accessFlag = false,
            message = "Access Denied: You don't have permission to access this resource"
        });
    }

    // POST: Auth/RegisterConfirm
    [HttpPost("RegisterConfirm")]
    public async Task<ActionResult<RegisterViewModel>> RegisterConfirm(RegisterViewModel registerViewModel)
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
        }

        return Ok(new
        {
            registerStageFlag = registerStage,
            message = resultMessage
        });
    }

    // POST: Auth/LoginConfirm
    [HttpPost("LoginConfirm")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<ActionResult<LoginViewModel>> LoginConfirm(LoginViewModel loginViewModel)
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

        var (loginState, token, resultMessage) = await _authenticationServices.LoginAccount(account);

        // If user fail to validate "success" return false
        if (!loginState) // Return false
        {
            return BadRequest(
                new { loginStateFlag = loginState, message = resultMessage }
            );
        }

        return Ok(new
        {
            loginStateFlag = loginState,
            accountToken = token,
            message = resultMessage
        });
    }
}