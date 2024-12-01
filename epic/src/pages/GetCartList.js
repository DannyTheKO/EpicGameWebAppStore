import React, { useState } from 'react';
import '../styles/pages/GetCartList.css';  // Cập nhật CSS cho trang giỏ hàng

const GetCartList = () => {
    const [selectedCart, setSelectedCart] = useState(null);  // Lưu giỏ hàng đã chọn
    const [cartList, setCartList] = useState([
        { id: 1, totalPrice: 120, status: 'pending', date: '2024-11-25', items: [{ game: 'Game A', price: 50, discount: 10 }, { game: 'Game B', price: 70, discount: 15 }] },
        { id: 2, totalPrice: 200, status: 'completed', date: '2024-11-22', items: [{ game: 'Game C', price: 100, discount: 20 }, { game: 'Game D', price: 100, discount: 10 }] }
    ]);

    const handleCartClick = (cart) => {
        setSelectedCart(cart);  // Chuyển sang chi tiết giỏ hàng
    };

    const handleBackToList = () => {
        setSelectedCart(null);  // Quay lại danh sách giỏ hàng
    };

    if (selectedCart) {
        // Hiển thị chi tiết giỏ hàng
        return (
            <div className="cart-detail-container">
                <button className="back-btn" onClick={handleBackToList}>Back to Cart List</button>
                <h3>Cart Details</h3>
                    <h4>Order Date: {selectedCart.date}</h4>
                    <h4>Status: <span className={selectedCart.status === 'completed' ? 'status completed' : 'status pending'}>{selectedCart.status}</span></h4>
                    <h4>Total Price: ${selectedCart.totalPrice}</h4>
                    <div className="cart-items">
                        {selectedCart.items.map((item, index) => (
                            <div key={index} className="game-item">
                                <span>{item.game}</span>
                                <span>${item.price}</span>
                                <span>-${item.discount}</span>
                            </div>
                        ))}
                </div>
            </div>
        );
    }

    // Hiển thị danh sách giỏ hàng
    return (
        <div className="cart-list-container">
            <h2>Your Cart List</h2>
                {cartList.map(cart => (
                    <div key={cart.id} className="cart-item" onClick={() => handleCartClick(cart)}>
                        <div className="cart-header">
                            <h3>Order #{cart.id}</h3>
                            <span className={`status ${cart.status === 'completed' ? 'completed' : 'pending'}`}>{cart.status}</span>
                        </div>
                        <div className="cart-summary">
                            <p>Total Price: ${cart.totalPrice}</p>
                            <p>Order Date: {cart.date}</p>
                        </div>
                    </div>
                ))}
            </div>
    );
};

export default GetCartList;
