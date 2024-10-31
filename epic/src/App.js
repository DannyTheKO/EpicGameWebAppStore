import { BrowserRouter, Routes, Route } from 'react-router-dom';
import './App.css';
import Login from "./Components/Login/Login.jsx"
import Register from './Components/Register/Register.jsx';
import Forgotpass from './Components/Forgotpass/Forgotpass.jsx'
import Admin from './Components/Admin/Admin.jsx';
import Game from './Components/Admin/Game.jsx';
import Orders from './Components/Admin/Orders.jsx';
import Customers from './Components/Admin/Customers.jsx';

function App() {
  return (
    <BrowserRouter> {/* Bọc Routes trong BrowserRouter */}
    <Routes>
   
      <Route path="/login" element={<Login />} />
      <Route path="/register" element={<Register />} />
      <Route path="/forgot_pass" element={<Forgotpass />} />
      <Route path="/" element={<Admin />}>
      <Route path="/game" element={<Game />}/>
      <Route path="/orders" element={<Orders />}/>
      <Route path="/customers" element={<Customers />}/>
    </Route>
    </Routes>
  </BrowserRouter>
  );
}

export default App;
