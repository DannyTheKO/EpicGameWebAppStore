using System.Diagnostics;
using Application.Interfaces;
using EpicGameWebAppStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace EpicGameWebAppStore.Controllers;

public class HomeController : Controller
{
    private readonly IAuthenticationServices _authenticationServices;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return Redirect("swagger/index.html");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}