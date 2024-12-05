import React, { useState, useEffect } from 'react';
import '../styles/pages/LibraryPage.css';
import { FaSearch } from 'react-icons/fa';

const LibraryPage = () => {
    const [searchTerm, setSearchTerm] = useState('');
    const [currentPage, setCurrentPage] = useState(1);
    const [ownedGames, setOwnedGames] = useState([]); // State lưu game đã sở hữu
    const [detailedOwnedGames, setDetailedOwnedGames] = useState([]); // State lưu thông tin chi tiết game
    const [loading, setLoading] = useState(true);

    const accountToken = localStorage.getItem('authToken'); // Token từ localStorage
    const gamesPerPage = 15; // Số game hiển thị mỗi trang

    // Fetch game đã sở hữu từ API
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
                // Fetch chi tiết từng game sau khi có danh sách game
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
                setDetailedOwnedGames(gamesWithDetails); // Lưu lại game với chi tiết
                setLoading(false); // Đánh dấu dữ liệu đã tải xong
            } else {
                console.error('Error fetching owned games:', data.message);
                setLoading(false);
            }
        } catch (error) {
            console.error('Error fetching owned games:', error);
            setLoading(false);
        }
    };

    useEffect(() => {
        if (accountToken) {
            fetchOwnedGames(); // Gọi API khi có token
        } else {
            setLoading(false); // Nếu không có token, không làm gì
        }
    }, [accountToken]);

    // Lọc game theo từ khóa tìm kiếm
    const filteredGames = detailedOwnedGames.filter(game =>
        game.title.toLowerCase().includes(searchTerm.toLowerCase())
    );

    // Tính toán phân trang
    const totalPages = Math.ceil(filteredGames.length / gamesPerPage);
    const startIndex = (currentPage - 1) * gamesPerPage;
    const paginatedGames = filteredGames.slice(startIndex, startIndex + gamesPerPage);

    const handlePageChange = (pageNumber) => {
        setCurrentPage(pageNumber);
    };

    return (
        <div className="library-page">
            <h2>Your Library</h2>
            <div className="search-container" style={{ margin: '0 auto 40px' }}>
                <input
                    type="text"
                    placeholder="Search for a game..."
                    value={searchTerm}
                    onChange={(e) => {
                        setSearchTerm(e.target.value);
                        setCurrentPage(1); // Reset trang khi tìm kiếm
                    }}
                />
                <FaSearch className="search-icon" />
            </div>

            {loading ? (
                <div>Loading...</div> // Hiển thị khi dữ liệu đang tải
            ) : (
                <div className="games-list">
                    {paginatedGames.length > 0 ? (
                        paginatedGames.map((game) => (
                            <div className="game-item" key={game.accountGameId}>
                                <img src={game.coverImage} alt={game.title} className="game-cover" />
                                <h3>{game.title}</h3>
                                <p><strong>Genre:</strong> {game.genre}</p>
                            </div>
                        ))
                    ) : (
                        <p>No games found. Try a different search!</p>
                    )}
                </div>
            )}

            {/* Hiển thị các nút chuyển trang */}
            <div className="pagination">
                {Array.from({ length: totalPages }, (_, index) => (
                    <button
                        key={index + 1}
                        onClick={() => handlePageChange(index + 1)}
                        className={currentPage === index + 1 ? 'active' : ''}
                    >
                        {index + 1}
                    </button>
                ))}
            </div>
        </div>
    );
};

export default LibraryPage;
