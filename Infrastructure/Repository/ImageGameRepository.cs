using DataAccess.EpicGame;
using Domain.Entities;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
	public class ImageGameRepository : IImageGameRepository
	{
		private readonly EpicGameDbContext _context;

		public ImageGameRepository(EpicGameDbContext context)
		{
			_context = context;
		}

		// Get all image game
		public async Task<IEnumerable<ImageGame>> GetAll()
		{
			return await _context.ImageGames.ToListAsync();
		}

		// Get Image Game by ImageGameID
		public async Task<ImageGame> GetById(int imageGameID)
		{
			var imageGameList = await GetAll();
			return imageGameList.SingleOrDefault(ig => ig.ImageGameId == imageGameID);
		}

		// Add Image into database
		public async Task Add(ImageGame imageGame)
		{
			await _context.ImageGames.AddAsync(imageGame);
			await _context.SaveChangesAsync();
		}

		// Delete Image from database
		public async Task Delete(int imageGameID)
		{
			var checkImageGame = await GetById(imageGameID);
			if (checkImageGame != null) //Found
			{
				_context.Remove(checkImageGame);
				await _context.SaveChangesAsync();
			}
		}

		public async Task Update(ImageGame imageGame)
		{
			var checkImageGame = await GetById(imageGame.ImageGameId);
			if (checkImageGame != null)
			{
				_context.Update(checkImageGame);
				await _context.SaveChangesAsync();
			}
		}
	}
}
