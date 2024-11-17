using Domain.Entities;

namespace Application.Interfaces;

public interface ICartService
{
    Task<IEnumerable<Cart>> GetAllCarts();
    Task<Cart> AddCart(Cart cart);
    Task<Cart> UpdateCart(Cart cart);
    Task<Cart> DeleteCart(int id);
    Task<Cart> GetCartById(int id);
    Task<IEnumerable<Cart>> GetCartsByAccountId(int accountId);

    Task<string> GetAccountByCartId(int accountId);
    Task<string> GetPaymentMethodNameById(int paymentMethodId);
}