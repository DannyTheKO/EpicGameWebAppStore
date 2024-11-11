import React, { useState } from "react";
import './Login.css';
import { FaUserAlt, FaEye, FaEyeSlash } from "react-icons/fa";
import axios from 'axios';

const LoginForm = () => {
    const [isPasswordVisible, setIsPasswordVisible] = useState(false);
    const [errorMessage, setErrorMessage] = useState(""); 
    const [username, setUsername] = useState(""); 
    const [password, setPassword] = useState(""); 

    const toggleVisibility = () => {
        setIsPasswordVisible(!isPasswordVisible);
    };

    const handleSignIn = async (event) => {
        event.preventDefault();
    
        const payload = {
            Username: username,  // Kiểm tra tên biến phải giống với mô hình backend
            Password: password   // Kiểm tra tên biến phải giống với mô hình backend
        };
    
        try {
            const response = await axios.post('http://localhost:5084/Auth/LoginConfirm', payload);
            console.log(response.data);  // Kiểm tra phản hồi từ server
        } catch (error) {
            console.error("Server responded with error: ", error.response.data);  // Xử lý lỗi nếu có
        }
    };
    
    
    return (
        <div className="wrapper-login">
            <form onSubmit={handleSignIn}>
                <div className="logo"><img src="/Images/EpicGames_Logo.png" alt="" /></div>
                <h1>SIGN IN</h1>
                <div className="input_box">
                    <input 
                        type="text" 
                        placeholder="Username" 
                        value={username} 
                        onChange={(e) => setUsername(e.target.value)} 
                        required 
                    />
                    <FaUserAlt className="icons" />
                </div>
                <div className="input_box">
                    <input
                        type={isPasswordVisible ? "text" : "password"}
                        placeholder="Password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                    />
                    <span className="iconseyes" onClick={toggleVisibility} style={{ cursor: 'pointer', color: 'white' }}>
                        {isPasswordVisible ? <FaEye /> : <FaEyeSlash />}
                    </span>
                </div>
                {errorMessage && <div className="error">{errorMessage}</div>}
                <div className="forgot">
                    <a href="/forgot_pass">Forgot password?</a>
                </div>
                <button type="submit">Sign In</button>
                <div className="register-link">
                    <a href="/register">Create account</a>
                </div>
            </form>
        </div>
    );
};

export default LoginForm;
