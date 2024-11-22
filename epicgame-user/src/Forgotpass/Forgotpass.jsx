import React, { useState } from "react";
import { FaEnvelope } from "react-icons/fa";
import { Link } from "react-router-dom";
import EpicGamesLogo from '../images/EpicGames_Logo.png';
import "./forgot.css";

const Forgotpass = () => {
    const [email, setEmail] = useState("");
    const [errorMessage, setErrorMessage] = useState("");
    const [successMessage, setSuccessMessage] = useState("");

    const handleForgotPassword = (e) => {
        e.preventDefault();
        if (!email) {
            setErrorMessage("Vui lòng nhập email của bạn.");
        } else {
            setErrorMessage("");
            setSuccessMessage("Email đặt lại mật khẩu đã được gửi, vui lòng kiểm tra hộp thư của bạn.");
        }
    };

    return (
        <div className="forgot-container">
            <div className="forgot-box">
                <div className="logo">
                    <Link to="/">
                        <img src={EpicGamesLogo} alt="Epic Games Logo" />
                    </Link>
                </div>
                <h1 className="forgot-title">FORGOT PASSWORD</h1>
                <form className="forgot-form" onSubmit={handleForgotPassword}>
                    <div className="input_box icon-container">
                        <input
                            type="email"
                            placeholder="Email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            required
                        />
                        <FaEnvelope className="icon" />
                    </div>
                    {errorMessage && <div className="error">{errorMessage}</div>}
                    {successMessage && <div className="success">{successMessage}</div>}
                    <button className="btn" type="submit">Send</button>
                    <div className="login-link">
                        <Link to="/login">Already have an account? Sign in</Link>
                    </div>
                </form>
            </div>
        </div>
    );
};

export default Forgotpass;
