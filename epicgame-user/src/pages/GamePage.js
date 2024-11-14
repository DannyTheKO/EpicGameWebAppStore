import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import '../styles/pages/GamePage.css';

const GamePage = () => {
    const { id } = useParams();
    const [game, setGame] = useState(null);
    const [publisherName, setPublisherName] = useState(null);
    const [genreName, setGenreName] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [rating, setRating] = useState(0);
    const [review, setReview] = useState('');
    const [reviews, setReviews] = useState([]);

    useEffect(() => {
        const fetchGameDetails = async () => {
            try {
                const response = await fetch(`http://localhost:5084/Game/GetById/${id}`);
                if (!response.ok) {
                    throw new Error("Failed to load game details.");
                }
                const data = await response.json();
                setGame(data);

                // Log game data for debugging
                console.log("Game data:", data);

                // Check and fetch publisher name
                if (!data.publisher && data.publisherId) {
                    console.log("Fetching publisher with ID:", data.publisherId); // Log publisherId
                    const publisherResponse = await fetch(`http://localhost:5084/Publisher/GetById/${data.publisherId}`);
                    if (publisherResponse.ok) {
                        const publisherData = await publisherResponse.json();
                        setPublisherName(publisherData.name);
                        console.log("Publisher data:", publisherData); // Log publisher response data
                    } else {
                        console.log("Failed to fetch publisher data");
                    }
                } else {
                    setPublisherName(data.publisher);
                }

                // Check and fetch genre name
                if (!data.genre && data.genreId) {
                    console.log("Fetching genre with ID:", data.genreId); // Log genreId
                    const genreResponse = await fetch(`http://localhost:5084/Genre/GetById/${data.genreId}`);
                    if (genreResponse.ok) {
                        const genreData = await genreResponse.json();
                        setGenreName(genreData.name);
                        console.log("Genre data:", genreData); // Log genre response data
                    } else {
                        console.log("Failed to fetch genre data");
                    }
                } else {
                    setGenreName(data.genre);
                }
            } catch (error) {
                setError(error.message);
            } finally {
                setLoading(false);
            }
        };
        fetchGameDetails();
    }, [id]);

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

    return (
        <div className="game-page">
            <div className="game-header">
                <h1>{game.title || "Game Title"}</h1>
            </div>

            <div className="game-details">
                <img src={game.coverUrl || "default-cover-url.jpg"} alt="Game Cover" className="game-cover" />
                <div className="game-info">
                    <h2>Description</h2>
                    <p>{game.description || "No description available."}</p>
                    <div className="game-rating">
                        <h3>Overall Rating:</h3>
                        <span><span className="rating-value">{game.rating || "N/A"}</span> <span className="star">★</span></span>
                    </div>
                    <div className="game-price">
                        <h3>Price:</h3>
                        <span className="price-value">${game.price || "N/A"}</span>
                    </div>
                    <button className="purchase-button">Purchase Now</button>
                    <button className="add-to-cart-button">Add to Cart</button>
                </div>

                {/* Additional Information */}
                <div className="game-metadata">
                    <p><span className="label">Author:</span> <span className="value">{game.author}</span></p>
                    <p><span className="label">Publisher:</span> <span className="value">{publisherName || "N/A"}</span></p>
                    <p><span className="label">Genre:</span> <span className="value">{genreName || "N/A"}</span></p>
                    <p><span className="label">Release Date:</span> <span className="value">{game.release}</span></p>
                </div>
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
