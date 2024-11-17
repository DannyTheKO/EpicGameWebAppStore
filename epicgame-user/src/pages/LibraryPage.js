import React, { useState } from 'react';
import '../styles/pages/LibraryPage.css';
import { FaSearch } from 'react-icons/fa';
import images from '../assets'; // Import tất cả ảnh từ thư mục assets

// Danh sách các game đã mua
const purchasedGames = [
    { id: 1, name: "CyberQuest", description: "Explore futuristic worlds in this epic RPG.", image: images['game1.png'] },
    { id: 2, name: "Fantasy Wars", description: "Lead your army to victory in this strategy game.", image: images['game2.png'] },
    { id: 3, name: "Pixel Racer", description: "Fast-paced racing action with retro vibes.", image: images['game3.png'] },
    { id: 4, name: "Shadow Realms", description: "Survive the darkness in this horror adventure.", image: images['game4.png'] },
    { id: 5, name: "Sky Explorers", description: "Soar through the skies in this flight simulator.", image: images['game5.png'] },
    { id: 6, name: "Ocean Quest", description: "Explore the depths of the ocean.", image: images['game6.png'] },
    { id: 7, name: "Battle Royale", description: "Last one standing wins!", image: images['game7.png'] },
    { id: 8, name: "Mystic Adventures", description: "Solve puzzles and uncover secrets.", image: images['game8.png'] },
    { id: 9, name: "Space Rangers", description: "Conquer the universe.", image: images['game9.png'] },
];

const LibraryPage = () => {
    const [searchTerm, setSearchTerm] = useState('');
    const [currentPage, setCurrentPage] = useState(1);
    const gamesPerPage = 15; // Hiển thị 15 game mỗi trang

    // Lọc danh sách các game theo từ khóa tìm kiếm
    const filteredGames = purchasedGames.filter(game =>
        game.name.toLowerCase().includes(searchTerm.toLowerCase())
    );

    // Tính toán phân trang
    const totalPages = Math.ceil(filteredGames.length / gamesPerPage);
    const startIndex = (currentPage - 1) * gamesPerPage;
    const paginatedGames = filteredGames.slice(startIndex, startIndex + gamesPerPage);

    const handlePageChange = (pageNumber) => {
        setCurrentPage(pageNumber);
    };
    return (
        <div className="library-page">
            <h2>Your Library</h2>
            <div className="search-container" style={{margin: "0 auto 40px",}}>              
                <input
                    type="text"
                    placeholder="Search for a game..."
                    value={searchTerm}
                    onChange={(e) => {
                        setSearchTerm(e.target.value);
                        setCurrentPage(1); // Reset về trang đầu tiên khi tìm kiếm
                    }}
                />
                <FaSearch className="search-icon" />
            </div>
            <div className="games-list">
                {paginatedGames.length > 0 ? (
                    paginatedGames.map(game => (
                        <div className="game-item" key={game.id}>
                            <img src={game.image} alt={game.name} />
                            <h3>{game.name}</h3>
                            <p>{game.description}</p>
                        </div>
                    ))
                ) : (
                    <p>No games found. Try a different search!</p>
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

export default LibraryPage;
