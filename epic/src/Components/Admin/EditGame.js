﻿import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import '../../styles/pages/EditGame.css';

const EditGame = () => {
    const { id } = useParams();
    const [game, setGame] = useState(null);
    const navigate = useNavigate();

    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [currentImageIndex, setCurrentImageIndex] = useState(0);
    const [alert, setAlert] = useState(null); // Thêm alert state

    const fetchGameDetails = async () => {
        try {
            const response = await fetch(`http://localhost:5084/Store/GamePage/${id}`);
            if (!response.ok) {
                throw new Error('Failed to load game details.');
            }
            const data = await response.json();
            setGame(data);
        } catch (error) {
            setError(error.message);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchGameDetails();
    }, [id]);
    const handleChange = (e) => {
        const { name, value } = e.target;
        setGame((prev) => ({ ...prev, [name]: value }));
    };

    const handleThumbnailChange = (e) => {
        const file = e.target.files[0];
        if (file) {
            const updatedThumbnail = {
                filePath: URL.createObjectURL(file),
                fileName: file.name
            };
            setGame((prev) => ({ ...prev, thumbnail: updatedThumbnail }));
        }
    };

    const handleGameplayChange = (index, e) => {
        const file = e.target.files[0];
        if (file) {
            const updatedImages = [...game.imageGame];
            updatedImages[index] = { ...updatedImages[index], filePath: URL.createObjectURL(file), fileName: file.name };
            setGame((prev) => ({ ...prev, imageGame: updatedImages }));
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const response = await fetch(`http://localhost:5084/Store/GamePage/UpdateGame/${id}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(game),
            });

            if (!response.ok) {
                throw new Error('Failed to update game details.');
            }

            navigate(`/game/${id}`);
        } catch (error) {
            setError(error.message);
        }
    };
    const handleNextImage = () => {
        if (gameplayImages.length > 0) {
            setCurrentImageIndex((prevIndex) => (prevIndex + 1) % gameplayImages.length);
        }
    };

    const handlePrevImage = () => {
        if (gameplayImages.length > 0) {
            setCurrentImageIndex((prevIndex) =>
                (prevIndex - 1 + gameplayImages.length) % gameplayImages.length
            );
        }
    };


    const getVisibleImages = () => {
        if (!gameplayImages || gameplayImages.length === 0) return []; // Trả về danh sách trống nếu không có ảnh
        const len = gameplayImages.length;
        const leftIndex = (currentImageIndex - 1 + len) % len; // Xử lý chỉ số vòng lặp
        const rightIndex = (currentImageIndex + 1) % len;

        return [
            gameplayImages[leftIndex],
            gameplayImages[currentImageIndex],
            gameplayImages[rightIndex],
        ];
    };



    const handleImageClick = (clickedIndex) => {
        if (gameplayImages.length > 0) {
            const len = gameplayImages.length;
            const newIndex = (clickedIndex + len) % len; // Đảm bảo chỉ số luôn hợp lệ
            setCurrentImageIndex(newIndex);
        }
    };


    if (loading) return <div>Loading...</div>;
    if (error) return <div>{error}</div>;
    if (!game) return <div>No game details available</div>;

    // Kiểm tra giá
    const gamePrice = typeof game.price === 'number' && !isNaN(game.price)
        ? game.price.toFixed(2)
        : 'Price Not Available';

    // Lọc các ảnh thuộc folder gameplay
    const gameplayImages =
        game.imageGame?.filter((img) => img.imageType === 'Screenshot') || [];

    // Đảm bảo không có ảnh gameplay
    const hasGameplayImages = gameplayImages.length > 0;

    return (
        <div className="game-page">
            <form onSubmit={handleSubmit} className="edit-game-form">
                <div className="game-header">
                    <input
                        type="text"
                        id="title"
                        name="title"
                        value={game.title || "Game Title"}
                        onChange={handleChange}
                        required
                    />
                </div>
                <div className="game-details">
                    <div className="game-details-thumbnail">
                    <img
                        src={game.thumbnail?.filePath || `${process.env.PUBLIC_URL}${game.imageGame[0]?.filePath}${game.imageGame[0]?.fileName}`}
                        alt="Game Thumbnail"
                        className="game-thumbnail"
                    />
                    <input
                        type="file"
                        id="thumbnail"
                        accept="image/*"
                        onChange={handleThumbnailChange}
                        />
                    </div>
                <div className="game-info">
                    <h2>Description</h2>
                    <textarea
                        id="description"
                        name="description"
                        value={game.description || 'No description available.'}
                        onChange={handleChange}
                        required
                    ></textarea>
                    <div className="game-rating">
                        <h3>Overall Rating:</h3>
                            <input
                                type="text"
                                id="rating"
                                name="rating"
                                value={game.rating || "Rating"}
                                onChange={handleChange}
                                required
                            />
                    </div>
                    <div className="game-price">
                        <h3>Price:</h3>
                            <input
                                type="text"
                                id="price"
                                name="price"
                                value={game.price || "Price"}
                                onChange={handleChange}
                                required
                            />
                    </div>
                </div>
                <div className="game-metadata">
                        <p>
                            <span className="label">Author:</span>
                            <input
                            type="text"
                            id="author"
                            name="author"
                                value={game.author || "Author"}
                            onChange={handleChange}
                            required
                            />
                        </p>
                        <p>
                            <span className="label">Publisher:</span>
                            <input
                            type="text"
                            id="publisher"
                            name="publisher"
                                value={game.publisher?.name || "Publisher"}
                            onChange={handleChange}
                            required
                            />
                        </p>
                        <p>
                            <span className="label">Genre:</span>
                            <input
                                type="text"
                                id="genre"
                                name="genre"
                                value={game.genre?.name || "Genre"}
                                onChange={handleChange}
                                required
                            />
                        </p>
                        <p>
                            <span className="label">Release Date:</span>
                            <input
                                type="date"
                                id="release"
                                name="release"
                                value={game.release}
                                onChange={handleChange}
                                required
                            />
                        </p>
                </div>
                </div>
            </form>
            <div className="gameplay-section">
                <h2>Gameplay</h2>
                {gameplayImages.length > 0 ? (
                    <div className="gameplay-slider">
                        <div className="gameplay-images">
                            {gameplayImages.map((img, index) => (
                                <div key={index} className="gameplay-image">
                                    <img
                                        src={`${process.env.PUBLIC_URL}${img.filePath}${img.fileName}`}
                                        alt={`Gameplay ${index + 1}`}
                                        className="gameplay-image-preview"
                                    />
                                    <input
                                        type="file"
                                        accept="image/*"
                                        onChange={(e) => handleGameplayChange(index, e)}
                                    />
                                </div>
                            ))}
                        </div>
                    </div>
                ) : (
                    <div className="no-gameplay-message">Không có hình ảnh gameplay.</div>
                )}
            </div>
        </div>
    );
};

export default EditGame;
