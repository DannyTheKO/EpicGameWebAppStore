using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.DataAccess;
using Application.Interfaces;

namespace EpicGameWebAppStore.Controllers
{
    [Route("Cart")] // Route gốc cho controller
    public class CartController : Controller
    {
        private readonly EpicGameDbContext _context;
        private readonly ICartService _cartService;

        public CartController(EpicGameDbContext context)
        {
            _context = context;
        }

        // GET: carts
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Carts.Include(c => c.Account).Include(c => c.PaymentMethod).ToListAsync());
        }

        // GET: carts/details/5
        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.Account)
                .Include(c => c.PaymentMethod)
                .FirstOrDefaultAsync(m => m.CartId == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: carts/create
        [HttpGet("create")]
        public IActionResult Create()
        {
            ViewData["AccountId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Accounts, "AccountId", "Username");
            ViewData["PaymentMethodId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Paymentmethods, "PaymentMethodId", "Name");
            return View();
        }

        // POST: carts/create
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,PaymentMethodId")] Cart cart)
        {
            cart.CreatedOn = DateTime.Now;
            cart.TotalAmount = 0;

            if (ModelState.IsValid)
            {
                try
                {
                    _cartService.AddCartAsync(cart);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Unable to save changes. " + ex.Message);
                }
            }

            ViewData["AccountId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Accounts, "AccountId", "Username", cart.AccountId);
            ViewData["PaymentMethodId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Paymentmethods, "PaymentMethodId", "Name", cart.PaymentMethodId);
            return View(cart);
        }

        // GET: carts/edit/5
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Accounts, "AccountId", "Username", cart.AccountId);
            ViewData["PaymentMethodId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Paymentmethods, "PaymentMethodId", "Name", cart.PaymentMethodId);
            return View(cart);
        }

        // POST: carts/edit/5
        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CartId,AccountId,PaymentMethodId,TotalAmount,CreatedOn")] Cart cart)
        {
            if (id != cart.CartId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.CartId))
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
            ViewData["AccountId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Accounts, "AccountId", "Username", cart.AccountId);
            ViewData["PaymentMethodId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Paymentmethods, "PaymentMethodId", "Name", cart.PaymentMethodId);
            return View(cart);
        }

        // GET: carts/delete/5
        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.Account)
                .Include(c => c.PaymentMethod)
                .FirstOrDefaultAsync(m => m.CartId == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: carts/delete/5
        [HttpPost("delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.CartId == id);
        }
    }
}
