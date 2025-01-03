﻿import React, { useState, useEffect } from 'react';
import DetailAccountProfile from './DetailAccountProfile'; // Component cho chi tiết tài khoản
import GetCartList from './GetCartList'; // Component cho danh sách giỏ hàng
import '../styles/pages/UserProfile.css';

const UserProfile = () => {
    const [activeSection, setActiveSection] = useState('detail');
    const [user, setUser] = useState(null); // State lưu thông tin người dùng
    const [loading, setLoading] = useState(true); // State để theo dõi trạng thái tải
    const [carts, setCarts] = useState([]); // State để lưu danh sách giỏ hàng
    const [updatedUser, setUpdatedUser] = useState(null); // Lưu thông tin người dùng khi sửa
    const [showPassword, setShowPassword] = useState(false);
    const accountToken = localStorage.getItem('authToken'); // Lấy token từ localStorage
    const [ownedGames, setOwnedGames] = useState([]); // State lưu game đã sở hữu
    const [detailedOwnedGames, setDetailedOwnedGames] = useState([]);
    const fetchOwnedGames = async () => {
        try {
            const response = await fetch('http://localhost:5084/Profile/ProfileUser/GetOwnedGames', {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${accountToken}`,
                    'Accept': 'application/json',
                },
            });

            if (!response.ok) {
                console.error('Response:', response.status, response.statusText);
                throw new Error('Failed to fetch owned games');
            }

            const data = await response.json();
            console.log('Owned games data:', data);

            if (data.success) {
                const gamesWithDetails = await Promise.all(
                    data.data.map(async (ownedGame) => {
                        const gameResponse = await fetch(`http://localhost:5084/Game/GetGameId/${ownedGame.gameId}`, {
                            method: 'GET',
                            headers: {
                                'Authorization': `Bearer ${accountToken}`,
                                'Accept': 'application/json',
                            },
                        });

                        if (gameResponse.ok) {
                            const gameData = await gameResponse.json();
                            return {
                                ...ownedGame,
                                title: gameData.title,
                                genre: gameData.genre.name,
                                publisher: gameData.publisher.name,
                                coverImage: `${gameData.imageGame.find(img => img.imageType === 'Thumbnail').filePath}${gameData.imageGame.find(img => img.imageType === 'Thumbnail').fileName}`,
                                description: gameData.description,
                            };
                        } else {
                            console.error(`Failed to fetch game details for gameId: ${ownedGame.gameId}`);
                            return ownedGame;
                        }
                    })
                );
                setDetailedOwnedGames(gamesWithDetails);
            } else {
                console.error('Error fetching owned games:', data.message);
            }
        } catch (error) {
            console.error('Error fetching owned games:', error);
        }
    };



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
                    setUpdatedUser(data.data); // Lưu vào updatedUser để sử dụng trong form
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

    const handleUpdateProfile = async (e) => {
        e.preventDefault();
        try {
            const response = await fetch('http://localhost:5084/Profile/ProfileUser/UpdateProfile', {
                method: 'PUT',
                headers: {
                    'Authorization': `Bearer ${accountToken}`,
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(updatedUser),
            });

            const data = await response.json();
            if (data.success) {
                localStorage.setItem('username', updatedUser.username); // Cập nhật localStorage
                alert('Profile updated successfully');
                window.location.reload();
            } else {
                alert('Error updating profile');
            }
        } catch (error) {
            console.error('Error updating profile:', error);
            alert(`Error: ${error.message}`);
        }
    };

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
                return (
                    <div className="update-profile-form">
                        <h2>Update Profile</h2>
                        <form onSubmit={handleUpdateProfile}>
                            <label htmlFor="username">Username:</label>
                            <input
                                type="text"
                                id="username"
                                value={updatedUser?.username || ''}
                                onChange={(e) => setUpdatedUser({ ...updatedUser, username: e.target.value })}
                            />

                            <label htmlFor="password">Password:</label>
                            <div className="password-input-container">
                                <input
                                    type={showPassword ? 'text' : 'password'} // Loại input thay đổi dựa vào state
                                    id="password"
                                    value={updatedUser?.password || ''}
                                    onChange={(e) => setUpdatedUser({ ...updatedUser, password: e.target.value })}
                                />
                                <button
                                    type="button" // Nút không submit form
                                    onClick={() => setShowPassword(!showPassword)} // Đổi trạng thái showPassword
                                    className="toggle-password-btn"
                                >
                                    {showPassword ? 'Hide' : 'Show'} {/* Nội dung nút thay đổi */}
                                </button>
                            </div>

                            <label htmlFor="email">Email:</label>
                            <input
                                type="email"
                                id="email"
                                value={updatedUser?.email || ''}
                                onChange={(e) => setUpdatedUser({ ...updatedUser, email: e.target.value })}
                            />

                            <button type="submit">Update</button>
                        </form>
                    </div>
                );
            case 'ownedGames':
                return (
                    <div className="owned-games-section">
                        <h2>Owned Games</h2>
                        {detailedOwnedGames.length > 0 ? (
                            <ul className="owned-games-list">
                                {detailedOwnedGames.map((game) => (
                                    <li key={game.accountGameId} className="owned-game-item">
                                        <img src={game.coverImage} alt={game.title} className="game-cover" />
                                        <div className="game-info">
                                            <h3>{game.title}</h3>
                                            <p><strong>Genre:</strong> {game.genre}</p>
                                            <p><strong>Publisher:</strong> {game.publisher}</p>
                                            <p><strong>Date Added:</strong> {new Date(game.dateAdded).toLocaleString()}</p>
                                            <p><strong>Description:</strong> {game.description}</p>
                                        </div>
                                    </li>
                                ))}
                            </ul>
                        ) : (
                            <p>You don't own any games yet.</p>
                        )}
                    </div>
                );

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
                    className={activeSection === 'ownedGames' ? 'active' : ''}
                    onClick={() => {
                        setActiveSection('ownedGames');
                        fetchOwnedGames(); // Gọi API khi chuyển sang phần "Game đã sở hữu"
                    }}
                >
                    View Owned Games
                </button>

            </div>
            <div className="content">{renderSection()}</div>
        </div>
    );
};

export default UserProfile;
