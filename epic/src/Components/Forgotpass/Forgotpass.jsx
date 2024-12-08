import React, { useState } from "react";
import axios from "axios";
import "./forgot.css";

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
            // Gửi yêu cầu POST tới API backend
            const response = await axios.post(
                "http://localhost:5084/Authenticate/ForgotPassword",
                {
                    Username: username,
                    Email: email,
                },
                {
                    headers: {
                        "Content-Type": "application/json", // Đảm bảo header đúng
                    },
                }
            );

            // Kiểm tra trạng thái trả về
            if (response.data.forgotPasswordState) {
                setSuccessMessage(response.data.message);
                setUsername("");
                setEmail("");
            } else {
                setErrorMessage(response.data.message);
            }
        } catch (error) {
            // Xử lý lỗi từ server
            setErrorMessage(error.response?.data?.message || "Có lỗi xảy ra. Vui lòng thử lại.");
        }
    };

    return (
        <div className="wrapper-forgot">
            <form className="form-register" onSubmit={handleForgotPassword}>
                <div className="logo">
                    <img src="/Images/EpicGames_Logo.png" alt="Epic Games Logo" />
                </div>
                <h1>FORGOT PASSWORD</h1>
                <div className="input_box">
                    <input
                        type="text"
                        placeholder="Username"
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                        required
                    />
                </div>
                <div className="input_box">
                    <input
                        type="email"
                        placeholder="Email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        required
                    />
                </div>
                {errorMessage && <div className="error">{errorMessage}</div>}
                {successMessage && <div className="success">{successMessage}</div>}

                <button className="register" type="submit">Send</button>

                <div className="login-link">
                    <p>Already have an account? <a href="/login">Sign in</a></p>
                    <p><a href="/register">Create account</a></p>
                </div>
            </form>
        </div>
    );
};

export default Forgotpass;
