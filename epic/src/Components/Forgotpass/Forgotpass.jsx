import React,{useState} from "react";
import "./forgot.css"
const RegisterForm = () => {
    const [errorMessage, setErrorMessage] = useState(""); // Trạng thái lỗi
    return (
        <div className="wrapper-forgot">
            <form className="form-register" action="">
            <div className="logo"><img src="/Images/EpicGames_Logo.png" alt="" /></div>
                <h1>FORGOT PASSWORD</h1>
                <div className="input_box">
                    <input type="email" placeholder="Email" required />
                    
                </div>
                {errorMessage && <div className="error">{errorMessage}</div>}
                <button className="register" type="submit">Send</button>
                <div className="login-link">
                <p>Already have an account ? <a href="/login">Sign in</a></p>
                <p><a href="/register">Creart account</a></p>
                </div>
            </form>
        </div>
    );
};

export default RegisterForm;
