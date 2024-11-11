using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EpicGameWebAppStore.Controllers;

//[Authorize(Roles = "Admin, Moderator, Editor")]
[Route("[controller]")]
public class GameController : _BaseController
{
    private readonly IAuthenticationServices _authenticationServices;
    private readonly IAuthorizationServices _authorizationServices;
    private readonly IGameService _gameServices;
    private readonly IGenreService _genreService;
    private readonly IPublisherService _publisherService;

    public GameController(
        IGameService gameServices,
        IGenreService genreService,
        IPublisherService publisherService,
        IAuthenticationServices authenticationServices,
        IAuthorizationServices authorizationServices,
        IAccountService accountService,
        IRoleService roleService)
        : base(authenticationServices, authorizationServices, accountService, roleService)
    {
        _gameServices = gameServices;
        _genreService = genreService;
        _publisherService = publisherService;
        _authenticationServices = authenticationServices;
        _authorizationServices = authorizationServices;
    }

    // GET: Game/Index
    [HttpGet("Index")]
    public async Task<IActionResult> Index()
    {
        var games = await _gameServices.GetAllGame();
        return View(games);
    }

    // GET: Game/CreatePage
    [HttpGet("CreatePage")]
    public async Task<IActionResult> CreatePage()
    {
        // Chờ kết quả từ các phương thức bất đồng bộ
        ViewBag.GenreId = new SelectList(await _genreService.GetAllGenresAsync(), "GenreId", "Name");
        ViewBag.PublisherId = new SelectList(await _publisherService.GetAllPublishersAsync(), "PublisherId", "Name");

        return View("Create");
    }

    // POST: Game/CreateConfirm
    [HttpPost("CreateConfirm")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateConfirm(Game game)
    {
        if (ModelState.IsValid)
        {
            await _gameServices.AddGame(game); // Assuming you have an AddGame method
            return RedirectToAction(nameof(Index));
        }

        ViewBag.GenreId = new SelectList(await _genreService.GetAllGenresAsync(), "GenreId", "Name", game.GenreId);
        ViewBag.PublisherId = new SelectList(await _publisherService.GetAllPublishersAsync(), "PublisherId", "Name", game.PublisherId);
        return View("Index");
    }

    // GET: Game/UpdatePage/{id}
    [HttpGet("UpdatePage/{id}")]
    public async Task<IActionResult> UpdatePage(int id)
    {
        var game = await _gameServices.GetGameById(id);
        if (game == null) return NotFound();

        ViewBag.GenreId = new SelectList(await _genreService.GetAllGenresAsync(), "GenreId", "Name", game.GenreId);
        ViewBag.PublisherId = new SelectList(await _publisherService.GetAllPublishersAsync(), "PublisherId", "Name",
            game.PublisherId);
        return View("Update");
    }

    // POST: Game/UpdateConfirm/{id}
    [HttpPut("UpdateConfirm/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateConfirm(int id, Game game)
    {
        if (id != game.GameId) return BadRequest();

        if (ModelState.IsValid)
        {
            await _gameServices.UpdateGame(game); // Assuming you have an UpdateGame method
            return RedirectToAction(nameof(Index));
        }

        ViewBag.GenreId = new SelectList(await _genreService.GetAllGenresAsync(), "GenreId", "Name", game.GenreId);
        ViewBag.PublisherId = new SelectList(await _publisherService.GetAllPublishersAsync(), "PublisherId", "Name", game.PublisherId);
        return View("Index");
    }

    // GET: Game/DeletePage/{id}
    [HttpGet("DeletePage/{id}")]
    public async Task<IActionResult> DeletePage(int id)
    {
        var game = await _gameServices.GetGameById(id); // Assuming this method exists
        if (game == null) return NotFound();

        return View("Index");
    }

    // POST: Game/DeleteConfirm/{id}
    [HttpDelete("DeleteConfirm/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _gameServices.DeleteGame(id); // Assuming you have a DeleteGame method
        return RedirectToAction(nameof(Index));
    }
}