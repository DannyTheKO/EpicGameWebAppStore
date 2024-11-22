import { Button, Space, Table, Modal, Input,Select,Typography } from "antd";
import { useEffect, useState } from "react";
import {  GetAllAccountgame,GetAllgame,GetAccount } from "./API";
import "./table.css";
const { Text } = Typography;
const { Option }=Select;

function Accountgame() {
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
    setAccountGameRecord({ accountId: "", gameId: "", dateAdded: "" }); // Dữ liệu trống cho thêm mới
    setIsEditing(false);
  } else {
    setAccountGameRecord(record); // Dữ liệu cho sửa
    setIsEditing(true);
  }
  setIsModalOpen(true);
};

  const handleDelete = (record) => {
    // xóa 
  };
  const validateGameRecord = () => {
    // const { title, author, price, rating, release, description } = gameRecord;
    // if (!title || !author || price <= 0 || rating < 0 || rating > 10 || !release || !description) {
    //     Modal.error({
    //         title: 'Lỗi',
    //         content: 'Vui lòng điền đầy đủ thông tin hợp lệ cho tất cả các trường.',
    //     });
    //     return false;
    // }

    // return true;
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
            title: "ID",
            dataIndex: "accountGameId", // Dữ liệu vẫn là accountId
            key: "accountGameId ",
            render: (accountgameId) => <Text>{accountgameId}</Text>,
          },      
          {
            title: "Account",
            dataIndex: "accountId", // Dữ liệu vẫn là accountId
            key: "accountId",
            render: (accountId) => {
              const account = dataNameAccount.find((item) => item.accountId === accountId); // Tìm account theo ID
              return <Text>{account ? account.username : accountId}</Text>; // Hiển thị username hoặc thông báo lỗi
            },
          },         
          {
            title: "Game",
            dataIndex: "gameId", // Dữ liệu vẫn là accountId
            key: "gameId",
            render: (gameId) => {
              const titlegame = dataTitleGame.find((item) => item.gameId === gameId); // Tìm account theo ID
              return <Text>{gameId ? titlegame.title : gameId}</Text>; // Hiển thị username hoặc thông báo lỗi
            },
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
          pagination={{ pageSize: 5,position: [ "bottomCenter"], }}
          scroll={{ x: "max-content" }}
      ></Table>
      <Modal
          className="form_addedit"
          title={"Thêm tài khoản mới"}
          open={isModalOpen}
          onCancel={() => setIsModalOpen(false)}
          onOk={handleSave}
        >
            <label htmlFor="">Chọn tài khoản</label>
            <Select
  placeholder="Chọn Username"
  value={gameRecord.accountId || ""} // Phải sử dụng gameRecord.accountId
  onChange={(value) => setAccountGameRecord({ ...gameRecord, accountId: value })}
  style={{ width: "100%", height: "47px" }}
>
  {dataNameAccount.map((account) => (
    <Option key={account.accountId} value={account.accountId}>
      {account.username}
    </Option>
  ))}
</Select>
<label htmlFor="">Chọn game</label>
<Select
  placeholder="Chọn Game"
  value={gameRecord.gameId || ""} // Phải sử dụng gameRecord.gameId
  onChange={(value) => setAccountGameRecord({ ...gameRecord, gameId: value })}
  style={{ width: "100%", height: "47px" }}
>
  {dataTitleGame.map((game) => (
    <Option key={game.gameId} value={game.gameId}>
      {game.title}
    </Option>
  ))}
</Select>



        </Modal>
    </Space>
  );
}
export default Accountgame;
