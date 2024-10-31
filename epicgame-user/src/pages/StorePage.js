// src/pages/StorePage.js
import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import '../styles/pages/StorePage.css';
import { FaSearch } from 'react-icons/fa';
import images from '../assets'; // Import tất cả ảnh từ assets

const games = [
    { id: 1, name: "Game 1", price: "$19.99", image: images['game1.png'] },
    { id: 2, name: "Game 2", price: "$29.99", image: images['game2.png'] },
    { id: 3, name: "Game 3", price: "$39.99", image: images['game3.png'] },
    { id: 4, name: "Game 4", price: "$49.99", image: images['game4.png'] },
    { id: 5, name: "Game 5", price: "$59.99", image: images['game5.png'] },
    { id: 6, name: "Game 6", price: "$69.99", image: images['game6.png'] },
    { id: 7, name: "Game 7", price: "$79.99", image: images['game7.png'] },
    { id: 8, name: "Game 8", price: "$89.99", image: images['game8.png'] },
    { id: 9, name: "Game 9", price: "$99.99", image: images['game9.png'] },
];

const StorePage = () => {
    const [searchTerm, setSearchTerm] = useState('');

    // Hàm để lọc game dựa trên tên game
    const filteredGames = games.filter(game =>
        game.name.toLowerCase().includes(searchTerm.toLowerCase())
    );

    return (
        <div className="store-page">
            <h2>All Games</h2>
            <div className="filter-container">
                <div className="filter-options">
                    <label htmlFor="filter">Filter by:</label>
                    <select id="filter">
                        <option value="all">All Games</option>
                        <option value="new">New Releases</option>
                        <option value="popular">Popular</option>
                    </select>
                </div>
                <div className="search-container">
                    <input 
                        type="text" 
                        placeholder="Search for a game..." 
                        value={searchTerm}
                        onChange={(e) => setSearchTerm(e.target.value)} // Cập nhật giá trị của thanh tìm kiếm
                    />
                    <FaSearch className="search-icon" />
                </div>
                <div className="genre-container">
                    <label htmlFor="genre">Genre:</label>
                    <select id="genre">
                        <option value="all">All Genres</option>
                        <option value="action">Action</option>
                        <option value="adventure">Adventure</option>
                        <option value="rpg">RPG</option>
                        <option value="shooter">Shooter</option>
                    </select>
                </div>
            </div>
            <div className="games-list">
                {filteredGames.map(game => (
                    <Link to={`/game/${game.id}`} key={game.id}>
                        <div className="game-item">
                            <img src={game.image} alt={game.name} />
                            <h3>{game.name}</h3>
                            <p>{game.price}</p>
                        </div>
                    </Link>
                ))}
            </div>
        </div>
    );
};

export default StorePage;
