import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import '../styles/pages/HomePage.css';

const fetchBannerImages = async () => {
    try {
        const response = await fetch('http://localhost:5084/Store/FeaturePage/GetTopNewReleases');
        if (!response.ok) throw new Error("Failed to fetch Top New Releases");
        const data = await response.json();

        const latestBanners = data.sort((a, b) => new Date(b.createdOn) - new Date(a.createdOn)).slice(0, 3);

        return latestBanners.map(banner => `${process.env.PUBLIC_URL}${banner.imageGame.find(img => img.imageType.toLowerCase() === 'banner').filePath}${banner.imageGame.find(img => img.imageType.toLowerCase() === 'banner').fileName}`);
    } catch (error) {
        console.error("Error fetching Top New Releases:", error);
        return [];
    }
};

const HomePage = () => {
    const [topNewReleases, setTopNewReleases] = useState([]);
    const [trendingGames, setTrendingGames] = useState([]);
    const [freegame, setFreeGame] = useState([]);
    const [bannerImages, setBannerImages] = useState([]);
    const [featuredPublishers, setFeaturedPublishers] = useState([]);
    const [genres, setGenres] = useState([]);

    const fetchTopNewReleases = async () => {
        try {
            const response = await fetch('http://localhost:5084/Store/FeaturePage/GetTopNewReleases');
            if (!response.ok) throw new Error("Failed to fetch Top New Releases");
            const data = await response.json();

            const updatedGames = data.map(game => {
                const thumbnailImage = game.imageGame.find(img => img.imageType.toLowerCase() === 'thumbnail');
                const thumbnailPath = thumbnailImage ? `${process.env.PUBLIC_URL}${thumbnailImage.filePath}${thumbnailImage.fileName}` : '';
                return { ...game, thumbnailPath };
            });

            setTopNewReleases(updatedGames.slice(0, 5));
        } catch (error) {
            console.error("Error fetching Top New Releases:", error);
            setTopNewReleases([]);
        }
    };
    const fetchFreegame = async () => {
        try {
            const response = await fetch('http://localhost:5084/Store/FeaturePage/GetTopFreeToPlay');
            if (!response.ok) throw new Error("Failed to fetch Top New Releases");
            const data = await response.json();

            const updatedGames = data.map(game => {
                const thumbnailImage = game.imageGame.find(img => img.imageType.toLowerCase() === 'thumbnail');
                const thumbnailPath = thumbnailImage ? `${process.env.PUBLIC_URL}${thumbnailImage.filePath}${thumbnailImage.fileName}` : '';
                return { ...game, thumbnailPath };
            });

            setFreeGame(updatedGames.slice(0, 5));
        } catch (error) {
            console.error("Error fetching Top New Releases:", error);
            setFreeGame([]);
        }
    };
    const fetchTrendingGames = async () => {
        try {
            const response = await fetch('http://localhost:5084/Store/FeaturePage/GetTrendingGames');
            if (!response.ok) throw new Error("Failed to fetch Trending Games");
            const data = await response.json();

            const updatedGames = data.map(game => {
                const thumbnailImage = game.imageGame.find(img => img.imageType.toLowerCase() === 'thumbnail');
                const thumbnailPath = thumbnailImage ? `${process.env.PUBLIC_URL}${thumbnailImage.filePath}${thumbnailImage.fileName}` : '';
                return { ...game, thumbnailPath };
            });

            setTrendingGames(updatedGames.slice(0, 5));
        } catch (error) {
            console.error("Error fetching Trending Games:", error);
            setTrendingGames([]);
        }
    };

    const fetchFeaturedPublishers = async () => {
        try {
            const response = await fetch('http://localhost:5084/Store/FeaturePage/GetFeaturedPublishers');
            if (!response.ok) throw new Error("Failed to fetch Featured Publishers");
            const data = await response.json();
            setFeaturedPublishers(data);
        } catch (error) {
            console.error("Error fetching Featured Publishers:", error);
            setFeaturedPublishers([]);
        }
    };

    const fetchTopGenres = async () => {
        try {
            const response = await fetch('http://localhost:5084/Store/FeaturePage/GetTopGenres');
            if (!response.ok) throw new Error("Failed to fetch Top Genres");
            const data = await response.json();
            setGenres(data);
        } catch (error) {
            console.error("Error fetching Top Genres:", error);
            setGenres([]);
        }
    };

    useEffect(() => {
        fetchFreegame();
        fetchTopNewReleases();
        fetchTrendingGames();
        fetchFeaturedPublishers();
        fetchTopGenres();
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

            {/* Free To Play */}
            <section className="game-section">
                <h2>Top New Releases</h2>
                <div className="game-grid">
                    {freegame.length > 0 ? (
                        freegame.map(game => (
                            <Link to={`/game/${game.gameId}`} key={game.gameId}>
                                <div className="game-item">
                                    {game.thumbnailPath && (
                                        <img src={game.thumbnailPath} alt={game.title} className="game-cover" />
                                    )}
                                    <h3>{game.title}</h3>
                                    <p>{game.price}</p>
                                </div>
                            </Link>
                        ))
                    ) : (
                        <p>No games available</p>
                    )}
                </div>
            </section>
            {/* Featured Publishers */}
            <section className="publisher-section">
                <h2>Featured Publishers</h2>
                <div className="publisher-grid">
                    {featuredPublishers.length > 0 ? (
                        featuredPublishers.map(publisher => (
                            <div className="publisher-item-h" key={publisher.publisherId}>
                                <h3>{publisher.name}</h3>
                                {publisher.games.slice(0, 4).map(game => (
                                    <Link to={`/game/${game.gameId}`} key={game.gameId}>
                                        <div className="publisher-item">
                                            {game.imageGame.find(img => img.imageType.toLowerCase() === 'thumbnail') && (
                                                <img src={`${process.env.PUBLIC_URL}${game.imageGame.find(img => img.imageType.toLowerCase() === 'thumbnail').filePath}${game.imageGame.find(img => img.imageType.toLowerCase() === 'thumbnail').fileName}`} alt={game.title} className="game-cover-publisher" />
                                            )}
                                            <div className="game-detail-publisher">
                                                <h4>{game.title}</h4>
                                                <p>${game.price.toFixed(2)}</p>
                                            </div>
                                        </div>
                                    </Link>
                                ))}
                            </div>
                        ))
                    ) : (
                        <p>No publishers available</p>
                    )}
                </div>
            </section>

            {/* Genres */}
            <section className="genre-section">
                <h2>Genres</h2>
                <div className="genre-grid">
                    {genres.length > 0 ? (
                        genres.map(genre => (
                            <div className="genre-item-h" key={genre.id}>
                                <h3>{genre.name}</h3>
                                {genre.games.slice(0, 4).map(game => (
                                    <Link to={`/game/${game.gameId}`} key={game.gameId}>
                                        <div className="genre-item">
                                            {game.imageGame.find(img => img.imageType.toLowerCase() === 'thumbnail') && (
                                                <img src={`${process.env.PUBLIC_URL}${game.imageGame.find(img => img.imageType.toLowerCase() === 'thumbnail').filePath}${game.imageGame.find(img => img.imageType.toLowerCase() === 'thumbnail').fileName}`} alt={game.title} className="game-cover-genre" />
                                            )}
                                            <div className="game-detail-genre">
                                                <h4>{game.title}</h4>
                                                <p>${game.price.toFixed(2)}</p>
                                            </div>
                                        </div>
                                    </Link>
                                ))}
                            </div>
                        ))
                    ) : (
                        <p>No genres available</p>
                    )}
                </div>
            </section>
        </div>
    );
};

export default HomePage;
