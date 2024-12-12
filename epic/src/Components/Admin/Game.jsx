import {
  Button,
  Rate,
  Space,
  Table,
  Typography,
  Modal,
  Input,
  Select,
  Upload,
} from "antd";
import { useEffect, useState } from "react";
import { jwtDecode } from "jwt-decode";
import { useNavigate } from 'react-router-dom';
import {
  GetAllgame,
  DeleteGame,
  GetAllPublisher,
  GetAllGenre,
  UpdateGame,
} from "./API";
import "./table.css";
import axios from "axios";
import { Navigate } from "react-router-dom";

const { Text } = Typography;
const { Option } = Select;
function Game() {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [dataSource, setDataSource] = useState([]);
  const [datagenre, setgenre] = useState([]);
  const [datapublisher, setpublisher] = useState([]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isEditing, setIsEditing] = useState(false);
  const [selectedImage, setSelectedImage] = useState(null);
  const [gameRecord, setGameRecord] = useState({
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

  const fetchGame = async () => {
    setLoading(true);
    try {
      const [res, genre, publisher] = await Promise.all([
        GetAllgame(),
        GetAllGenre(),
        GetAllPublisher(),
      ]);
      setDataSource(res || []);
      setgenre(genre || []);
      setpublisher(publisher || []);
    } catch (error) {
      console.error("Lỗi khi tải dữ liệu", error);
    }
    setLoading(false);
  };

  useEffect(() => {
    fetchGame();
  }, []);
 


  // Hàm xử lý khi file thay đổi


 
  // Hàm xử lý cập nhật ảnh


  const isAdmin = () => {
    const role = localStorage.getItem("authToken");
    const decodedToken = jwtDecode(role);
    const userRole =
      decodedToken[
        "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
      ];
    return userRole === "Admin";
  };
  const openModal = async(record = null) => {
    if (record) {
      setGameRecord({
        gameId: record.gameId,
        title: record.title,
        author: record.author,
        price: record.price,
        rating: record.rating,
        release: record.release.split("T")[0],
        description: record.description,
        genreId: record.genre.genreId,
        publisherId: record.publisher.publisherId,
      });

      setIsEditing(true);
    } else {
      const updatedDataSource = await GetAllgame();
      setDataSource(updatedDataSource);
      const maxId =dataSource.length > 0
          ? Math.max(...dataSource.map((item) => item.gameId))
          : 0;
  
      setGameRecord({
        gameId: maxId + 1,
        title: "",
        author: "",
        price: 0,
        rating: 0,
        release: "",
        description: "",
        genreId: "",
        publisherId: "",
      });
      setIsEditing(false);
    }
    setIsModalOpen(true);
  };

  const handleSave = async () => {
    if (!validateGameRecord()) {
      return;
    }
    if (isEditing) {
      try {
        const result = await UpdateGame(gameRecord.gameId, gameRecord);

        if (result.success) {
          const updatedDataSource = await GetAllgame();
          setDataSource(updatedDataSource);
          Modal.success({
            title: "Success",
            content: result.message,
          });
        } else {
          Modal.error({
            title: "Error",
            content: result.message,
          });
        }
      } catch (error) {
        console.error("Error deleting account:", error);
        Modal.error({
          title: "Error",
          content:
            "An error occurred while updatingx the account. Please try again.",
        });
      }
    } else {
      try {
        const token = localStorage.getItem("authToken");
        if (!token) {
          console.error("Token not found!");
          Modal.error({
            title: "Error",
            content: "Authentication token is missing. Please log in again.",
          });
          return;
        }

        if (!selectedImage) {
          console.error("No image selected!");
          Modal.error({
            title: "Error",
            content: "No image selected. Please select an image to upload.",
          });
          return;
        }

        if (!gameRecord || !gameRecord.gameId) {
          console.error("Invalid game record");
          Modal.error({
            title: "Error",
            content:
              "Invalid game data. Please check the game details and try again.",
          });
          return;
        }

        const formData = new FormData();
        formData.append("Price", gameRecord.price);
        formData.append("Author", gameRecord.author);
        formData.append("PublisherId", gameRecord.publisherId);
        formData.append("Title", gameRecord.title);
        formData.append("Description", gameRecord.description);
        formData.append("Rating", gameRecord.rating);
        formData.append("GenreId", gameRecord.genreId);
        formData.append("Release", gameRecord.release);
        formData.append("imageFile", selectedImage);
    
        const response = await axios.post(
          "http://localhost:5084/Store/Dashboard/Game/Create",
          formData,
          {
            headers: {
              "Content-Type": "multipart/form-data",
              Authorization: `Bearer ${token}`,
            },
          }
        );
        if (response.status === 200) {
          const updatedDataSource = await GetAllgame();
          setDataSource(updatedDataSource);

          Modal.success({
            title: "Game added successfully",
            content: response.data.message,
          });
        } else if (response.status === 400) {
          Modal.error({
            title: "Failed to add the game",
            content: response.data.message,
          });
        } else {
          throw new Error("Failed to add the game. Please try again.");
        }
      } catch (error) {
        console.error(
          "Error adding game:",
          error.response ? error.response.data : error.message
        );
        Modal.error({
          title: "Error",
          content: `An error occurred while adding the game. Error details: ${
            error.response ? error.response.data : error.message
          }`,
        });
      }
    }

    setIsModalOpen(false);
    fetchGame();
  };
  const handleFileChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      const validImageTypes = ["image/jpeg", "image/png", "image/gif"];
      if (!validImageTypes.includes(file.type)) {
        console.error("Invalid file type. Please select a valid image.");
        return;
      }
      const maxSize = 5 * 1024 * 1024;
      if (file.size > maxSize) {
        alert("File is too large. Maximum size is 5MB.");
        return;
      }
      const reader = new FileReader();
      reader.onload = (event) => {
        const img = new Image();
        img.src = event.target.result;

        img.onload = () => {
          const width = img.width;
          const height = img.height;
          const ratio16_9 = Math.abs(width / height - 16 / 9) < 0.01;
          const ratio3_4 = Math.abs(width / height - 3 / 4) < 0.01;
          if (ratio16_9 || ratio3_4) {
            console.log(
              `Image resolution is valid: ${width}x${height} (${
                ratio16_9 ? "16:9" : "3:4"
              })`
            );
            setSelectedImage(file);
          } else {
            console.error("Image resolution must be 16:9 or 3:4.");
            alert("Image resolution must be 16:9 or 3:4.");
          }
        };
      };
      reader.readAsDataURL(file);
    }
  };
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
          .then((result) => {
            if (result.success) {
              // Nếu xóa thành công, cập nhật lại dữ liệu nguồn
              setDataSource((prevDataSource) =>
                prevDataSource.filter((item) => item.gameId !== gameId)
              );
              Modal.success({
                title: "Success",
                content: result.message,
              });
            } else {
              // Nếu xóa không thành công
              Modal.error({
                title: "Error",
                content: result.message,
              });
            }
          })
          .catch((error) => {
            // Nếu xảy ra lỗi khi gọi API
            console.error("Error deleting game:", error);
            Modal.error({
              title: "Error",
              content:
                error.message ||
                "Something went wrong while deleting the game.",
            });
          });
      },
    });
  };
  const validateGameRecord = () => {
    const {
      title,
      author,
      price,
      rating,
      release,
      description,
      publisherId,
      genreId,
    } = gameRecord;
    if (!publisherId) {
      Modal.error({
        title: "Error",
        content:
          "Please fill in all required fields with valid information and select a Publisher .",
      });
      return false;
    }
    if (!genreId) {
      Modal.error({
        title: "Error",
        content: "Please fill in all required fields  and Genre.",
      });
      return false;
    }
    if (!title || title.trim() === "") {
      Modal.error({
        title: "Error",
        content: "Title cannot be empty.",
      });
      return false;
    }
    if (!author || author.trim() === "") {
      Modal.error({
        title: "Error",
        content: "Author cannot be empty.",
      });
      return false;
    }
    if (price < 0) {
      Modal.error({
        title: "Error",
        content: "Price must be greater than 0.",
      });
      return false;
    }
    if (rating < 0 || rating > 10) {
      Modal.error({
        title: "Error",
        content: "Rating must be between 0 and 10.",
      });
      return false;
    }
    if (!release) {
      gameRecord.release = new Date().toISOString().split("T")[0];
    }
    if (!description || description.trim() === "") {
      Modal.error({
        title: "Error",
        content: "Description cannot be empty.",
      });
      return false;
    }

    return true;
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
      width: 150,
    },
    {
      title: "Publisher",
      dataIndex: "publisher",
      key: "publisher",
      render: (publisher) => <Text>{publisher?.name || "N/A"}</Text>,
    },
    {
      title: "Genre",
      dataIndex: "genre",
      key: "genre",
      render: (genre) => <Text>{genre?.name || "N/A"}</Text>,
    },
    {
      title: "Price",
      dataIndex: "price",
      key: "price",
      render: (price) => `$${price.toFixed(2)}`,
      width: 100,
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
      title: "Actions",
      key: "actions",
      render: (record) => (
        <Space size="middle">
            <Button onClick={() => navigate(`/edit/${record.gameId}`)} type="primary">
          Edit
        </Button>
          {isAdmin() && (
            <Button danger onClick={() => handleDelete(record)}>
              Delete
            </Button>
          )}
         
        </Space>
      ),
    },
  ];

  return (
    <Space className="size_table" size={10} direction="vertical">
      {isAdmin() && (
        <Button
          onClick={() => openModal()}
          type="primary"
          style={{ marginLeft: "1450px", marginTop: "20px" }}
        >
          Add
        </Button>
      )}
      <Table
        className="data"
        loading={loading}
        columns={columns}
        dataSource={dataSource}
        rowKey="gameId"
        pagination={{ pageSize: 6, position: ["bottomCenter"] }}
        scroll={{ x: "max-content" }}
      />

      <Modal
        title={isEditing ? "Edit Game Info" : "Add New Game"}
        visible={isModalOpen}
        onCancel={() => setIsModalOpen(false)}
        onOk={handleSave}
        okText={isEditing ? "Update" : "Add"}
      >
        <label>ID Game</label>
        <Input
          className="modal-input"
          placeholder="Game ID"
          value={gameRecord.gameId}
          disabled
        />
        <br />
        <label>Genre</label>
        <Select
          placeholder="Select Genre"
          value={gameRecord.genreId}
          onChange={(value) => setGameRecord({ ...gameRecord, genreId: value })}
          style={{ width: "100%", height: "52px" }}
        >
          {datagenre.map((genre) => (
            <Option key={genre.genreId} value={genre.genreId}>
              {genre.name}
            </Option>
          ))}
        </Select>
        <label>Publisher</label>
        <Select
          placeholder="Select Publisher"
          value={gameRecord.publisherId}
          onChange={(value) =>
            setGameRecord({ ...gameRecord, publisherId: value })
          }
          style={{ width: "100%", height: "52px" }}
        >
          {datapublisher.map((publisher) => (
            <Option key={publisher.publisherId} value={publisher.publisherId}>
              {publisher.name} {/* Hiển thị tên publisher */}
            </Option>
          ))}
        </Select>
        <label>Title</label>
        <br />
        <Input
          className="modal-input"
          placeholder="Title"
          value={gameRecord.title}
          onChange={(e) =>
            setGameRecord({ ...gameRecord, title: e.target.value })
          }
          // style={{width:10}}
        />
        <br />
        <label>Author</label>
        <Input
          className="modal-input"
          placeholder="Author"
          value={gameRecord.author}
          onChange={(e) =>
            setGameRecord({ ...gameRecord, author: e.target.value })
          }
        />
        <label>Price</label>
        <Input
          className="modal-input"
          placeholder="Price"
          type="number"
          value={gameRecord.price}
          onChange={(e) =>
            setGameRecord({ ...gameRecord, price: parseFloat(e.target.value) })
          }
        />
        <label>Rating</label>
        <Input
          className="modal-input"
          placeholder="Rating"
          value={gameRecord.rating}
          onChange={(e) => {
            const value = e.target.value;
            if (
              value === "" ||
              (/^\d*\.?\d*$/.test(value) &&
                parseFloat(value) >= 0 &&
                parseFloat(value) <= 10)
            ) {
              setGameRecord({ ...gameRecord, rating: value });
            }
          }}
          onKeyPress={(e) => {
            if (
              !/[0-9.]/.test(e.key) &&
              e.key !== "Backspace" &&
              e.key !== "Delete"
            ) {
              e.preventDefault();
            }
            if (e.key === "." && gameRecord.rating.includes(".")) {
              e.preventDefault();
            }
          }}
        />
        <label>Release Date</label>
        <Input
          className="modal-input"
          type="date"
          placeholder="Release Date"
          value={gameRecord.release}
          onChange={(e) =>
            setGameRecord({ ...gameRecord, release: e.target.value })
          }
        />
        <label>Description</label>
        <Input
          className="modal-input"
          placeholder="Description"
          value={gameRecord.description}
          onChange={(e) =>
            setGameRecord({ ...gameRecord, description: e.target.value })
          }
        />
        {!isEditing && (
          <>
            <label>Image</label>
            <Input
              className="modal-input"
              type="file"
              accept="image/*"
              onChange={handleFileChange}
            />
          </>
        )}{" "}
      </Modal>
     

    </Space>
  );
}

export default Game;
