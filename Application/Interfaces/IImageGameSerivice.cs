using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
	public interface IImageGameService
	{
		Task<IEnumerable<ImageGame>> GetAllImageGame();
		
		Task<ImageGame> GetImageGameById(int imageGameID);

		Task<IEnumerable<ImageGame>> GetImageGameByGameId(int gameId);

		Task<(ImageGame imageGame, string Message)> UpdateImageGame(IFormFile image, ImageGame imageGame);

		Task<(bool Success, string Message)> DeleteImageGame(int imageGameID);

		Task<(ImageGame imageGame, bool Flag, string Message)> UploadImageGame(IFormFile image, int gameId, string imageType);
	}
}
