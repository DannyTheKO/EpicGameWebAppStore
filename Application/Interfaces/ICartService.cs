using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface ICartService
    {
        Task<IEnumerable<Cart>> GetAllCartsAsync();
        Task<Cart> AddCartAsync(Cart cart);
        Task<Cart> UpdateCartAsync(Cart cart);
        Task<Cart> DeleteCartAsync(int id);
        Task<Cart> GetCartByIdAsync(int id);
        Task<IEnumerable<Cart>> GetCartsByAccountIdAsync(int accountId);

        Task<string> GetAccountNameByIdAsync(int accountId);
        Task<string> GetPaymentMethodNameByIdAsync(int paymentMethodId);
    }
}
