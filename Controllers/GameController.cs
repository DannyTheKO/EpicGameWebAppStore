using Microsoft.AspNetCore.Mvc;
using Application.Interfaces; // Adjust according to your structure
using Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EpicGameWebAppStore.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameService _gameServices;
        private readonly IGenreService _genreService; // Assuming you have a genre service
        private readonly IPublisherService _publisherService; // Assuming you have a publisher service

        public GameController(IGameService gameServices, IGenreService genreService, IPublisherService publisherService)
        {
            _gameServices = gameServices;
            _genreService = genreService;
            _publisherService = publisherService;
        }

        // GET: Game/Index
        public async Task<IActionResult> Index()
        {
            var games = await _gameServices.GetAllGameAsync();
            return View(games);
        }

        // GET: Game/Create
        public async Task<IActionResult> Create()
        {
            // Chờ kết quả từ các phương thức bất đồng bộ
            ViewBag.GenreId = new SelectList(await _genreService.GetAllGenresAsync(), "GenreId", "Name");
            ViewBag.PublisherId = new SelectList(await _publisherService.GetAllPublishersAsync(), "PublisherId", "Name");

            return View();
        }

        // POST: Game/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Game game)
        {
            if (ModelState.IsValid)
            {
                await _gameServices.AddGameAsync(game); // Assuming you have an AddGameAsync method
                return RedirectToAction(nameof(Index));
            }

            ViewBag.GenreId = new SelectList(await _genreService.GetAllGenresAsync(), "GenreId", "Name", game.GenreId);
            ViewBag.PublisherId = new SelectList(await _publisherService.GetAllPublishersAsync(), "PublisherId", "Name", game.PublisherId);
            return View(game);
        }

        // GET: Game/Update/{id}
        public async Task<IActionResult> Update(int id)
        {
            var game = await _gameServices.GetGameByIdAsync(id); // Assuming this method exists
            if (game == null)
            {
                return NotFound();
            }

            ViewBag.GenreId = new SelectList(await _genreService.GetAllGenresAsync(), "GenreId", "Name", game.GenreId);
            ViewBag.PublisherId = new SelectList(await _publisherService.GetAllPublishersAsync(), "PublisherId", "Name", game.PublisherId);
            return View(game);
        }

        // POST: Game/Update/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Game game)
        {
            if (id != game.GameId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _gameServices.UpdateGameAsync(game); // Assuming you have an UpdateGameAsync method
                return RedirectToAction(nameof(Index));
            }

            ViewBag.GenreId = new SelectList(await _genreService.GetAllGenresAsync(), "GenreId", "Name", game.GenreId);
            ViewBag.PublisherId = new SelectList(await _publisherService.GetAllPublishersAsync(), "PublisherId", "Name", game.PublisherId);
            return View(game);
        }

        // GET: Game/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            var game = await _gameServices.GetGameByIdAsync(id); // Assuming this method exists
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Game/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _gameServices.DeleteGameAsync(id); // Assuming you have a DeleteGameAsync method
            return RedirectToAction(nameof(Index));
        }

    }
}
