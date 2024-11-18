using System.Runtime.CompilerServices;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EpicGameWebAppStore.Controllers;

[Authorize(Roles = "Admin, Moderator, Editor")]
[Route("Dashboard")]
public class DashboardController : Controller
{
    private readonly IAuthorizationServices _authorizationServices;
    private readonly IAuthenticationServices _authenticationServices;

    private readonly IAccountService _accountService;
    private readonly IRoleService _roleService;
    private readonly IGameService _gameService;
    private readonly ICartService _cartService;
    
    public DashboardController(
        IAuthorizationServices authorizationServices,
        IAuthenticationServices authenticationServices,
        IAccountService accountService,
        IRoleService roleService,
        IGameService gameService,
        ICartService cartService)
    {
        _authorizationServices = authorizationServices;
        _authenticationServices = authenticationServices;

        _accountService = accountService;
        _roleService = roleService;

        _gameService = gameService;
        _cartService = cartService;
    }

    [HttpGet("Index")]
    public async Task<IActionResult> Index()
    {
        var getAllAccount = await _accountService.GetAllAccounts();
        var getAllGame= await _gameService.GetAllGame();

        // select only Admin account
        var onlyAdminAccount = getAllAccount.Where(a => a.Role.Name == "Admin");
        var onlyGuestAccount = getAllAccount.Where(a => a.Role.Name == "Guest");

        // select only Game from EA
        var gameFromEA = getAllGame.Where(g => g.Publisher.Name == "Electronic Arts");


        //return View(account);
        return Ok(new
        {
	        adminAccount = onlyAdminAccount,
	        guestAccount = onlyGuestAccount,
            EAGame = gameFromEA,
        });
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