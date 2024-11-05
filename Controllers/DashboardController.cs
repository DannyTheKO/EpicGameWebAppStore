using Application.Interfaces;
using Domain.Entities;
using Domain.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EpicGameWebAppStore.Controllers;

[Authorize(Roles = "Admin, Moderator")]
[Route("Admin")]
public class DashboardController : _BaseController
{
	private readonly IAuthenticationServices _authenticationServices;
	private readonly IAuthorizationServices _authorizationServices;

	public DashboardController(
		IAuthorizationServices authorizationServices, IAuthenticationServices authenticationServices, IAccountRepository accountRepository) 
		: base(authenticationServices, authorizationServices)
	{
		_authorizationServices = authorizationServices;
		_authenticationServices = authenticationServices;
	}

	[HttpGet("Index")]
	public async Task<IActionResult> Index()
	{
		var account = await _authenticationServices.GetAllAccounts();
		return View(account);
	}

    private async Task PopulateViewBags()
    {
        var roles = await _authorizationServices.GetAllRoles();
        ViewBag.RoleId = new SelectList(roles, "RoleId", "Name");

        var isActive = new List<SelectListItem>
        {
            new() { Value = "Y", Text = "Yes" },
            new() { Value = "N", Text = "No" }
        };

        ViewBag.IsActive = new SelectList(isActive, "Value", "Text");
    }

    [HttpGet("CreatePage")]
    public async Task<IActionResult> CreatePage()
    {
        await PopulateViewBags();
        return View("Create");
    }

    [HttpPost("CreateConfirm")]
    public async Task<IActionResult> CreateConfirm([Bind("RoleId", "Username", "Password", "Email", "IsActive")] Account account)
    {
        // Check if both Username and Email already exist
        var flagUsername = await _authenticationServices.GetAccountByUsername(account.Username ?? string.Empty);
        var flagEmail = await _authenticationServices.GetAccountByEmail(account.Email ?? string.Empty);
        if (flagUsername != null && flagEmail != null)
        {
            ModelState.AddModelError(string.Empty, "Username and Email already exist");
            await PopulateViewBags();
            return View("Create");
        }

        if (flagEmail != null)
        {
            ModelState.AddModelError(string.Empty, "Email already exist");
            await PopulateViewBags();
            return View("Create");
        }

        if (flagUsername != null)
        {
            ModelState.AddModelError(string.Empty, "Username already exist");
            await PopulateViewBags();
            return View("Create");
        }

        await _authenticationServices.AddAccount(account);
        return RedirectToAction("Index", "Dashboard");
    }
}