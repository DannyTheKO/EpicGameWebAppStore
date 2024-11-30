using Domain.Entities;

namespace Application.Interfaces;

public interface ICartService
{
    Task<IEnumerable<Cart>> GetAllCarts();
    Task AddCart(Cart cart);
    Task UpdateCart(Cart cart);
    Task DeleteCart(int id);
    Task<Cart> GetCartById(int id);
    Task<IEnumerable<Cart>> GetCartsByAccountId(int accountId);
    Task<(Cart, string Message)> GetLatestCart(int accountId);

    Task<IEnumerable<Cart>> GetCompleteCartByAccountId(int accountId);
    Task<Cart> GetActiveCartByAccountId(int accountId);

    Task<string> GetAccountByCartId(int accountId);
    Task<string> GetPaymentMethodNameById(int paymentMethodId);
    Task<decimal> CalculateTotalAmount(int cartId);
    Task<(Cart, string Message)> AddGameToCart(int accountId, int gameId, int paymentMethodId);
}