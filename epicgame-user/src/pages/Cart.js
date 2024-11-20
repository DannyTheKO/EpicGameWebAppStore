// src/pages/Cart.js
import React, { useState } from 'react';
import '../styles/pages/Cart.css';
import Payment from './Payment'; // Đúng đường dẫn vì Payment nằm cùng thư mục với Cart

const initialCart = [
    { id: 1, name: "Game 1", description: "An exciting action game.", price: 19.99, genre: "Action", image: require('../images/game1.png') },
    { id: 2, name: "Game 2", description: "A thrilling adventure awaits.", price: 29.99, genre: "Adventure", image: require('../images/game2.png') },
    { id: 3, name: "Game 3", description: "Explore the world of RPGs.", price: 39.99, genre: "RPG", image: require('../images/game3.png') },
];

const Cart = () => {
    const [cart, setCart] = useState(initialCart);

    const handleRemoveItem = (id) => {
        setCart(cart.filter(item => item.id !== id));
    };

    const totalPrice = cart.reduce((acc, item) => acc + item.price, 0).toFixed(2);

    return (
        <div className="cart-page">
            <h1 className="cart-title">Your Shopping Cart</h1>
            <div className="cart-container">
                <div className="cart-items">
                    {cart.map(item => (
                        <div key={item.id} className="cart-item">
                            <img src={item.image} alt={item.name} className="cart-item-image" />
                            <div className="cart-item-info">
                                <span className="item-name">{item.name}</span>
                                <span className="item-description">{item.description}</span>
                                <span className="item-price">${item.price.toFixed(2)}</span>
                            </div>
                            <button className="remove-button" onClick={() => handleRemoveItem(item.id)}>
                                Remove
                            </button>
                        </div>
                    ))}
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
