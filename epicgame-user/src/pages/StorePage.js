import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import '../styles/pages/StorePage.css';
import { FaSearch } from 'react-icons/fa';

const StorePage = () => {
    const [games, setGames] = useState([]);
    const [searchTerm, setSearchTerm] = useState('');
    const [selectedGenre, setSelectedGenre] = useState('all');
    const [currentPage, setCurrentPage] = useState(1); // Thêm state để quản lý trang hiện tại
    const gamesPerPage = 20; // Số trò chơi mỗi trang

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

    const fetchGames = async () => {
        try {
            const response = await fetch('http://localhost:5084/Game/GetAll');
            const data = await response.json();
            setGames(data);
        } catch (error) {
            console.error("Error fetching games:", error);
        }
    };

    useEffect(() => {
        fetchGames();
    }, []);

    const filteredGames = games.filter(game => {
        const matchesTitle = game.title.toLowerCase().includes(searchTerm.toLowerCase());
        const matchesGenre = selectedGenre === 'all' || game.genreId === genres[selectedGenre];
        return matchesTitle && matchesGenre;
    });

    // Tính toán chỉ số bắt đầu và kết thúc cho các trò chơi trên trang hiện tại
    const startIndex = (currentPage - 1) * gamesPerPage;
    const endIndex = startIndex + gamesPerPage;
    const paginatedGames = filteredGames.slice(startIndex, endIndex);

    // Số trang tổng cộng
    const totalPages = Math.ceil(filteredGames.length / gamesPerPage);

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
                                <img src={game.image} alt={game.title} />
                                <h3>{game.title}</h3>
                                <p>${game.price}</p>
                            </div>
                        </Link>
                    ))
                )}
            </div>
            {/* Hiển thị các nút chuyển trang */}
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
