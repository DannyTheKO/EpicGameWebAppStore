import { Button, Space, Table, Modal, Input,Select,Typography } from "antd";
import { useEffect, useState } from "react";
import {  GetAllAccountgame,GetAllUsername,GetAllTitle } from "./API";
import "./table.css";
const { Text } = Typography;
const { Option }=Select;

function Cart() {
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
          GetAllUsername(),
          GetAllTitle()
        ]);
        setDataSource(res || []);
        setNameAccount(username || []);
        setTitleGame(title || []);
      } catch (error) {
        console.log("lỗi load data")
      }
      setLoading(false);
    };  
    fetchAccountGame();
  }, []);

  const openModal = (record = null) => {
    if (record) {
      record.dateaddedd = record.dateadded.split("T")[0];
      setAccountGameRecord(record); // hiển thi Dữ liệu  game đang sửa
      setIsEditing(true); 
    } else {
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
            title: "ID Account",
            dataIndex: "accountid",
            key: "accountid",
            render: (accountid) => <Text>{accountid}</Text>,
          },         
          {
            title: "ID Game",
            dataIndex: "gameid",
           
          },
          {
            title: "Date Addrd",
            dataIndex: "dateadded",
            key: "dateadded",
            render: (release) => new Date(release).toLocaleDateString(),
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
          rowKey=""

          pagination={{ pageSize: 10 }}
          scroll={{ x: 'max-content' }}
      ></Table>
      <Modal
          className="form_addedit"
          title={isEditing ? "Sửa tài khoản game" : "Thêm tài khoản mới"}
          open={isModalOpen}
          onCancel={() => setIsModalOpen(false)}
          onOk={handleSave}
        >
         <Select
          placeholder="Chọn Username"
          value={gameRecord.namepublisher}
          style={{ width: "100%", marginTop: "20px",height:"47px"  }}
        >
          {dataNameAccount.map((username)=>(
            <Option key ={username.id} value={username.name}>
              {username.name}
            </Option>
          ))}
          
        </Select>
        <Select
          placeholder="Chọn Game"
          value={gameRecord.namepublisher}
          style={{ width: "100%", marginTop: "20px",height:"47px"  }}
        >
          {dataTitleGame.map((title)=>(
            <Option key ={title.id} value={title.name}>
              {title.name}
            </Option>
          ))}
          
        </Select>
        <Input
            type="date"
            placeholder="Date"
            value={gameRecord.release} 
            onChange={(e) => setAccountGameRecord({ ...gameRecord, dateadded: e.target.value })} // Cập nhật giá trị
            style={{ width: "100%", height: "52px", marginTop: "20px" }}
          />
        </Modal>
    </Space>
  );
}
export default Orders;
