using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EpicGameWebAppStore.Controllers;

[Route("Discount")]
public class DiscountController : Controller
{
    private readonly IDiscountService _discountService;

    public DiscountController(IDiscountService discountService)
    {
        _discountService = discountService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var discounts = await _discountService.GetAllDiscountAsync();
        return View(discounts);
    }

    // GET: discounts/details/5
    [HttpGet("details/{id}")]
    public async Task<IActionResult> Details(int id)
    {
        var discount = await _discountService.GetDiscountByIdAsync(id);
        if (discount == null) return NotFound();

        return View(discount);
    }


    [HttpGet("create")]
    public IActionResult Create()
    {
        return View();
    }


    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("DiscountId,GameId,Percent,Code,StartOn,EndOn")] Discount discount)
    {
        if (ModelState.IsValid)
        {
            await _discountService.AddDiscountAsync(discount);
            return RedirectToAction(nameof(Index));
        }

        return View(discount);
    }


    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var discount = await _discountService.GetDiscountByIdAsync(id);
        if (discount == null) return NotFound();
        return View(discount);
    }

    [HttpPost("edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind("DiscountId,GameId,Percent,Code,StartOn,EndOn")] Discount discount)
    {
        if (id != discount.DiscountId) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                await _discountService.UpdateDiscountAsync(discount);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await DiscountExists(discount.DiscountId))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(discount);
    }

    [HttpGet("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var discount = await _discountService.GetDiscountByIdAsync(id);
        if (discount == null) return NotFound();

        return View(discount);
    }

    [HttpPost("delete/{id}")]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _discountService.DeleteDiscountAsync(id);
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> DiscountExists(int id)
    {
        var discount = await _discountService.GetDiscountByIdAsync(id);
        return discount != null;
    }
}