import { Button, Space, Table, Typography, Modal, Input, Select } from "antd";
import { useEffect, useState } from "react";
import {
  GetAllgame,
  GetAllDiscount,
  DeleteDiscount,
  UpdateDiscount,
  AddDiscount,
} from "./API";
import {jwtDecode} from 'jwt-decode';
import "./table.css";

const { Text } = Typography;
const { Option } = Select;
function Discount() {
  const [loading, setLoading] = useState(false);
  const [dataSource, setDataSource] = useState([]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isEditing, setIsEditing] = useState(false);
  const [Count, setCount] = useState([]);
  const [discountRecord, setDiscountRecord] = useState({
    id: "",
    gameid: "",
    percent: 0,
    code: "",
    starton: null,
    endon: null,
  });
  const [dataGame, setDataGame] = useState([]);
  useEffect(() => {
    const fetchGame = async () => {
      setLoading(true);
      try {
        const [res, game] = await Promise.all([GetAllDiscount(), GetAllgame()]);
        setDataSource(res || []);
        setDataGame(game || []);
        const maxId = res.length > 0 ? Math.max(...res.map(item => item.discountId)) : 0;
        setCount(maxId);
      } catch (error) {
        console.log("lỗi load data");
      }
      setLoading(false);
    };
    fetchGame();
  }, []);

  const openModal = (record = null) => {
    if (record) {
      setDiscountRecord({
        id: record.discountId || "", // Lấy ID của discount
        gameid: record.game?.gameId || "", // Lấy gameId từ record
        percent: record.percent || 0, // Lấy percent
        code: record.code || "", // Lấy code
        starton: record.startOn ? record.startOn.split("T")[0] : null, // Chuyển ngày thành định dạng YYYY-MM-DD
        endon: record.endOn ? record.endOn.split("T")[0] : null, // Tương tự cho ngày kết thúc
      });
      console.log(discountRecord.starton);
      setIsEditing(true); // Chế độ sửa
    } else {
      setDiscountRecord({
        id: Count+1,
        gameid: "",
        percent: 0,
        code: "",
        starton: null,
        endon: null,
      }); // Dữ liệu trống cho thêm mới
      setIsEditing(false); // Chế độ thêm
    }
    setIsModalOpen(true); // Mở modal
  };
  const isAdmin = () => {
    const role = localStorage.getItem('authToken'); // Assuming role is stored in localStorage
    const decodedToken = jwtDecode(role);
    const userRole = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    return userRole === "Admin";
  
  };
  const handleDelete = async (record) => {
    // Chuyển tham số record vào ngay trong dấu ngoặc

    Modal.confirm({
      title: "Are you sure you want to delete this discount?",
      content: "This action cannot be undone.",
      okText: "Delete",
      okType: "danger",
      cancelText: "Cancel",
      onOk: () => {
        const discountid = record.discountId;
        console.log(discountid);
        DeleteDiscount(discountid)
          .then(() => {
            setDataSource((prevDataSource) =>
              prevDataSource.filter((item) => item.discountId !== discountid)
            );
          })
          .catch((error) => {
            console.error("Error deleting game:", error);
          });
      },
    });
  };
  const validateDiscountRecord = () => {
    const { gameid, percent, code, starton, endon } = discountRecord;
  
    // Kiểm tra xem game có được chọn chưa
    if (!gameid) {
      Modal.error({
        title: 'Lỗi',
        content: 'Vui lòng chọn một game.',
      });
      return false;
    }
  
    // Kiểm tra tỷ lệ phần trăm (percent)
    if (percent <= 0 || percent > 100) {
      Modal.error({
        title: 'Lỗi',
        content: 'Tỷ lệ phần trăm phải nằm trong khoảng từ 1 đến 100.',
      });
      return false;
    }
  
    // Kiểm tra mã giảm giá (code) không được để trống
    if (!code) {
      Modal.error({
        title: 'Lỗi',
        content: 'Vui lòng nhập mã giảm giá.',
      });
      return false;
    }
    
    // Kiểm tra mã giảm giá phải là chữ in và có 6 ký tự
    if (!/^[A-Z]{6}$/.test(code)) {
      Modal.error({
        title: 'Lỗi',
        content: 'Mã giảm giá phải là chữ in và có đúng 6 ký tự.',
      });
      return false;
    }
  
    // Kiểm tra ngày bắt đầu (starton) và ngày kết thúc (endon)
    if (!starton) {
      Modal.error({
        title: 'Lỗi',
        content: 'Vui lòng chọn ngày bắt đầu.',
      });
      return false;
    }
  
    if (!endon) {
      Modal.error({
        title: 'Lỗi',
        content: 'Vui lòng chọn ngày kết thúc.',
      });
      return false;
    }
  
    // Kiểm tra ngày kết thúc có phải sau ngày bắt đầu không
    if (new Date(starton) > new Date(endon)) {
      Modal.error({
        title: 'Lỗi',
        content: 'Ngày kết thúc phải sau ngày bắt đầu.',
      });
      return false;
    }
  
    return true; // Nếu tất cả các điều kiện hợp lệ
  };
  
  const handleSave = async () => {
    // Kiểm tra dữ liệu có hợp lệ không trước khi thực hiện hành động
    if (!validateDiscountRecord()) {
      return; // Nếu không hợp lệ, dừng lại và không lưu
    }

    try {
      setLoading(true); // Bật trạng thái loading
      console.log(discountRecord.id, discountRecord);
      if (isEditing) {
        // Gọi API cập nhật discount
        await UpdateDiscount(discountRecord.id, discountRecord);

        // Cập nhật dữ liệu mới từ API
        const updatedDataSource = await GetAllDiscount();
        setDataSource(updatedDataSource);

        // Hiển thị thông báo thành công
        Modal.success({
          title: "Success",
          content: `Discount ID ${discountRecord.id} đã được cập nhật thành công.`,
        });
      } else {
        await AddDiscount(discountRecord);

        // Cập nhật dữ liệu mới từ API
        const updatedDataSource = await GetAllDiscount();
        setDataSource(updatedDataSource);

        // Hiển thị thông báo thành công
        Modal.success({
          title: "Success",
          content: `Discount ID ${discountRecord.id} đã được cập nhật thành công.`,
        });
      }
    } catch (error) {
      console.error("Error updating discount:", error);

      // Hiển thị thông báo lỗi
      Modal.error({
        title: "Error",
        content:
          "Đã xảy ra lỗi trong quá trình cập nhật discount. Vui lòng thử lại.",
      });
    } finally {
      setLoading(false); // Tắt trạng thái loading
      setIsModalOpen(false); // Đóng modal
    }
  };

  return (
    <Space className="size_table" size={10} direction="vertical">
       {/* Nút Add nằm phía trên bảng */}
  {isAdmin() && (
    <Button type="primary" onClick={() => openModal()} style={{ marginLeft: "1500px" ,marginTop: "20px"  }}>
      Add
    </Button>
  )}
      <Table
        className="data"
        loading={loading}
        columns={[
          {
            title: "Discout ID",
            dataIndex: "discountId",
            key: "discountId",
            render: (discountid) => <Text>{discountid}</Text>,
          },
          {
            title: "Game Title",
            key: "gameTitle",
            render: (record) => <Text>{record.game?.title || "Unknown"}</Text>,
          },
          {
            title: "Percent",
            dataIndex: "percent",
            key: "percent",
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
            dataIndex: "startOn",
            key: "startOn",
            render: (starton) => new Date(starton).toLocaleDateString(),
          },
          {
            title: "End on",
            dataIndex: "endOn",
            key: "endOn",
            render: (starton) => new Date(starton).toLocaleDateString(),
          },
          {
            title: "Actions",
            key: "actions",
            render: (record) => (
              <Space size="middle">
          
              
               <Button onClick={() => openModal(record)} type="primary">
                 Edit
               </Button>
              {
               isAdmin() &&
               <Button danger onClick={() => handleDelete(record)}>
               Delete
             </Button>
              }
             </Space>
            ),
            className: "text-center",
          },
        ]}
        dataSource={dataSource.map((item) => ({ ...item, key: item.id }))}
        rowKey="discountid"
        pagination={{ pageSize: 8, position: ["bottomCenter"] }}
        scroll={{ x: "max-content" }}
      />

      {/* Modal cho cả Thêm và Sửa */}
      <Modal
        className="form_addedit"
        title={isEditing ? "Sửa thông tin discount" : "Thêm discount mới"}
        open={isModalOpen}
        onCancel={() => setIsModalOpen(false)}
        onOk={handleSave}
      >
        <label>ID Discount</label>
        <Input
          placeholder="ID Discount"
          value={discountRecord.id}
          onChange={(e) =>
            setDiscountRecord({ ...discountRecord, id: e.target.value })
          }
          disabled
        />
        <label>Chọn game</label>
        <Select
          placeholder="Chọn Game"
          value={discountRecord.gameid}
          onChange={(value) =>
            setDiscountRecord({ ...discountRecord, gameid: value })
          }
          style={{ width: "100%", height: "47px" }}
        >
          {dataGame.map((game) => (
            <Option key={game.gameId} value={game.gameId}>
              {game.title}
            </Option>
          ))}
        </Select>

        <label>Percent</label>
        <Input
          placeholder="Percent"
          type="number"
          value={discountRecord.percent}
          onChange={(e) =>
            setDiscountRecord({
              ...discountRecord,
              percent: parseFloat(e.target.value),
            })
          }
        />
        <label>Code</label>
        <Input
          placeholder="Code"
          value={discountRecord.code}
          onChange={(e) =>
            setDiscountRecord({ ...discountRecord, code: e.target.value })
          }
        />
        <label>Start date</label>
        <Input
          type="date"
          placeholder="Start Date"
          value={discountRecord.starton || ""} // Hiển thị dữ liệu ngày
          onChange={(e) =>
            setDiscountRecord({ ...discountRecord, starton: e.target.value })
          } // Cập nhật dữ liệu ngày bắt đầu
          style={{ width: "100%", height: "52px" }}
        />

        <label>End date</label>
        <Input
          type="date"
          placeholder="End Date"
          value={discountRecord.endon || ""} // Hiển thị dữ liệu ngày
          onChange={(e) =>
            setDiscountRecord({ ...discountRecord, endon: e.target.value })
          } // Cập nhật dữ liệu ngày kết thúc
          style={{ width: "100%", height: "52px" }}
        />
      </Modal>
    </Space>
  );
}

export default Discount;
