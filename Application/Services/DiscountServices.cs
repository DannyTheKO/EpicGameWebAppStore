using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Domain
using EpicGameWebAppStore.Domain.Entities;
using EpicGameWebAppStore.Domain.Repository;

// Application
using EpicGameWebAppStore.Application.Interfaces;
using Org.BouncyCastle.Asn1.Mozilla;

namespace Application.Services
{
    public class DiscountServices : IDiscountServices
    {
        private readonly IDiscountRepository _discountRepository;

        public DiscountServices(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        // == Basic CRUD Function ==
        public async Task<IEnumerable<Discount>> GetAllDiscountAsync()
        {
            try
            {
                return await _discountRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get all the discounts", ex);
            }
        }

        public async Task<Discount> AddDiscountAsync(Discount discount)
        {
            try
            {
                await _discountRepository.Add(discount);
                return discount;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add the discount", ex);
            }
        }

        public async Task<Discount> UpdateDiscountAsync(Discount discount)
        {
            try
            {
                await _discountRepository.Update(discount);
                return discount;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update the discount", ex);
            }
        }

        public async Task<Discount> DeleteDiscountAsync(int id)
        {
            var discount = await _discountRepository.GetById(id);
            if (discount == null)
            {
                throw new Exception("Discount not found");
            }

            await _discountRepository.Delete(id);
            return discount;
        }

        // == Feature Function ==

        // Search by Discount ID
        public async Task<Discount> GetDiscountByIdAsync(int id)
        {
            try
            {
                return await _discountRepository.GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get the discount", ex);
            }
        }

    }
}
