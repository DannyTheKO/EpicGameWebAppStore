import React, { useState } from "react";
import axios from "axios";
import "./forgot.css";
import EpicGamesLogo from "../../assets/EpicGames_Logo.png";
const Forgotpass = () => {
    const [email, setEmail] = useState("");
    const [username, setUsername] = useState("");
    const [errorMessage, setErrorMessage] = useState("");
    const [successMessage, setSuccessMessage] = useState("");

    const handleForgotPassword = async (e) => {
        e.preventDefault();
        setErrorMessage("");
        setSuccessMessage("");

        try {
            const response = await axios.post(
                "http://localhost:5084/Authenticate/ForgotPassword",
                { Username: username, Email: email },
                { headers: { "Content-Type": "application/json" } }
            );

            if (response.data.forgotPasswordState) {
                setSuccessMessage(response.data.message);
                setUsername("");
                setEmail("");
            } else {
                setErrorMessage(response.data.message);
            }
        } catch (error) {
            setErrorMessage(error.response?.data?.message || "Có lỗi xảy ra. Vui lòng thử lại.");
        }
    };

    return (
        <div className="forgot-container">
            <div className="forgot-box">
                <div className="forgot-logo">
                    <img src={EpicGamesLogo} alt="Epic Games Logo" />
                </div>
                <h1 className="forgot-title">Forgot Password</h1>
                <form className="forgot-form" onSubmit={handleForgotPassword}>
                    <div className="forgot-input">
                        <input
                            type="text"
                            placeholder="Username"
                            value={username}
                            onChange={(e) => setUsername(e.target.value)}
                            required
                        />
                    </div>
                    <div className="forgot-input">
                        <input
                            type="email"
                            placeholder="Email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            required
                        />
                    </div>
                    {errorMessage && <div className="forgot-error">{errorMessage}</div>}
                    {successMessage && <div className="forgot-success">{successMessage}</div>}
                    <button type="submit" className="forgot-button">Send</button>
                </form>
                <div className="forgot-links">
                    <p><a href="/login">Sign in</a></p>
                    <p><a href="/register">Create account</a></p>
                </div>
            </div>
        </div>
    );
};

export default Forgotpass;
