using Domain.Entities;

namespace Domain.Repository;

public interface ICartRepository
{
    Task<IEnumerable<Cart>> GetAll();
    Task<Cart> GetById(int id);
    Task Add(Cart cart);
    Task Update(Cart cart);
    Task Delete(int id);
    Task<IEnumerable<Cart>> GetAllCartFromAccountId(int accountId);
    Task<Account> GetAccountByCartId(int accountId);
    Task<Paymentmethod> GetPaymentMethodById(int paymentMethodId);
}