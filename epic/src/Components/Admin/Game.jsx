  import { Button, Rate, Space, Table, Typography, Modal, Input,Select } from "antd";
  import { useEffect, useState } from "react";
  import { GetAllgame,AddGame,DeleteGame,GetAllPublisher,GetAllGenre} from "./API";
  import "./table.css";

  const { Text } = Typography;
  const { Option } = Select;
  function Game() {

    const [loading, setLoading] = useState(false);
    const [dataSource, setDataSource] = useState([]);
    const [dataNameGenre, setNameGenre] = useState([]);
    const [dataNamePublisher, setNamePublisher] = useState([]);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [isEditing, setIsEditing] = useState(false);
    const [gameCount, setGameCount] = useState(0); 
    const [gameRecord, setGameRecord] = useState({id:"", title: "", author: "", price: 0 ,rating:0,release:null ,description:"",  namegenre:"",namepublisher:""});

    useEffect(() => {
      const fetchGame = async () => {
        setLoading(true);
        try {
          const [res, publisher, genre]=await Promise.all([
            GetAllgame(),
            GetAllPublisher(),
            GetAllGenre()
          ]);
          setDataSource(res || []);
          setNamePublisher(publisher || []);
          setNameGenre(genre || []);
          setGameCount(res.length); 
        } catch (error) {
          console.log("lỗi load data")
        }
        setLoading(false);
      };
      fetchGame();
    }, []);

    const openModal = (record = null) => {
      if (record) {
        record.release = record.release.split("T")[0];
        setGameRecord(record); 
        setIsEditing(true); 
      } else {
        setGameRecord({id :gameCount, title: "", author: "", price: 0,rating:0,description:"" }); 
        setIsEditing(false); 
      }
      setIsModalOpen(true);
    };

    const handleDelete = (record) => {
      Modal.confirm({
        title: "Bạn có chắc chắn muốn xóa sản phẩm này không?",
        content: "Hành động này không thể hoàn tác",
        okText: "Xóa",
        okType: "danger",
        cancelText: "Hủy",
        onOk: () => {
          const gameId = parseInt(record.gameId, 10);
          if (isNaN(gameId)) {
            console.error("Id  game không phải là số hợp lệ:", record.gameId);
            return;
          }
          DeleteGame(parseInt(record.gameId)) 
            .then(() => {
              setDataSource((prevDataSource) =>
                prevDataSource.filter((item) => item.gameId !== record.gameId)
              );
              console.log("Đã xóa sản phẩm: ", record.gameId);
            })
            .catch((error) => {
              console.error("Lỗi khi xóa sản phẩm:", error);
            });
        },
      });
    };
    
  const validateGameRecord = () => {
    const { title, author, price, rating, release, description } = gameRecord;

    // Kiểm tra các trường bắt buộc
    if (!title || !author || price <= 0 || rating < 0 || rating > 10 || !release || !description) {
        Modal.error({
            title: 'Lỗi',
            content: 'Vui lòng điền đầy đủ thông tin hợp lệ cho tất cả các trường.',
        });
        return false;
    }

    return true;
};
    const handleSave = async () => { 
      if (!validateGameRecord()) {
        return;
    }
      if (isEditing) {
          console.log("Lưu dữ liệu đã sửa:", gameRecord);
          // Thêm logic lưu dữ liệu đã sửa ở đây
      } else {
          console.log("Thêm sản phẩm mới:", gameRecord);
          const addedGame = await AddGame(gameRecord); // thêm mới game
          console.log("Added Game:", addedGame);
      }
      setIsModalOpen(false); 
  };
      return (
      <Space className="size_table" size={10} direction="vertical">
        <Table
          className="data"
          loading={loading}
          columns={[
            {
              title: "Game ID",
              dataIndex: "gameId",
              key: "gameId",
              render: (gameId) => <Text>{gameId}</Text>,
              
            },
            {
              title: "Publisher",
              dataIndex: "publisher",
              key: "publisher",
              render: (publisher) => <Text>{publisher}</Text>,
              
            },
            {
              title: "Genre",
              dataIndex: "genre",
              key: "genre",
              render: (genre) => <Text>{genre}</Text>,
              
            },
            {
              title: "Title",
              dataIndex: "title",
              key: "title",
              render: (title) => <Text>{title}</Text>,
              className: "text-center",
            },
            {
              title: "Author",
              dataIndex: "author",
              key: "author",
              className: "text-center",
            },
            {
              title: "Price",
              dataIndex: "price",
              key: "price",
              render: (price) => `$${price.toFixed(2)}`,
              className: "text-center",
            },
            {
              title: "Rating",
              dataIndex: "rating",
              key: "rating",
              render: (rating) => <Rate value={rating / 2} allowHalf disabled />,
              className: "text-center",
            },
            {
              title: "Release Date",
              dataIndex: "release",
              key: "release",
              render: (release) => new Date(release).toLocaleDateString(),
              className: "text-center",
            },
            {
              title: "Description",
              dataIndex: "description",
              key: "description",
              render: (description) => (
                <Text ellipsis style={{ maxWidth: 200 }}>
                  {description}
                </Text>
              ),
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
          rowKey="gameId"

          pagination={{ pageSize: 10 }}
          scroll={{ x: 'max-content' }}
        />
        <Modal
          className="form_addedit"
          title={isEditing ? "Sửa thông tin game" : "Thêm Game mới"}
          open={isModalOpen}
          onCancel={() => setIsModalOpen(false)}
          onOk={handleSave}
        >
          <Input
            placeholder="ID Game"
            value={gameRecord.id}
            onChange={(e) => setGameRecord({ ...gameRecord, id: e.target.value })}
            disabled
          />
          <Input
            placeholder="Title"
            value={gameRecord.title}
            onChange={(e) => setGameRecord({ ...gameRecord, title: e.target.value })}
          />
          <Input
            placeholder="Author"
            value={gameRecord.author}
            onChange={(e) => setGameRecord({ ...gameRecord, author: e.target.value })}
          />
          <Input
            placeholder="Price"
            type="number"
            value={gameRecord.price}
            onChange={(e) => setGameRecord({ ...gameRecord, price: parseFloat(e.target.value) })}
          />
          <Input
            placeholder="Rating"
            value={gameRecord.rating}
            onChange={(e) => setGameRecord({ ...gameRecord, rating: e.target.value })}
          />
          <Input
            type="date"
            placeholder="Release Date"
            value={gameRecord.release} 
            onChange={(e) => setGameRecord({ ...gameRecord, release: e.target.value })} // Cập nhật giá trị
            style={{ width: "100%", height: "52px", marginTop: "20px" }}
          />
           
          <Input
            placeholder="Description"
            type="string"
            value={gameRecord.description}
            onChange={(e) => setGameRecord({ ...gameRecord, description: e.target.value })}
          />
          <Select
          placeholder="Chọn nhà sản xuất"
          value={gameRecord.namepublisher}
          onChange={(value) => setGameRecord({ ...gameRecord, namepublisher: value })}
          style={{ width: "100%", marginTop: "20px",height:"47px"  }}
        >
          {dataNamePublisher.map((publisher)=>(
            <Option key ={publisher.id} value={publisher.name}>
              {publisher.name}
            </Option>
          ))}
          
        </Select>
        <Select
          placeholder="Chọn thể loại"
          value={gameRecord.namegenre}
          onChange={(value) => setGameRecord({ ...gameRecord, namegenre: value })}
          style={{ width: "100%", marginTop: "20px",height:"47px"  }}
        >
          {dataNameGenre.map((genre)=>(
            <Option key ={genre.id} value={genre.name}>
              {genre.name}
            </Option>
          ))}
          
        </Select>
        </Modal>
      </Space>
    );
  }

  export default Game;  
