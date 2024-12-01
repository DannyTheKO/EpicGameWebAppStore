import React, { useState, useEffect } from 'react';
import DetailAccountProfile from './DetailAccountProfile'; // Component cho chi tiết tài khoản
import GetCartList from './GetCartList'; // Component cho danh sách giỏ hàng
import '../styles/pages/UserProfile.css';

const UserProfile = () => {
    const [activeSection, setActiveSection] = useState('detail');
    const [user, setUser] = useState(null); // State lưu thông tin người dùng
    const [loading, setLoading] = useState(true); // State để theo dõi trạng thái tải
    const [carts, setCarts] = useState([]); // State để lưu danh sách giỏ hàng

    // Lấy token từ localStorage (giả sử bạn đã lưu token sau khi đăng nhập)
    const accountToken = localStorage.getItem('authToken'); // Hoặc sử dụng cookie hoặc context

    // Gọi API để lấy thông tin người dùng
    useEffect(() => {
        const fetchUserProfile = async () => {
            try {
                const response = await fetch('http://localhost:5084/Profile/ProfileUser/GetProfile', {
                    method: 'GET',
                    headers: {
                        'Authorization': `Bearer ${accountToken}`,
                        'Accept': 'application/json',
                    },
                });

                if (!response.ok) {
                    throw new Error('Failed to fetch user profile');
                }

                const data = await response.json();
                console.log('User profile data:', data); // Kiểm tra phản hồi từ API

                if (data.success) {
                    setUser(data.data); // Lưu thông tin người dùng vào state
                } else {
                    console.error('Error fetching user profile');
                }
            } catch (error) {
                console.error('Error fetching user profile:', error);
            } finally {
                setLoading(false); // Cập nhật trạng thái tải
            }
        };

        if (accountToken) {
            fetchUserProfile();
        } else {
            setLoading(false); // Nếu không có token, không làm gì
        }
    }, [accountToken]);

    // Fetch danh sách giỏ hàng khi nhấn nút "Get Cart List"
    const fetchCartList = async () => {
        try {
            const response = await fetch('http://localhost:5084/Profile/ProfileUser/GetCartList', {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${accountToken}`,
                    'Accept': 'application/json',
                },
            });
            const data = await response.json();
            console.log('Cart list data:', data); // Kiểm tra phản hồi từ API

            if (data.success) {
                setCarts(data.data.cart); // Lưu danh sách giỏ hàng vào state
            } else {
                console.error('Failed to fetch cart list');
            }
        } catch (error) {
            console.error('Error fetching cart list:', error);
        }
    };

    // Render nội dung bên phải dựa trên phần section
    const renderSection = () => {
        console.log('User:', user); // Kiểm tra dữ liệu user

        switch (activeSection) {
            case 'detail':
                return loading ? (
                    <div>Loading...</div> // Hiển thị khi đang tải thông tin
                ) : (
                    <DetailAccountProfile user={user} />
                );
            case 'cart':
                return <GetCartList carts={carts} />; // Hiển thị danh sách giỏ hàng
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
                    onClick={() => {
                        setActiveSection('cart');
                        fetchCartList(); // Fetch giỏ hàng khi nhấn nút
                    }}
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
