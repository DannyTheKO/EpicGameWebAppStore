import React, { useState, useEffect } from 'react';
import '../styles/pages/Cart.css';
import Payment from './Payment';

const Cart = () => {
    const [cart, setCart] = useState([]);

    // Lấy giỏ hàng từ localStorage khi component được mount
    useEffect(() => {
        const username = localStorage.getItem('username'); // Lấy username từ localStorage
        const savedCart = JSON.parse(localStorage.getItem(`cart_${username}`)) || []; // Lấy giỏ hàng của người dùng
        setCart(savedCart); // Cập nhật trạng thái giỏ hàng
    }, []); // useEffect chỉ chạy 1 lần khi trang được tải

    // Xử lý xóa 1 sản phẩm khỏi giỏ hàng
    const handleRemoveItem = (id) => {
        const username = localStorage.getItem('username'); // Lấy username từ localStorage
        const updatedCart = cart.filter(item => item.gameId !== id); // Loại bỏ sản phẩm có id tương ứng
        setCart(updatedCart);
        localStorage.setItem(`cart_${username}`, JSON.stringify(updatedCart)); // Cập nhật giỏ hàng mới vào localStorage
    };

    // Tính tổng giá trị của các sản phẩm trong giỏ hàng
    const totalPrice = cart.reduce((acc, item) => acc + item.price, 0).toFixed(2);

    return (
        <div className="cart-page">
            <h1 className="cart-title">Your Shopping Cart</h1>
            <div className="cart-container">
                <div className="cart-items">
                    {cart.length > 0 ? (
                        cart.map(item => {
                            // Lọc ảnh cover từ imageGame
                            const coverImage = item.imageGame.find(image => image.filePath.includes('cover'));

                            return (
                                <div key={item.gameId} className="cart-item">
                                    {/* Hiển thị hình ảnh của game */}
                                    {coverImage && (
                                        <img
                                            src={`${process.env.PUBLIC_URL}${coverImage.filePath}${coverImage.fileName}`}
                                            alt={item.title}
                                            className="cart-item-image"
                                        />
                                    )}
                                    <div className="cart-item-info">
                                        <span className="item-name">{item.title}</span>
                                        <span className="item-price">${item.price.toFixed(2)}</span>
                                        <span className="item-genre">{item.genre?.name || 'N/A'}</span> {/* Thể loại */}
                                    </div>
                                    {/* Nút xóa game này khỏi giỏ */}
                                    <button className="remove-button" onClick={() => handleRemoveItem(item.gameId)}>
                                        Remove
                                    </button>
                                </div>
                            );
                        })
                    ) : (
                        <div>Your cart is empty.</div>
                    )}
                </div>
                <div className="cart-summary">
                    <h3>Total: ${totalPrice}</h3>
                    <button className="checkout-button">Proceed to Checkout</button>
                    <Payment />
                </div>
            </div>
        </div>
    );
};

export default Cart;
