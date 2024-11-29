using System.Runtime.CompilerServices;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EpicGameWebAppStore.Controllers;

[Authorize(Roles = "Admin, Moderator, Editor")]
[Route("[controller]")]
public class DashboardController : _BaseController
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
        ICartService cartService) : base(authorizationServices)
    {
        _authorizationServices = authorizationServices;
        _authenticationServices = authenticationServices;

        _accountService = accountService;
        _roleService = roleService;

        _gameService = gameService;
        _cartService = cartService;
    }
}