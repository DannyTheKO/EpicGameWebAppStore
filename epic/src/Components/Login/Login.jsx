    import React, { useState } from "react";
    import './Login.css';
    import { jwtDecode } from "jwt-decode";
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
        const checkUserRole = () => {
            const token = localStorage.getItem("accountToken");
            if (!token) {
                console.log("Token not found, user is not logged in.");
                return false;
            }
        
            try {
                // Giải mã token để lấy thông tin người dùng
                const decodedToken = jwtDecode(token);
                console.log("Decoded token:", decodedToken);
                const role= decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]
                if (role === '1' || role === '2' || role === '4') {
                    console.log("User is admin");
                    window.location.href = "/admin";
                    return true;  // Quản trị viên có quyền
                }
                
                if (role === '3') {
                    console.log("User is normal user");
                    window.location.href = "/index";
                    return true;  // Người dùng bình thường
                }
                else {
                    console.log("User role is not recognized.");
                    return false;
                }
            } catch (error) {
                console.error("Error decoding token:", error);
                return false;
            }
        };
        const handleSignIn = async (event) => {
            event.preventDefault(); // Ngăn chặn reload trang mặc định
        
            const payload = {
                Username: username, // Phải trùng với tên thuộc tính của backend
                Password: password
            };
        
            try {
                const response = await axios.post('http://localhost:5084/Auth/LoginConfirm', payload, {
                    headers: {
                        'Content-Type': 'application/json',
                        //  'Authorization': `Bearer ${token}`
                       
                    }
                });
                // Kiểm tra token từ phản hồi của server
                if (response.data.loginStateFlag ) {
                    console.log( response.data.message,response.data.username);
                    const token = response.data.accountToken;
                    // Lưu token vào localStorage
                    localStorage.setItem("accountToken", token);
                    console.log("Login successful! Token:", token);
                    // Điều hướng người dùng sau khi đăng nhập thành công (ví dụ: đến trang Dashboard)
                    checkUserRole();
                } else {
                    setErrorMessage("Token không tồn tại hoặc đăng nhập thất bại.");
                }
            } catch (error) {
                if (error.response) {
                    // Nếu có phản hồi từ server với lỗi cụ thể
                    setErrorMessage(error.response.data.message || "Lỗi đăng nhập! Vui lòng thử lại.");
                    console.error("Server responded with error: ", error.response.data);
                } else {
                    // Lỗi khác (ví dụ: không thể kết nối tới server)
                    setErrorMessage("Không thể kết nối tới server. Vui lòng kiểm tra lại.");
                    console.error("Error connecting to server: ", error.message);
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
