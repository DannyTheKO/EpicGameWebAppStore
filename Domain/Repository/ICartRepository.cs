using Domain.Entities;

namespace Domain.Repository;

public interface ICartRepository
{
    Task<IEnumerable<Cart>> GetAll();
    Task<Cart> GetById(int id);
    Task Add(Cart cart);
    Task Update(Cart cart);
    Task Delete(int id);
    Task<IEnumerable<Cart>> GetByAccountId(int accountId);
    Task<Account> GetAccountById(int accountId);
    Task<Paymentmethod> GetPaymentMethodById(int paymentMethodId);
}