import {
  Button,
  Rate,
  Space,
  Table,
  Typography,
  Modal,
  Input,
  Select,
} from "antd";
import { useEffect, useState } from "react";
import { jwtDecode } from "jwt-decode";
import { GrFormPrevious, GrFormNext } from "react-icons/gr";
import {
  GetAllgame,
  DeleteGame,
  GetAllPublisher,
  GetAllGenre,
  UpdateGame,
  GetImgGame,
} from "./API";
import "./table.css";
import axios from "axios";

const { Text } = Typography;
const { Option } = Select;
function Game() {
  const [loading, setLoading] = useState(false);
  const [dataSource, setDataSource] = useState([]);
  const [datagenre, setgenre] = useState([]);
  const [currentIndex, setCurrentIndex] = useState(0);
  const [datapublisher, setpublisher] = useState([]);
  const [clickimg, setclickimg] = useState(null);
  const imageTypes = ["Thumbnail", "Screenshot", "Banner", "Background"];
  const [selectedType, setSelectedType] = useState("");
  const [dataImg, setDataImg] = useState([
    // {
    //   imageGameId: 5,
    //   gameId: 5,
    //   imageType: "Thumbnail",
    //   fileName: "god_of_war_cover.jpg",
    //   filePath: "/Images/god_of_war/5.jpg",
    //   createdOn: "2022-11-09T00:00:00"
    // }
  ]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isEditing, setIsEditing] = useState(false);
  const [selectedImage, setSelectedImage] = useState(null);
  const [isImageModalOpen, setIsImageModalOpen] = useState(false);
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
  const isValidImage = (src) => {
    // Kiểm tra đường dẫn hợp lệ
    return src && src.trim() !== "";
  };

  const ImageSlider = ({ imageUrls }) => {
    const validImageUrls = imageUrls.filter(isValidImage);

    if (!validImageUrls || validImageUrls.length === 0) {
      return <p>No valid images available</p>;
    }
    const imagesPerPage = 3; // Số ảnh hiển thị trên một trang

    const nextImage = () => {
      setCurrentIndex(
        (prevIndex) => (prevIndex + imagesPerPage) % validImageUrls.length
      );
    };

    const prevImage = () => {
      setCurrentIndex(
        (prevIndex) =>
          (prevIndex - imagesPerPage + validImageUrls.length) %
          validImageUrls.length
      );
    };

    // Lấy các ảnh hiển thị trong trang hiện tại
    const displayedImages = validImageUrls.slice(
      currentIndex,
      currentIndex + imagesPerPage
    );

    // Nếu không đủ ảnh, lấy thêm ảnh từ đầu
    if (displayedImages.length < imagesPerPage) {
      displayedImages.push(
        ...validImageUrls.slice(0, imagesPerPage - displayedImages.length)
      );
    }

    return (
      <div style={{ textAlign: "center" }}>
        <div style={{ display: "flex", gap: "10px", justifyContent: "center" }}>
          <button
            onClick={prevImage}
            style={{
              marginTop: "25px",
              width: "50px",
              height: "50px",
              borderRadius: "50%",
            }}
          >
            <GrFormPrevious />
          </button>
          {displayedImages.map((url, index) => (
            <img
              key={index}
              src={url}
              alt={`Image ${currentIndex + index + 1}`}
              onClick={() => setclickimg(url)} // Lưu ảnh được chọn
              style={{
                width: "100px",
                height: "100px",
                borderRadius: "10px",
                border: clickimg === url ? "5px solid blue" : "5px solid #ccc", // Đánh dấu ảnh được chọn
                boxShadow: "0 4px 8px rgba(0, 0, 0, 0.2)",
                cursor: "pointer",
              }}
            />
          ))}

          <button
            onClick={nextImage}
            style={{
              marginTop: "25px",  
              width: "50px",
              height: "50px",
              borderRadius: "50%",
            }}
          >
            <GrFormNext />
          </button>
        </div>
      </div>
    );
  };

  const getMaxGameId = () => {
    if (dataSource.length === 0) return null;
    return dataSource.reduce((maxId, game) => {
      return game.gameId > maxId ? game.gameId : maxId;
    }, 0);
  };
  const isAdmin = () => {
    const role = localStorage.getItem("authToken");
    const decodedToken = jwtDecode(role);
    const userRole =
      decodedToken[
        "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
      ];
    return userRole === "Admin";
  };
  const handleCloseModal = () => {
    setIsImageModalOpen(false);
  };
  const openModal = (record = null) => {
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
      setGameRecord({
        gameId: getMaxGameId() + 1,
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
  const openModalImg = async (record = null) => {
    if (record) {
      setGameRecord(record);
      try {
        const imgData = await GetImgGame(record.gameId);
        setDataImg(imgData);
      } catch (error) {
        console.error("Lỗi khi lấy ảnh:", error);
      }
    }
    setIsImageModalOpen(true);
  };

  const handleSave = async () => {
    if (!validateGameRecord()) {
      return;
    }
    if (isEditing) {
      try {
        await UpdateGame(gameRecord.gameId, gameRecord);
        const updatedDataSource = await GetAllgame();
        setDataSource(updatedDataSource);
        Modal.success({
          title: "Account update successfully",
          content: `The account with ID ${gameRecord.gameId} has been update.`,
        });
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

        // Tạo đối tượng FormData
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
        // for (let [key, value] of formData.entries()) {
        //   console.log(`${key}: ${value}`);
        // }
        const response = await axios.post(
          "http://localhost:5084/Game/CreateGame",
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
            content: `The game with ID ${gameRecord.gameId} has been added.`,
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
      width: 200,
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
      title: "Description",
      dataIndex: "description",
      key: "description",
      render: (description) => (
        <Text ellipsis style={{ maxWidth: 200 }}>
          {description}
        </Text>
      ),
    },
    {
      title: "Actions",
      key: "actions",
      render: (record) => (
        <Space size="middle">
          <Button onClick={() => openModal(record)} type="primary">
            Edit
          </Button>
          {isAdmin() && (
            <Button danger onClick={() => handleDelete(record)}>
              Delete
            </Button>
          )}
          <Button onClick={() => openModalImg(record)} type="primary">
            Xem ảnh
          </Button>
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
        pagination={{ pageSize: 7, position: ["bottomCenter"] }}
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
        <Input
          className="modal-input"
          placeholder="Title"
          value={gameRecord.title}
          onChange={(e) =>
            setGameRecord({ ...gameRecord, title: e.target.value })
          }
        />
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
      <Modal
        open={isImageModalOpen}
        onCancel={() => setIsImageModalOpen(false)}
        title={`Ảnh của ${gameRecord.title || "N/A"}`}
        footer={[
          <Button key="add" onClick={handleDelete}>
            Thêm
          </Button>,
          <Button key="update" onClick={handleCloseModal}>
            Thay đổi ảnh
          </Button>,
          <Button key="delete" onClick={handleDelete} danger>
            Xóa
          </Button>,
          <Button key="cancel" onClick={handleCloseModal}>
            Hủy
          </Button>,
        ]}
      >
        {dataImg.length === 0 ? (
          <p>Lỗi chưa lấy dữ liệu được</p>
        ) : (
          <div>
            {["Thumbnail", "Banner", "Background"].map((type) => (
              <div key={type}>
                <h3>{type}</h3>
                <br />
                {dataImg.some((img) => img.imageType === type) ? (
                  <div
                    style={{
                      display: "flex",
                      flexWrap: "wrap",
                      justifyContent: "center",
                    }}
                  >
                    {dataImg.map(
                      (img, index) =>
                        img.imageType === type && (
                          <img
                            key={index}
                            src={`${process.env.PUBLIC_URL}${img.filePath}${img.fileName}`}
                            alt={img.fileName}
                            onClick={() => setclickimg(img)}
                            style={{
                              width: "80px",
                              height: "80px",
                              borderRadius: "8px",
                              marginBottom: "10px",
                              cursor: "pointer",
                              border:
                                clickimg?.fileName === img.fileName
                                  ? "5px solid blue" // Đánh dấu ảnh được chọn
                                  : "5px solid #ccc",
                              margin: "5px", // Khoảng cách giữa các ảnh
                            }}
                          />
                        )
                    )}
                  </div>
                ) : (
                  <p>Không có ảnh {type}</p>
                )}
              </div>
            ))}
          </div>
        )}
        {/* Background Section */}
        <div>
          <h3>Screenshot</h3>
          <br />
          {dataImg.some((img) => img.imageType === "Screenshot") ? (
            <div
              style={{
                display: "flex",
                flexWrap: "wrap",
                justifyContent: "center",
              }}
            >
              {
                <ImageSlider
                  // Thêm key cho mỗi phần tử trong map
                  imageUrls={
                    dataImg
                      .filter((img) => img.imageType === "Screenshot") // Lọc ảnh "Screenshot"
                      .map(
                        (img) =>
                          `${process.env.PUBLIC_URL}${img.filePath}${img.fileName}`
                      ) // Tạo mảng các đường dẫn ảnh
                  }
                />
              }
            </div>
          ) : (
            <p>Không có ảnh Screenshot </p>
          )}
        </div>
        <div>
          <Select
            placeholder="Chọn loại ảnh"
            style={{ width: "100%", marginTop: 30, marginBottom: 30 }}
            onChange={(value) => setSelectedType(value)} // Cập nhật kiểu ảnh khi chọn
            allowClear // Thêm tùy chọn để xóa chọn
          >
            {imageTypes.map((type) => (
              <Option key={type} value={type}>
                {type}
              </Option>
            ))}
          </Select>
          <Input
            className="modal-input"
            type="file"
            accept="image/*"
            onChange={handleFileChange}
          />
        </div>
      </Modal>
    </Space>
  );
}

export default Game;
