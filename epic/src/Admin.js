// AdminRoutes.js
import { Routes, Route } from "react-router-dom";
import Admin from "./Components/Admin/Admin";
import Dashboard from "./Components/Admin/Dashboard.jsx";
import Game from "./Components/Admin/Game.jsx";
import AccountGame from "./Components/Admin/AccountGame.jsx";
import Account from "./Components/Admin/Account.jsx";
import Discount from "./Components/Admin/Discount.js";
import Publisher from "./Components/Admin/Publisher.jsx";

const AdminRoutes = () => {
  return (
    <Routes>
      {/* Route cha Admin với Outlet cho các route con */}
      <Route path="/admin" element={<Admin />}>
        <Route path="dashboard" element={<Dashboard />} />
        <Route path="admingame" element={<Game />} />
        <Route path="adminaccountgame" element={<AccountGame />} />
        <Route path="adminaccount" element={<Account />} />
        <Route path="admindiscount" element={<Discount />} />
        <Route path="adminpublisher" element={<Publisher />} />
      </Route>
    </Routes>
  );
};

export default AdminRoutes;
