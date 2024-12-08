import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import '../styles/pages/GamePage.css';

const GamePage = () => {
    const { id } = useParams();
    const [game, setGame] = useState(null);
    const navigate = useNavigate();
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [currentImageIndex, setCurrentImageIndex] = useState(0);

    // Trạng thái cho phần đánh giá và nhận xét
    const [rating, setRating] = useState(0);
    const [review, setReview] = useState('');
    const [reviews, setReviews] = useState([]);
    const [alert, setAlert] = useState(null); // Thêm alert state

    const handleAddToCart = async (e) => {
        e.preventDefault();

        const authToken = localStorage.getItem('authToken');
        if (!authToken) {
            setAlert({ message: 'Bạn phải đăng nhập để thêm sản phẩm vào giỏ hàng.', type: 'error' });
            setTimeout(() => setAlert(null), 3000);
            navigate('/login');
            return;
        }

        try {
            // Fetch owned games for the current user
            const ownedGamesResponse = await fetch(`http://localhost:5084/Profile/ProfileUser/GetOwnedGames`, {
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${authToken}`
                }
            });

            if (!ownedGamesResponse.ok) {
                throw new Error('Failed to fetch owned games.');
            }

            const ownedGamesData = await ownedGamesResponse.json();
            const ownedGameIds = ownedGamesData.data.map(game => game.gameId);

            // Check if the current game already exists in the user's owned games
            if (ownedGameIds.includes(parseInt(id))) {
                setAlert({ message: 'You already have this game !.', type: 'error' });
                setTimeout(() => setAlert(null), 3000);
                return;
            }

            // Proceed to add to cart if not already owned
            const response = await fetch(`http://localhost:5084/Store/GamePage/AddToCart?gameid=${id}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${authToken}`
                }
            });

            if (!response.ok) {
                throw new Error('Failed to add game to cart.');
            }

            const data = await response.json();
            setAlert({ message: data.message, type: 'success' });

        } catch (error) {
            setAlert({ message: error.message, type: 'error' });
        }
    };

    useEffect(() => {
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
    const gameplayImages =
        game.imageGame?.filter((img) => img.imageType === 'Screenshot') || [];

    // Đảm bảo không có ảnh gameplay
    const hasGameplayImages = gameplayImages.length > 0;

    return (
        <div className="game-page">
            {alert && (
                <div className={`alert-message ${alert.type}`}>
                    <span>{alert.message}</span>
                    <button className="close-btn" onClick={() => setAlert(null)}>&times;</button>
                </div>
            )}
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
                    <button onClick={handleAddToCart} className="add-to-cart-button">Add to Cart</button>
                </div>
                <div className="game-metadata">
                    <p><span className="label">Author:</span> <span className="value">{game.author}</span></p>
                    <p><span className="label">Publisher:</span> <span className="value">{game.publisher?.name || 'N/A'}</span></p>
                    <p><span className="label">Genre:</span> <span className="value">{game.genre?.name || 'N/A'}</span></p>
                    <p><span className="label">Release Date:</span> <span className="value">{game.release}</span></p>
                </div>
            </div>
            <div className="gameplay-section">
                <h2>Gameplay</h2>
                {hasGameplayImages ? (
                    <div className="gameplay-slider">
                        <button className="prev-button" onClick={handlePrevImage}>❮</button>
                        <img
                            src={`${process.env.PUBLIC_URL}${gameplayImages[currentImageIndex]?.filePath}${gameplayImages[currentImageIndex]?.fileName}`}
                            alt={`Gameplay ${currentImageIndex + 1}`}
                            className="gameplay-image"
                            onError={(e) => {
                                e.target.src = '/images/placeholder.png'; // Ảnh dự phòng nếu lỗi
                                console.log('Lỗi khi tải hình ảnh, sử dụng ảnh placeholder');
                            }}
                        />
                        <button className="next-button" onClick={handleNextImage}>❯</button>
                    </div>
                ) : (
                    <div className="no-gameplay-message">Không có hình ảnh gameplay.</div>
                )}
            </div>
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
                        value={review}
                        onChange={(e) => setReview(e.target.value)}
                        placeholder="Write your review here"
                        rows="5"
                    />
                    <button type="submit" className="submit-review-button">Submit Review</button>
                </form>
                <div className="reviews">
                    {reviews.map((rev, index) => (
                        <div key={index} className="review">
                            <span className="review-rating">Rating: {rev.rating} ★</span>
                            <p>{rev.review}</p>
                        </div>
                    ))}
                </div>
            </div>
        </div>
    );
};

export default GamePage;
