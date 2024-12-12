import { Button, Space, Table, Modal, Select, Typography } from "antd";
import { useEffect, useState } from "react";

import {
  GetAllAccountgame,
  GetAllgame,
  GetAccount,
  DeleteAccountgame,
  AddAccountgame,
} from "./API";
import "./table.css";
const { Text } = Typography;
const { Option } = Select;

function Accountgame() {
  const [loading, setLoading] = useState(false);
  const [dataSource, setDataSource] = useState([]);
  const [dataNameAccount, setNameAccount] = useState([]);
  const [dataTitleGame, setTitleGame] = useState([]);
  const [isEditing, setIsEditing] = useState(false);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [AcountgameRecord, setAccountGameRecord] = useState({
    accoutid: "",
    username: "",
    gameid: "",
    title: "",
    dateadded: "",
  });
  useEffect(() => {
    const fetchAccountGame = async () => {
      setLoading(true);
      try {
        const [res, username, title] = await Promise.all([
          GetAllAccountgame(),
          GetAccount(),
          GetAllgame(),
        ]);
        setDataSource(res || []);
        setNameAccount(username || []);
        setTitleGame(title || []);
      } catch (error) {
        console.log("lỗi load data");
      }
      setLoading(false);
    };
    fetchAccountGame();
   
  }, []);

  const openModal = (record = null) => {
    if (!record) {
      setAccountGameRecord({ accountId: "", gameId: "", dateAdded: "" }); 
      setIsEditing(false);
    } 
    setIsModalOpen(true);
  };

  const handleDelete = (record) => {
    Modal.confirm({
      title: "Are you sure you want to delete this  acount game?",
      content: "This action cannot be undone.",
      okText: "Delete",
      okType: "danger",
      cancelText: "Cancel",
      onOk: () => {
        const acountgameid = record.accountGameId;
        console.log(acountgameid);
        DeleteAccountgame(acountgameid)
          .then(() => {
            setDataSource((prevDataSource) =>
              prevDataSource.filter(
                (item) => item.accountGameId !== acountgameid
              )
            );
          })
          .catch((error) => {
            console.error("Error deleting account game:", error);
          });
      },
    });
  };

  const validateAccountGameRecord = () => {
    const { accountId, gameId} = AcountgameRecord;
    if (!accountId || !gameId) {
      Modal.error({
        title: "Lỗi",
        content: "Vui lòng chọn tài khoản và game.",
      });
      return false; 
    }
    return true;
  };
  

  const handleSave = async () => {
    if (!validateAccountGameRecord()) {
      return;
    }
    setLoading(true); 
    try {
      if (!isEditing) {
        const result = await AddAccountgame(AcountgameRecord);
        if (result.success) {
          Modal.success({
            title: "Thành công",
            content: result.message,
          });
        } else {
          Modal.error({
            title: "Thất bại",
            content: result.message,
          });
      } 
      const updatedDataSource = await GetAllAccountgame();
      setDataSource(updatedDataSource);
    }
    } catch (error) {
      console.error("Lỗi khi thêm tài khoản game:", error);
      Modal.error({
        title: "Lỗi",
        content: "Có lỗi xảy ra khi thêm tài khoản game. Vui lòng thử lại.",
      });
    } finally {
      setLoading(false);
      setIsModalOpen(false);
    }
  };
  
  return (
    <Space className="size_table" size={20} direction="vertical">
      <Button type="primary" onClick={() => openModal()}  style={{ marginLeft: "1450px" ,marginTop: "20px"  }}>
                  Add
                </Button>
      <Table
        className="data"
        loading={loading}
        columns={[
          {
            title: "ID",
            dataIndex: "accountGameId", 
            key: "accountGameId ",
            render: (accountgameId) => <Text>{accountgameId}</Text>,
          },
          {
            title: "Account",
            dataIndex: "accountId", 
            key: "accountId",
            render: (accountId) => {
              const account = dataNameAccount.find(
                (item) => item.accountId === accountId
              ); // Tìm account theo ID
              return <Text>{account ? account.username : accountId}</Text>;
            },
          },
          {
            title: "Game",
            dataIndex: "gameId",
            key: "gameId",
            render: (gameId) => {
              const titlegame = dataTitleGame.find(
                (item) => item.gameId === gameId
              ); 
              return <Text>{gameId ? titlegame.title : gameId}</Text>; 
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
                <Button danger onClick={() => handleDelete(record)}>
                  Delete
                </Button>
              </Space>
            ),
            className: "text-center",
          },
        ]}
        dataSource={dataSource.map((item) => ({ ...item, key: item.id }))}
        rowKey="accountGameId"
        pagination={{ pageSize: 7, position: ["bottomCenter"] }}
        scroll={{ x: "max-content" }}
      ></Table>
      <Modal
        className="form_addedit"
        title={"Add account game new"}
        open={isModalOpen}
        onCancel={() => setIsModalOpen(false)}
        onOk={handleSave}
      >
        <label htmlFor="">Chọn tài khoản</label>
        <Select
          placeholder="Chọn Username"
          value={AcountgameRecord.accountId || ""} 
          onChange={(value) =>
            setAccountGameRecord({ ...AcountgameRecord, accountId: value })
          }
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
          value={AcountgameRecord.gameId || ""} 
          onChange={(value) =>
            setAccountGameRecord({ ...AcountgameRecord, gameId: value })
          }
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
