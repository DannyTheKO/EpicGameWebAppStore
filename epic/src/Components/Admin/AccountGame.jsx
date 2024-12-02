import { Button, Space, Table, Modal, Input, Select, Typography } from "antd";
import { useEffect, useState } from "react";
import {jwtDecode} from "jwt-decode";
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
      setAccountGameRecord({ accountId: "", gameId: "", dateAdded: "" }); // Dữ liệu trống cho thêm mới
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
    const { accountId, gameId, dateAdded } = AcountgameRecord;
  
    // Kiểm tra nếu accountId hoặc gameId trống
    if (!accountId || !gameId) {
      Modal.error({
        title: "Lỗi",
        content: "Vui lòng chọn tài khoản và game.",
      });
      return false; // Trả về false nếu dữ liệu không hợp lệ
    }
  
    // Kiểm tra nếu dateAdded không hợp lệ (có thể thêm các kiểm tra khác tùy theo yêu cầu)
   
    // Nếu tất cả các điều kiện trên đều hợp lệ, trả về true
    return true;
  };
  
  // const handleSave = async () => {
  //   if (validateAccountGameRecord()) {
  //     //kiểm tra dữ liệu thêm /sửa
  //     return;
  //   }

  //   try {
  //     // Thực hiện thêm tài khoản game mới
  //     console.log("Dữ liệu gửi đi:", AcountgameRecord);
  //     await AddAccountgame(AcountgameRecord);

  //     // Lấy lại danh sách tài khoản game sau khi thêm
  //     const updatedDataSource = await GetAllAccountgame();
  //     setDataSource(updatedDataSource);

  //     // Thông báo thành công
  //     Modal.success({
  //       title: "Thành công",
  //       content: "Thêm tài khoản game thành công.",
  //     });
  //   } catch (error) {
  //     console.error("Lỗi khi thêm tài khoản game:", error);
  //     Modal.error({
  //       title: "Lỗi",
  //       content: "Có lỗi xảy ra khi thêm tài khoản game. Vui lòng thử lại.",
  //     });
  //   } finally {
  //     setLoading(false);
  //     setIsModalOpen(false); // Đóng mod
  //   }
  // };
  const handleSave = async () => {
    if (!validateAccountGameRecord()) {
      return; // Nếu dữ liệu không hợp lệ, dừng thực hiện
    }
  
    setLoading(true); // Hiển thị trạng thái loading khi đang lưu
  
    try {
      if (!isEditing) {
        // Thêm tài khoản game mới
        console.log("Dữ liệu gửi đi:", AcountgameRecord);
        await AddAccountgame(AcountgameRecord);
  
        Modal.success({
          title: "Thành công",
          content: "Thêm tài khoản game thành công.",
        });
      } 
      // Nếu có thể sửa (logic sửa thiếu hiện tại)
      // else {
      //   // Logic cho sửa nếu cần
      // }
  
      // Refresh lại data
      const updatedDataSource = await GetAllAccountgame();
      setDataSource(updatedDataSource);
    } catch (error) {
      console.error("Lỗi khi thêm tài khoản game:", error);
      Modal.error({
        title: "Lỗi",
        content: "Có lỗi xảy ra khi thêm tài khoản game. Vui lòng thử lại.",
      });
    } finally {
      setLoading(false);
      setIsModalOpen(false); // Đóng modal bất kể thành công hay lỗi
    }
  };
  
  return (
    <Space className="size_table" size={20} direction="vertical">
      <Button type="primary" onClick={() => openModal()}  style={{ marginLeft: "1500px" ,marginTop: "20px"  }}>
                  Thêm
                </Button>
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
              const account = dataNameAccount.find(
                (item) => item.accountId === accountId
              ); // Tìm account theo ID
              return <Text>{account ? account.username : accountId}</Text>; // Hiển thị username hoặc thông báo lỗi
            },
          },
          {
            title: "Game",
            dataIndex: "gameId", // Dữ liệu vẫn là accountId
            key: "gameId",
            render: (gameId) => {
              const titlegame = dataTitleGame.find(
                (item) => item.gameId === gameId
              ); // Tìm account theo ID
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
                
                <Button danger onClick={() => handleDelete(record)}>
                  Xóa
                </Button>
              </Space>
            ),
            className: "text-center",
          },
        ]}
        dataSource={dataSource.map((item) => ({ ...item, key: item.id }))}
        rowKey="accountGameId"
        pagination={{ pageSize: 8, position: ["bottomCenter"] }}
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
          value={AcountgameRecord.accountId || ""} // Phải sử dụng gameRecord.accountId
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
          value={AcountgameRecord.gameId || ""} // Phải sử dụng gameRecord.gameId
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
