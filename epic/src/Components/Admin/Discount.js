import { Button, Space, Table, Typography, Modal, Input, Select } from "antd";
import { useEffect, useState } from "react";
import {
  GetAllgame,
  GetAllDiscount,
  DeleteDiscount,
  UpdateDiscount,
  AddDiscount,
} from "./API";
import { jwtDecode } from "jwt-decode";
import "./table.css";
import { CiSearch } from "react-icons/ci";

const { Text } = Typography;
const { Option } = Select;
function Discount() {
  const [loading, setLoading] = useState(false);
  const [dataSource, setDataSource] = useState([]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isEditing, setIsEditing] = useState(false);
  const [searchText, setSearchText] = useState("");
  const [status, setStatus] = useState("");
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
      } catch (error) {
        console.log("lỗi load data");
      }
      setLoading(false);
    };
    fetchGame();
  }, []);
  const handleSearch = async () => {
    // Lấy danh sách tất cả mã giảm giá
    const updatedDataSource = await GetAllDiscount();
  
    // Lọc theo trạng thái (nếu có)
    let filteredData = updatedDataSource;
    if (status) {
      filteredData = filteredData.filter((item) => {
        const isValid = checkDiscountValidity(item.startOn, item.endOn);
        return status === "active" ? isValid : !isValid;
      });
    }
  
    // Lọc theo từ khóa tìm kiếm (nếu có)
    if (searchText.trim()) {
    
      const searchTextLower = searchText.toLowerCase();
      console.log(searchText);
      filteredData = filteredData.filter((item) => {
        return (
          item.discountId.toString().includes(searchText) || // Tìm theo Discount 
          item.code.toLowerCase().includes(searchTextLower) || // Tìm theo Code
          item.game?.title.toLowerCase().includes(searchTextLower) || // Tìm theo Game Title
          (item.startOn && item.startOn.toLowerCase().includes(searchTextLower)) || // Tìm theo Start Date
          (item.endOn && item.endOn.toLowerCase().includes(searchTextLower)) // Tìm theo End Date
        );
      });
    }
  
    // Cập nhật danh sách hiển thị
    setDataSource(filteredData);
  };
  
