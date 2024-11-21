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
    Task<Cart> GetLatestCart(int accountId);

    Task<string> GetAccountByCartId(int accountId);
    Task<string> GetPaymentMethodNameById(int paymentMethodId);
    Task<decimal> CalculateTotalAmount(int cartId);
}