using Application.Interfaces;
using Domain.Entities;
using Domain.Repository;
using Org.BouncyCastle.Asn1.Cmp;
using ZstdSharp.Unsafe;

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
		var existingGenre = await _genreRepository.GetById(genre.GenreId);
		if (existingGenre == null) throw new Exception("Genre Not Found!");


		// Update Genre Value
		existingGenre.GenreId = genre.GenreId;
		existingGenre.Name = genre.Name;

		await _genreRepository.Update(existingGenre);
		return existingGenre;
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

	public async Task<IEnumerable<Genre>> GetGenreByName(string genreName)
	{
		var genreList = await _genreRepository.GetAll();
		var filteredGenre = genreList.Where(f => f.Name == genreName);
		return filteredGenre;
	}
}
