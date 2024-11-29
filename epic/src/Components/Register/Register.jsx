import React, { useState } from "react";
import { FaUserAlt, FaEye, FaEyeSlash } from "react-icons/fa";
import axios from "axios";
import "./regester.css";

const RegisterForm = () => {
  const [email, setEmail] = useState("");
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [errorMessage, setErrorMessage] = useState("");
  const [successMessage, setSuccessMessage] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [isPasswordVisible, setIsPasswordVisible] = useState(false);
  const toggleVisibility = () => setIsPasswordVisible(!isPasswordVisible);
  const handleRegister = async (e) => {
    e.preventDefault();

    // Kiểm tra mật khẩu xác nhận
    if (password !== confirmPassword) {
      setErrorMessage("Passwords do not match!");
      return;
    }
    const accountNew = {
      Username :username,
      Email:email,
      Password:password,
      ConfirmPassword:confirmPassword
    };

    try {
        const response = await axios.post(
            "http://localhost:5084/auth/RegisterConfirm", // Đảm bảo đường dẫn đúng
            accountNew, // Dữ liệu được gửi đi
            {
              headers: {
                'Content-Type': 'application/json'
              }
            }
          );
          window.location.href = "/login";
          

      if (response.status === 201) {
        setSuccessMessage("Registration successful!");
        setErrorMessage("");
        // Reset form sau khi đăng ký thành công
        setEmail("");
        setUsername("");
        setPassword("");
        setConfirmPassword("");
      }
    } catch (error) {
      setErrorMessage(
        error.response?.data?.message || "An error occurred. Please try again."
      );
      setSuccessMessage("");
    }
  };
  return (
    <div className="wrapper">
      <form className="form-register" onSubmit={handleRegister}>
        <div className="logo">
          <img src="/Images/EpicGames_Logo.png" alt="" />
        </div>
        <h1>REGISTER</h1>
        <div className="input_box">
          <input
            type="email"
            placeholder="Email"
            required
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />
        </div>
        <div className="input_box">
          <input
            type="text"
            placeholder="Username"
            required
            value={username}
            onChange={(e) => setUsername(e.target.value)}
          />
        </div>
        <div className="input_box">
          <input
            type={isPasswordVisible ? "text" : "password"}
            placeholder="Password"
            required
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
          <span className="iconseyes" onClick={toggleVisibility}>
            {isPasswordVisible ? <FaEye /> : <FaEyeSlash />}
          </span>
        </div>
        <div className="input_box">
          <input
            type={isPasswordVisible ? "text" : "password"}
            placeholder="Confirm Password"
            required
            value={confirmPassword}
            onChange={(e) => setConfirmPassword(e.target.value)}
          />
          <span className="iconseyes" onClick={toggleVisibility}>
            {isPasswordVisible ? <FaEye /> : <FaEyeSlash />}
          </span>
        </div>

        {errorMessage && <div className="error">{errorMessage}</div>}
        {successMessage && <div className="success">{successMessage}</div>}
        <button className="register" type="submit">
          Register
        </button>
        <div className="login-link">
          <p>
            Already have an account? <a href="/login">Sign in</a>
          </p>
        </div>
      </form>
    </div>
  );
};

export default RegisterForm;
