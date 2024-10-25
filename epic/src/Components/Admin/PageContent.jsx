import AppRoutes from "./Dashboard";
import Customers from "./Customers";
import Dash from "./Dashboard";
import Admin from './Admin';
import Inventory from "./Inventory";
import Orders from "./Orders";
import {  Routes, Route } from 'react-router-dom';
function PageContent() {
  return (
    <div className="PageContent">
      {/* Tại đây bạn có thể render các route con */}
      <Routes>
      <Route path="/" element={<Dash />} />
        <Route path="/inventory" element={<Inventory />} />
        <Route path="/orders" element={<Orders />} />
        <Route path="/customers" element={<Customers />} />
      </Routes>
    </div>
  );
}
export default PageContent;
