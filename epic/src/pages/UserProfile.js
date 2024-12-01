import React, { useState } from 'react';
import DetailAccountProfile from './DetailAccountProfile'; // Component cho chi tiết tài khoản
import GetCartList from './GetCartList'; // Component cho danh sách giỏ hàng
import '../styles/pages/UserProfile.css';

const UserProfile = () => {
    const [activeSection, setActiveSection] = useState('detail');

    // Dữ liệu mẫu cho giỏ hàng (thay bằng dữ liệu thật từ backend)
    const sampleCarts = [
        {
            id: 'CART001',
            total: 120.5,
            status: 'Completed',
            createdAt: '2024-12-01',
            games: [
                { name: 'Game A', price: 50, discount: 10 },
                { name: 'Game B', price: 70, discount: 0 },
            ],
        },
        {
            id: 'CART002',
            total: 80.0,
            status: 'Pending',
            createdAt: '2024-12-02',
            games: [
                { name: 'Game C', price: 80, discount: 0 },
            ],
        },
    ];

    // Render nội dung bên phải dựa trên phần section
    const renderSection = () => {
        switch (activeSection) {
            case 'detail':
                return (
                    <DetailAccountProfile
                        user={{
                            username: 'john_doe',
                            password: '12345678',
                            email: 'john.doe@example.com',
                        }}
                    />
                );
            case 'cart':
                return <GetCartList carts={sampleCarts} />;
            case 'update':
                return <div>Form to Update Profile</div>;
            case 'history':
                return <div>List of Purchase History</div>;
            case 'owned':
                return <div>List of Owned Games</div>;
            case 'activeCart':
                return <div>Details of the Active Cart</div>;
            case 'cartDetail':
                return <div>Details of Cart Items by Cart ID</div>;
            default:
                return <div>Select a section</div>;
        }
    };

    return (
        <div className="user-profile">
            <div className="sidebar">
                <button
                    className={activeSection === 'detail' ? 'active' : ''}
                    onClick={() => setActiveSection('detail')}
                >
                    Detail Account Profile
                </button>
                <button
                    className={activeSection === 'cart' ? 'active' : ''}
                    onClick={() => setActiveSection('cart')}
                >
                    Get Cart List
                </button>
                <button
                    className={activeSection === 'update' ? 'active' : ''}
                    onClick={() => setActiveSection('update')}
                >
                    Update Profile
                </button>
                <button
                    className={activeSection === 'history' ? 'active' : ''}
                    onClick={() => setActiveSection('history')}
                >
                    Get Purchase History
                </button>
                <button
                    className={activeSection === 'owned' ? 'active' : ''}
                    onClick={() => setActiveSection('owned')}
                >
                    Get Owned Game
                </button>
                <button
                    className={activeSection === 'activeCart' ? 'active' : ''}
                    onClick={() => setActiveSection('activeCart')}
                >
                    Get Active Cart
                </button>
                <button
                    className={activeSection === 'cartDetail' ? 'active' : ''}
                    onClick={() => setActiveSection('cartDetail')}
                >
                    Get CartDetail in CartId
                </button>
            </div>
            <div className="content">{renderSection()}</div>
        </div>
    );
};

export default UserProfile;
