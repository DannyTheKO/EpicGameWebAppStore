using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EpicGameWebAppStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IAccountService _accountService; // Giả sử bạn có dịch vụ này
        private readonly IPaymentMethodService _paymentMethodService; // Giả sử bạn có dịch vụ này

        public CartController(ICartService cartService, IAccountService accountService, IPaymentMethodService paymentMethodService)
        {
            _cartService = cartService;
            _accountService = accountService;
            _paymentMethodService = paymentMethodService;
        }

        public async Task<IActionResult> Index()
        {
            var carts = await _cartService.GetAllCartsAsync();
            return View(carts);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateAccountAndPaymentMethodDropDowns();
            return View(new Cart());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cart cart)
        {
            if (ModelState.IsValid)
            {
                await _cartService.AddCartAsync(cart);
                TempData["Message"] = "Cart created successfully.";
                return RedirectToAction(nameof(Index));
            }
            await PopulateAccountAndPaymentMethodDropDowns();
            return View(cart);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var cart = await _cartService.GetCartByIdAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            await PopulateAccountAndPaymentMethodDropDowns();
            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cart cart)
        {
            if (id != cart.CartId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _cartService.UpdateCartAsync(cart);
                TempData["Message"] = "Cart updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            await PopulateAccountAndPaymentMethodDropDowns();
            return View(cart);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var cart = await _cartService.GetCartByIdAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            return View(cart);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _cartService.DeleteCartAsync(id);
            TempData["Message"] = "Cart deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetByAccount(int accountId)
        {
            var carts = await _cartService.GetCartsByAccountIdAsync(accountId);
            if (carts == null)
            {
                return NotFound();
            }
            return View("Index", carts);
        }

        private async Task PopulateAccountAndPaymentMethodDropDowns()
        {
            var accounts = await _accountService.GetAllAccountsAsync(); // Giả sử có phương thức này
            ViewBag.AccountId = new SelectList(accounts, "AccountId", "AccountName"); // Thay đổi theo tên của thuộc tính tài khoản

            var paymentMethods = await _paymentMethodService.GetAllPaymentMethodsAsync(); // Giả sử có phương thức này
            ViewBag.PaymentMethodId = new SelectList(paymentMethods, "PaymentMethodId", "PaymentMethodName"); // Thay đổi theo tên của thuộc tính phương thức thanh toán
        }
    }
}
