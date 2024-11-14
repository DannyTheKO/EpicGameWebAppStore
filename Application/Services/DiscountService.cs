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
		return await _discountRepository.GetAll();
	}

	public async Task<Discount> AddDiscountAsync(Discount discount)
	{
		await _discountRepository.Add(discount);
		return discount;
	}

	public async Task<Discount> UpdateDiscountAsync(Discount discount)
	{
		await _discountRepository.Update(discount);
		return discount;
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
		return await _discountRepository.GetById(id);
	}

	// Search Code by Discount Code
	public async Task<IEnumerable<Discount>> GetDiscountByCode(string code)
	{
        var discountList = await _discountRepository.GetAll();
        discountList = discountList.Where(d => d.Code == code);
        return discountList.ToList();
	}

    public async Task<IEnumerable<Discount>> GetDiscountByGameId(int gameId)
	{
		var discountList = await _discountRepository.GetAll();
		discountList = discountList.Where(d => d.GameId == gameId);
		return discountList.ToList();
	}
}