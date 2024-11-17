import React, { useState } from "react";
import { FaUserAlt, FaEnvelope, FaEye, FaEyeSlash } from "react-icons/fa";
import { Link, useNavigate } from "react-router-dom";
import EpicGamesLogo from '../assets/EpicGames_Logo.png';
import "./register.css";

const RegisterForm = () => {
    const [isPasswordVisible, setIsPasswordVisible] = useState(false);
    const [email, setEmail] = useState("");
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [errorMessage, setErrorMessage] = useState("");
    const navigate = useNavigate();

    const toggleVisibility = () => setIsPasswordVisible(!isPasswordVisible);

    const handleRegister = (e) => {
        e.preventDefault();

        // Giả sử đăng ký thành công
        console.log("Register button clicked");

        // Chuyển hướng người dùng đến trang Login
        navigate("/login");
    };

    return (
        <div className="register-container">
            <div className="register-box">
                <div className="logo">
                    <Link to="/">
                        <img src={EpicGamesLogo} alt="Epic Games Logo" />
                    </Link>
                </div>
                <h1 className="register-title">REGISTER</h1>
                <form className="register-form" onSubmit={handleRegister}>
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
                    <button className="btn" type="submit">Register</button>
                    <div className="login-link">
                        <Link to="/login">Already have an account? Sign in</Link>
                    </div>
                </form>
            </div>
        </div>
    );
};

export default RegisterForm;
