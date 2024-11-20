import React, { useState } from "react";
import './Login.css';
import { FaUserAlt, FaEye, FaEyeSlash } from "react-icons/fa";
import { Link, useNavigate } from "react-router-dom";
import axios from 'axios';
import EpicGamesLogo from '../assets/EpicGames_Logo.png';

const LoginForm = () => {
    const [isPasswordVisible, setIsPasswordVisible] = useState(false);
    const [errorMessage, setErrorMessage] = useState("");
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const navigate = useNavigate();

    const toggleVisibility = () => setIsPasswordVisible(!isPasswordVisible);

    const handleSignIn = async (event) => {
        event.preventDefault();
        setErrorMessage(""); // Reset lỗi cũ

        const payload = {
            Username: username,
            Password: password,
        };

        try {
            const response = await axios.post('http://localhost:5084/Auth/LoginConfirm', payload, {
                headers: { 'Content-Type': 'application/json' }
            });

            const { loginStateFlag, accountToken, message } = response.data;

            if (loginStateFlag) {
                localStorage.setItem('authToken', accountToken);
                alert("Login successful!");
                navigate("/store");
            } else {
                setErrorMessage(message || "Login failed. Please try again.");
            }
        } catch (error) {
            const statusCode = error.response?.status || 500;
            const serverError = error.response?.data?.message || "Server error occurred. Please try again later.";

            console.error(`Error ${statusCode}:`, serverError);
            setErrorMessage(
                statusCode === 401
                    ? "Invalid credentials. Please check your username or password."
                    : "Server error occurred. Please try again later."
            );
        }
    };


    return (
        <div className="login-container">
            <div className="login-box">
                <div className="logo">
                    <Link to="/" className="navbar-logo">
                        <img src={EpicGamesLogo} alt="Epic Games Logo" className="logo-image" />
                    </Link>
                </div>
                <h1 className="login-title">SIGN IN</h1>
                <form className="login-form" onSubmit={handleSignIn}>
                    <div className="input_box icon-container">
                        <input
                            type="text"
                            placeholder="Username"
                            value={username}
                            onChange={(e) => setUsername(e.target.value)}
                            required
                        />
                        <FaUserAlt className="icon" />
                    </div>
                    <div className="input_box icon-container">
                        <input
                            type={isPasswordVisible ? "text" : "password"}
                            placeholder="Password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            required
                        />
                        <span className="iconseyes" onClick={toggleVisibility}>
                            {isPasswordVisible ? <FaEye /> : <FaEyeSlash />}
                        </span>
                    </div>
                    {errorMessage && <div className="error">{errorMessage}</div>}
                    <div className="forgot">
                        <Link to="/forgot_pass">Forgot password?</Link>
                    </div>
                    <button className="btn" type="submit">Sign In</button>
                    <div className="register-link">
                        <Link to="/register">Create account</Link>
                    </div>
                </form>
            </div>
        </div>
    );
};

export default LoginForm;
