using Application.Interfaces;
using DataAccess.EpicGame;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EpicGameWebAppStore.Controllers;

[Route("Cartdetail")]
public class CartdetailController : Controller
{
	private readonly ICartdetailService _cartdetailService;

	public CartdetailController(ICartdetailService cartdetailService)
	{
		_cartdetailService = cartdetailService;
	}

	// GET: Cartdetail/Index
	[HttpGet]
	public async Task<IActionResult> Index()
	{
		return View(await _cartdetailService.GetAllCartdetails());
	}

	// GET: Cartdetail/details/5
	[HttpGet("Detail/{id}")]
	public async Task<IActionResult> Details(int id)
	{
		if (id == null) return NotFound();

		var cartDetail = await _cartdetailService.GetCartdetailById(id);

		if (cartDetail == null) return NotFound();

		return View(cartDetail);
	}

	// GET: Cartdetail/Create
	[HttpGet("Create")]
	public async Task<IActionResult> Create()
	{
		ViewData["CartId"] = new SelectList(await _cartdetailService.GetAllCartdetails(), "CartId", "CartId");
		ViewData["GameId"] = new SelectList(await _cartdetailService.GetAllCartdetails(), "GameId", "Title");
		return View();
	}

	// POST: Cartdetail/Create
	[HttpPost("Create")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(
		[Bind("CartDetailId,CartId,GameId,Quantity,Price,Discount")] Cartdetail cartdetail)
	{
		if (ModelState.IsValid)
		{
			_cartdetailService.AddCartdetail(cartdetail);
			return RedirectToAction(nameof(Index));
		}

		ViewData["CartId"] = new SelectList(await _cartdetailService.GetAllCartdetails(), "CartId", "CartId", cartdetail.CartId);
		ViewData["GameId"] = new SelectList(await _cartdetailService.GetAllCartdetails(), "GameId", "Title", cartdetail.GameId);
		return View(cartdetail);
	}

	// GET: cartdetails/edit/5
	[HttpGet("edit/{id}")]
	public async Task<IActionResult> Edit(int id)
	{
		if (id == null) return NotFound();

		var cartdetail = await _cartdetailService.GetCartdetailById(id);
		if (cartdetail == null) return NotFound();
		ViewData["CartId"] = new SelectList(await _cartdetailService.GetAllCartdetails(), "CartId", "CartId", cartdetail.CartId);
		ViewData["GameId"] = new SelectList(await _cartdetailService.GetAllCartdetails(), "GameId", "Title", cartdetail.GameId);
		return View(cartdetail);
	}

	// POST: cartdetails/edit/5
	[HttpPost("edit/{id}")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(int id,
		[Bind("CartDetailId,CartId,GameId,Quantity,Price,Discount")] Cartdetail cartdetail)
	{
		var cartDetailExisting = await _cartdetailService.GetCartdetailById(cartdetail.CartId);
		if (cartDetailExisting != null) // FOUND
		{
			await _cartdetailService.UpdateCartdetail(cartdetail);
		}

		ViewData["CartId"] = new SelectList(await _cartdetailService.GetAllCartdetails(), "CartId", "CartId", cartdetail.CartId);
		ViewData["GameId"] = new SelectList(await _cartdetailService.GetAllCartdetails(), "GameId", "Title", cartdetail.GameId);
		return View(cartdetail);
	}

	// GET: cartdetails/delete/5
	[HttpGet("delete/{id}")]
	public async Task<IActionResult> Delete(int id)
	{
		if (id == null) return NotFound();

		var cartDetail = await _cartdetailService.GetCartdetailById(id);

		if (cartDetail == null) return NotFound();

		return View(cartDetail);
	}

	// POST: cartdetails/delete/5
	[HttpPost("delete/{id}")]
	[ActionName("Delete")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteConfirmed(int id)
	{
		var cartDetail = await _cartdetailService.DeleteCartdetail(id);
		return RedirectToAction(nameof(Index));
	}

	// Populate ViewBag
	//private async Task PopulateViewBag()
	//{
	//	var cartDetail = await _cartdetailService.GetAllCartdetails();
	//	ViewBag.CartId = new SelectList(cartDetail, "CartDetailId", "");
		
	//}

	//private async Task PopulateAccountAndPaymentMethodDropDowns()
	//{
	//	var accounts = await _accountService.GetAllAccounts();
	//	ViewBag.AccountId = new SelectList(accounts, "AccountId", "Username");

	//	var paymentMethods = await _paymentMethodService.GetAllPaymentMethodsAsync();
	//	ViewBag.PaymentMethodId = new SelectList(paymentMethods, "PaymentMethodId", "Name");
	//}
}