;
  
  const checkDiscountValidity = (startOn, endOn) => {
    const currentDate = new Date();  // Ngày hiện tại
    const startDate = new Date(startOn);  // Ngày bắt đầu
    const endDate = new Date(endOn);  // Ngày kết thúc
  
    // Kiểm tra xem ngày hiện tại có nằm trong khoảng thời gian không
    return currentDate >= startDate && currentDate <= endDate;
  };

  const openModal = async (record = null) => {
    if (record) {
      setDiscountRecord({
        id: record.discountId || "",
        gameid: record.game?.gameId || "",
        percent: record.percent || 0,
        code: record.code || "",
        starton: record.startOn ? record.startOn.split("T")[0] : null,
        endon: record.endOn ? record.endOn.split("T")[0] : null,
      });
      setIsEditing(true);
    } else {
      const updatedDataSource = await GetAllDiscount();
      setDataSource(updatedDataSource);
      const maxId =
        dataSource.length > 0
          ? Math.max(...dataSource.map((item) => item.discountId))
          : 0;

      setDiscountRecord({
        id: maxId + 1,
        gameid: "",
        percent: 0,
        code: "",
        starton: null,
        endon: null,
      });
      setIsEditing(false);
    }
    setIsModalOpen(true);
  };
  const isAdmin = () => {
    const role = localStorage.getItem("authToken");
    const decodedToken = jwtDecode(role);
    const userRole =
      decodedToken[
        "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
      ];
    return userRole === "Admin";
  };
  const handleDelete = async (record) => {
    Modal.confirm({
      title: "Are you sure you want to delete this discount?",
      content: "This action cannot be undone.",
      okText: "Delete",
      okType: "danger",
      cancelText: "Cancel",
      onOk: async () => {
        // Đảm bảo onOk là một hàm async
        const discountid = record.discountId;
        console.log(discountid);
        const result = await DeleteDiscount(discountid); // Sử dụng await trong onOk
        if (result.success) {
          // Nếu xóa thành công, cập nhật lại data source
          setDataSource((prevDataSource) =>
            prevDataSource.filter((item) => item.discountId !== discountid)
          );

          // Thông báo thành công
          Modal.success({
            title: "Thành công",
            content: `Mã giảm giá đã được xóa thành công.`,
          });
        } else {
          // Nếu có lỗi, hiển thị thông báo lỗi
          Modal.error({
            title: "Lỗi",
            content: result.message,
          });
        }
      },
    });
  };

  const validateDiscountRecord = () => {
    const { gameid, percent, code, starton, endon } = discountRecord;
    const currentDate = new Date(); // Ngày hiện tại
    if (!gameid) {
      Modal.error({
        title: "Lỗi",
        content: "Vui lòng chọn một game.",
      });
      return false;
    }
    if (percent <= 0 || percent > 100) {
      Modal.error({
        title: "Lỗi",
        content: "Tỷ lệ phần trăm phải nằm trong khoảng từ 1 đến 100.",
      });
      return false;
    }
    if (!code) {
      Modal.error({
        title: "Lỗi",
        content: "Vui lòng nhập mã giảm giá.",
      });
      return false;
    }
    if (!/^[A-Z]{6}$/.test(code)) {
      Modal.error({
        title: "Lỗi",
        content: "Mã giảm giá phải là chữ in và có đúng 6 ký tự.",
      });
      return false;
    }
    if (!starton) {
      Modal.error({
        title: "Lỗi",
        content: "Vui lòng chọn ngày bắt đầu.",
      });
      return false;
    }
    if (!endon) {
      Modal.error({
        title: "Lỗi",
        content: "Vui lòng chọn ngày kết thúc.",
      });
      return false;
    }
    if (new Date(starton) < currentDate) {
      Modal.error({
        title: "Lỗi",
        content: "Ngày bắt đầu không được ở quá khứ.",
      });
      return;
    }
    if (new Date(starton) > new Date(endon)) {
      Modal.error({
        title: "Lỗi",
        content: "Ngày kết thúc phải sau ngày bắt đầu.",
      });
      return false;
    }
    return true;
  };
  function findNameById(id, list) {
    const item = list.find((element) => element.id === id);
    return item ? item.name : "Unknown"; // Trả về tên hoặc "Unknown" nếu không tìm thấy
  }
  const handleSave = async () => {
    if (!validateDiscountRecord()) {
      return;
    }

    try {
      setLoading(true);
      console.log(discountRecord.id, discountRecord);
      if (isEditing) {
        const result = await UpdateDiscount(discountRecord.id, discountRecord);
        if (result.success) {
          const updatedDataSource = await GetAllDiscount();
          setDataSource(updatedDataSource);
          Modal.success({
            title: "Success",
            content: result.message,
          });
        } else {
          Modal.error({
            title: "Sửa giảm giá thất bại",
            content: result.message,
          });
        }
      } else {
        const result = await AddDiscount(discountRecord);

        if (result.success) {
          const updatedDataSource = await GetAllDiscount();
          setDataSource(updatedDataSource);
          Modal.success({
            title: "Success",
            content: result.message,
          });
        } else {
          Modal.error({
            title: "Thêm giảm giá thất bại",
            content: result.message,
          });
        }
      }
    } catch (error) {
      console.error("Error updating discount:", error);
      Modal.error({
        title: "Error",
        content:
          "Đã xảy ra lỗi trong quá trình cập nhật discount. Vui lòng thử lại.",
      });
    } finally {
      setLoading(false);
      setIsModalOpen(false);
    }
  };

  return (
    <Space className="size_table" size={10} direction="vertical">
      <div
        style={{
          display: "flex",
          alignItems: "center",
          gap: "10px",
          margin: "20px",
        }}
      >
        <Select
          placeholder="Select an option"
          value={status}
          onChange={(value) => setStatus(value)}
          style={{
            width: "150px",
          }}
        >
               <Option value=""></Option>
          <Option value="active">Còn hạn sử dụng</Option>
          <Option value="expired">Hết hạn sử dụng</Option>
        </Select>
        <Input
          placeholder=" Nhập thứ bạn muốn tìm kiếm"
          value={searchText}
          onChange={(e) => setSearchText(e.target.value)}
          style={{
            height: "30px", // Chiều cao giống Select
            lineHeight: "32px", // Đồng bộ nội dung
            width: "300px",
          }}
        />
        <Button onClick={handleSearch}>
          <CiSearch />
        </Button>
        {isAdmin() && (
          <Button
            type="primary"
            onClick={() => openModal()}
            style={{ marginLeft: "auto" }}
          >
            Add
          </Button>
        )}
      </div>

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
            render: (record) => {
              const game = dataGame.find(
                (game) => game.gameId === record.gameId
              );
              return <Text>{game ? game.title : "Unknown"}</Text>; // Ensure there's a fallback for missing game
            },
          },

          {
            title: "Percent",
            dataIndex: "percent",
            key: "percent",
            render: (Percent) => `${Percent.toFixed(2)}%`,
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
                {isAdmin() && (
                  <Button danger onClick={() => handleDelete(record)}>
                    Delete
                  </Button>
                )}
              </Space>
            ),
            className: "text-center",
          },
        ]}
        dataSource={dataSource.map((item) => ({ ...item, key: item.id }))}
        rowKey="discountid"
        pagination={{ pageSize: 7, position: ["bottomCenter"] }}
        scroll={{ x: "max-content" }}
      />
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
          value={
            dataGame.find((game) => game.gameId === discountRecord.id)?.title ||
            "Chọn Game"
          }
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
          value={discountRecord.starton || ""}
          onChange={(e) =>
            setDiscountRecord({ ...discountRecord, starton: e.target.value })
          }
          style={{ width: "100%", height: "52px" }}
        />
        <label>End date</label>
        <Input
          type="date"
          placeholder="End Date"
          value={discountRecord.endon || ""}
          onChange={(e) =>
            setDiscountRecord({ ...discountRecord, endon: e.target.value })
          }
          style={{ width: "100%", height: "52px" }}
        />
      </Modal>
    </Space>
  );
}

export default Discount;
