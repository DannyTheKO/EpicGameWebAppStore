import React, { useState } from "react";
import './Login.css';
import { IoEyeOutline } from "react-icons/io5";
import { FaUserAlt, FaEye, FaEyeSlash } from "react-icons/fa"; // Import icons từ react-icons
import { GoEye } from "react-icons/go";

const LoginForm = () => {
    // Tạo state để lưu trữ trạng thái hiển thị mật khẩu
    const [isPasswordVisible, setIsPasswordVisible] = useState(false);
    const [errorMessage, setErrorMessage] = useState(""); // Trạng thái lỗi
    const [username, setUsername] = useState(""); // Trạng thái username
    const [password, setPassword] = useState(""); // Trạng thái password
    // Hàm để toggle trạng thái hiển thị mật khẩu
    const toggleVisibility = () => {
        setIsPasswordVisible(!isPasswordVisible);
    };
    const handleSignIn = () => {
        // Kiểm tra điều kiện lỗi (ví dụ: username hoặc password rỗng)
        if (!username) {
            setUsername("");
            setErrorMessage("Username không được để trống");
        } else if (!password) {
            setErrorMessage("Password không được để trống");
            setPassword("");
        } else if (password.length < 6) {
            setErrorMessage("Password phải có ít nhất 6 ký tự");
        } else {
            setErrorMessage(""); // Xóa lỗi nếu thông tin hợp lệ
            console.log("Đăng nhập thành công");
            // Xử lý đăng nhập...
        }
    };
    return (
        <div className="wrapper-login">
            <form action="">
                <div className="logo"><img src="/Images/EpicGames_Logo.png" alt="" /></div>
                <h1>SIGN IN</h1>
                <div className="input_box">
                    <input type="text" placeholder="Username" name="" id="" 
                    value={username} // Ràng buộc giá trị
                    onChange={(e) => setUsername(e.target.value)}
                    required />
                    <FaUserAlt className="icons" />
                </div>
                <div className="input_box">
                    {/* Thay đổi type của input theo trạng thái của mật khẩu */}
                    <input
                        type={isPasswordVisible ? "text" : "password"}
                        placeholder="Password"
                        value={password} // Ràng buộc giá trị
                         onChange={(e) => setPassword(e.target.value)} 
                        required
                    />
                    {/* Icon để toggle hiển thị mật khẩu */}
                    <span className="iconseyes" onClick={toggleVisibility} style={{ cursor: 'pointer',color: 'white' }}>
                        {isPasswordVisible ?<FaEye /> : <FaEyeSlash />  }
                    </span>
                </div>
                {errorMessage && <div className="error">{errorMessage}</div>}
                <div className="forgot">
                    <a href="/forgot_pass">Forgot password?</a>
                </div>
            
                <button type="submit"onClick={handleSignIn}>Sign In</button>
                <div className="register-link">
                     <a href="/register">Creart account</a>
                </div>
            </form>
        </div>
    );
};

// Chỉ export LoginForm vì ToggleIcon không cần thiết trong trường hợp này
export default LoginForm;
