import React,{useState} from "react";

import "./regester.css"
const RegisterForm = () => {
    const [errorMessage, setErrorMessage] = useState(""); // Trạng thái lỗi
    return (
        <div className="wrapper">
            <form className="form-register" action="">
            <div className="logo"><img src="/Images/EpicGames_Logo.png" alt="" /></div>
                <h1>REGISTER</h1>
                <div className="input_box">
                    <input type="email" placeholder="Email" required />
                </div>
                <div className="input_box">
                    <input type="text" placeholder="Username" required />
                </div>
                <div className="input_box">
                    <input type="password" placeholder="Password" required />
                </div>
               
                
                {errorMessage && <div className="error">{errorMessage}</div>}
                <button className="register" type="submit">Register</button>
                <div className="login-link">
                <p>Already have an account ? <a href="/login">Sign in</a></p>
                </div>
            </form>
        </div>
    );
};

export default RegisterForm;
