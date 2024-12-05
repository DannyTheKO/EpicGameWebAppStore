import React, { useState, useEffect } from 'react';
import '../styles/pages/Cart.css';
import Payment from './Payment';
import { useNavigate } from 'react-router-dom';

const Cart = () => {
    const [cart, setCart] = useState([]);
    const [showPayment, setShowPayment] = useState(false); // Trạng thái để hiển thị phần thanh toán
    const navigate = useNavigate();

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
        navigate(0); // Tải lại trang để cập nhật giỏ hàng
    };

    // Tính tổng giá trị của các sản phẩm trong giỏ hàng
    const totalPrice = cart.reduce((acc, item) => acc + item.price, 0).toFixed(2);

    // Xử lý nút "Proceed to Checkout"
    const handleProceedToCheckout = () => {
        if (cart.length === 0) {
            alert('Your cart is empty. Please add some items to your cart before proceeding to checkout.');
            return;
        }

        // Kiểm tra nếu người dùng chưa đăng nhập
        const authToken = localStorage.getItem('authToken');
        if (!authToken) {
            alert('You must be logged in to proceed to checkout.');
            navigate('/login'); // Điều hướng đến trang đăng nhập nếu chưa đăng nhập
            return;
        }

        // Hiển thị phần thanh toán ngay trên trang Cart
        setShowPayment(true);
    };

    return (
        <div className="cart-page">
            <h1 className="cart-title">Your Shopping Cart</h1>
            <div className="cart-container">
                <div className="cart-items">
                    {cart.length > 0 ? (
                        cart.map(item => {
                            // Kiểm tra cấu trúc và in ra console
                            console.log(item.imageGame); // Để kiểm tra dữ liệu của imageGame
                            const thumbnailImage = item.imageGame.find(image => image.imageType.toLowerCase() === 'thumbnail');
                            console.log(thumbnailImage); // In ra thông tin thumbnailImage

                            return (
                                <div key={item.gameId} className="cart-item">
                                    {/* Hiển thị hình ảnh của game */}
                                    {thumbnailImage ? (
                                        <img
                                            src={`${process.env.PUBLIC_URL}${thumbnailImage.filePath}${thumbnailImage.fileName}`}
                                            alt={item.title}
                                            className="cart-item-image"
                                        />
                                    ) : (
                                        <p>No thumbnail available</p> // Nếu không có ảnh thumbnail, hiển thị thông báo
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
                    <button className="checkout-button" onClick={handleProceedToCheckout}>
                        Proceed to Checkout
                    </button>
                    {showPayment && (
                        <div className="payment-section">
                            <Payment /> {/* Hiển thị phần thanh toán */}
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
};

export default Cart;
