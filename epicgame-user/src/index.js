import React from 'react';
import ReactDOM from 'react-dom/client'; // Sử dụng `react-dom/client` cho React 18+
import App from './App';
import './styles/main.css'; // Đường dẫn đúng đến main.css
import images from './images'; // Import tất cả ảnh từ assets

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
    <React.StrictMode>
        <App />
    </React.StrictMode>
);
