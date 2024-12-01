
import { BrowserRouter, Routes, Route ,Navigate } from "react-router-dom";

// Components
import Navbar from "./Components/Navbar";
import Footer from "./Components/Footer";
import Login from "./Components/Login/Login.jsx";
import Register from "./Components/Register/Register.jsx";
import Forgotpass from "./Components/Forgotpass/Forgotpass.jsx";
import Admin from "./Components/Admin/Admin.jsx";
import Game from "./Components/Admin/Game.jsx";
import AccountGame from "./Components/Admin/AccountGame.jsx";
import Account from "./Components/Admin/Account.jsx";
import Discount from "./Components/Admin/Discount.js";
import Publisher from "./Components/Admin/Publisher.jsx";
import Cart from "./Components/Admin/Cart.jsx";
import {jwtDecode} from 'jwt-decode';

// User Routes
import UserRoutes from "./User.js";  // Import UserRoutes

function App() {
  const isAdmin = () => {
    const role = localStorage.getItem('authToken'); // Assuming role is stored in localStorage
    const decodedToken = jwtDecode(role);
    const userRole = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    return userRole === "Admin";
  
  };
  
return (
    <BrowserRouter>
      {/* Navbar luôn hiển thị */}
            
      <Routes>
        {/* Routes cho người dùng */}
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/forgot_pass" element={<Forgotpass />} />
        
        {/* Routes dành cho người dùng */}
        <Route path="/*" element={<UserRoutes />} />  {/* Bao bọc UserRoutes trong Route */}

        {/* Route cha Admin với Outlet cho các route con */}
        <Route path="/admin/*"element={
        isAdmin ? (
      <Admin /> // Nếu là admin, render Admin page
    ) : (
      <Navigate to="/login" /> // Nếu không phải admin, chuyển hướng đến login
    )
  }>
          <Route path="admingame" element={<Game />} />
          <Route path="adminaccountgame" element={<AccountGame />} />
          <Route path="adminaccount" element={<Account />} />
          <Route path="admindiscount" element={<Discount />} />
          <Route path="adminpublisher" element={<Publisher />} />
          <Route path="admincard" element={<Cart />} />
        </Route>
      </Routes>
    </BrowserRouter>  
  );
}

export default App;
