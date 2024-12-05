import React, { useState, useEffect } from 'react';
import { Link, useLocation, useNavigate } from 'react-router-dom';
import { FaShoppingCart } from 'react-icons/fa';
import '../styles/components/Navbar.css';
import EpicGamesLogo from '../assets/EpicGames_Logo.png';

const Navbar = () => {
    const location = useLocation();
    const navigate = useNavigate();
    const isHomePage = location.pathname === '/';

    const [isLoggedIn, setIsLoggedIn] = useState(false);
    const [username, setUsername] = useState("User");
    const [isDropdownOpen, setIsDropdownOpen] = useState(false);
    const [prevScrollPos, setPrevScrollPos] = useState(window.pageYOffset);
    const [isNavbarVisible, setIsNavbarVisible] = useState(true);
    const [cartItemCount, setCartItemCount] = useState(0);

    // Kiểm tra trạng thái đăng nhập và tên người dùng từ localStorage
    useEffect(() => {
        const token = localStorage.getItem('authToken');
        const storedUsername = localStorage.getItem('username');
        if (token && storedUsername) {
            setIsLoggedIn(true);
            setUsername(storedUsername);
        }
    }, []);

    // Lấy số lượng sản phẩm trong giỏ hàng từ localStorage mỗi khi giỏ hàng thay đổi
    useEffect(() => {
        const storedUsername = localStorage.getItem('username');
        const savedCart = JSON.parse(localStorage.getItem(`cart_${storedUsername}`)) || [];
        setCartItemCount(savedCart.length);
    }, [location.pathname]);

    // Ẩn/Hiện navbar khi cuộn trang
    useEffect(() => {
        const handleScroll = () => {
            const currentScrollPos = window.pageYOffset;
            setIsNavbarVisible(prevScrollPos > currentScrollPos || currentScrollPos < 10);
            setPrevScrollPos(currentScrollPos);
        };

        window.addEventListener('scroll', handleScroll);
        return () => window.removeEventListener('scroll', handleScroll);
    }, [prevScrollPos]);

    // Xử lý đăng xuất
    const handleLogout = () => {
        const storedUsername = localStorage.getItem('username');

        // Xóa giỏ hàng của người dùng nếu có
        if (storedUsername) {
            localStorage.removeItem(`cart_${storedUsername}`);
        }

        localStorage.removeItem('authToken');
        localStorage.removeItem('username');
        setIsLoggedIn(false);
        setUsername("User");
        navigate('/');
    };

    // Xử lý chuyển đến trang UserProfile
    const handleUserProfile = () => {
        navigate('/userprofile');
    };

    return (
        <nav className={`navbar ${isNavbarVisible ? 'visible' : 'hidden'}`}>
            <div className="navbar-left">
                <Link to="/" className="navbar-logo">
                    <img src={EpicGamesLogo} alt="Epic Games Logo" />
                </Link>
                <ul className="navbar-links">
                    <li><Link to="/">Home</Link></li>
                    <li><Link to="/store">Store</Link></li>
                    <li><Link to="/library">Library</Link></li>
                </ul>
            </div>

            <div className="navbar-buttons">
                {!isHomePage && (
                    <Link to="/cart" className="btn cart">
                        <FaShoppingCart size={20} />
                        {cartItemCount > 0 && (
                            <span className="cart-item-count">{cartItemCount}</span>
                        )}
                    </Link>
                )}
                {isLoggedIn ? (
                    <div
                        className="user-menu"
                        onMouseEnter={() => setIsDropdownOpen(true)}
                        onMouseLeave={() => setIsDropdownOpen(false)}
                    >
                        <button className="btn user-name">
                            {username}
                        </button>
                        {isDropdownOpen && (
                            <div className="dropdown-menu">
                                <button onClick={handleUserProfile} className="dropdown-item">
                                    User Profile
                                </button>
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
