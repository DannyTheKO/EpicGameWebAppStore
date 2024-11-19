import { Button , Space, Table ,Modal,Input,Select,Typography} from "antd";
import { useEffect, useState } from "react";
import { GetAccount ,GetRole} from "./API";
import "./table.css";
const { Text } = Typography;
const { Option } = Select;
function Account() {
  const [loading, setLoading] = useState(false);
  const [dataSource, setDataSource] = useState([]);
  const [dataRole, setRole] = useState([]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isEditing, setIsEditing] = useState(false);
  const [Count, setCount] = useState(0); 
  const [AccountRecord, setAccountRecord] = useState({id:"",role:"", username: "", email: "",createon:null,isactive: "" });
  useEffect(() => {
    const fetchAccount = async () => {
      setLoading(true);
      try {
        const [res, role]=await Promise.all([
          GetAccount(),
          GetRole()
        ]);
        setDataSource(res || []);
        setRole( role|| []);
        setCount(res.length)
      } catch (error) {
        console.log("lỗi load data")
      }
      setLoading(false);
    };
    fetchAccount();
  }, []);

  const openModal = (record = null) => {
    if (record) {
      record.createon = record.createon.split("T")[0];
      setAccountRecord(record); 
      setIsEditing(true); 
    } else {
      setAccountRecord({id:Count,role:"", username: "", email: "",createon:null,isactive:""}); 
      setIsEditing(false); 
    }
    setIsModalOpen(true);
  };

  const validateGameRecord = () => {
    // const { title, author, price, rating, release, description } = gameRecord;

    // // Kiểm tra các trường bắt buộc
    // if (!title || !author || price <= 0 || rating < 0 || rating > 10 || !release || !description) {
    //     Modal.error({
    //         title: 'Lỗi',
    //         content: 'Vui lòng điền đầy đủ thông tin hợp lệ cho tất cả các trường.',
    //     });
    //     return false;
    // }

    return true;
};
    const handleSave = async () => { 
      if (!validateGameRecord()) {
        return;
    }
      if (isEditing) {
          // console.log("Lưu dữ liệu đã sửa:", gameRecord);
          // Thêm logic lưu dữ liệu đã sửa ở đây
      } else {
          // console.log("Thêm sản phẩm mới:", gameRecord);
          // const addedGame = await AddGame(gameRecord); // thêm mới game
          // console.log("Added Game:", addedGame);
      }
      setIsModalOpen(false); 
  };

  const handleDelete = (record) => {
    console.log("Xóa sản phẩm: ", record);
    // Thêm logic xóa sản phẩm ở đây
  };
  return (
    <Space className="size_table" size={10} direction="vertical">
      
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
            title: "ID Role",
            dataIndex: "roleId",
            key: "roleId",
            render: (RoleId) => <Text>{RoleId}</Text>,
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
                  <Button type="primary" onClick={() => openModal()}>Thêm</Button>
                  <Button onClick={() => openModal(record)}>Sửa</Button>
                  <Button danger onClick={() => handleDelete(record)}>Xóa</Button>
                </Space>
              );
            },
          }
          
        ]}
        dataSource={dataSource.map((item) => ({ ...item, key: item.id }))}
          rowKey="AccountID"

          pagination={{ pageSize: 10 }}
          scroll={{ x: 'max-content' }}
      ></Table>
    <Modal
          className="form_addedit"
          title={isEditing ? "Sửa thông tin game" : "Thêm Game mới"}
          open={isModalOpen}
          onCancel={() => setIsModalOpen(false)}
          onOk={handleSave}
        >
          <Input
            placeholder="ID Acount"
            value={AccountRecord.id}
            onChange={(e) => setAccountRecord({ ...AccountRecord, id: e.target.value })}
            disabled
          />
          <Input
            placeholder="Username"
            value={AccountRecord.username}
            onChange={(e) => setAccountRecord({ ...AccountRecord, username: e.target.value })}
          />
          <Input
            placeholder="Email"
            value={AccountRecord.email}
            onChange={(e) => setAccountRecord({ ...AccountRecord, email: e.target.value })}
          />
          
          <Input
            type="date"
            placeholder="Create on"
            value={AccountRecord.createon}
            onChange={(e) => setAccountRecord({ ...AccountRecord, createon: e.target.value })} // Cập nhật giá trị
            
          />
          <Input
            placeholder="Is Active"
            type="string"
            value={AccountRecord.isactive}
            onChange={(e) => setAccountRecord({ ...AccountRecord, isactive: e.target.value })}
          />
          <Select
          placeholder="Chọn quyền"
          value={AccountRecord.role}
          onChange={(value) => setAccountRecord({ ...AccountRecord, role: value })}
          
        >
          {dataRole.map((role)=>(
            <Option key ={role.id} value={role.name}>
              {role.name}
            </Option>
          ))}
          
        </Select>
        
        </Modal>
      </Space>
  );
}
export default Account;
