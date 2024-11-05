  import { Button, Rate, Space, Table, Typography, Modal, Input,Select } from "antd";
  import { useEffect, useState } from "react";
  import { GetAllgame ,AddGame} from "./API";
  import "./table.css";

  const { Text } = Typography;
  const { Option } = Select;
  function Game() {

    const [loading, setLoading] = useState(false);
    const [dataSource, setDataSource] = useState([]);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [isEditing, setIsEditing] = useState(false);
    const [gameRecord, setGameRecord] = useState({ title: "", author: "", price: 0 ,rating:0,release:null ,description:""});

    useEffect(() => {
      const fetchGame = async () => {
        setLoading(true);
        const res = await GetAllgame();
        setDataSource(res || []);
        setLoading(false);
      };
      fetchGame();
    }, []);

    const openModal = (record = null) => {
      if (record) {
        record.release = record.release.split("T")[0]; // Lấy phần ngày
        setGameRecord(record); // Dữ liệu cho game đang sửa
        console.log(record)
        console.log(record.release )
        setIsEditing(true); // Chế độ sửa
      } else {
        setGameRecord({ title: "", author: "", price: 0 }); // Dữ liệu trống cho thêm mới
        setIsEditing(false); // Chế độ thêm
      }
      setIsModalOpen(true); // Mở modal
    };

    const handleDelete = async (record) => { // Chuyển tham số record vào ngay trong dấu ngoặc
      try {
          // await DeleteGame(record.gameId); // Gọi hàm DeleteGame với gameId
          setDataSource(dataSource.filter((item) => item.gameId !== record.gameId)); // Cập nhật danh sách
          console.log(`Xóa game với ID: ${record.gameId} thành công!`);
      } catch (error) {
          console.error("Lỗi khi xóa game:", error); // Xử lý lỗi nếu có
      }
  };
  const validateGameRecord = () => {
    const { title, author, price, rating, release, description } = gameRecord;

    // Kiểm tra các trường bắt buộc
    if (!title || !author || price <= 0 || rating < 0 || rating > 10 || !release || !description) {
        Modal.error({
            title: 'Lỗi',
            content: 'Vui lòng điền đầy đủ thông tin hợp lệ cho tất cả các trường.',
        });
        return false;
    }

    return true;
};
    const handleSave = async () => { // Thêm 'async' vào đây
      if (!validateGameRecord()) {
        return; // Nếu dữ liệu không hợp lệ, dừng lại
    }
      if (isEditing) {
          console.log("Lưu dữ liệu đã sửa:", gameRecord);
          // Thêm logic lưu dữ liệu đã sửa ở đây
      } else {
          console.log("Thêm sản phẩm mới:", gameRecord);
          const addedGame = await AddGame(gameRecord); // Sử dụng await ở đây
          console.log("Added Game:", addedGame); // Kiểm tra dữ liệu vừa thêm
      }
      setIsModalOpen(false); // Đóng modal sau khi lưu
  };
  

    return (
      <Space className="size_table" size={20} direction="vertical">
      

        <Table
          className="data"
          loading={loading}
          columns={[
            {
              title: "Game ID",
              dataIndex: "gameId",
              key: "gameId",
              render: (gameId) => <Text>{gameId}</Text>,
              className: "text-center",
            },
            {
              title: "Title",
              dataIndex: "title",
              key: "title",
              render: (title) => <Text>{title}</Text>,
              className: "text-center",
            },
            {
              title: "Author",
              dataIndex: "author",
              key: "author",
              className: "text-center",
            },
            {
              title: "Price",
              dataIndex: "price",
              key: "price",
              render: (price) => `$${price.toFixed(2)}`,
              className: "text-center",
            },
            {
              title: "Rating",
              dataIndex: "rating",
              key: "rating",
              render: (rating) => <Rate value={rating / 2} allowHalf disabled />,
              className: "text-center",
            },
            {
              title: "Release Date",
              dataIndex: "release",
              key: "release",
              render: (release) => new Date(release).toLocaleDateString(),
              className: "text-center",
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
              className: "text-center",
            },
            {
              title: "Actions",
              key: "actions",
              render: (record) => (
                <Space size="middle">
                  <Button type="primary" onClick={() => openModal()}>Thêm </Button>
                  <Button onClick={() => openModal(record)}>Sửa</Button>
                  <Button danger onClick={() => handleDelete(record)}>Xóa</Button>
                </Space>
              ),
              className: "text-center",
            },
          ]}
          dataSource={dataSource.map((item) => ({ ...item, key: item.id }))}
          rowKey="gameId"
          pagination={{ pageSize: 15 }}
        />

        {/* Modal cho cả Thêm và Sửa */}
        <Modal
          className="form_addedit"
          title={isEditing ? "Sửa thông tin game" : "Thêm Game mới"}
          open={isModalOpen}
          onCancel={() => setIsModalOpen(false)}
          onOk={handleSave}
        >
          <Input
            placeholder="Title"
            value={gameRecord.title}
            onChange={(e) => setGameRecord({ ...gameRecord, title: e.target.value })}
          />
          <Input
            placeholder="Author"
            value={gameRecord.author}
            onChange={(e) => setGameRecord({ ...gameRecord, author: e.target.value })}
          />
          <Input
            placeholder="Price"
            type="number"
            value={gameRecord.price}
            onChange={(e) => setGameRecord({ ...gameRecord, price: parseFloat(e.target.value) })}
          />
          <Input
            placeholder="Rating"
            value={gameRecord.rating}
            onChange={(e) => setGameRecord({ ...gameRecord, rating: e.target.value })}
          />
          <Input
            type="date"
            placeholder="Release Date"
            value={gameRecord.release} // Hiển thị dữ liệu ngày
            onChange={(e) => setGameRecord({ ...gameRecord, release: e.target.value })} // Cập nhật giá trị
            style={{ width: "100%", height: "52px", marginTop: "20px" }}
          />
           
          <Input
            placeholder="Description"
            type="string"
            value={gameRecord.description}
            onChange={(e) => setGameRecord({ ...gameRecord, description: e.target.value })}
          />
          <Select
          placeholder="Chọn nền tảng"
          value={gameRecord.platform}
          onChange={(value) => setGameRecord({ ...gameRecord, platform: value })}
          style={{ width: "100%", marginTop: "20px",height:"47px"  }}
        >
          <Option value="pc">PC</Option>
          <Option value="xbox">Xbox</Option>
          <Option value="playstation">PlayStation</Option>
          <Option value="mobile">Mobile</Option>
        </Select>
        <Select
          placeholder="Chọn nền tảng"
          value={gameRecord.platform}
          onChange={(value) => setGameRecord({ ...gameRecord, platform: value })}
          style={{ width: "100%", marginTop: "20px",height:"47px" }}
        >
          <Option value="pc">PC</Option>
          <Option value="xbox">Xbox</Option>
          <Option value="playstation">PlayStation</Option>
          <Option value="mobile">Mobile</Option>
        </Select>
        </Modal>
      </Space>
    );
  }

  export default Game;  
