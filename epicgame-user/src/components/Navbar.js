import React from 'react';
import { Link, useLocation } from 'react-router-dom';
import '../styles/components/Navbar.css'; // Điều chỉnh đường dẫn nếu cần
import { FaSearch, FaShoppingCart } from 'react-icons/fa';

const Navbar = () => {
    const location = useLocation(); // Lấy đối tượng location

    // Kiểm tra xem hiện tại có phải là trang Home không
    const isHomePage = location.pathname === '/';

    return (
        <nav className="navbar">
            <div className="navbar-logo">Epic Store</div>
            <ul className="navbar-links">
                <li><Link to="/">Home</Link></li>
                <li><Link to="/store">Store</Link></li>
                <li><Link to="/library">Library</Link></li>
            </ul>
            
            
            <div className="navbar-buttons">
                {!isHomePage && (
                    <Link to="/cart" className="btn cart">
                        <FaShoppingCart size={20} color="white" /> {/* Biểu tượng giỏ hàng */}
                    </Link>
                )}
                <button className="btn sign-in">Sign In</button>
                <button className="btn download">Download</button>
            </div>
        </nav>
    );
};

export default Navbar;
