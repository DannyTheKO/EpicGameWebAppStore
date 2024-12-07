import { Button, Space, Table, Modal, Input, Select, Typography } from "antd";
import { useEffect, useState } from "react";
import { GetAccount, GetRole, UpdateAccount, AddAccountu } from "./API";
import "./table.css";
import {jwtDecode} from 'jwt-decode';
const { Text } = Typography;
const { Option } = Select;
function Account() {
  const [loading, setLoading] = useState(false);
  const [dataSource, setDataSource] = useState([]);
  const [dataRole, setRole] = useState([]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isEditing, setIsEditing] = useState(false);
  const [Count, setCount] = useState(0);

  const [AccountRecord, setAccountRecord] = useState({
    id: "",
    role: "",
    username: "",
    email: "",
    createdOn: null, // Thống nhất sử dụng createdOn
    isActive: "",
    password:"",
  });
  useEffect(() => {
    const fetchAccount = async () => {
      setLoading(true);
      try {
        const [res, role] = await Promise.all([GetAccount(), GetRole()]);
        setDataSource(res || []);
        setRole(role || []);
        setCount(res.length);
      } catch (error) {
        console.log("lỗi load data");
      }
      setLoading(false);
    };
    fetchAccount();
  }, []);
  const isAdmin = () => {
    const role = localStorage.getItem('authToken'); // Assuming role is stored in localStorage
    const decodedToken = jwtDecode(role);
    const userRole = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    return userRole === "Admin";
  
  };
  const openModal = (record = null) => {
    if (record) {
      
      const matchedRole = dataRole.find(
        (role) => role.roleId === record.roleId
      );
      setAccountRecord({
        ...record,
        createdOn: record.createdOn ? record.createdOn.split("T")[0] : "", // Đảm bảo ngày ở định dạng "yyyy-MM-dd"
        role: matchedRole ? matchedRole.name : "", // Lưu tên role
        id: record.accountId, // Đảm bảo lấy đúng id từ record
    
      });
      setIsEditing(true);
    } else {
      setAccountRecord({
        id: Count + 1, // Tạo ID mới cho tài khoản
        role: "",
        username: "",
        email: "",
        createdOn: null,
        isActive: "Y",
        password :"123456",
      });
      setIsEditing(false);
    }
    setIsModalOpen(true);
  };

  const validateAccountRecord = () => {
    const { username, email, role, isActive } = AccountRecord;
  
    // Kiểm tra các trường bắt buộc
    if (!username || !email || !role || !isActive) {
      Modal.error({
        title: "Lỗi",
        content: "Vui lòng điền đầy đủ thông tin hợp lệ cho tất cả các trường.",
      });
      return false;
    }
  
    // Kiểm tra định dạng email
    const emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
    if (!emailRegex.test(email)) {
      Modal.error({
        title: "Lỗi",
        content: "Vui lòng nhập một địa chỉ email hợp lệ.",
      });
      return false;
    }
  
    // Kiểm tra nếu Role không hợp lệ
    if (!role) {
      Modal.error({
        title: "Lỗi",
        content: "Vui lòng chọn một quyền hợp lệ.",
      });
      return false;
    }
  
    // Kiểm tra nếu trạng thái "Is Active" không hợp lệ
    if (isActive !== "Y" && isActive !== "N") {
      Modal.error({
        title: "Lỗi",
        content: "Trạng thái 'Is Active' phải là 'Y' hoặc 'N'.",
      });
      return false;
    }
  
    return true; // Nếu tất cả các kiểm tra đều hợp lệ
  };
  
  const handleSave = async () => {
    if (!validateAccountRecord()) {
      return;
    }
    const roleId = dataRole.find((role) => role.name === AccountRecord.role)?.roleId;
  const recordToSave = { ...AccountRecord, roleId };
    if (isEditing) {
      console.log("Lưu dữ liệu đã sửa:", recordToSave);
      try {
        await UpdateAccount(AccountRecord.id, recordToSave);
        const updatedDataSource = await GetAccount(); // Lấy lại danh sách tài khoản từ DB
        setDataSource(updatedDataSource); // Cập nhật state với danh sách mới

        Modal.success({
          title: "Account update successfully",
          content: `The account with ID ${AccountRecord.id} has been deleted.`,
        });
      } catch (error) {
        console.error("Error deleting account:", error);
        Modal.error({
          title: "Error",
          content:
            "An error occurred while deleting the account. Please try again.",
        });
      }
      } else {

          console.log("Thêm sản phẩm mới:", AccountRecord);
          try {
            const roleId = dataRole.find((role) => role.name === AccountRecord.role)?.roleId;
            console.log(roleId);
        
            if (roleId) {
              const newAccount = { ...AccountRecord, roleId }; // Thêm roleId vào dữ liệu tài khoản
              delete newAccount.role; // Nếu cần loại bỏ key role tránh bị override.
              console.log(newAccount);
              await AddAccountu(newAccount); // Thêm tài khoản vào hệ thống
        
              const addDataSource = await GetAccount(); // Lấy lại danh sách tài khoản từ DB
              setDataSource(addDataSource); // Cập nhật state với danh sách mới
        
              Modal.success({
                title: "Success",
                content: "Account added successfully.",
              });
            } else {
              Modal.error({
                title: "Error",
                content: "Please select a valid role.",
              });
            }
          } catch (error) {
            console.error("Error adding account:", error);
            Modal.error({
              title: "Error",
              content: "An error occurred while adding the account. Please try again.",
            });
          }
        
        
      }
    setIsModalOpen(false);
  };  

  const handleDelete = (record) => {
    Modal.confirm({
      title: "Are you sure you want to delete this account?",
      content: "This action cannot be undone.",
      okText: "Delete",
      okType: "danger",
      cancelText: "Cancel",
      onOk: async () => {
        try {
          const accountID = record.accountId;
          record.isActive = "N"; // Đánh dấu tài khoản là không hoạt động

          console.log("Deleting account:", accountID);
          console.log("Updated record:", record);

          await UpdateAccount(accountID, record); // Cập nhật trạng thái trong cơ sở dữ liệu
          const updatedDataSource = await GetAccount(); // Lấy lại danh sách tài khoản từ DB
          setDataSource(updatedDataSource); // Cập nhật state với danh sách mới

          Modal.success({
            title: "Account deleted successfully",
            content: `The account with ID ${accountID} has been deleted.`,
          });
        } catch (error) {
          console.error("Error deleting account:", error);
          Modal.error({
            title: "Error",
            content:
              "An error occurred while deleting the account. Please try again.",
          });
        }
      },
    });
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
            title: "ID",
            dataIndex: "accountId",
            key: "accountId",
            render: (AccountID) => <Text>{AccountID}</Text>,
          },
          {
            title: "Role",
            dataIndex: "roleId",
            key: "roleId",
            render: (roleId) => {
              const role = dataRole.find((item) => item.roleId === roleId); // Tìm account theo ID
              return <Text>{roleId ? role.name : roleId}</Text>; // Hiển thị username hoặc thông báo lỗi
            },
          },
          {
            title: "Username",
            dataIndex: "username",
            key: "username",
            render: (Username) => <Text>{Username}</Text>,
          },
          {
            title: "Email",
            dataIndex: "email",
            key: "email",
            render: (Email) => <Text>{Email}</Text>,
          },
          {
            title: "Create On",
            dataIndex: "createdOn",
            key: "createdOn",
            render: (Createon) => new Date(Createon).toLocaleDateString(),
            
          },
          {
            title: "Is Active",
            dataIndex: "isActive",
            key: "isActive",
            render: (IsActive) => <Text>{IsActive}</Text>,
          },
          {
            title: "Actions", // Cột chứa các nút
            render: (record) => {
              return (
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
              );
            },
          },
        ]}
        dataSource={dataSource.map((item) => ({ ...item, key: item.id }))}
        rowKey="accountId"
        pagination={{ pageSize: 7, position: ["bottomCenter"] }}
        scroll={{ x: "max-content" }}
        
      ></Table>
      <Modal
        className="form_addedit"
        title={isEditing ? "Sửa thông tin tài khoản" : "Thêm tài khoản mới"}
        open={isModalOpen}
        onCancel={() => setIsModalOpen(false)}
        onOk={handleSave}
      >
        <label>ID</label>
        <Input
          placeholder="ID Acount"
          value={AccountRecord.id}
          onChange={(e) =>
            setAccountRecord({ ...AccountRecord, id: e.target.value })
          }
          disabled
        />
        <label>Usernmae</label>
        <Input
          placeholder="Username"
          value={AccountRecord.username}
          onChange={(e) =>
            setAccountRecord({ ...AccountRecord, username: e.target.value })
          }
        />
        <label>Email</label>
        <Input
          placeholder="Email"
          value={AccountRecord.email}
          onChange={(e) =>
            setAccountRecord({ ...AccountRecord, email: e.target.value })
          }
        />
        <label >Create on</label>

        <Input
          type="date"
          placeholder="Create on"
          value={AccountRecord.createdOn} // Sử dụng createdOn
          onChange={(e) =>
            setAccountRecord({ ...AccountRecord, createdOn: e.target.value })
          } // Cập nhật giá trị
          disabled={isEditing}
        />
        <label htmlFor="">Active</label>
        <Input
          placeholder="Is Active"
          type="text"
          value={AccountRecord.isActive}
          onChange={(e) =>
            setAccountRecord({ ...AccountRecord, isActive: e.target.value })
          }
          disabled={!isEditing}
        />

        <label htmlFor="">Chọn quyền</label>
        <Select
          placeholder="Chọn quyền"
          value={AccountRecord.role}
          onChange={(value) =>
            setAccountRecord({ ...AccountRecord, role: value })
          }
          style={{ width: "100%", height: "47px" }}
        >
          {dataRole.map((role) => (
            <Option key={role.roleId} value={role.name}>
              {role.name}
            </Option>
          ))}
        </Select>
      </Modal>
    </Space>
  );
}
export default Account;
