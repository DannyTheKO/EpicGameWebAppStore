using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EpicGameWebAppStore.Controllers;

[Authorize(Roles = "Admin, Moderator, Editor")]
[Route("Dashboard")]
public class DashboardController : _BaseController
{
    private readonly IAuthorizationServices _authorizationServices;
    private readonly IAuthenticationServices _authenticationServices;
    private readonly IAccountService _accountService;
    private readonly IRoleService _roleService;
        
    public DashboardController(
        IAuthorizationServices authorizationServices,
        IAuthenticationServices authenticationServices,
        IAccountService accountService,
        IRoleService roleService)
        : base(authenticationServices, authorizationServices, accountService, roleService)
    {
        _authorizationServices = authorizationServices;
        _authenticationServices = authenticationServices;
        _accountService = accountService;
    }

    [HttpGet("Index")]
    public async Task<IActionResult> Index()
    {
        var account = await _accountService.GetAllAccounts();
        return View(account);
    }

    private async Task PopulateViewBags()
    {
        var roles = await _roleService.GetAllRoles();
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
    public async Task<IActionResult> CreateConfirm(
        [Bind("RoleId", "Username", "Password", "Email", "IsActive")] Account account)
    {
        // Check if both Username and Email already exist
        var flagUsername = await _accountService.GetAccountByUsername(account.Username ?? string.Empty);
        var flagEmail = await _accountService.GetAccountByEmail(account.Email ?? string.Empty);
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

        await _accountService.AddAccount(account);
        return RedirectToAction("Index", "Dashboard");
    }
}