﻿import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import '../styles/pages/StorePage.css';
import { FaSearch } from 'react-icons/fa';

const StorePage = () => {
    const [games, setGames] = useState([]);
    const [searchTerm, setSearchTerm] = useState('');
    const [selectedGenre, setSelectedGenre] = useState('all');
    const [genres, setGenres] = useState([]);
    const [currentPage, setCurrentPage] = useState(1);
    const gamesPerPage = 15;

    // Fetch games from API
    const fetchGames = async () => {
        try {
            const response = await fetch('http://localhost:5084/Store/FeaturePage/GetAll');
            const data = await response.json();

            // Update images for each game
            const updatedGames = data.map((game) => {
                // Find the Thumbnail image
                const thumbnailImage = game.imageGame.find(image => image.imageType.toLowerCase() === 'thumbnail');

                // Build the path for the Thumbnail image
                const thumbnailPath = thumbnailImage
                    ? `${process.env.PUBLIC_URL}${thumbnailImage.filePath}${thumbnailImage.fileName}`
                    : '';

                return { ...game, thumbnailPath };
            });

            setGames(updatedGames);
        } catch (error) {
            console.error("Error fetching games:", error);
        }
    };

    // Fetch genres from API or define a static list
    const fetchGenres = async () => {
        try {
            const response = await fetch('http://localhost:5084/Store/FeaturePage/GetAllGenre');
            const data = await response.json();
            setGenres(data);
        } catch (error) {
            console.error("Error fetching genres:", error);
            setGenres([
                { genreId: 1, name: "Action" },
                { genreId: 2, name: "Adventure" },
                { genreId: 3, name: "Role-Playing" },
                { genreId: 4, name: "Simulation" },
                { genreId: 5, name: "Strategy" },
                { genreId: 6, name: "Sports" },
                { genreId: 7, name: "Shooter" },
                { genreId: 8, name: "Platformer" },
                { genreId: 9, name: "Horror" },
                { genreId: 10, name: "Fighting" },
                { genreId: 11, name: "Educational" },
                { genreId: 12, name: "Music" }
            ]);
        }
    };

    useEffect(() => {
        fetchGames();
        fetchGenres();
    }, []);

    // Filter games by search term and genre
    const filteredGames = games.filter(game => {
        const matchesTitle = game.title.toLowerCase().includes(searchTerm.toLowerCase());
        const matchesGenre = selectedGenre === 'all' || game.genreId === parseInt(selectedGenre);
        return matchesTitle && matchesGenre;
    });

    // Pagination logic
    const startIndex = (currentPage - 1) * gamesPerPage;
    const endIndex = startIndex + gamesPerPage;
    const paginatedGames = filteredGames.slice(startIndex, endIndex);

    // Calculate total pages
    const totalPages = Math.ceil(filteredGames.length / gamesPerPage);

    // Handle page change
    const handlePageChange = (pageNumber) => {
        setCurrentPage(pageNumber);
    };

    return (
        <div className="store-page">
            <h2>All Games</h2>
            <div className="filter-container">
                <div className="search-container">
                    <input
                        type="text"
                        placeholder="Search for a game..."
                        value={searchTerm}
                        onChange={(e) => setSearchTerm(e.target.value)}
                    />
                    <FaSearch className="search-icon" />
                </div>
                <div className="genre-container">
                    <label htmlFor="genre">Genre:</label>
                    <select id="genre" value={selectedGenre} onChange={(e) => setSelectedGenre(e.target.value)}>
                        <option value="all">All Genres</option>
                        {genres.map(genre => (
                            <option key={genre.genreId} value={genre.genreId}>
                                {genre.name}
                            </option>
                        ))}
                    </select>
                </div>
            </div>

            <div className="games-list">
                {paginatedGames.length === 0 ? (
                    <div>No games found for this genre.</div>
                ) : (
                    paginatedGames.map(game => (
                        <Link to={`/game/${game.gameId}`} key={game.gameId}>
                            <div className="game-item">
                                {game.thumbnailPath && (
                                    <img
                                        src={game.thumbnailPath}
                                        alt={game.title}
                                        className="game-cover"
                                    />
                                )}
                                <h3>{game.title}</h3>
                                <p>${game.price.toFixed(2)}</p>
                            </div>
                        </Link>
                    ))
                )}
            </div>

            <div className="pagination">
                {Array.from({ length: totalPages }, (_, index) => (
                    <button
                        key={index + 1}
                        onClick={() => handlePageChange(index + 1)}
                        className={currentPage === index + 1 ? 'active' : ''}
                    >
                        {index + 1}
                    </button>
                ))}
            </div>
        </div>
    );
};

export default StorePage;
