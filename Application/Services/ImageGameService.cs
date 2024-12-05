using Application.Interfaces;
using Domain.Entities;
using Domain.Repository;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
	public class ImageGameService : IImageGameService
	{
		private readonly IImageGameRepository _imageGameRepository;
		private readonly IGameRepository _gameRepository;

		public ImageGameService(IImageGameRepository imageGameRepository, IGameRepository gameRepository)
		{
			_imageGameRepository = imageGameRepository;
			_gameRepository = gameRepository;
		}

		public async Task<IEnumerable<ImageGame>> GetAllImageGame()
		{
			return await _imageGameRepository.GetAll();
		}

		public async Task<ImageGame> GetImageGameById(int imageGameID)
		{
			return await _imageGameRepository.GetById(imageGameID);
		}

		public async Task DeleteImageGame(int imageGameID)
		{
			var checkImageGame = await _imageGameRepository.GetById(imageGameID);
			if (checkImageGame == null) throw new Exception("Image Game not found!");
			
			await _imageGameRepository.Delete(imageGameID);
		}

		// Service

		public async Task<(ImageGame imageGame, bool Flag)> UploadImageGame(IFormFile image, int gameId, string imageType)
		{
			// Check if that game is existing
			var game = await _gameRepository.GetById(gameId);
			if (game == null) return (null, false);

			// Create directory if the game folder does not exist
			var gameFolder = game.Title.Replace(" ", "_").ToLower();
			var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Images", gameFolder, imageType);
			Directory.CreateDirectory(uploadsFolder);

			// Create ImageGame first to get the ID
			var imageGame = new ImageGame()
			{
				GameId = gameId,
				CreatedOn = DateTime.UtcNow,
				FileName = "N/A", // don't remove this line or the database will throw an error
				ImageType = imageType
			};

			// Add to database to generate ImageId
			await _imageGameRepository.Add(imageGame);

			// Generate File name using ImageId
			var fileName = $"{imageGame.ImageGameId}{Path.GetExtension(image.FileName)}";
			var filePath = Path.Combine(uploadsFolder, fileName);

			// Save file to directory
			await using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await image.CopyToAsync(stream);
			}

			// Update ImageGame with file info
			imageGame.FileName = fileName;
			imageGame.FilePath = $"/Images/{gameFolder}/{imageType}/{fileName}";

			// Update the entity
			await _imageGameRepository.Update(imageGame);

			return (imageGame, true);
		}

		public Task<IEnumerable<ImageGame>> GetImageGameByGameId(int gameId)
		{
			throw new NotImplementedException();
		}
	}
}
