import Account from "./Account";
import Dash from "./Dashboard";
import Game from "./Game";
import AccountGame from "./AccountGame";
import Discount from "./Discount.js";
import Publisher from "./Publisher.jsx";
import Cart from "./Cart.jsx";
import {jwtDecode} from 'jwt-decode';
import { Routes, Route ,useNavigate  } from "react-router-dom";
import { useEffect } from "react";
import "react-toastify/dist/ReactToastify.css";
import { toast, ToastContainer } from "react-toastify";

function PageContent() {
  const role = localStorage.getItem("authToken");
  const navigate = useNavigate();
  useEffect(() => {
    if (role) {
      try {
        const decodedToken = jwtDecode(role);
        const userRole = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
        
        if (userRole !== "Admin") {
          toast.warn("Bạn không có quyền truy cập!", { position: "top-center" });
          setTimeout(() => navigate("/"), 0); // Điều hướng sau khi thông báo
        }
      } catch (error) {
        toast.error("Token không hợp lệ, vui lòng đăng nhập lại!", { position: "top-center" });
        navigate("/login");
      }
    } else {
      toast.warn("Bạn chưa đăng nhập, vui lòng đăng nhập!", { position: "top-center" });
      navigate("/login");
    }
  }, [role, navigate]);
  return (
    <div className="PageContent">
       <ToastContainer />
      <Routes>
        <Route path="/" element={<Dash />} />
        <Route path="/admingame" element={<Game />} />
        <Route path="/adminaccountgame" element={<AccountGame />} />
        <Route path="/adminaccount" element={<Account />} />
        <Route path="/admindiscount" element={<Discount />} />
        <Route path="/adminpublisher" element={<Publisher />} />
        <Route path="/admincard" element={<Cart />} />
      </Routes>
    </div>
  );
}
export default PageContent;
