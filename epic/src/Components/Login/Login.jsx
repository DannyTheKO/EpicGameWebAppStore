import React, { useState } from "react";
import './Login.css';
import { FaUserAlt, FaEye, FaEyeSlash } from "react-icons/fa";
import { Link, useNavigate } from "react-router-dom";
import axios from 'axios';
import {jwtDecode} from 'jwt-decode';
import EpicGamesLogo from '../../assets/EpicGames_Logo.png';

const LoginForm = () => {
    const [isPasswordVisible, setIsPasswordVisible] = useState(false);
    const [errorMessage, setErrorMessage] = useState("");
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const navigate = useNavigate();

    const toggleVisibility = () => setIsPasswordVisible(!isPasswordVisible);

    const handleSignIn = async (event) => {
        event.preventDefault();

        const payload = {
            Username: username,
            Password: password
        };

        try {
            const response = await axios.post('http://localhost:5084/Auth/LoginConfirm', payload, {
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            console.log(response.data);
            const { loginStateFlag, accountToken, username: returnedUsername, message } = response.data;
            // console

            if (loginStateFlag) {
                localStorage.setItem('authToken', accountToken);
                localStorage.setItem('username', returnedUsername); // Lưu username từ server

                // Khởi tạo giỏ hàng mới cho tài khoản này nếu chưa có
                const savedCart = JSON.parse(localStorage.getItem(`cart_${returnedUsername}`)) || [];
                localStorage.setItem(`cart_${returnedUsername}`, JSON.stringify(savedCart));
                const decodedToken = jwtDecode(accountToken);
  
            const userRole = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
            
            if (userRole === "Admin"|| userRole ==="Moderator")  {
                navigate('/admin'); // Chuyển hướng đến trang admin
            } else {
                navigate('/');
                window.location.reload();
            }  // Chuyển hướng và reload lại trang
              
            } else {
                setErrorMessage(message || "Login failed. Please try again.");
            }
        } catch (error) {
            console.error("Server responded with error: ", error.response?.data);
            const statusCode = error.response?.status || 500;
            const serverError = error.response?.data?.message || "Server error occurred. Please try again later.";
            setErrorMessage(
                statusCode === 401
                    ? "Invalid credentials. Please check your username or password."
                    : serverError
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