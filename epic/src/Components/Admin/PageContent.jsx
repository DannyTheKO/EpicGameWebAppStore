
import Account from "./Account";
import Dash from "./Dashboard";
import Game from "./Game";
import AccountGame from "./AccountGame";
import Discount from "./Discount.jsx";
import Publisher from "./Publisher.jsx";
import {  Routes, Route } from 'react-router-dom';
function PageContent() {
  return (
    <div className="PageContent">
      {/* Tại đây bạn có thể render các route con */}
      <Routes>
      <Route path="/" element={<Dash />} />
        <Route path="/game" element={<Game />} />
        <Route path="/accountgame" element={<AccountGame />} />
        <Route path="/account" element={<Account />} />
        <Route path="/discount" element={<Discount />}/>
      <Route path="/publisher" element={<Publisher />}/>
      </Routes>
    </div>
  );
}
export default PageContent;
