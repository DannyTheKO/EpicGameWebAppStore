using Application.Interfaces;
using Domain.Entities;
using Domain.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using ZstdSharp;

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

		public async Task<IEnumerable<ImageGame>> GetImageGameByGameId(int gameId)
		{
			var imageGameList = await _imageGameRepository.GetAll();
			return imageGameList.Where(ig => ig.GameId == gameId);

		}

		public async Task<(bool Success, string Message)> DeleteImageGame(int imageGameID)
		{
			try
			{
				var imageGame = await _imageGameRepository.GetById(imageGameID);
				if (imageGame == null)
				{
					return (false, "Image Game not found!");
				}

				// Delete physical file if it exists
				if (!string.IsNullOrEmpty(imageGame.FilePath))
				{
					var fullPath = Path.Combine(Directory.GetCurrentDirectory(), imageGame.FilePath.TrimStart('/'));
					if (File.Exists(fullPath))
					{
						File.Delete(fullPath);
					}
				}

				await _imageGameRepository.Delete(imageGameID);
				return (true, "Image deleted successfully");
			}
			catch (Exception ex)
			{
				return (false, $"Failed to delete image: {ex.Message}");
			}
		}


		public async Task<(ImageGame imageGame, string Message)> UpdateImageGame(IFormFile image, ImageGame imageGame)
		{
			// Check if ImageGame is in the database
			var checkImageGame = await _imageGameRepository.GetById(imageGame.ImageGameId);
			if (checkImageGame == null)
			{
				return (null, "Image Game not found!");
			}

			// Check if game exists
			var game = await _gameRepository.GetById(imageGame.GameId);
			if (game == null)
			{
				return (null, "Game not found!");
			}

			// Validate image type limit
			if (!await ValidateImageTypeLimit(imageGame.GameId, imageGame.ImageType))
			{
				return (null, $"Only one {imageGame.ImageType} image is allowed per game");
			}

			// Create directory path for new location
			var gameFolder = game.Title.Replace(" ", "_").ToLower();
			var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "epic", "public", "images", gameFolder, imageGame.ImageType);
			Directory.CreateDirectory(uploadsFolder);

			// Delete old image from its original location
			if (!string.IsNullOrEmpty(checkImageGame.FilePath))
			{
				var oldFilePath = Path.Combine(
					Directory.GetCurrentDirectory(),
					checkImageGame.FilePath.TrimStart('/')
				);

				if (File.Exists(oldFilePath))
				{
					File.Delete(oldFilePath);
				}
			}

			// Generate new filename using existing ImageId
			var fileName = $"{checkImageGame.ImageGameId}{Path.GetExtension(image.FileName)}";
			var filePath = Path.Combine(uploadsFolder, fileName);

			// Save new file
			await using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await image.CopyToAsync(stream);
			}

			// Update image properties
			checkImageGame.FileName = fileName;
			checkImageGame.FilePath = $"/epic/public/images/{gameFolder}/{imageGame.ImageType}/{fileName}";
			checkImageGame.ImageType = imageGame.ImageType;
			checkImageGame.GameId = imageGame.GameId;

			await _imageGameRepository.Update(checkImageGame);

			return (checkImageGame, "Image updated successfully");
		}


		public async Task<(ImageGame imageGame, bool Flag, string Message)> UploadImageGame(IFormFile image, int gameId, string imageType)
		{
			// Check if that game is existing
			var game = await _gameRepository.GetById(gameId);
			if (game == null) return (null, false, "Game not found!");

			// Validate image type limit
			if (!await ValidateImageTypeLimit(gameId, imageType))
			{
				return (null, false, $"Only one {imageType} image is allowed per game");
			}

			// Create directory if the game folder does not exist
			var gameFolder = game.Title.Replace(" ", "_").ToLower();
			var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "epic", "public", "images", gameFolder, imageType);
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
			imageGame.FilePath = $"/epic/public/images/{gameFolder}/{imageType}/{fileName}";

			// Update the entity
			await _imageGameRepository.Update(imageGame);

			return (imageGame, true, "Add Image Successfully");
		}

		private async Task<bool> ValidateImageTypeLimit(int gameId, string imageType)
		{
			var existingImages = await GetImageGameByGameId(gameId);

			switch (imageType.ToLower())
			{
				case "thumbnail":
				case "banner":
				case "background":
					// Check if image of this type already exists
					if (existingImages.Any(i => i.ImageType.ToLower() == imageType.ToLower()))
					{
						return false;
					}
					break;
				case "screenshot":
					// Screenshots are unlimited
					return true;
				default:
					return false;
			}

			return true;
		}

	}
}
