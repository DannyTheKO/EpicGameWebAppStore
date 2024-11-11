import { Button, Rate, Space, Table, Typography, Modal, Input } from "antd";
import { useEffect, useState } from "react";
import { GetAllgame, AddGame, DeleteGame } from "./API";  // Import các API
import "./table.css";

const { Text } = Typography;

function Game() {
  const [loading, setLoading] = useState(false);
  const [dataSource, setDataSource] = useState([]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isEditing, setIsEditing] = useState(false);
  const [gameRecord, setGameRecord] = useState({
    gameId: "", // Sử dụng gameId làm ID duy nhất
    title: "",
    author: "",
    price: 0,
    rating: 0,
    release: null,
    description: "",
    genreId: "",
    publisherId: "",
  });

  useEffect(() => {
    const fetchGame = async () => {
      setLoading(true);
      try {
        const res = await GetAllgame();
        setDataSource(res || []);
      } catch (error) {
        console.error("Lỗi khi tải dữ liệu", error);
      }
      setLoading(false);
    };
    fetchGame();
  }, []);

  // Open modal to add or edit a game
  const openModal = (record = null) => {
    if (record) {
      setGameRecord({
        gameId: record.gameId,
        title: record.title,
        author: record.author,
        price: record.price,
        rating: record.rating,
        release: record.release.split("T")[0], // Format date to YYYY-MM-DD
        description: record.description,
        genreId: record.genre.genreId,
        publisherId: record.publisher.publisherId,
      });
      setIsEditing(true);
    } else {
      setGameRecord({
        gameId: "",
        title: "",
        author: "",
        price: 0,
        rating: 0,
        release: null,
        description: "",
        genreId: "",
        publisherId: "",
      });
      setIsEditing(false);
    }
    setIsModalOpen(true);
  };

  // Handle deleting a game
  const handleDelete = (record) => {
    Modal.confirm({
      title: "Are you sure you want to delete this game?",
      content: "This action cannot be undone.",
      okText: "Delete",
      okType: "danger",
      cancelText: "Cancel",
      onOk: () => {
        const gameId = record.gameId;
        DeleteGame(gameId)
          .then(() => {
            setDataSource((prevDataSource) =>
              prevDataSource.filter((item) => item.gameId !== gameId)
            );
          })
          .catch((error) => {
            console.error("Error deleting game:", error);
          });
      },
    });
  };

  // Validate game data before saving
  const validateGameRecord = () => {
    const { title, author, price, rating, release, description } = gameRecord;
    if (!title || !author || price <= 0 || rating < 0 || rating > 10 || !release || !description) {
      Modal.error({
        title: 'Error',
        content: 'Please fill in all required fields with valid information.',
      });
      return false;
    }
    return true;
  };

  // Handle saving game data
  const handleSave = async () => {
    if (!validateGameRecord()) {
      return;
    }
    if (isEditing) {
      await AddGame(gameRecord); 
    } else {
      await AddGame(gameRecord);
    }
    setIsModalOpen(false);
  };

  const columns = [
    {
      title: "Game ID",
      dataIndex: "gameId",
      key: "gameId",
      render: (gameId) => <Text>{gameId}</Text>,
    },
    {
      title: "Title",
      dataIndex: "title",
      key: "title",
      render: (title) => <Text>{title}</Text>,
    },
    {
      title: "Publisher",
      dataIndex: "publisher",
      key: "publisher",
      render: (publisher) => <Text>{publisher?.name||"N/A"}</Text>,
    },
    {
      title: "Genre",
      dataIndex: "genre",
      key: "genre",
      render: (genre) => <Text>{genre?.name|| "N/A"}</Text>,
    },
    {
      title: "Price",
      dataIndex: "price",
      key: "price",
      render: (price) => `$${price.toFixed(2)}`,
    },
    {
      title: "Rating",
      dataIndex: "rating",
      key: "rating",
      render: (rating) => <Rate value={rating / 2} allowHalf disabled />,
    },
    {
      title: "Release Date",
      dataIndex: "release",
      key: "release",
      render: (release) => new Date(release).toLocaleDateString(),
    },
    {
      title: "Description",
      dataIndex: "description",
      key: "description",
      render: (description) => <Text ellipsis style={{ maxWidth: 200 }}>{description}</Text>,
    },
    {
      title: "Actions",
      key: "actions",
      render: (record) => (
        <Space size="middle">
          <Button onClick={() => openModal(record)} type="primary">Add</Button>
          <Button onClick={() => openModal(record)} type="primary">Edit</Button>
          <Button danger onClick={() => handleDelete(record)}>Delete</Button>
        </Space>
      ),
    },
  ];

  return (
    <Space className="size_table" size={10} direction="vertical">
      <Table
        className="data"
        loading={loading}
        columns={columns}
        dataSource={dataSource}
        rowKey="gameId"
        pagination={{ pageSize: 10 }}
        scroll={{ x: 'max-content' }}
      />

<Modal
  title={isEditing ? "Edit Game Info" : "Add New Game"}
  visible={isModalOpen}
  onCancel={() => setIsModalOpen(false)}
  onOk={handleSave}
>
  <Input
    className="modal-input"
    placeholder="Game ID"
    value={gameRecord.gameId}
    disabled
  />
  <Input
    className="modal-input"
    placeholder="Title"
    value={gameRecord.title}
    onChange={(e) => setGameRecord({ ...gameRecord, title: e.target.value })}
  />
  <Input
    className="modal-input"
    placeholder="Author"
    value={gameRecord.author}
    onChange={(e) => setGameRecord({ ...gameRecord, author: e.target.value })}
  />
  <Input
    className="modal-input"
    placeholder="Price"
    type="number"
    value={gameRecord.price}
    onChange={(e) => setGameRecord({ ...gameRecord, price: parseFloat(e.target.value) })}
  />
  <Input
    className="modal-input"
    placeholder="Rating"
    value={gameRecord.rating}
    onChange={(e) => setGameRecord({ ...gameRecord, rating: parseInt(e.target.value) })}
  />
  <Input
    className="modal-input"
    type="date"
    placeholder="Release Date"
    value={gameRecord.release}
    onChange={(e) => setGameRecord({ ...gameRecord, release: e.target.value })}
  />
  <Input
    className="modal-input"
    placeholder="Description"
    value={gameRecord.description}
    onChange={(e) => setGameRecord({ ...gameRecord, description: e.target.value })}
  />
</Modal>

    </Space>
  );
}

export default Game;
