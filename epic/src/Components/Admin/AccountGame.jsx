import { Button, Space, Table, Modal, Input,Select,Typography } from "antd";
import { useEffect, useState } from "react";
import {  GetAllAccountgame,GetAllgame,GetAccount } from "./API";
import "./table.css";
const { Text } = Typography;
const { Option }=Select;

function Orders() {
  const [loading, setLoading] = useState(false);
  const [dataSource, setDataSource] = useState([]);
  const [dataNameAccount, setNameAccount] = useState([]);
  const [dataTitleGame, setTitleGame] = useState([]);
  const [isEditing, setIsEditing] = useState(false);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [gameRecord, setAccountGameRecord] = useState({accoutid:"",username:"",gameid:"",title:"",dateadded:""});
  useEffect(() => {
    const fetchAccountGame = async () => {
      setLoading(true);
      try {
        const [res, username, title]=await Promise.all([
          GetAllAccountgame(),
          GetAccount(),
          GetAllgame()
        ]);
        setDataSource(res || []);
        setNameAccount(username || []);
        setTitleGame(title || []);
        console.log(" sad" + dataTitleGame);
      } catch (error) {
        console.log("lỗi load data")
      }
      setLoading(false);
    };  
    fetchAccountGame();
  }, []);

  const openModal = (record = null) => {
    if (!record) {
    
      setAccountGameRecord(); // Dữ liệu trống cho thêm mới
      setIsEditing(false); // 
    }
    setIsModalOpen(true); 
  };
  const handleDelete = (record) => {
    // xóa 
  };
  const validateGameRecord = () => {
    const { title, author, price, rating, release, description } = gameRecord;
    if (!title || !author || price <= 0 || rating < 0 || rating > 10 || !release || !description) {
        Modal.error({
            title: 'Lỗi',
            content: 'Vui lòng điền đầy đủ thông tin hợp lệ cho tất cả các trường.',
        });
        return false;
    }

    return true;
};
    const handleSave = async () => { 
      if (!validateGameRecord()) {//kiểm tra dữ liệu thêm /sửa
        return; 
    }
      if (isEditing) {
          console.log("Lưu dữ liệu đã sửa:", gameRecord);
          // Thêm logic lưu dữ liệu đã sửa ở đây 
      } else {
          console.log("Thêm sản phẩm mới:", gameRecord);
          // const addedGame = await AddGame(gameRecord); //thêm

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
            title: "Account",
            dataIndex: "accountId", // Dữ liệu vẫn là accountId
            key: "accountId",
            render: (accountId, record) => <Text>{record.account.username}</Text>,
          },         
          {
            title: "Game",
            dataIndex: "gameId", // Dữ liệu vẫn là accountId
            key: "gameId",
            render: (gameId, record) => <Text>{record.game.title}</Text>,
           
          },
          {
            title: "Date Addrd",
            dataIndex: "dateAdded",
            key: "dateAdded",
            render: (release) => new Date(release).toLocaleDateString(),
          },
            {
              title: "Actions",
              key: "actions",
              render: (record) => (
                <Space size="middle">
                  <Button type="primary" onClick={() => openModal()}>Thêm </Button>
                  <Button danger onClick={() => handleDelete(record)}>Xóa</Button>
                </Space>
              ),
              className: "text-center",
            },
        ]}
        dataSource={dataSource.map((item) => ({ ...item, key: item.id }))}
          rowKey=""

          pagination={{ pageSize: 10 }}
          scroll={{ x: 'max-content' }}
      ></Table>
      <Modal
          className="form_addedit"
          title={"Thêm tài khoản mới"}
          open={isModalOpen}
          onCancel={() => setIsModalOpen(false)}
          onOk={handleSave}
        >
        <Select
  placeholder="Chọn Username"
  value={gameRecord.username || ""} // Hiển thị username đã chọn hoặc rỗng nếu chưa chọn
  onChange={(value) => setAccountGameRecord({ ...gameRecord, username: value })} // Cập nhật username vào gameRecord
  style={{ width: "100%", marginTop: "20px", height: "47px" }}
>
  {dataNameAccount.map((account) => (
    <Option key={account.accountId} value={account.username}> 
      {account.name} {/* Hiển thị tên người dùng */}
    </Option>
  ))}
</Select>

<Select
  placeholder="Chọn Game"
  value={gameRecord.title || ""} // Hiển thị game title đã chọn hoặc rỗng nếu chưa chọn
  onChange={(value) => setAccountGameRecord({ ...gameRecord, title: value })} // Cập nhật title vào gameRecord
  style={{ width: "100%", marginTop: "20px", height: "47px" }}
>
  {dataTitleGame.map((game) => (
    <Option key={game.gameId} value={game.title}> 
      {game.name} {/* Hiển thị tên game */}
    </Option>
  ))}
</Select>


        </Modal>
    </Space>
  );
}
export default Orders;
