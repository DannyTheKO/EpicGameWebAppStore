// import { BrowserRouter ,Router, Routes, Route } from "react-router-dom";


// // Components
// import Navbar from "./Components/Navbar";
// import Footer from "./Components/Footer";
// import Login from "./Components/Login/Login.jsx";
// import Register from "./Components/Register/Register.jsx";
// import Forgotpass from "./Components/Forgotpass/Forgotpass.jsx";
// import Admin from "./Components/Admin/Admin.jsx";
// import Game from "./Components/Admin/Game.jsx";
// import AccountGame from "./Components/Admin/AccountGame.jsx";
// import Account from "./Components/Admin/Account.jsx";
// import Discount from "./Components/Admin/Discount.js";
// import Publisher from "./Components/Admin/Publisher.jsx";
// import PageAdmin from "./Components/Admin/PageContent.jsx"
// import Header from "./Components/Admin/Header.jsx"

// // Pages
// import HomePage from "./pages/HomePage.js";
// import StorePage from "./pages/StorePage.js";
// import GamePage from "./pages/GamePage.js";
// import LibraryPage from "./pages/LibraryPage.js";
// import Cart from "./pages/Cart.js";
// import Dashboard from "./Components/Admin/Dashboard.jsx";
// import UserRoutes from "./User.js";
// import AdminRoutes from "./Admin.js";
// function App() {
//   return (
//     <BrowserRouter>
//     <Routes>
//       <Route path="/login" element={<Login />} />
//       <Route path="/register" element={<Register />} />
//       <Route path="/forgot_pass" element={<Forgotpass />} />
//       <Route path="/*" element={<UserRoutes />} />
//       {/* Route cha Admin với Outlet cho các route con */}
//       <Route path="/" element={<Admin />}>
//         <Route path="admingame" element={<Game />} />
//         <Route path="adminaccountgame" element={<AccountGame />} />
//         <Route path="adminaccount" element={<Account />} />
//         <Route path="admindiscount" element={<Discount />} />
//         <Route path="adminpublisher" element={<Publisher />} />
//       </Route>
//     </Routes>
//   </BrowserRouter>
  



//   );
// }

// export default App;
// App.jsx
import { BrowserRouter, Routes, Route } from "react-router-dom";

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
import Header from "./Components/Admin/Header.jsx";

// User Routes
import UserRoutes from "./User.js";  // Import UserRoutes

function App() {
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
        <Route path="/admin/*" element={<Admin />}>
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
