using System.Diagnostics;
using Application.Interfaces;
using EpicGameWebAppStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace EpicGameWebAppStore.Controllers;

public class HomeController : _BaseController
{
    private readonly IAuthenticationServices _authenticationServices;
	private readonly IAuthorizationServices _authorizationServices;
	private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, IAuthorizationServices authorizationServices) : base(authorizationServices)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return Redirect("swagger/index.html");
    }
}