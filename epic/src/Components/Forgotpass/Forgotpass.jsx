import React,{useState} from "react";
import "./forgot.css"
const Forgotpass = () => {
    const [email, setEmail] = useState("");
    const [errorMessage, setErrorMessage] = useState(""); 
    const [successMessage, setSuccessMessage] = useState("");
    const handleForgotPassword = (e) => {
        e.preventDefault();
        if (!email) {
            setErrorMessage("Vui lòng nhập email của bạn.");
            setSuccessMessage("");
        } else {
            setErrorMessage("");
            setSuccessMessage("Email đặt lại mật khẩu đã được gửi, vui lòng kiểm tra hộp thư của bạn.");
            setEmail("");
        }
    };

    return (
        <div className="wrapper-forgot">
            <form className="form-register" onSubmit={handleForgotPassword}>
            <div className="logo"><img src="/Images/EpicGames_Logo.png" alt="" /></div>
                <h1>FORGOT PASSWORD</h1>
                <div className="input_box">
                    <input
                     type="email"
                      placeholder="Email" 
                      onChange={(e) => setEmail(e.target.value)}
                      required />
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

export default Forgotpass;
