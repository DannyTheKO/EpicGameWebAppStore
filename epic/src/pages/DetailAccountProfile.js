import React, { useState } from "react";
import "../styles/pages/DetailAccountProfile.css";

const DetailAccountProfile = ({ user }) => {
    const [showPassword, setShowPassword] = useState(false);

    const togglePasswordVisibility = () => {
        setShowPassword(!showPassword);
    };

    return (
        <div className="detail-account-profile">
            <h2>Account Details</h2>
            <div className="profile-item">
                <label>Username:</label>
                <span>{user.username}</span>
            </div>
            <div className="profile-item">
                <label>Password:</label>
                <div className="password-container">
                    <input
                        type={showPassword ? "text" : "password"}
                        value={user.password}
                        readOnly
                    />
                    <button onClick={togglePasswordVisibility}>
                        {showPassword ? "Hide" : "Show"}
                    </button>
                </div>
            </div>
            <div className="profile-item">
                <label>Email:</label>
                <span>{user.email}</span>
            </div>
        </div>
    );
};

export default DetailAccountProfile;
