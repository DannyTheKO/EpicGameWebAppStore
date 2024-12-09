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
    Task<Discount> GetDiscountByIdAsync(int id);

	// Search Game available discount
    Task<IEnumerable<Discount>> GetDiscountByGameId(int gameId);

	// Search by Discount Code
	Task<IEnumerable<Discount>> GetDiscountByCode(string code);

    // Search Active Discount
    Task<IEnumerable<Discount>> GetActiveDiscount();

    // Search Expired Discount
    Task<IEnumerable<Discount>> GetExpiredDiscount();
}