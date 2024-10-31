// src/pages/GamePage.js
import React, { useState } from 'react';
import '../styles/pages/GamePage.css';

const GamePage = () => {
    const [rating, setRating] = useState(0);
    const [review, setReview] = useState('');
    const [reviews, setReviews] = useState([]);

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

    return (
        <div className="game-page">
            <div className="game-header">
                <h1>Game Title</h1>
            </div>
            <div className="game-details">
                <img src={require('../assets/game1.png')} alt="Game Cover" />
                <div className="game-info">
                    <h2>Description</h2>
                    <p>This is a detailed description of the game. It includes information about the gameplay, features, and other details.</p>
                    <h3>Price: $19.99</h3>
                    <button className="purchase-button">Purchase Now</button>
                    <button className="add-to-cart-button">Add to Cart</button>
                </div>
            </div>

            {/* Phần yêu cầu cấu hình */}
            <div className="requirements">
                <h2>Requirements</h2>
                <div className="requirement-content">
                    <div className="requirement-category">
                        <h3>Minimum</h3>
                        <p><strong>OS:</strong> Windows 10 x64</p>
                        <p><strong>Processor:</strong> Intel Core i5-4590 (4 * 3300) or AMD FX-8350 (4 * 4000) or equivalent</p>
                        <p><strong>Memory:</strong> 8 GB RAM</p>
                        <p><strong>Storage:</strong> 65GB SSD</p>
                        <p><strong>DirectX:</strong> 11</p>
                        <p><strong>Graphics:</strong> GeForce GTX 960 (4096 MB) or Radeon RX 480 (8192 MB) or Intel Arc A380 (8192 MB)</p>
                    </div>
                    <div className="requirement-category">
                        <h3>Recommended</h3>
                        <p><strong>OS:</strong> Windows 10 x64</p>
                        <p><strong>Processor:</strong> Intel Core i9-9900k (8 * 3600) or AMD Ryzen 5 5600X (6 * 3700 ) or equivalent</p>
                        <p><strong>Memory:</strong> 16 GB RAM</p>
                        <p><strong>Storage:</strong> 65GB SSD</p>
                        <p><strong>DirectX:</strong> 12</p>
                        <p><strong>Graphics:</strong> GeForce RTX 2070 Super (8192 MB) or Radeon RX 6800 XT (16384 MB) or Intel Arc A770 (16384 MB)</p>
                    </div>
                </div>
            </div>

            {/* Phần đánh giá và review */}
            <div className="review-section">
                <h2>User Ratings and Reviews</h2>
                <form onSubmit={handleReviewSubmit}>
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
