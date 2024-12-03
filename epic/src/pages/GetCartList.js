import React, { useState } from 'react';
import '../styles/pages/GetCartList.css';  // Cập nhật CSS cho trang giỏ hàng

const GetCartList = ({ carts }) => {
    const [selectedCart, setSelectedCart] = useState(null);  // Lưu trữ giỏ hàng đã chọn

    const handleCartClick = (cart) => {
        setSelectedCart(cart);  // Chuyển sang chế độ xem chi tiết giỏ hàng
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
                <h4>Order Date: {selectedCart.createdAt}</h4>
                <h4>Status: <span className={selectedCart.cartStatus === 'Completed' ? 'status completed' : 'status pending'}>{selectedCart.cartStatus}</span></h4>
                <h4>Total Price: ${selectedCart.total}</h4>
                <div className="cart-items">
                    <table className="cart-items-table">
                        <thead>
                            <tr>
                                <th>Game Title</th>
                                <th>Price</th>
                                <th>Discount</th>
                            </tr>
                        </thead>
                        <tbody>
                            {selectedCart.cartDetails.map(detail => (
                                <tr key={detail.cartDetailId}>
                                    <td>{detail.cartDetail.title}</td>
                                    <td>${detail.cartDetail.price}</td>
                                    <td>{detail.cartDetail.discount}%</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            </div>
        );
    }

    // Hiển thị danh sách giỏ hàng dưới dạng bảng
    return (
        <div className="cart-list-container">
            <h2>Your Cart List</h2>
            {carts.length === 0 ? (
                <p>No carts found</p>
            ) : (
                <table className="cart-table">
                    <thead>
                        <tr>
                            <th>Order #</th>
                            <th>Payment Method</th>
                            <th>Total Price</th>
                            <th>Order Date</th>
                            <th>Status</th>
                            <th>Details</th> {/* Thêm cột "Details" */}
                        </tr>
                    </thead>
                    <tbody>
                        {carts.map(cart => (
                            <tr key={cart.cartId}>
                                <td>{cart.cartId}</td>
                                <td>{cart.paymentMethod}</td>
                                <td>${cart.total}</td>
                                <td>{cart.createdAt}</td>
                                <td>
                                    <span className={`status ${cart.cartStatus === 'Completed' ? 'completed' : 'pending'}`}>
                                        {cart.cartStatus}
                                    </span>
                                </td>
                                <td>
                                    {/* Thêm nút để xem chi tiết giỏ hàng */}
                                    <button onClick={() => handleCartClick(cart)} className="view-detail-btn">
                                        View Details
                                    </button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            )}
        </div>
    );
};

export default GetCartList;
