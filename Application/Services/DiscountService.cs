using Application.Interfaces;
using Domain.Entities;
using Domain.Repository;

// Domain

// Application

namespace Application.Services;

public class DiscountService : IDiscountService
{
	private readonly IDiscountRepository _discountRepository;

	public DiscountService(IDiscountRepository discountRepository)
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
		if (discount == null) throw new Exception("Discount not found");

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