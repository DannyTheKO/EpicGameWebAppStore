import { Button, Space, Table, Typography, Modal, Input, Select } from "antd";
import { useEffect, useState } from "react";
import { GetAllPublisher, UpdatePublisher,AddPublisher } from "./API";
import "./table.css";
import {jwtDecode} from 'jwt-decode';
const { Text } = Typography;

function Publisher() {
  const [loading, setLoading] = useState(false);
  const [dataSource, setDataSource] = useState([]);
  const [Count, setCount] = useState([]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isEditing, setIsEditing] = useState(false);
  const [publisherRecord, setpublisherRecord] = useState({
    id: "",
    name: "",
    address: "",
    email: "",
    phone: "",
    website: "",
  });

  useEffect(() => {
    const fetchGame = async () => {
      setLoading(true);
      const res = await GetAllPublisher();
      setDataSource(res || []);
      setCount(res.length);
      setLoading(false);
    };
    fetchGame();
  }, []);
  const isAdmin = () => {
    const role = localStorage.getItem('authToken'); // Assuming role is stored in localStorage
    const decodedToken = jwtDecode(role);
    const userRole = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    return userRole === "Admin";
  
  };
  const openModal = (record = null) => {
    if (record) {
      setpublisherRecord(record);
      setIsEditing(true); // Chế độ sửa
    } else {
      const maxId = dataSource.length > 0 ? Math.max(...dataSource.map(item => item.discountId)) : 0;
      setCount(maxId);;
      setpublisherRecord({
        id: Count + 1,
        name: "",
        address: "",
        email: "",
        phone: "",
        website: "",
      });
      setIsEditing(false); // Chế độ thêm
    }
    setIsModalOpen(true); // Mở modal
  };

  
  const validatePublisherRecord = () => {
    const { name, phone, email, address, website } = publisherRecord;
  
    // Kiểm tra tên nhà sản xuất
    if (!name) {
      Modal.error({
        title: "Lỗi",
        content: "Vui lòng nhập tên nhà sản xuất.",
      });
      return false;
    }
  
    // Kiểm tra email (đảm bảo là email hợp lệ)
    const emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
    if (!email || !emailRegex.test(email)) {
      Modal.error({
        title: "Lỗi",
        content: "Vui lòng nhập địa chỉ email hợp lệ.",
      });
      return false;
    }
  
    // Kiểm tra số điện thoại (đảm bảo là số hợp lệ)
    const phoneRegex = /^[0-9]{10,15}$/;
    if (!phone || !phoneRegex.test(phone)) {
      Modal.error({
        title: "Lỗi",
        content: "Vui lòng nhập số điện thoại hợp lệ (10-15 chữ số).",
      });
      return false;
    }
  
    // Kiểm tra địa chỉ
    if (!address) {
      Modal.error({
        title: "Lỗi",
        content: "Vui lòng nhập địa chỉ.",
      });
      return false;
    }
  
    // Kiểm tra website (nếu có) - đảm bảo là URL hợp lệ
    if (website && !/^https?:\/\/[^\s]+$/.test(website)) {
      Modal.error({
        title: "Lỗi",
        content: "Vui lòng nhập địa chỉ website hợp lệ.",
      });
      return false;
    }
  
    return true; // Nếu tất cả các trường hợp lệ
  };
  
  const handleSave = async () => {

    if (isEditing) {
      console.log(publisherRecord.publisherId, publisherRecord);
 if (!validatePublisherRecord()) {
      return; // Nếu không hợp lệ thì không lưu
    }
      try {
        // Cập nhật dữ liệu (Ví dụ: gọi API để cập nhật publisher)
        await UpdatePublisher(publisherRecord.publisherId, publisherRecord);

        // Cập nhật lại danh sách sau khi thay đổi
        const updatedDataSource = await GetAllPublisher();
        setDataSource(updatedDataSource);

        // Hiển thị thông báo thành công
        Modal.success({
          title: "Success",
          content: `Publisher ID ${publisherRecord.id} đã được cập nhật thành công.`,
        });
      } catch (error) {
        console.error("Update failed", error);
        Modal.error({
          title: "Error",
          content: "Cập nhật không thành công, vui lòng thử lại sau.",
        });
      }
    } else {
      try {
        // Thêm mới game hoặc publisher
        // console.log("Thêm sản phẩm mới:", gameRecord);
        // const addedGame = await AddGame(gameRecord); // Sử dụng await ở đây
        // console.log("Added Game:", addedGame); // Kiểm tra dữ liệu vừa thêm
        await AddPublisher(publisherRecord);

        // Cập nhật lại danh sách sau khi thay đổi
        const updatedDataSource = await GetAllPublisher();
        setDataSource(updatedDataSource);
        // Hiển thị thông báo thành công
        Modal.success({
          title: "Success",
          content: `Game ID  đã được thêm mới thành công.`,
        });
      } catch (error) {
        console.error("Add failed", error);
        Modal.error({
          title: "Error",
          content: "Thêm sản phẩm không thành công, vui lòng thử lại sau.",
        });
      }
    }

    setIsModalOpen(false); // Đóng modal sau khi lưu
  };

  return (
    <Space className="size_table" size={10} direction="vertical">
        { isAdmin() &&
               <Button onClick={() => openModal()} type="primary" style={{ marginLeft: "1500px" ,marginTop: "20px"  }}>
               Add
             </Button>
             }
      <Table
        className="data"
        loading={loading}
        columns={[
          {
            title: "Publisher ID",
            dataIndex: "publisherId",
            key: "publisherId",
            render: (PublisherID) => <Text>{PublisherID}</Text>,
          },
          {
            title: "Name",
            dataIndex: "name",
            key: "name",
            render: (Name) => <Text>{Name}</Text>,
          },
          {
            title: "Address",
            dataIndex: "address",
            key: "address",
            render: (Address) => <Text>{Address}</Text>,
            width: 200,
          },
          {
            title: "Email",
            dataIndex: "email",
            key: "email",
            render: (Email) => <Text>{Email}</Text>,
          },
          {
            title: "Phone",
            dataIndex: "phone",
            key: "phone",
            render: (Phone) => <Text>{Phone}</Text>,
          },
          {
            title: "Website",
            dataIndex: "website",
            key: "website",
            render: (Website) => <Text>{Website}</Text>,
          },
          {
            title: "Actions",
            key: "actions",
            render: (record) => (
              <Space size="middle">
          
            
               <Button onClick={() => openModal(record)} type="primary">
                 Edit
               </Button>
              
             </Space>
            ),
            className: "text-center",
          },
        ]}
        dataSource={dataSource.map((item) => ({ ...item, key: item.id }))}
        rowKey="PublisherID"
        pagination={{ pageSize: 4, position: ["bottomCenter"] }}
        scroll={{ x: "max-content" }}
      />

      {/* Modal cho cả Thêm và Sửa */}
      <Modal
        className="form_addedit"
        title={
          isEditing ? "Sửa thông tin nhà sản xuất" : "Thêm nhà sản xuất mới"
        }
        open={isModalOpen}
        onCancel={() => setIsModalOpen(false)}
        onOk={handleSave}
      >
        <label>Publisher ID</label>
        <Input
          placeholder="ID Publisher"
          value={publisherRecord.publisherId || publisherRecord.id}
          onChange={(e) =>
            setpublisherRecord({ ...publisherRecord, id: e.target.value })
          }
          disabled
        />
        <label>Name</label>
        <Input
          placeholder="Name"
          value={publisherRecord.name}
          onChange={(e) =>
            setpublisherRecord({ ...publisherRecord, name: e.target.value })
          }
        />
        <label>Phone</label>
        <Input
          placeholder="Phone"
          type="phone"
          value={publisherRecord.phone}
          onChange={(e) =>
            setpublisherRecord({ ...publisherRecord, phone: e.target.value })
          }
        />
        <label>Email</label>
        <Input
          placeholder="Email"
          type="email"
          value={publisherRecord.email}
          onChange={(e) =>
            setpublisherRecord({ ...publisherRecord, email: e.target.value })
          }
        />
        <label>Address</label>
        <Input
          placeholder="Address"
          value={publisherRecord.address}
          onChange={(e) =>
            setpublisherRecord({ ...publisherRecord, address: e.target.value })
          }
        />
        <label>Website</label>
        <Input
          placeholder="Website"
          type="text"
          value={publisherRecord.website}
          onChange={(e) =>
            setpublisherRecord({ ...publisherRecord, website: e.target.value })
          }
        />
      </Modal>
    </Space>
  );
}

export default Publisher;
