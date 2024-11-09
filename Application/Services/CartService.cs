using Application.Interfaces;
using Domain.Entities;
using Domain.Repository;

namespace Application.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartdetailRepository _cartdetailRepository;

    public CartService(ICartRepository cartRepository, ICartdetailRepository cartdetailRepository)
    {
        _cartRepository = cartRepository;
        _cartdetailRepository = cartdetailRepository;
    }

    public async Task<IEnumerable<Cart>> GetAllCartsAsync()
    {
        return await _cartRepository.GetAll();
    }

    public async Task<Cart> AddCartAsync(Cart cart)
    {
        await _cartRepository.Add(cart);
        return cart;
    }

    public async Task<Cart> UpdateCartAsync(Cart cart)
    {
        await _cartRepository.Update(cart);
        return cart;
    }

    public async Task<Cart> DeleteCartAsync(int id)
    {
        var cart = await _cartRepository.GetById(id);
        if (cart == null) throw new Exception("Cart not found.");
        await _cartRepository.Delete(id);
        return cart;
    }

    public async Task<Cart> GetCartByIdAsync(int id)
    {
        return await _cartRepository.GetById(id);
    }

    public async Task<IEnumerable<Cart>> GetCartsByAccountIdAsync(int accountId)
    {
        return await _cartRepository.GetByAccountId(accountId);
    }

    public async Task<string> GetAccountNameByIdAsync(int accountId)
    {
        var account = await _cartRepository.GetCartById(accountId);
        return account?.Username ?? throw new Exception("Account not found.");
    }

    public async Task<string> GetPaymentMethodNameByIdAsync(int paymentMethodId)
    {
        var paymentMethod = await _cartRepository.GetPaymentMethodById(paymentMethodId);
        return paymentMethod?.Name ?? throw new Exception("Payment method not found.");
    }

    #region Services Function

    //Calculate Total Amount from the AccountId and CartId
    //public async Task<(int amount, string message)> GetTotalAmount(int accountId, int cartId)
    //{
    //    var accountCartDetail = await _cartdetailRepository.GetByCartId(cartId);
    //    var getAccountId = await _cartRepository.GetById(accountId);
    //    if (accountCartDetail == null) // NOT FOUND
    //    {
    //        return (0, "NOT FOUND");
    //    }

    //    var totalAmount = accountCartDetail
    //        .Where(a => a.)
    //        .Sum(g => g.Price);
    //}

    #endregion

}