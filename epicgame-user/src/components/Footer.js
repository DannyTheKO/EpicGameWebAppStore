// src/components/Footer.js
import React from 'react';
import '../styles/components/Footer.css'; // Import CSS cho Footer

const Footer = () => {
    return (
        <footer className="footer">
            <div className="footer-content">
                <div className="footer-text">
                    <p>&copy; 2024 Epic Game Store. All rights reserved.</p>
                </div>
                <ul className="footer-links">
                    <li><a href="#">Privacy Policy</a></li>
                    <li><a href="#">Terms of Service</a></li>
                    <li><a href="#">Help Center</a></li>
                </ul>
            </div>
        </footer>
    );
};

export default Footer;
