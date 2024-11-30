using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EpicGameWebAppStore.Controllers.EndUser;

[Authorize]
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
		ICartService cartService, 
		IGameService gameService,
		IAuthorizationServices authorizationServices) : base(authorizationServices)
    {
        _accountService = accountService;
        _cartService = cartService;
        _gameService = gameService;
    }

    [HttpGet("GetProfile")]
    public async Task<ActionResult<Account>> GetProfile()
    {
        var currentAccount = GetDetailAccount();
        var profile = await _accountService.GetAccountById(currentAccount.AccountId);
        return Ok(new { success = true, data = profile });
    }

    [HttpGet("GetCartList")]
    public async Task<ActionResult<IEnumerable<Cart>>> GetCartList()
    {
        var currentAccount = GetDetailAccount();
        var carts = await _cartService.GetCartsByAccountId(currentAccount.AccountId);
        return Ok(new { success = true, data = carts });
    }

    [HttpPut("UpdateProfile")]
    public async Task<ActionResult<Account>> UpdateProfile([FromBody] Account updatedAccount)
    {
        var currentAccount = GetDetailAccount();
        updatedAccount.AccountId = currentAccount.AccountId; // Ensure we update the correct account
        
        var result = await _accountService.UpdateAccount(updatedAccount);
        return Ok(new { success = true, data = result });
    }

    //[HttpGet("GetPurchaseHistory")]
    //public async Task<ActionResult<IEnumerable<Cart>>> GetPurchaseHistory()
    //{
    //    var currentAccount = GetDetailAccount();
    //    var purchaseHistory = await _cartService.GetCompletedCartsByUserId(currentAccount.AccountId);
    //    return Ok(new { success = true, data = purchaseHistory });
    //}

    [HttpGet("GetOwnedGames")]
    public async Task<ActionResult<IEnumerable<Game>>> GetOwnedGames()
    {
        var currentAccount = GetDetailAccount();
        var ownedGames = await _accountGameService.GetAccountGameByAccountId(currentAccount.AccountId);
        return Ok(new { success = true, data = ownedGames });
    }

    [HttpGet("GetActiveCart")]
    public async Task<ActionResult<Cart>> GetActiveCart()
    {
        var currentAccount = GetDetailAccount();
        var activeCart = await _cartService.GetLatestCart(currentAccount.AccountId);
        return Ok(new { success = true, data = activeCart });
    }

    [HttpGet("GetCartDetails/{cartId}")]
    public async Task<ActionResult<IEnumerable<Cart>>> GetCartDetails(int cartId)
    {
        var currentAccount = GetDetailAccount();
        var cartDetails = await _cartService.GetCartById(cartId);

		if (cartDetails == null)
            return NotFound(new { success = false, message = "Cart not found or unauthorized" });

        return Ok(new { success = true, data = cartDetails });
    }

}

