import { Routes, Route } from "react-router-dom";
import Navbar from "./Components/Navbar";  // Import Navbar vào đây
import Footer from "./Components/Footer";

import HomePage from "./pages/HomePage.js";
import StorePage from "./pages/StorePage.js";
import GamePage from "./pages/GamePage.js";
import LibraryPage from "./pages/LibraryPage.js";
import Cart from "./pages/Cart.js";

const UserRoutes = () => {
  return (
    <>
      {/* Navbar sẽ chỉ hiển thị ở đây, trong UserRoutes */}
      <Navbar />
      
      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route path="/store" element={<StorePage />} />
        <Route path="/game/:id" element={<GamePage />} />
        <Route path="/library" element={<LibraryPage />} />
        <Route path="/cart" element={<Cart />} />
      </Routes>
      
      {/* Footer luôn hiển thị trong UserRoutes nếu cần */}
      <Footer />
    </>
  );
};

export default UserRoutes;
