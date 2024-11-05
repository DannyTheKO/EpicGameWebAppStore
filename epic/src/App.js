import { BrowserRouter, Routes, Route } from 'react-router-dom';
import './App.css';
import Login from "./Components/Login/Login.jsx"
import Register from './Components/Register/Register.jsx';
import Forgotpass from './Components/Forgotpass/Forgotpass.jsx'
import Admin from './Components/Admin/Admin.jsx';
import Game from './Components/Admin/Game.jsx';
import AccountGame from './Components/Admin/AccountGame.jsx';
import Acount from './Components/Admin/Account.jsx';
import Discount from "./Components/Admin/Discount.jsx";
import Publisher from "./Components/Admin/Publisher.jsx";

function App() {
  return (
    <BrowserRouter> {/* Bọc Routes trong BrowserRouter */}
    <Routes>
   
      <Route path="/login" element={<Login />} />
      <Route path="/register" element={<Register />} />
      <Route path="/forgot_pass" element={<Forgotpass />} />
      <Route path="/" element={<Admin />}>
      <Route path="/game" element={<Game />}/>
      <Route path="/accountgame" element={<AccountGame />}/>
      <Route path="/account" element={<Acount />}/>
      <Route path="/discount" element={<Discount />}/>
      <Route path="/publisher" element={<Publisher />}/>
    </Route>
    </Routes>
  </BrowserRouter>
  );
}

export default App;
