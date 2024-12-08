import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import '../styles/pages/HomePage.css';

// Fetch dữ liệu từ API và lấy 3 game mới nhất làm ảnh banner
const fetchBannerImages = async () => {
    try {
        const response = await fetch('http://localhost:5084/Store/FeaturePage/GetTopNewReleases');
        if (!response.ok) throw new Error("Failed to fetch Top New Releases");
        const data = await response.json();

        // Lọc ra 3 game mới nhất
        const latestBanners = data.sort((a, b) => new Date(b.createdOn) - new Date(a.createdOn)).slice(0, 3);

        // Cập nhật đường dẫn ảnh banner
        return latestBanners.map(banner => `${process.env.PUBLIC_URL}${banner.imageGame.find(img => img.imageType.toLowerCase() === 'banner').filePath}${banner.imageGame.find(img => img.imageType.toLowerCase() === 'banner').fileName}`);
    } catch (error) {
        console.error("Error fetching Top New Releases:", error);
        return []; // Trả về mảng rỗng nếu có lỗi
    }
};

const HomePage = () => {
    const [topNewReleases, setTopNewReleases] = useState([]);
    const [trendingGames, setTrendingGames] = useState([]);
    const [bannerImages, setBannerImages] = useState([]);

    // Fetch dữ liệu từ API
    const fetchTopNewReleases = async () => {
        try {
            const response = await fetch('http://localhost:5084/Store/FeaturePage/GetTopNewReleases');
            if (!response.ok) throw new Error("Failed to fetch Top New Releases");
            const data = await response.json();

            // Cập nhật đường dẫn ảnh thumbnail
            const updatedGames = data.map(game => {
                const thumbnailImage = game.imageGame.find(img => img.imageType.toLowerCase() === 'thumbnail');
                const thumbnailPath = thumbnailImage ? `${process.env.PUBLIC_URL}${thumbnailImage.filePath}${thumbnailImage.fileName}` : '';
                return { ...game, thumbnailPath };
            });

            setTopNewReleases(updatedGames.slice(0, 5)); // Lấy 5 game mới nhất
        } catch (error) {
            console.error("Error fetching Top New Releases:", error);
            setTopNewReleases([]); // Set rỗng nếu có lỗi
        }
    };

    const fetchTrendingGames = async () => {
        try {
            const response = await fetch('http://localhost:5084/Store/FeaturePage/GetTrendingGames');
            if (!response.ok) throw new Error("Failed to fetch Trending Games");
            const data = await response.json();

            // Cập nhật đường dẫn ảnh thumbnail
            const updatedGames = data.map(game => {
                const thumbnailImage = game.imageGame.find(img => img.imageType.toLowerCase() === 'thumbnail');
                const thumbnailPath = thumbnailImage ? `${process.env.PUBLIC_URL}${thumbnailImage.filePath}${thumbnailImage.fileName}` : '';
                return { ...game, thumbnailPath };
            });

            setTrendingGames(updatedGames.slice(0, 5)); // Lấy 5 game trending
        } catch (error) {
            console.error("Error fetching Trending Games:", error);
            setTrendingGames([]); // Set rỗng nếu có lỗi
        }
    };

    useEffect(() => {
        fetchTopNewReleases();
        fetchTrendingGames();
        fetchBannerImages().then(images => setBannerImages(images));
    }, []);

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
                    {topNewReleases.length > 0 ? (
                        topNewReleases.map(game => (
                            <Link to={`/game/${game.gameId}`} key={game.gameId}>
                                <div className="game-item">
                                    {game.thumbnailPath && (
                                        <img src={game.thumbnailPath} alt={game.title} className="game-cover" />
                                    )}
                                    <h3>{game.title}</h3>
                                    <p>${game.price.toFixed(2)}</p>
                                </div>
                            </Link>
                        ))
                    ) : (
                        <p>No games available</p>
                    )}
                </div>
            </section>

            {/* Trending */}
            <section className="game-section">
                <h2>Trending</h2>
                <div className="game-grid">
                    {trendingGames.length > 0 ? (
                        trendingGames.map(game => (
                            <Link to={`/game/${game.gameId}`} key={game.gameId}>
                                <div className="game-item">
                                    {game.thumbnailPath && (
                                        <img src={game.thumbnailPath} alt={game.title} className="game-cover" />
                                    )}
                                    <h3>{game.title}</h3>
                                    <p>${game.price.toFixed(2)}</p>
                                </div>
                            </Link>
                        ))
                    ) : (
                        <p>No games available</p>
                    )}
                </div>
            </section>
        </div>
    );
};

export default HomePage;
