import { Button,  Space, Table, Typography, Modal, Input,Select } from "antd";
import { useEffect, useState } from "react";
import { GetAllgame ,GetAllDiscount} from "./API";
import "./table.css";

const { Text } = Typography;
const { Option } = Select;
function Discount() {

  const [loading, setLoading] = useState(false);
  const [dataSource, setDataSource] = useState([]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isEditing, setIsEditing] = useState(false);
  const [Count, setCount] = useState([]);
  const [discountRecord, setDiscountRecord] = useState({ id:"",gameid: "", percent: 0, code: "" ,starton:null,endon:null });
  const [dataGame, setDataGame] = useState([]);
  useEffect(() => {
    const fetchGame = async () => {
      setLoading(true);
      try {
        const [ res,game]=await Promise.all([
          GetAllDiscount(),
          GetAllgame()
        ]);
        setDataSource(res || []);
        setDataGame(game || []);
        setCount(res.length)
      } catch (error) {
        console.log("lỗi load data")
      }
      setLoading(false);
    };
    fetchGame();
  }, []);

  const openModal = (record = null) => {
    if (record) {
      record.starton = record.starton.split("T")[0]; // Lấy phần ngày
      record.endon=record.endon.split("T")[0];
      setDiscountRecord(record); // Dữ liệu cho game đang sửa
      setIsEditing(true); // Chế độ sửa
    } else {
      setDiscountRecord({ id:Count, gameid: "", percent: 0, code: "" ,starton:null,endon:null }); // Dữ liệu trống cho thêm mới
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
  // const { title, author, price, rating, release, description } = gameRecord;

  // // Kiểm tra các trường bắt buộc
  // if (!title || !author || price <= 0 || rating < 0 || rating > 10 || !release || !description) {
  //     Modal.error({
  //         title: 'Lỗi',
  //         content: 'Vui lòng điền đầy đủ thông tin hợp lệ cho tất cả các trường.',
  //     });
  //     return false;
  // }

  return true;
};
  const handleSave = async () => { // Thêm 'async' vào đây
  //   if (!validateGameRecord()) {
  //     return; // Nếu dữ liệu không hợp lệ, dừng lại
  // }
  //   if (isEditing) {
  //       console.log("Lưu dữ liệu đã sửa:", gameRecord);
  //       // Thêm logic lưu dữ liệu đã sửa ở đây
  //   } else {
  //       console.log("Thêm sản phẩm mới:", gameRecord);
  //       const addedGame = await AddGame(gameRecord); // Sử dụng await ở đây
  //       console.log("Added Game:", addedGame); // Kiểm tra dữ liệu vừa thêm
  //   }
  //   setIsModalOpen(false); // Đóng modal sau khi lưu
};


  return (
    <Space className="size_table" size={10} direction="vertical">
    

      <Table
        className="data"
        loading={loading}
        columns={[
          {
            title: "Discout ID",
            dataIndex: "discountid",
            key: "discountid",
            render: (discountid) => <Text>{discountid}</Text>,
            
          },
          {
            title: "ID Game",
            dataIndex: "gameid",
            key: "gameid",
            render: (gameid) => <Text>{gameid}</Text>,
          },
          {
            title: "Percent",
            dataIndex: "Percent",
            key: "Percent",
            render: (Percent) => `$${Percent.toFixed(2)}`,
          },
         
          {
            title: "Code",
            dataIndex: "code",
            key: "code",
            render: (code) => <Text>{code}</Text>,
          },
          {
            title: "Start on",
            dataIndex: "starton",
            key: "starton",
            render: (starton) => new Date(starton).toLocaleDateString(),
          },
          {
            title: "End on",
            dataIndex: "endon",
            key: "starton",
            render: (starton) => new Date(starton).toLocaleDateString(),
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
        rowKey="discountid"

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
          placeholder="ID Discount"
          value={discountRecord.id}
          onChange={(e) => setDiscountRecord({ ...discountRecord, id: e.target.value })}
        />
          <Select
          placeholder="Chọn Game"
          value={discountRecord.gameid}
          onChange={(value) => setDiscountRecord({ ...discountRecord, gameid: value })}
          style={{ width: "100%", marginTop: "20px", height: "47px" }}
        >
          {dataGame.map((game) => (
            <Option key={game.id} value={game.id}>
              {game.name}
            </Option>
          ))}
        </Select>
        <Input
          placeholder="Percent"
          type="number"
          value={discountRecord.percent}
          onChange={(e) => setDiscountRecord({ ...discountRecord, percent: parseFloat(e.target.value) })}
        />
        <Input
          placeholder="Code"
          value={discountRecord.code}
          onChange={(e) => setDiscountRecord({ ...discountRecord, code: e.target.value })}
        />
        <Input
          type="date"
          placeholder="Start Date"
          value={discountRecord.starton} // Hiển thị dữ liệu ngày
          onChange={(e) => setDiscountRecord({ ...discountRecord, starton: e.target.value })} // Cập nhật giá trị
          style={{ width: "100%", height: "52px", marginTop: "20px" }}
        />
         
         <Input
          type="date"
          placeholder="End Date"
          value={discountRecord.endon} // Hiển thị dữ liệu ngày
          onChange={(e) => discountRecord({ ...discountRecord, endon: e.target.value })} // Cập nhật giá trị
          style={{ width: "100%", height: "52px", marginTop: "20px" }}
        />
       
      </Modal>
    </Space>
  );
}

export default Discount;  
