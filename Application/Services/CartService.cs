using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repository;
using Application.Interfaces;

namespace Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
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
            var account = await _cartRepository.GetAccountById(accountId);
            return account?.Username ?? throw new Exception("Account not found.");
        }

        public async Task<string> GetPaymentMethodNameByIdAsync(int paymentMethodId)
        {
            var paymentMethod = await _cartRepository.GetPaymentMethodById(paymentMethodId);
            return paymentMethod?.Name ?? throw new Exception("Payment method not found.");
        }
    }
}
