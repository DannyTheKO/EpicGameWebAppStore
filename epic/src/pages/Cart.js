import React, { useState, useEffect } from 'react';
import '../styles/pages/Cart.css';
import { useNavigate } from 'react-router-dom';
import { FaCreditCard, FaPaypal, FaUniversity, FaGift, FaBitcoin } from 'react-icons/fa';
import '../styles/pages/Payment.css';

const Cart = () => {
    const [cart, setCart] = useState([]);
    const [selectedMethod, setSelectedMethod] = useState(localStorage.getItem('selectedPaymentMethod') || null);
    const navigate = useNavigate();

    const handlePaymentSelect = (method) => {
        setSelectedMethod(method);
        localStorage.setItem('selectedPaymentMethod', method); // Lưu phương thức thanh toán vào localStorage
    };

    // Lấy giỏ hàng từ API
    const fetchCart = async () => {
        try {
            const authToken = localStorage.getItem('authToken');
            const headers = {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${authToken}`
            };
            const response = await fetch('http://localhost:5084/Store/CheckoutPage/CurrentCartList', { headers });

            if (response.status === 401) {
                alert('You are not authorized to view this cart. Please log in.');
                navigate('/login'); // Điều hướng người dùng đến trang đăng nhập nếu chưa đăng nhập
                return;
            }

            if (!response.ok) {
                throw new Error('Failed to fetch cart');
            }

            const data = await response.json();

            if (data.cartdetails) {
                const cartDetails = await Promise.all(data.cartdetails.map(async (item) => {
                    const gameResponse = await fetch(`http://localhost:5084/Store/FeaturePage/GetGameId/${item.gameId}`);
                    const gameData = await gameResponse.json();
                    return {
                        ...item,
                        ...gameData
                    };
                }));
                setCart(cartDetails);
            }
        } catch (error) {
            console.error("Error fetching cart:", error);
        }
    };

    useEffect(() => {
        fetchCart();
    }, []);

    // Xử lý xóa 1 sản phẩm khỏi giỏ hàng
    const handleRemoveItem = async (id) => {
        try {
            const authToken = localStorage.getItem('authToken');
            const response = await fetch(`http://localhost:5084/Store/CheckoutPage/RemoveCartItem/${id}`, {
                method: 'DELETE',
                headers: {
                    'Authorization': `Bearer ${authToken}`
                }
            });
            const result = await response.json();
            if (result.success) {
                setCart(cart.filter(item => item.cartDetailId !== id)); // Cập nhật lại giỏ hàng sau khi xóa
                navigate(0); // Tải lại trang
            } else {
                alert(result.message);
            }
        } catch (error) {
            console.error("Error removing item:", error);
        }
    };

    // Tính tổng giá trị của các sản phẩm trong giỏ hàng
    const totalPrice = cart.reduce((acc, item) => acc + item.price, 0).toFixed(2);

    // Xử lý nút "Proceed to Checkout"
    const handleProceedToCheckout = async () => {
        if (cart.length === 0) {
            alert('Your cart is empty. Please add some items to your cart before proceeding to checkout.');
            return;
        }

        const authToken = localStorage.getItem('authToken');
        if (!authToken) {
            alert('You must be logged in to proceed to checkout.');
            navigate('/login'); // Điều hướng đến trang đăng nhập nếu chưa đăng nhập
            return;
        }

        if (!selectedMethod) {
            alert('Please select a valid payment method.');
            return;
        }

        const cartId = cart[0]?.cartId;
        if (!cartId) {
            alert('Invalid cart information. Please try again.');
            return;
        }

        let paymentMethodId;
        switch (selectedMethod) {
            case 'credit-card':
                paymentMethodId = 1;
                break;
            case 'paypal':
                paymentMethodId = 2;
                break;
            case 'bank-transfer':
                paymentMethodId = 3;
                break;
            case 'gift-card':
                paymentMethodId = 4;
                break;
            case 'cryptocurrency':
                paymentMethodId = 5;
                break;
            default:
                alert('Please select a valid payment method.');
                return;
        }

        try {
            const response = await fetch(`http://localhost:5084/Store/CheckoutPage/CompleteCheckout?cartId=${cartId}&paymentMethodId=${paymentMethodId}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${authToken}`
                }
                
            });

            if (response.ok) {
                // Fetch profile information after successful checkout
                const profileResponse = await fetch('http://localhost:5084/Profile/ProfileUser/GetProfile', {
                    method: 'GET',
                    headers: {
                        'Authorization': `Bearer ${authToken}`
                    }
                });
                alert('Checkout Complete');
                if (profileResponse.ok) {
                    const profileData = await profileResponse.json();
                    const accountId = profileData.data.accountId;

                    // Prepare data to link account with the purchased game
                    const addAccountGamePromises = cart.map(item => {
                        const accountGameData = {
                            AccountId: accountId,
                            GameId: item.gameId
                        };

                        // Call AddAccountGame API to link account with the game
                        return fetch('http://localhost:5084/Store/CheckoutPage/AddAccountGame', {
                            method: 'POST',
                            headers: {
                                'Content-Type': 'application/json',
                                'Authorization': `Bearer ${authToken}`
                            },
                            body: JSON.stringify(accountGameData)
                        });
                    });

                    try {
                        await Promise.all(addAccountGamePromises);
                    } catch (error) {
                        console.error('Error linking accounts with games:', error);
                        alert('Error linking accounts with games.');
                    }
                } else {
                    console.error('Error fetching profile:', profileResponse.statusText);
                    alert('Error fetching profile information.');
                }

                navigate(0); // Reload lại trang để cập nhật lại giỏ hàng
            } else {
                const error = await response.json();
                console.error('Server Error:', error.message);
                alert(`Error completing checkout: ${error.message}`);
            }
        } catch (error) {
            console.error("Error completing checkout:", error);
            alert('An error occurred while processing your payment. Please try again.');
        }
    };


    return (
        <div className="cart-page">
            <h1 className="cart-title">Your Shopping Cart</h1>
            <div className="cart-container">
                <div className="cart-items">
                    {cart.length > 0 ? (
                        cart.map(item => (
                            <div key={item.cartDetailId} className="cart-item">
                                {item.imageGame.length > 0 ? (
                                    <img
                                        src={`${process.env.PUBLIC_URL}${item.imageGame[0].filePath}${item.imageGame[0].fileName}`}
                                        alt={item.title}
                                        className="cart-item-image"
                                    />
                                ) : (
                                    <p>No thumbnail available</p>
                                )}
                                <div className="cart-item-info">
                                    <span className="item-name">{item.title || "No title available"}</span>
                                    <span className="item-price">${item.price.toFixed(2)}</span>
                                    <span className="item-genre">{item.genre?.name || 'N/A'}</span>
                                </div>
                                <button className="remove-button" onClick={() => handleRemoveItem(item.cartDetailId)}>
                                    Remove
                                </button>
                            </div>
                        ))
                    ) : (
                        <div>Your cart is empty.</div>
                    )}
                </div>
                <div className="cart-summary">
                    <h3>Total: ${totalPrice}</h3>
                    <button className="checkout-button" onClick={handleProceedToCheckout}>
                        Proceed to Checkout
                    </button>
                    <div className="payment-container">
                        <h2 className="payment-title">Payment Options</h2>
                        <div className="payment-methods">
                            <div
                                className={`payment-method ${selectedMethod === 'credit-card' ? 'selected' : ''}`}
                                onClick={() => handlePaymentSelect('credit-card')}
                            >
                                <FaCreditCard className="payment-icon" />
                                <span>Credit Card</span>
                            </div>
                            <div
                                className={`payment-method ${selectedMethod === 'paypal' ? 'selected' : ''}`}
                                onClick={() => handlePaymentSelect('paypal')}
                            >
                                <FaPaypal className="payment-icon" />
                                <span>PayPal</span>
                            </div>
                            <div
                                className={`payment-method ${selectedMethod === 'bank-transfer' ? 'selected' : ''}`}
                                onClick={() => handlePaymentSelect('bank-transfer')}
                            >
                                <FaUniversity className="payment-icon" />
                                <span>Bank Transfer</span>
                            </div>
                            <div
                                className={`payment-method ${selectedMethod === 'gift-card' ? 'selected' : ''}`}
                                onClick={() => handlePaymentSelect('gift-card')}
                            >
                                <FaGift className="payment-icon" />
                                <span>Gift Card</span>
                            </div>
                            <div
                                className={`payment-method ${selectedMethod === 'cryptocurrency' ? 'selected' : ''}`}
                                onClick={() => handlePaymentSelect('cryptocurrency')}
                            >
                                <FaBitcoin className="payment-icon" />
                                <span>Cryptocurrency</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Cart;
