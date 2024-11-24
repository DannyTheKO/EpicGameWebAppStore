  import { Button,Space, Table, Typography, Modal, Input,Select } from "antd";
  import { useEffect, useState } from "react";
  import { GetAllPublisher,UpdatePublisher } from "./API";
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
        setIsEditing(true); // Chế độ sửa
      } else {
        setpublisherRecord({ id: Count + 1, name: "", address: "", email: "", phone: "", website: "" });
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
  const handleSave = async () => {
    // if (!validateGameRecord()) {
    //   console.log("error");
    //   return; // Nếu dữ liệu không hợp lệ, dừng lại
    // }

    if (isEditing) {
      console.log(publisherRecord.publisherId, publisherRecord);

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
        console.error('Update failed', error);
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

        // Hiển thị thông báo thành công
        Modal.success({
          title: "Success",
          // content: `Game ID ${addedGame.id} đã được thêm mới thành công.`,
        });
      } catch (error) {
        console.error('Add failed', error);
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
                  
                </Space>
              ),
              className: "text-center",
            },
          ]}
          dataSource={dataSource.map((item) => ({ ...item, key: item.id }))}
          rowKey="PublisherID"

          pagination={{ pageSize: 5,position: [ "bottomCenter"], }}
          scroll={{ x: "max-content" }}
        />

        {/* Modal cho cả Thêm và Sửa */}
        <Modal
          className="form_addedit"
          title={isEditing ? "Sửa thông tin nhà sản xuất" : "Thêm nhà sản xuất mới"}
          open={isModalOpen}
          onCancel={() => setIsModalOpen(false)}
          onOk={handleSave}
        >
          <label>Publisher ID</label>
          <Input
            placeholder="ID Publisher"
            value={publisherRecord.publisherId||publisherRecord.id}
            onChange={(e) => setpublisherRecord({ ...publisherRecord, id: e.target.value })}
            disabled
          />
          <label>Name</label>
          <Input
            placeholder="Name"
            value={publisherRecord.name}
            onChange={(e) => setpublisherRecord({ ...publisherRecord, name: e.target.value })}
          />
          <label>Phone</label>
          <Input
            placeholder="Phone"
            type="phone"
            value={publisherRecord.phone}
            onChange={(e) => setpublisherRecord({ ...publisherRecord, phone: e.target.value })}
          />
          <label>Email</label>
          <Input
            placeholder="Email"
            type="email"
            value={publisherRecord.email}
            onChange={(e) => setpublisherRecord({ ...publisherRecord, email:e.target.value })}
          />
          <label>Address</label>
          <Input
            placeholder="Address"
            value={publisherRecord.address}
            onChange={(e) => setpublisherRecord({ ...publisherRecord, address: e.target.value })}
          />
          <label>Website</label>
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
