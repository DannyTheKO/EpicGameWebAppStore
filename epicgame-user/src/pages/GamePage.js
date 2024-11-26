import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import '../styles/pages/GamePage.css';

const GamePage = () => {
    const { id } = useParams(); // Lấy ID game từ URL
    const [game, setGame] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [currentImageIndex, setCurrentImageIndex] = useState(0);

    // Trạng thái cho phần đánh giá và nhận xét
    const [rating, setRating] = useState(0);
    const [review, setReview] = useState('');
    const [reviews, setReviews] = useState([]);

    useEffect(() => {
        const fetchGameDetails = async () => {
            try {
                const response = await fetch(`http://localhost:5084/Game/GetGameId/${id}`);
                if (!response.ok) {
                    throw new Error('Failed to load game details.');
                }
                const data = await response.json();
                console.log(data); // Log dữ liệu để kiểm tra
                setGame(data);
            } catch (error) {
                setError(error.message);
            } finally {
                setLoading(false);
            }
        };

        fetchGameDetails();
    }, [id]);

    const handleNextImage = () => {
        if (hasGameplayImages) {
            setCurrentImageIndex((prevIndex) => (prevIndex + 1) % gameplayImages.length);
        }
    };

    const handlePrevImage = () => {
        if (hasGameplayImages) {
            setCurrentImageIndex((prevIndex) =>
                (prevIndex - 1 + gameplayImages.length) % gameplayImages.length
            );
        }
    };

    const handleRatingChange = (newRating) => {
        setRating(newRating);
    };

    const handleReviewSubmit = (e) => {
        e.preventDefault();
        if (rating > 0 && review) {
            setReviews([...reviews, { rating, review }]);
            setRating(0);
            setReview('');
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
    const gameplayImages = game?.imageGame?.filter(image => image.filePath.includes("gameplay")) || [];

    // Đảm bảo không có ảnh gameplay
    const hasGameplayImages = gameplayImages.length > 0;

    console.log("Gameplay Images:", gameplayImages); // Log danh sách ảnh gameplay để kiểm tra

    return (
        <div className="game-page">
            <div className="game-header">
                <h1>{game.title || 'Game Title'}</h1>
            </div>

            <div className="game-details">
                <img
                    src={`${process.env.PUBLIC_URL}${game.imageGame[0]?.filePath}${game.imageGame[0]?.fileName}`}
                    alt="Game Cover"
                    className="game-cover"
                />
                <div className="game-info">
                    <h2>Description</h2>
                    <p>{game.description || 'No description available.'}</p>
                    <div className="game-rating">
                        <h3>Overall Rating:</h3>
                        <span>
                            <span className="rating-value">{game.rating || 'N/A'}</span>
                            <span className="star"></span>
                        </span>
                    </div>
                    <div className="game-price">
                        <h3>Price:</h3>
                        <span className="price-value">${gamePrice}</span>
                    </div>
                    <button className="purchase-button">Purchase Now</button>
                    <button className="add-to-cart-button">Add to Cart</button>
                </div>

                {/* Thông tin bổ sung */}
                <div className="game-metadata">
                    <p><span className="label">Author:</span> <span className="value">{game.author}</span></p>
                    <p><span className="label">Publisher:</span> <span className="value">{game.publisher?.name || 'N/A'}</span></p>
                    <p><span className="label">Genre:</span> <span className="value">{game.genre?.name || 'N/A'}</span></p>
                    <p><span className="label">Release Date:</span> <span className="value">{game.release}</span></p>
                </div>
            </div>

            {/* Phần chuyển đổi ảnh gameplay */}
            <div className="gameplay-section">
                <h2>Gameplay</h2>
                {hasGameplayImages ? (
                    <div className="gameplay-slider">
                        <button className="prev-button" onClick={handlePrevImage}>❮</button>
                        <img
                            src={`/images/games/apex_legends/gameplay/${gameplayImages[currentImageIndex]?.fileName}`}
                            alt={`Gameplay ${currentImageIndex + 1}`}
                            className="gameplay-image"
                            onError={(e) => {
                                e.target.src = '/images/placeholder.png'; // Ảnh dự phòng
                                console.log('Error loading image, using placeholder');
                            }}
                        />
                        <button className="next-button" onClick={handleNextImage}>❯</button>
                    </div>
                ) : (
                    <div className="no-gameplay-message">Không có hình ảnh gameplay.</div>
                )}
            </div>

            {/* Ratings and Reviews Section */}
            <div className="review-section">
                <h2>User Ratings and Reviews</h2>
                <form onSubmit={handleReviewSubmit} className="review-form">
                    <div className="rating">
                        <label>Rating:</label>
                        {[1, 2, 3, 4, 5].map((star) => (
                            <span
                                key={star}
                                onClick={() => handleRatingChange(star)}
                                className={`star ${star <= rating ? 'selected' : ''}`}
                            >
                                ★
                            </span>
                        ))}
                    </div>
                    <textarea
                        placeholder="Write your review here..."
                        value={review}
                        onChange={(e) => setReview(e.target.value)}
                    />
                    <button type="submit" className="submit-review">Submit Review</button>
                </form>
                <div className="reviews-list">
                    {reviews.map((r, index) => (
                        <div key={index} className="review-item">
                            <div className="review-rating">
                                {[...Array(r.rating)].map((_, i) => (
                                    <span key={i} className="star selected">★</span>
                                ))}
                            </div>
                            <p>{r.review}</p>
                        </div>
                    ))}
                </div>
            </div>
        </div>
    );
};

export default GamePage;
