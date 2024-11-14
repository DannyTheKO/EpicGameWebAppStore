import React from 'react';
import { Link } from 'react-router-dom';
import '../styles/pages/HomePage.css';
import images from '../assets'; // Import tất cả ảnh từ assets

// Dùng hình ảnh banner từ assets
const bannerImages = [
    images['image1.jpg'],
    images['image2.jpg'],
    images['image3.jpg']
];

// Dữ liệu mẫu cho phần Top New Releases và Trending
const topNewReleases = [
    { id: 1, title: 'Game 1', image: images['game1.png'], price: '$19.99' },
    { id: 2, title: 'Game 2', image: images['game2.png'], price: '$29.99' },
    { id: 3, title: 'Game 3', image: images['game3.png'], price: '$39.99' },
    { id: 4, title: 'Game 4', image: images['game4.png'], price: '$49.99' },
    { id: 5, title: 'Game 5', image: images['game5.png'], price: '$59.99' },
];

const trendingGames = [
    { id: 6, title: 'Game 6', image: images['game6.png'], price: '$69.99' },
    { id: 7, title: 'Game 7', image: images['game7.png'], price: '$79.99' },
    { id: 8, title: 'Game 8', image: images['game8.png'], price: '$89.99' },
    { id: 9, title: 'Game 9', image: images['game9.png'], price: '$99.99' },
    { id: 10, title: 'Game 10', image: images['game10.png'], price: '$109.99' },
];

const HomePage = () => {
    return (
        <div className="home-page">
            {/* Banner */}
            <div className="banner">
                {bannerImages.map((image, index) => (
                    <Link to="/store" key={index}>
                        <img src={image} alt={`Banner ${index + 1}`} className="banner-image" />
                    </Link>
                ))}
            </div>

            {/* Top New Releases */}
            <section className="game-section">
                <h2>Top New Releases</h2>
                <div className="game-grid">
                    {topNewReleases.map(game => (
                        <Link to={`/game/${game.id}`} key={game.id}>
                            <div className="game-item">
                                <img src={game.image} alt={game.title} />
                                <h3>{game.title}</h3>
                                <p>{game.price}</p>
                            </div>
                        </Link>
                    ))}
                </div>
            </section>

            {/* Trending */}
            <section className="game-section">
                <h2>Trending</h2>
                <div className="game-grid">
                    {trendingGames.map(game => (
                        <Link to={`/game/${game.id}`} key={game.id}>
                            <div className="game-item">
                                <img src={game.image} alt={game.title} />
                                <h3>{game.title}</h3>
                                <p>{game.price}</p>
                            </div>
                        </Link>
                    ))}
                </div>
            </section>
        </div>
    );
};

export default HomePage;
