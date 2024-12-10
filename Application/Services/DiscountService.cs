using Application.Interfaces;
using Domain.Entities;
using Domain.Repository;

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
		var checkDiscount = await _discountRepository.GetById(discount.DiscountId);
		if (checkDiscount == null) throw new Exception("Discount not found.");

		checkDiscount.StartOn = discount.StartOn;
		checkDiscount.EndOn = discount.EndOn;
		checkDiscount.Code = discount.Code;
		checkDiscount.GameId = discount.GameId;
		checkDiscount.Percent = discount.Percent;

		await _discountRepository.Update(checkDiscount);
		return checkDiscount;
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

	// Search Code by Game ID
    public async Task<IEnumerable<Discount>> GetDiscountByGameId(int gameId)
	{
		var discountList = await _discountRepository.GetAll();
		discountList = discountList.Where(d => d.GameId == gameId);
		return discountList.ToList();
	}

	// Search Active Discount
	public async Task<IEnumerable<Discount>> GetActiveDiscount()
	{
		var discountList = await _discountRepository.GetAll();
		var filtered = discountList.Where(d => (d.StartOn <= DateTime.Now) && (d.EndOn >= DateTime.Now) );
		return filtered.ToList();
	}

	// Search Expired Discount
	public async Task<IEnumerable<Discount>> GetExpiredDiscount()
	{
		var discountList = await _discountRepository.GetAll();
		discountList = discountList.Where(d => d.EndOn <= DateTime.Now);
		return discountList.ToList();
	}
}