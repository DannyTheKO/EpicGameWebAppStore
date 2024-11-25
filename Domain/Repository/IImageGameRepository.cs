

using Domain.Entities;

namespace Domain.Repository
{
	public interface IImageGameRepository
	{
		Task<IEnumerable<ImageGame>> GetAll();

		Task<ImageGame> GetById(int imageGameID);

		Task Add(ImageGame imageGame);

		Task Delete(int imageGameID);

		Task Update(ImageGame imageGame);
	}
}
