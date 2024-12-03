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
		Task<(string Message, bool Flag)> AddImageGame(ImageGame imageGame);

		Task<IEnumerable<ImageGame>> GetAllImageGame();
		
		Task<ImageGame> GetImageGameById(int imageGameID);

		Task<IEnumerable<ImageGame>> GetImageGameByGameId(int gameId);

		Task DeleteImageGame(int imageGameID);

		Task<(ImageGame imageGame, bool Flag)> UploadImageGame(IFormFile image, int gameId, string imageType);
	}
}
