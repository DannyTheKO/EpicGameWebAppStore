using Application.Interfaces;
using Domain.Entities;
using Domain.Repository;
// Domain

// Application

namespace Application.Services;

public class GenreService : IGenreService
{
	private readonly IGenreRepository _genreRepository;

	public GenreService(IGenreRepository genreRepository)
	{
		_genreRepository = genreRepository;
	}

	// == Basic CRUD Function ==
	public async Task<IEnumerable<Genre>> GetAllGenres()
	{
		return await _genreRepository.GetAll();
	}

	public async Task<Genre> AddGenre(Genre genre)
	{
		await _genreRepository.Add(genre);
		return genre;
	}

	public async Task<Genre> UpdateGenre(Genre genre)
	{
		await _genreRepository.Update(genre);
		return genre;
	}

	public async Task<Genre> DeleteGenre(int id)
	{
		var genre = await _genreRepository.GetById(id);
		if (genre == null) throw new Exception("Genre not found.");
		await _genreRepository.Delete(id);
		return genre;
	}

	// == Feature Function ==

	// Search by Genre ID
	public async Task<Genre> GetGenreById(int id)
	{
		return await _genreRepository.GetById(id);
	}

	public async Task<IEnumerable<Genre>> GetGenreByName(string name)
	{
		var genreList = await _genreRepository.GetAll();
		var filteredGenre = genreList.Where(f => f.Name == name);
		return filteredGenre;
	}
}
