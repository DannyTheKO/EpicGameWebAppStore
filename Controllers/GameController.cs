using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.X509;

// Application
using Application.Interfaces;

// Domain
using Domain.Entities;


namespace EpicGameWebAppStore.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameServices _gameServices;

        public GameController(IGameServices gameServices)
        {
            _gameServices = gameServices;
        }

        // GET: Game/Index
        public async Task<IActionResult> Index()
        {
            var games = await _gameServices.GetAllGameAsync();
            return View(games);
        }

        // TODO: Create Function
        // GET: Game/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Game/Create
        
        // TODO: Update Function 
        // GET: Game/Update/{id}
        // POST: Game/Update/{id}

        // TODO: Delete Function
        // GET: Game/Delete/{id}
        // POST: Game/Delete/{id}


    }
}
