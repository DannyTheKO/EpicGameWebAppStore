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
            try
            {
                return await _cartRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get all the carts", ex);
            }
        }

        public async Task<Cart> AddCartAsync(Cart cart)
        {
            try
            {
                await _cartRepository.Add(cart);
                return cart;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add the cart", ex);
            }
        }

        public async Task<Cart> UpdateCartAsync(Cart cart)
        {
            try
            {
                await _cartRepository.Update(cart);
                return cart;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update the cart", ex);
            }
        }

        public async Task<Cart> DeleteCartAsync(int id)
        {
            var cart = await _cartRepository.GetById(id);
            if (cart == null)
            {
                throw new Exception("Cart not found.");
            }
            await _cartRepository.Delete(id);
            return cart;
        }

        public async Task<Cart> GetCartByIdAsync(int id)
        {
            try
            {
                return await _cartRepository.GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get specific cart id", ex);
            }
        }

        public async Task<IEnumerable<Cart>> GetCartsByAccountIdAsync(int accountId)
        {
            try
            {
                return await _cartRepository.GetByAccountId(accountId);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get carts for the specified account", ex);
            }
        }
    }
}
