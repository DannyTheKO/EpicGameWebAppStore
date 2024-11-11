import React,{useState} from "react";

import "./regester.css"
const RegisterForm = () => {
    const [email, setEmail] = useState("");
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [errorMessage, setErrorMessage] = useState("");
    const [successMessage, setSuccessMessage] = useState("");
    const handleRegister = async (e) => {
        
    }
    return (
        <div className="wrapper">
        <form className="form-register" onSubmit={handleRegister}>
            <div className="logo"><img src="/Images/EpicGames_Logo.png" alt="" /></div>
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
                    type="password"
                    placeholder="Password"
                    required
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                />
            </div>

            {errorMessage && <div className="error">{errorMessage}</div>}
            {successMessage && <div className="success">{successMessage}</div>}
            <button className="register" type="submit">Register</button>
            <div className="login-link">
                <p>Already have an account? <a href="/login">Sign in</a></p>
            </div>
        </form>
    </div>
    );
};

export default RegisterForm;
