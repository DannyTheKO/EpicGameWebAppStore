import { Button,Space, Table, Typography, Modal, Input,Select } from "antd";
import { useEffect, useState } from "react";
import { GetAllPublisher } from "./API";
import "./table.css";

const { Text } = Typography;

function Publisher() {

  const [loading, setLoading] = useState(false);
  const [dataSource, setDataSource] = useState([]);
  const [Count, setCount] = useState([]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isEditing, setIsEditing] = useState(false);
  const [publisherRecord, setpublisherRecord] = useState({id:"", name: "", address: "", email: "" ,phone:"",website:""});

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

  const openModal = (record = null) => {
    if (record) {
      setpublisherRecord(record); 
      setIsEditing(true); 
    } else {
      setpublisherRecord({id:Count, name: "", address: "", email: "" ,phone:"",website:""});
      setIsEditing(false); // Chế độ thêm
    }
    setIsModalOpen(true); // Mở modal
  };

  const handleDelete = async (record) => { // Chuyển tham số record vào ngay trong dấu ngoặc
    // try {
    //     // await DeleteGame(record.gameId); // Gọi hàm DeleteGame với gameId
    //     setDataSource(dataSource.filter((item) => item.gameId !== record.gameId)); // Cập nhật danh sách
    //     console.log(`Xóa game với ID: ${record.gameId} thành công!`);
    // } catch (error) {
    //     console.error("Lỗi khi xóa game:", error); // Xử lý lỗi nếu có
    // }
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

  // return true;
};
  const handleSave = async () => { // Thêm 'async' vào đây
  //   if (!validateGameRecord()) {
  //     return; // Nếu dữ liệu không hợp lệ, dừng lại
  // }
  //   if (isEditing) {
  //       console.log("Lưu dữ liệu đã sửa:", gameRecord);
  //       // Thêm logic lưu dữ liệu đã sửa ở đây
  //   } else {
  //       console.log("Thêm sản phẩm mới:", gameRecord);
  //       const addedGame = await AddGame(gameRecord); // Sử dụng await ở đây
  //       console.log("Added Game:", addedGame); // Kiểm tra dữ liệu vừa thêm
  //   }
  //   setIsModalOpen(false); // Đóng modal sau khi lưu
};


  return (
    <Space className="size_table" size={10} direction="vertical">
    

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
            width:200,
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
                <Button type="primary" onClick={() => openModal()}>Thêm </Button>
                <Button onClick={() => openModal(record)}>Sửa</Button>
                <Button danger onClick={() => handleDelete(record)}>Xóa</Button>
              </Space>
            ),
            className: "text-center",
          },
        ]}
        dataSource={dataSource.map((item) => ({ ...item, key: item.id }))}
        rowKey="PublisherID"

        pagination={{ pageSize: 10 }}
        scroll={{ x: 'max-content' }}
      />

      {/* Modal cho cả Thêm và Sửa */}
      <Modal
        className="form_addedit"
        title={isEditing ? "Sửa thông tin nhà sản xuất" : "Thêm nhà sản xuất mới"}
        open={isModalOpen}
        onCancel={() => setIsModalOpen(false)}
        onOk={handleSave}
      >
        <Input
           placeholder="ID Publisher"
           value={publisherRecord.id}
           onChange={(e) => setpublisherRecord({ ...publisherRecord, id: e.target.value })}
           disabled
        />
        <Input
          placeholder="Name"
          value={publisherRecord.name}
          onChange={(e) => setpublisherRecord({ ...publisherRecord, name: e.target.value })}
        />
        <Input
          placeholder="Phone"
          type="phone"
          value={publisherRecord.phone}
          onChange={(e) => setpublisherRecord({ ...publisherRecord, phone: e.target.value })}
        />
        <Input
          placeholder="Emain"
          type="email"
          value={publisherRecord.email}
          onChange={(e) => setpublisherRecord({ ...publisherRecord, email:e.target.value })}
        />
        <Input
          placeholder="Address"
          value={publisherRecord.address}
          onChange={(e) => setpublisherRecord({ ...publisherRecord, address: e.target.value })}
        />
       
         
        <Input
          placeholder="Website"
          type="string"
          value={publisherRecord.website}
          onChange={(e) => setpublisherRecord({ ...publisherRecord, description: e.target.value })}
        />
        
      </Modal>
    </Space>
  );
}

export default Publisher;  
