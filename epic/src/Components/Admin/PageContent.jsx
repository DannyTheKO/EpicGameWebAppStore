import Account from "./Account";
import Dash from "./Dashboard";
import Game from "./Game";
import AccountGame from "./AccountGame";
import Discount from "./Discount.js";
import Publisher from "./Publisher.jsx";
import Cart from "./Cart.jsx";
import { Routes, Route } from "react-router-dom";
function PageContent() {
  return (
    <div className="PageContent">
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
