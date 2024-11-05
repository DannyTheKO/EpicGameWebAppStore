  import { Button, Rate, Space, Table, Typography, Modal, Input,Select } from "antd";
  import { useEffect, useState } from "react";
  import { GetAllgame ,AddGame,DeleteGame} from "./API";
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

    const handleDelete = (record) => {
      Modal.confirm({
        title: "Bạn có chắc chắn muốn xóa sản phẩm này không?",
        content: "Hành động này không thể hoàn tác",
        okText: "Xóa",
        okType: "danger",
        cancelText: "Hủy",
        
        onOk: () => {
          console.log("Record:", record);

          console.log("Original gameId:", record.gameId, "Type:", typeof record.gameId);

          // Chuyển gameId về số nguyên
          const gameId = parseInt(record.gameId, 10);
    
          // Kiểm tra giá trị và kiểu dữ liệu sau khi ép kiểu
          console.log("Parsed gameId:", gameId, "Type:", typeof gameId);
    
          // Nếu gameId không phải là số hợp lệ, dừng lại và thông báo lỗi
          if (isNaN(gameId)) {
            console.error("gameId không phải là số hợp lệ:", record.gameId);
            return;
          }
          DeleteGame(parseInt(record.gameId)) // Giả sử hàm deleteOrder yêu cầu AccountID
            .then(() => {
              // Cập nhật lại danh sách sản phẩm sau khi xóa
              setDataSource((prevDataSource) =>
                prevDataSource.filter((item) => item.gameId !== record.gameId)
              );
              console.log("Đã xóa sản phẩm: ", record.gameId);
            })
            .catch((error) => {
              console.log("Đã xóa sản phẩm: ", (record.gameId).type);
      
              console.error("Lỗi khi xóa sản phẩm:", error);
            });
        },
      });
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

          pagination={{ pageSize: 10 }}
          scroll={{ x: 'max-content' }}
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
