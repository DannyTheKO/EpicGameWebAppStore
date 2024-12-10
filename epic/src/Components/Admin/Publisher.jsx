import { Button, Space, Table, Typography, Modal, Input } from "antd";
import { useEffect, useState } from "react";
import { GetAllPublisher, UpdatePublisher,AddPublisher } from "./API";
import "./table.css";
import {jwtDecode} from 'jwt-decode';
const { Text } = Typography;

function Publisher() {
  const [loading, setLoading] = useState(false);
  const [dataSource, setDataSource] = useState([]);
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
      
      setLoading(false);
    };
    fetchGame();
  }, []);
  const isAdmin = () => {
    const role = localStorage.getItem('authToken'); 
    const decodedToken = jwtDecode(role);
    const userRole = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    return userRole === "Admin";
  
  };
  const openModal = async  (record = null) => {
    if (record) {
      setpublisherRecord(record);
      setIsEditing(true); 
    } else {
      const updatedDataSource = await GetAllPublisher();
      setDataSource(updatedDataSource);
      const maxId = Math.max(...dataSource.map(item => item.publisherId)) ;

      setpublisherRecord({
        id: maxId + 1,
        name: "",
        address: "",
        email: "",
        phone: "",
        website: "",
      });
      setIsEditing(false); 
    }
    setIsModalOpen(true); 
  };

  
  const validatePublisherRecord = () => {
    const { name, phone, email, address, website } = publisherRecord;
    if (!name) {
      Modal.error({
        title: "Lỗi",
        content: "Vui lòng nhập tên nhà sản xuất.",
      });
      return false;
    }
    const emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
    if (!email || !emailRegex.test(email)) {
      Modal.error({
        title: "Lỗi",
        content: "Vui lòng nhập địa chỉ email hợp lệ.",
      });
      return false;
    }
    const phoneRegex = /^[0-9]{10,15}$/;
    if (!phone || !phoneRegex.test(phone)) {
      Modal.error({
        title: "Lỗi",
        content: "Vui lòng nhập số điện thoại hợp lệ (10-15 chữ số).",
      });
      return false;
    }
    if (!address) {
      Modal.error({
        title: "Lỗi",
        content: "Vui lòng nhập địa chỉ.",
      });
      return false;
    }
    if (website && !/^https?:\/\/[^\s]+$/.test(website)) {
      Modal.error({
        title: "Lỗi",
        content: "Vui lòng nhập địa chỉ website hợp lệ.",
      });
      return false;
    }
    return true; 
  };
  
  const handleSave = async () => {
    if (!validatePublisherRecord()) {
      return;
    }
    if (isEditing) {
      console.log(publisherRecord.publisherId, publisherRecord);

      try {
        await UpdatePublisher(publisherRecord.publisherId, publisherRecord);
        const updatedDataSource = await GetAllPublisher();
        setDataSource(updatedDataSource);
        Modal.success({
          title: "Success",
          content: `Publisher đã được cập nhật thành công.`,
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
        await AddPublisher(publisherRecord);
        const updatedDataSource = await GetAllPublisher();
        setDataSource(updatedDataSource);
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

    setIsModalOpen(false); 
  };

  return (
    <Space className="size_table" size={10} direction="vertical">
        { isAdmin() &&
               <Button onClick={() => openModal()} type="primary" style={{ marginLeft: "1450px" ,marginTop: "20px"  }}>
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
        pagination={{ pageSize: 5, position: ["bottomCenter"] }}
        scroll={{ x: "max-content" }}
      />
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
