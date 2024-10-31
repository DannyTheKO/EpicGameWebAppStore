import React from 'react';
import { Link } from 'react-router-dom'; // Import Link từ react-router-dom
import '../styles/pages/HomePage.css';

// Import hình ảnh từ thư mục assets
import image1 from '../assets/image1.jpg';
import image2 from '../assets/image2.jpg';
import image3 from '../assets/image3.jpg';

const images = [image1, image2, image3];

const HomePage = () => {
    return (
        <div className="home-page">
            <div className="banner">
                {images.map((image, index) => (
                    <Link to="/store" key={index}> {/* Bọc hình ảnh bằng Link */}
                        <img
                            src={image}
                            alt={`Banner ${index + 1}`}
                            className="banner-image"
                        />
                    </Link>
                ))}
            </div>
        </div>
    );
};

export default HomePage;
