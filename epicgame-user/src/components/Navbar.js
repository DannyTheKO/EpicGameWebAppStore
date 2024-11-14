// src/components/Navbar.jsx
import React, { useState, useEffect } from 'react';
import { Link, useLocation, useNavigate } from 'react-router-dom';
import { FaSearch, FaShoppingCart } from 'react-icons/fa';
import '../styles/components/Navbar.css';
import EpicGamesLogo from '../assets/EpicGames_Logo.png';

const Navbar = () => {
    const location = useLocation();
    const navigate = useNavigate();
    const isHomePage = location.pathname === '/';

    const [isLoggedIn, setIsLoggedIn] = useState(false);
    const [username, setUsername] = useState("User");
    const [isDropdownOpen, setIsDropdownOpen] = useState(false);

    useEffect(() => {
        const token = localStorage.getItem('accountToken');
        if (token) {
            setIsLoggedIn(true);
            setUsername("YourUserName"); // Thay thế bằng tên người dùng thực tế từ API
        }
    }, []);

    const handleLogout = () => {
        localStorage.removeItem('accountToken');
        setIsLoggedIn(false);
        navigate('/');
    };

    return (
        <nav className="navbar">
            <Link to="/" className="navbar-logo">
                <img src={EpicGamesLogo} alt="Epic Games Logo" className="logo-image" />
            </Link>
            <ul className="navbar-links">
                <li><Link to="/">Home</Link></li>
                <li><Link to="/store">Store</Link></li>
                <li><Link to="/library">Library</Link></li>
            </ul>

            <div className="navbar-buttons">
                {!isHomePage && (
                    <Link to="/cart" className="btn cart">
                        <FaShoppingCart size={20} color="white" />
                    </Link>
                )}
                {isLoggedIn ? (
                    <div className="user-menu">
                        <button
                            className="btn user-name"
                            onClick={() => setIsDropdownOpen(!isDropdownOpen)}
                        >
                            {username}
                        </button>
                        {isDropdownOpen && (
                            <div className="dropdown-menu">
                                <button onClick={handleLogout} className="dropdown-item">
                                    Logout
                                </button>
                            </div>
                        )}
                    </div>
                ) : (
                    <Link to="/login" className="btn sign-in">Sign In</Link>
                )}
                <button className="btn download">Download</button>
            </div>
        </nav>
    );
};

export default Navbar;
