// src/pages/LibraryPage.js
import React, { useState } from 'react';
import '../styles/pages/LibraryPage.css';
import { FaSearch } from 'react-icons/fa';
import images from '../assets'; // Import tất cả ảnh từ assets

const purchasedGames = [
    { id: 1, name: "Game 1", description: "Description for Game 1", image: images['game1.png'] },
    { id: 2, name: "Game 2", description: "Description for Game 2", image: images['game2.png'] },
    { id: 3, name: "Game 3", description: "Description for Game 3", image: images['game3.png'] },
    { id: 4, name: "Game 4", description: "Description for Game 4", image: images['game4.png'] },
    { id: 5, name: "Game 5", description: "Description for Game 5", image: images['game5.png'] },
];

const LibraryPage = () => {
    const [searchTerm, setSearchTerm] = useState('');

    // Hàm lọc game dựa trên từ khóa tìm kiếm
    const filteredGames = purchasedGames.filter(game =>
        game.name.toLowerCase().includes(searchTerm.toLowerCase())
    );

    return (
        <div className="library-page">
            <h2>Your Library</h2>
            <div className="search-container">
                <input 
                    type="text" 
                    placeholder="Search for a game..." 
                    value={searchTerm} // Gán giá trị cho input
                    onChange={(e) => setSearchTerm(e.target.value)} // Cập nhật state khi người dùng nhập
                />
                <FaSearch className="search-icon" />
            </div>
            <div className="games-list">
                {filteredGames.map(game => (
                    <div className="game-item" key={game.id}>
                        <img src={game.image} alt={game.name} />
                        <h3>{game.name}</h3>
                        <p>{game.description}</p>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default LibraryPage;
