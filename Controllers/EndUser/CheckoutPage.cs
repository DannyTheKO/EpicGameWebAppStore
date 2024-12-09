using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using EpicGameWebAppStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EpicGameWebAppStore.Controllers.EndUser;

[Authorize]
[ApiController]
[Route("Store/[controller]")]
public class CheckoutPage : _BaseController
{
	private readonly ICartService _cartService;
	private readonly ICartdetailService _cartdetailService;
	private readonly IAuthorizationServices _authorizationServices;
    private readonly IAccountService _accountService;
    private readonly IGameService _gameService;
    private readonly IAccountGameService _accountGameService;

    public CheckoutPage(ICartService cartService,
		ICartdetailService cartdetailService,
		IAuthorizationServices authorizationServices,
        IAccountService accountService,
        IGameService gameService,
        IAccountGameService accountGameService)
		: base(authorizationServices)
	{
		_cartService = cartService;
		_cartdetailService = cartdetailService;
        _accountService = accountService;
        _gameService = gameService;
        _accountGameService = accountGameService;
    }

	[HttpGet("CurrentCartList")]
	public async Task<ActionResult<Cart>> GetCurrentCartList()
	{
		var carts = await _cartService.GetActiveCartByAccountId(GetCurrentDetailAccount().AccountId);
		if (carts == null)
		{
			return NotFound(new
			{
				success = false,
				message = "This Account don't have a current active cart"
			});
		}
		return Ok(carts);
	}

	[HttpPost("CompleteCheckout")]
	public async Task<ActionResult> CompleteCheckout(int cartId, int paymentMethodId)
	{
		var cart = await _cartService.GetCartsByAccountId(GetCurrentDetailAccount().AccountId);
		var activeCart = cart.FirstOrDefault(c => c.CartStatus == "Active");
		if (activeCart == null)
			return NotFound(new { success = false, message = "Cart not found" });

		if (activeCart.AccountId != GetCurrentDetailAccount().AccountId) // Just in case someone tries to access other's cart
			return Unauthorized(new { success = false, message = "Unauthorized access to cart" });

		await _cartService.CompleteCheckout(cartId, paymentMethodId);
		return Ok(new { success = true, message = "Checkout completed successfully" });
	}

	[HttpDelete("RemoveCartItem/{cartDetailId}")]
	public async Task<ActionResult> RemoveCartItem(int cartDetailId)
	{

		var cartDetail = await _cartdetailService.GetCartDetailById(cartDetailId);
		if (cartDetail == null)
			return NotFound(new { success = false, message = "Cart item not found" });

		var cart = await _cartService.GetCartById(cartDetail.CartId);
		if (cart.AccountId != GetCurrentDetailAccount().AccountId)
			return Unauthorized(new { success = false, message = "Unauthorized access to cart" });

		await _cartdetailService.DeleteCartDetail(cartDetailId);
		return Ok(new { success = true, message = "Item removed from cart successfully" });
	}

	[HttpPut("UpdateCartItem/{cartDetailId}")]
	public async Task<ActionResult> UpdateCartItem(int cartDetailId, [FromBody] Cartdetail cartDetail)
	{
		var cartDetailToUpdate = await _cartdetailService.GetCartDetailById(cartDetailId);
		if (cartDetailToUpdate == null)
			return NotFound(new { success = false, message = "Cart item not found" });
		
		var cart = await _cartService.GetCartById(cartDetailToUpdate.CartId);
		if (cart.AccountId != GetCurrentDetailAccount().AccountId) // Just in case
			return Unauthorized(new { success = false, message = "Unauthorized access to cart" });
		
		cartDetailToUpdate.Price = cartDetail.Price;
		cartDetailToUpdate.Discount = cartDetail.Discount;
		
		await _cartdetailService.UpdateCartDetail(cartDetailToUpdate);
		return Ok(new { success = true, message = "Item updated successfully" });
	}

    [HttpPost("AddAccountGame")]
    public async Task<ActionResult<AccountGameFormModel>> AddAccountGame([FromBody] AccountGameFormModel accountGameFormModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                success = false,
                errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
            });
        }

        // Retrieve both Account, Game and AccountGame data
        var checkAccount = await _accountService.GetAccountById(accountGameFormModel.AccountId);
        var checkGame = await _gameService.GetGameById(accountGameFormModel.GameId);

        // Check if both Account and game exist 
        if (checkAccount == null)
        {
            return BadRequest(new
            {
                success = false,
                message = "Account is not found!"
            });
        }

        if (checkGame == null)
        {
            return BadRequest(new
            {
                success = false,
                message = "Game is not found!"
            });
        }

        // Check if that Account already has that game
        var checkAccountGame = (await _accountGameService.GetAccountGameByAccountId(accountGameFormModel.AccountId))
            .Where(g => g.GameId == checkGame.GameId);

        if (checkAccountGame.Any())
        {
            return BadRequest(new
            {
                success = false,
                message = "That Account already has that Game."
            });
        }


        // Create new Account Game
        var accountGame = new AccountGame
        {
            AccountId = checkAccount.AccountId,
            GameId = checkGame.GameId,
            DateAdded = DateTime.UtcNow,
        };

        await _accountGameService.AddAccountGame(accountGame);
        return Ok(new
        {
            success = true,
            message = "Successfully added AccountGame",
            data = accountGame,
        });

    }

}

