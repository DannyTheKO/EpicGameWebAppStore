using Domain.Entities;

// Domain

namespace Application.Interfaces;

public interface IDiscountService
{
	// == Basic CRUD Function ==
	public Task<IEnumerable<Discount>> GetAllDiscountAsync();
	public Task<Discount> AddDiscountAsync(Discount discount);
	public Task<Discount> UpdateDiscountAsync(Discount discount);
	public Task<Discount> DeleteDiscountAsync(int id);

	// == Feature Function ==

	// Search by Discount ID
	public Task<Discount> GetDiscountByIdAsync(int id);

	// Search By Game Discount => Get Game "ID"
}