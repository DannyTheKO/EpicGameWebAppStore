// src/components/Footer.js
import React from 'react';
import '../styles/components/Footer.css'; // Import CSS cho Footer
import epicLogo from '../images/EpicGames_Logo.png'; // Đảm bảo logo Epic Games ở thư mục `assets`
import { FaFacebook, FaTwitter, FaYoutube } from 'react-icons/fa';
const Footer = () => {
    return (
        <footer className="footer">
            <div className="footer-content">
                <div className="footer-icons">
                    <a href="#" aria-label="Facebook"><FaFacebook /></a>
                    <a href="#" aria-label="Twitter"><FaTwitter /></a>
                    <a href="#" aria-label="YouTube"><FaYoutube /></a>
                </div>
                <div className="footer-logo">
                    <img src={epicLogo} alt="Epic Games Logo" />
                </div>
            </div>
            <div className="footer-description">
                <p>&copy; 2024, Epic Games, Inc. All rights reserved. Epic, Epic Games, the Epic Games logo, Fortnite, the Fortnite logo, Unreal, Unreal Engine, the Unreal Engine logo, Unreal Tournament, and the Unreal Tournament logo are trademarks or registered trademarks of Epic Games, Inc. in the United States of America and elsewhere. Other brands or product names are the trademarks of their respective owners.</p>
                <p>Our websites may contain links to other sites and resources provided by third parties. These links are provided for your convenience only. Epic Games has no control over the contents of those sites or resources, and accepts no responsibility for them or for any loss or damage that may arise from your use of them.</p>
                <div className="footer-policy-links">
                    <a href="#">Terms of Service</a> | <a href="#">Privacy Policy</a> | <a href="#">Store Refund Policy</a>
                </div>
            </div>
        </footer>
    );
};

export default Footer;
