using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EpicGameWebAppStore.Controllers.EndUser;

[Authorize]
[ApiController]
[Route("Profile/[controller]")]
public class ProfileUser : _BaseController
{
	private readonly IAccountService _accountService;
	private readonly IAccountGameService _accountGameService;
	private readonly ICartService _cartService;
	private readonly ICartdetailService _cartdetailService;
	private readonly IGameService _gameService;
	private readonly IAuthorizationServices _authorizationServices;

	public ProfileUser(
		IAccountService accountService,
		IAccountGameService accountGameService,
		ICartService cartService,
		ICartdetailService cartdetailService,
		IGameService gameService,
		IAuthorizationServices authorizationServices) 
		: base(authorizationServices)
	{
		_accountService = accountService;
		_accountGameService = accountGameService;
		_cartService = cartService;
		_cartdetailService = cartdetailService;
		_gameService = gameService;
		_authorizationServices = authorizationServices;
	}

    [HttpGet("GetProfile")]
    public async Task<ActionResult<Account>> GetProfile()
    {
        var currentAccount = GetCurrentDetailAccount();
        var profile = await _accountService.GetAccountById(currentAccount.AccountId);
        return Ok(new { success = true, data = profile });
    }

    [HttpGet("GetCartList")]
    public async Task<ActionResult> GetCartList()
    {
        var currentAccount = GetCurrentDetailAccount();
        var carts = await _cartService.GetCartsByAccountId(currentAccount.AccountId);

        var formattedResponse = new
        {
            accountId = currentAccount.AccountId,
            name = currentAccount.Username,
            cart = carts.Select(c => new
            {
                cartId = c.CartId,
                cartStatus = c.CartStatus,
                paymentMethodId = c.PaymentMethodId,
                paymentMethod = c.PaymentMethod.Name,
                createdAt = c.CreatedOn?.ToString("yyyy-MM-dd HH:mm:ss") ?? "N/A",// Nếu null, trả về "N/A"
                total = c.TotalAmount,
                cartDetails = c.Cartdetails.Select(cd => new
                {
                    cartDetailId = cd.CartDetailId,
                    cartDetail = new
                    {
                        gameId = cd.Game.GameId,
                        title = cd.Game.Title,
                        price = cd.Price,
                        discount = cd.Discount
                    }
                }).ToList()
            }).ToList()
        };

        return Ok(new { success = true, data = formattedResponse });
    }




    [HttpPut("UpdateProfile")]
    public async Task<ActionResult<Account>> UpdateProfile([FromBody] Account updatedAccount)
    {
        var currentAccount = GetCurrentDetailAccount();
        updatedAccount.AccountId = currentAccount.AccountId; // Ensure we update the correct account
        
        var result = await _accountService.UpdateAccount(updatedAccount);
        return Ok(new { success = true, data = result });
    }

    [HttpGet("GetPurchaseHistory")]
    public async Task<ActionResult<IEnumerable<Cart>>> GetPurchaseHistory()
    {
        var currentAccount = GetCurrentDetailAccount();
        var purchaseHistory = await _cartService.GetCompleteCartByAccountId(currentAccount.AccountId);
        return Ok(new { success = true, data = purchaseHistory });
    }

    [HttpGet("GetOwnedGames")]
    public async Task<ActionResult<IEnumerable<Game>>> GetOwnedGames()
    {
        var currentAccount = GetCurrentDetailAccount();
        var ownedGames = await _accountGameService.GetAccountGameByAccountId(currentAccount.AccountId);
        return Ok(new { success = true, data = ownedGames });
    }

    [HttpGet("GetActiveCart")]
    public async Task<ActionResult<Cart>> GetActiveCart()
    {
        var currentAccount = GetCurrentDetailAccount();
        var activeCart = await _cartService.GetLatestCart(currentAccount.AccountId);
        return Ok(new { success = true, data = activeCart });
    }

    [HttpGet("GetCartDetails/{cartId}")]
    public async Task<ActionResult<IEnumerable<Cart>>> GetCartDetails(int cartId)
    {
        var currentAccount = GetCurrentDetailAccount();
        var cartDetails = await _cartService.GetCartById(cartId);

		if (cartDetails == null)
            return NotFound(new { success = false, message = "Cart not found or unauthorized" });

        return Ok(new { success = true, data = cartDetails });
    }

}

