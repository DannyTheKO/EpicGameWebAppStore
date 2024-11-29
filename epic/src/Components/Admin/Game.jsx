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
import {
  GetAllgame,
  AddGame,
  DeleteGame,
  GetAllPublisher,
  GetAllGenre,
  UpdateGame,
} from "./API"; // Import các API
import "./table.css";

const { Text } = Typography;
const { Option } = Select;

function Game() {
  const [loading, setLoading] = useState(false);
  const [dataSource, setDataSource] = useState([]);
  const [datagenre, setgenre] = useState([]);
  const [datapublisher, setpublisher] = useState([]);
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

  const fetchGame = async () => {
    setLoading(true);
    try {
      // Sử dụng Promise.all để gọi đồng thời 3 API
      const [res, genre, publisher] = await Promise.all([
        GetAllgame(),
        GetAllGenre(),
        GetAllPublisher(),
      ]);
      setDataSource(res || []); // Cập nhật dữ liệu game
      setgenre(genre || []); // Cập nhật dữ liệu thể loại
      setpublisher(publisher || []); // Cập nhật dữ liệu nhà phát hành
    } catch (error) {
      console.error("Lỗi khi tải dữ liệu", error);
    }
    setLoading(false);
  };

  useEffect(() => {
    fetchGame(); // Gọi hàm fetchGame khi component mount
  }, []);
  const getMaxGameId = () => {
    if (dataSource.length === 0) return null; // Trả về null nếu mảng không có phần tử nào
    return dataSource.reduce((maxId, game) => {
      return game.gameId > maxId ? game.gameId : maxId;
    }, 0); // Khởi tạo maxId là 0
  };

  // Open modal to add or edit a game
  const openModal = (record = null) => {
    if (record) {
      // Nếu là chỉnh sửa game
      setGameRecord({
        gameId: record.gameId,
        title: record.title,
        author: record.author,
        price: record.price,
        rating: record.rating,
        release: record.release.split("T")[0], // Định dạng ngày thành YYYY-MM-DD
        description: record.description,
        genreId: record.genre.genreId,
        publisherId: record.publisher.publisherId,
      });
      setIsEditing(true); // Đặt trạng thái là đang chỉnh sửa
    } else {
      // Nếu là thêm mới game, reset gameRecord về giá trị mặc định
      setGameRecord({
        gameId: getMaxGameId() + 1, // Khởi tạo lại ID game với giá trị rỗng
        title: "",
        author: "",
        price: 0,
        rating: 0,
        release: "", // Khởi tạo ngày phát hành là rỗng
        description: "",
        genreId: "", // Khởi tạo genreId là rỗng
        publisherId: "", // Khởi tạo publisherId là rỗng
      });
      setIsEditing(false); // Đặt trạng thái là thêm mới
    }

    setIsModalOpen(true); // Mở modal để thêm hoặc chỉnh sửa game
  };

  const handleSave = async () => {
    if (!validateGameRecord()) {
      return; // Nếu không hợp lệ thì không lưu
    }

    if (isEditing) {
      // Cập nhật game
      console.log("Updating game with data:", gameRecord);

      try {
        console.log(gameRecord);
        // gameRecord.release = gameRecord.release.toISOString()
        await UpdateGame(gameRecord.gameId, gameRecord);
        const updatedDataSource = await GetAllgame(); // Lấy lại danh sách tài khoản từ DB
        setDataSource(updatedDataSource); // Cập nhật state với danh sách mới

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
        await AddGame(gameRecord);
        const updatedDataSource = await GetAllgame(); // Lấy lại danh sách tài khoản từ DB
        setDataSource(updatedDataSource); // Cập nhật state với danh sách mới

        Modal.success({
          title: "Game update successfully",
          content: `The Game with ID ${gameRecord.gameId} has been add.`,
        });
      } catch (error) {
        console.error("Error deleting game:", error);
        Modal.error({
          title: "Error",
          content: "An error occurred while add the game. Please try again.",
        });
      }
    }

    setIsModalOpen(false); // Đóng modal sau khi lưu
    fetchGame(); // Cập nhật lại danh sách game sau khi thêm/sửa
  };

  //test lại

  // const handleDelete = (record) => {
  //   Modal.confirm({
  //       title: "Are you sure you want to delete this game?",
  //       content: "This action cannot be undone.",
  //       okText: "Delete",
  //       okType: "danger",
  //       cancelText: "Cancel",
  //       onOk: () => {
  //           const gameId = record.gameId;
  //           DeleteGame(gameId)
  //               .then(() => {
  //                   setDataSource((prevDataSource) =>
  //                       prevDataSource.filter((item) => item.gameId !== gameId)
  //                   );
  //                   Modal.success({
  //                       title: 'Success',
  //                       content: 'Game has been deleted.',
  //                   });
  //               })
  //               .catch((error) => {
  //                   console.error("Error deleting game:", error);
  //                   Modal.error({
  //                       title: 'Error',
  //                       content: 'Failed to delete the game.',
  //                   });
  //               });
  //       },
  //   });
  // };

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
        console.log(gameId);
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

    // Kiểm tra Publisher và Genre
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
    // Kiểm tra Title và Author không được để trống
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

    // Kiểm tra Price phải lớn hơn 0
    if (price <= 0) {
      Modal.error({
        title: "Error",
        content: "Price must be greater than 0.",
      });
      return false;
    }

    // Kiểm tra Rating phải nằm trong khoảng từ 0 đến 10
    if (rating < 0 || rating > 10) {
      Modal.error({
        title: "Error",
        content: "Rating must be between 0 and 10.",
      });
      return false;
    }

    // Kiểm tra Release Date phải có giá trị
    if (!release) {
      gameRecord.release = new Date().toISOString().split("T")[0]; // Lấy ngày hiện tại và chuyển đổi về định dạng 'YYYY-MM-DD'
    }
    // Kiểm tra Description không được để trống
    if (!description || description.trim() === "") {
      Modal.error({
        title: "Error",
        content: "Description cannot be empty.",
      });
      return false;
    }

    return true;
  };

  // Handle saving game data

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
          <Button onClick={() => openModal()} type="primary">
            Add
          </Button>
          <Button onClick={() => openModal(record)} type="primary">
            Edit
          </Button>
          <Button danger onClick={() => handleDelete(record)}>
            Delete
          </Button>
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
        pagination={{ pageSize: 8, position: ["bottomCenter"] }}
        scroll={{ x: "max-content" }}
      />

      <Modal
        title={isEditing ? "Edit Game Info" : "Add New Game"}
        visible={isModalOpen}
        onCancel={() => setIsModalOpen(false)}
        onOk={handleSave}
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
          value={gameRecord.genreId} // Đảm bảo giá trị là genreId
          onChange={(value) => setGameRecord({ ...gameRecord, genreId: value })}
          style={{ width: "100%", height: "52px" }}
        >
          {datagenre.map((genre) => (
            <Option key={genre.genreId} value={genre.genreId}>
              {genre.name} {/* Hiển thị tên genre */}
            </Option>
          ))}
        </Select>
        <label>Publisher</label>
        <Select
          placeholder="Select Publisher"
          value={gameRecord.publisherId} // Lưu trữ publisherId trong gameRecord
          onChange={(value) =>
            setGameRecord({ ...gameRecord, publisherId: value })
          } // Cập nhật publisherId khi chọn publisher
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
            // Kiểm tra nếu giá trị là số và nằm trong khoảng từ 0 đến 10, cho phép xóa
            const parsedValue = value === "" ? "" : parseInt(value, 10);
            if (
              parsedValue === "" ||
              (!isNaN(parsedValue) && parsedValue >= 0 && parsedValue <= 10)
            ) {
              setGameRecord({ ...gameRecord, rating: parsedValue });
            }
          }}
          onKeyPress={(e) => {
            // Chỉ cho phép nhập các ký tự số từ 0 đến 9 và cho phép Backspace hoặc Delete
            if (
              !/[0-9]/.test(e.key) &&
              e.key !== "Backspace" &&
              e.key !== "Delete"
            ) {
              e.preventDefault(); // Ngăn chặn nhập ký tự không phải số
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
      </Modal>
    </Space>
  );
}

export default Game;
