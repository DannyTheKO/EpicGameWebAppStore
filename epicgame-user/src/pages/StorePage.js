import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import '../styles/pages/StorePage.css';
import { FaSearch } from 'react-icons/fa';

const StorePage = () => {
    const [games, setGames] = useState([]);
    const [searchTerm, setSearchTerm] = useState('');
    const [selectedGenre, setSelectedGenre] = useState('all');
    const [currentPage, setCurrentPage] = useState(1);
    const gamesPerPage = 15;

    const genres = {
        all: null,
        action: 1,
        adventure: 2,
        rpg: 3,
        simulation: 4,
        strategy: 5,
        sports: 6,
        puzzle: 7,
        racing: 8,
    };

    // Fetch games from API
    const fetchGames = async () => {
        try {
            const response = await fetch('http://localhost:5084/Game/GetAll');
            const data = await response.json();

            // Update images for each game
            const updatedGames = data.map((game) => {
                // Find the cover image
                const coverImage = game.imageGame.find(image => image.fileName.includes('cover'));

                // Build the path for the cover image
                const coverImagePath = coverImage ? `${process.env.PUBLIC_URL}${coverImage.filePath}${coverImage.fileName}` : '';

                return { ...game, coverImagePath };
            });

            setGames(updatedGames);
        } catch (error) {
            console.error("Error fetching games:", error);
        }
    };

    useEffect(() => {
        fetchGames();
    }, []);

    // Filter games by search term and genre
    const filteredGames = games.filter(game => {
        const matchesTitle = game.title.toLowerCase().includes(searchTerm.toLowerCase());
        const matchesGenre = selectedGenre === 'all' || game.genreId === genres[selectedGenre];
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
                        <option value="action">Action</option>
                        <option value="adventure">Adventure</option>
                        <option value="rpg">Role-Playing</option>
                        <option value="simulation">Simulation</option>
                        <option value="strategy">Strategy</option>
                        <option value="sports">Sports</option>
                        <option value="puzzle">Puzzle</option>
                        <option value="racing">Racing</option>
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
                                {/* Only show the cover image */}
                                {game.coverImagePath && (
                                    <img
                                        src={game.coverImagePath}
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

            {/* Pagination controls */}
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
