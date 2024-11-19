import React, { useState } from 'react';
import { FaCreditCard, FaPaypal, FaUniversity, FaGift, FaBitcoin } from 'react-icons/fa';
import '../styles/pages/Payment.css';

const Payment = () => {
    const [selectedMethod, setSelectedMethod] = useState(null);

    const handlePaymentSelect = (method) => {
        setSelectedMethod(method);
    };

    const renderPaymentForm = () => {
        switch (selectedMethod) {
            case 'credit-card':
                return (
                    <div className="payment-form">
                        <label>
                            Card Number:
                            <input type="text" placeholder="1234 5678 9012 3456" />
                        </label>
                        <label>
                            Expiry Date:
                            <input type="text" placeholder="MM/YY" />
                        </label>
                        <label>
                            CVV:
                            <input type="password" placeholder="123" />
                        </label>
                    </div>
                );
            case 'gift-card':
                return (
                    <div className="payment-form">
                        <label>
                            Gift Card Code:
                            <input type="text" placeholder="XXXX-XXXX-XXXX" />
                        </label>
                    </div>
                );
            case 'cryptocurrency':
                return (
                    <div className="payment-form">
                        <label>
                            Wallet Address:
                            <input type="text" placeholder="Enter your wallet address" />
                        </label>
                    </div>
                );
            default:
                return null; // Không hiển thị form với PayPal và Bank Transfer
        }
    };

    return (
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
            {renderPaymentForm()}
        </div>
    );
};

export default Payment;
