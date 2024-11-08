using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EpicGameWebAppStore.Controllers;

[Route("Cart")]
public class CartController : _BaseController
{
    private readonly IAccountService _accountService; // Giả sử bạn có dịch vụ này
    private readonly IAuthenticationServices _authenticationServices;
    private readonly IAuthorizationServices _authorizationServices;

    private readonly ICartService _cartService;
    private readonly IPaymentMethodService _paymentMethodService; // Giả sử bạn có dịch vụ này

    public CartController(
        ICartService cartService,
        IAccountService accountService,
        IRoleService roleService,
        IPaymentMethodService paymentMethodService,
        IAuthenticationServices authenticationServices,
        IAuthorizationServices authorizationServices)
        : base(
            authenticationServices,
            authorizationServices,
            accountService,
            roleService)
    {
        _cartService = cartService;
        _accountService = accountService;
        _paymentMethodService = paymentMethodService;
    }

    [HttpGet("Index")]
    public async Task<IActionResult> Index()
    {
        var carts = await _cartService.GetAllCartsAsync();
        return View(carts);
    }

    [HttpGet("CreatePage")]
    public async Task<IActionResult> CreatePage()
    {
        await PopulateAccountAndPaymentMethodDropDowns();
        return View("Create");
    }

    [HttpPost("CreateConfirm")]
    public async Task<IActionResult> CreateConfirm(Cart cart)
    {
        if (ModelState.IsValid)
        {
            await _cartService.AddCartAsync(cart);
            TempData["Message"] = "Cart created successfully.";
            return View("Index");
        }

        ModelState.AddModelError(string.Empty, "Error");
        await PopulateAccountAndPaymentMethodDropDowns();
        return View("Create", cart);
    }

    [HttpGet("EditPage/{id}")]
    public async Task<IActionResult> EditPage(int id)
    {
        var cart = await _cartService.GetCartByIdAsync(id);
        if (cart == null) return NotFound();
        await PopulateAccountAndPaymentMethodDropDowns();
        return View("Edit", cart);
    }

    [HttpPut("EditConfirm/{id}")]
    public async Task<IActionResult> EditConfirm(int id, Cart cart)
    {
        if (id != cart.CartId) return BadRequest();

        if (ModelState.IsValid)
        {
            await _cartService.UpdateCartAsync(cart);
            TempData["Message"] = "Cart updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        await PopulateAccountAndPaymentMethodDropDowns();
        return View("Edit", cart);
    }

    [HttpGet("DeletePage/{id}")]
    public async Task<IActionResult> DeletePage(int id)
    {
        var cart = await _cartService.GetCartByIdAsync(id);
        if (cart == null) return NotFound();
        return View("Delete", cart);
    }

    [HttpDelete("DeleteConfirm/{id}")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _cartService.DeleteCartAsync(id);
        TempData["Message"] = "Cart deleted successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("GetByAccount")]
    public async Task<IActionResult> GetByAccount(int accountId)
    {
        var carts = await _cartService.GetCartsByAccountIdAsync(accountId);
        if (carts == null) return NotFound();
        return View("Index", carts);
    }

    [HttpPatch("PopulateAccountAndPaymentMethodDropDowns")]
    private async Task PopulateAccountAndPaymentMethodDropDowns()
    {
        var accounts = await _accountService.GetAllAccounts();
        ViewBag.AccountId = new SelectList(accounts, "AccountId", "Username");

        var paymentMethods = await _paymentMethodService.GetAllPaymentMethodsAsync();
        ViewBag.PaymentMethodId = new SelectList(paymentMethods, "PaymentMethodId", "Name");
    }
}