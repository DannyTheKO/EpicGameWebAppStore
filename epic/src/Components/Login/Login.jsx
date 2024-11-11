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

        if (!username) {
            setErrorMessage("Username không được để trống");
        } else if (!password) {
            setErrorMessage("Password không được để trống");
        } else if (password.length < 6) {
            setErrorMessage("Password phải có ít nhất 6 ký tự");
        } else {
            setErrorMessage(""); 
            
            try {
                const response = await axios.post('https://localhost:7206/api/auth/login', {
                    username,
                    password,
                });
                
                const token = response.data.Token;
                localStorage.setItem('token', token); 
                console.log("Đăng nhập thành công");
                alert("Đăng nhập thành công!");
            } catch (error) {
                setErrorMessage("Đăng nhập thất bại. Vui lòng kiểm tra lại thông tin.");
                console.error(error);
            }
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
