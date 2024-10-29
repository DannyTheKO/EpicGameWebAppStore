using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.DataAccess;

namespace EpicGameWebAppStore.Controllers
{
    [Route("Cartdetail")]
    public class CartdetailController : Controller
    {
        private readonly EpicGameDbContext _context;

        public CartdetailController(EpicGameDbContext context)
        {
            _context = context;
        }

        // GET: cartdetails
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cartdetails
                .Include(c => c.Cart)
                .Include(c => c.Game)
                .ToListAsync());
        }

        // GET: cartdetails/details/5
        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartdetail = await _context.Cartdetails
                .Include(c => c.Cart)
                .Include(c => c.Game)
                .FirstOrDefaultAsync(m => m.CartDetailId == id);
            if (cartdetail == null)
            {
                return NotFound();
            }

            return View(cartdetail);
        }

        // GET: cartdetails/create
        [HttpGet("create")]
        public IActionResult Create()
        {
            ViewData["CartId"] = new SelectList(_context.Carts, "CartId", "CartId");
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "Title");
            return View();
        }

        // POST: cartdetails/create
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CartDetailId,CartId,GameId,Quantity,Price,Discount")] Cartdetail cartdetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cartdetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CartId"] = new SelectList(_context.Carts, "CartId", "CartId", cartdetail.CartId);
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "Title", cartdetail.GameId);
            return View(cartdetail);
        }

        // GET: cartdetails/edit/5
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartdetail = await _context.Cartdetails.FindAsync(id);
            if (cartdetail == null)
            {
                return NotFound();
            }
            ViewData["CartId"] = new SelectList(_context.Carts, "CartId", "CartId", cartdetail.CartId);
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "Title", cartdetail.GameId);
            return View(cartdetail);
        }

        // POST: cartdetails/edit/5
        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CartDetailId,CartId,GameId,Quantity,Price,Discount")] Cartdetail cartdetail)
        {
            if (id != cartdetail.CartDetailId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cartdetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartdetailExists(cartdetail.CartDetailId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CartId"] = new SelectList(_context.Carts, "CartId", "CartId", cartdetail.CartId);
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "Title", cartdetail.GameId);
            return View(cartdetail);
        }

        // GET: cartdetails/delete/5
        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartdetail = await _context.Cartdetails
                .Include(c => c.Cart)
                .Include(c => c.Game)
                .FirstOrDefaultAsync(m => m.CartDetailId == id);
            if (cartdetail == null)
            {
                return NotFound();
            }

            return View(cartdetail);
        }

        // POST: cartdetails/delete/5
        [HttpPost("delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cartdetail = await _context.Cartdetails.FindAsync(id);
            _context.Cartdetails.Remove(cartdetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartdetailExists(int id)
        {
            return _context.Cartdetails.Any(e => e.CartDetailId == id);
        }
    }
}
