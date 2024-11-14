// src/App.js
import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Navbar from './components/Navbar';
import HomePage from './pages/HomePage';
import StorePage from './pages/StorePage';
import GamePage from './pages/GamePage';
import LibraryPage from './pages/LibraryPage';
import Cart from './pages/Cart';
import Footer from './components/Footer';
import Login from './Login/Login.jsx';
import Register from './Register/Register.jsx';
import Forgotpass from './Forgotpass/Forgotpass.jsx';


const App = () => {
    return (
        <Router>
            <Navbar />
            <main>
                <Routes>
                    <Route path="/" element={<HomePage />} />
                    <Route path="/store" element={<StorePage />} />
                    <Route path="/game/:id" element={<GamePage />} />
                    <Route path="/library" element={<LibraryPage />} />
                    <Route path="/cart" element={<Cart />} />
                    <Route path="/login" element={<Login />} />
                    <Route path="/register" element={<Register />} />
                    <Route path="/forgot_pass" element={<Forgotpass />} />
                </Routes>
            </main>
            <Footer />
        </Router>
    );
};

export default App;
