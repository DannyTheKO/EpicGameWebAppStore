import { Button ,Avatar, Rate, Space, Table, Typography } from "antd";
import { useEffect, useState } from "react";
import { getInventory } from "./API";
import "./table.css";

function Inventory() {
  const [loading, setLoading] = useState(false);
  const [dataSource, setDataSource] = useState([]);

  useEffect(() => {
    setLoading(true);
    getInventory().then((res) => {
      setDataSource(res.products);
      setLoading(false);
    });
  }, []);
  const handleAdd = (record) => {
    console.log("Thêm sản phẩm: ", record);
    // Thêm logic thêm sản phẩm ở đây
  };

  const handleEdit = (record) => {
    console.log("Sửa sản phẩm: ", record);
    // Thêm logic sửa sản phẩm ở đây
  };

  const handleDelete = (record) => {
    console.log("Xóa sản phẩm: ", record);
    // Thêm logic xóa sản phẩm ở đây
  };
  return (
    <Space className="size_table" size={20} direction="vertical">
    
      <Table
        className="data"
        loading={loading}
        columns={[
          {
            title: "Thumbnail",
            dataIndex: "thumbnail",
            render: (link) => {
              return <Avatar src={link} />;
            },
            className: "text-center",
          },
          {
            title: "Title",
            dataIndex: "title",
            className: "text-center",
          },
          {
            title: "Price",
            dataIndex: "price",
            render: (value) => <span>${value}</span>,
            className: "text-center",
          },
          {
            title: "Rating",
            dataIndex: "rating",
            render: (rating) => {
              return <Rate value={rating} allowHalf disabled />;
            },
            className: "text-center",
          },
          {
            title: "Stock",
            dataIndex: "stock",
            className: "text-center",
          },

          {
            title: "Brand",
            dataIndex: "brand",
            className: "text-center",
          },
          {
            title: "Category",
            dataIndex: "category",
            className: "text-center",
          },
          {
            title: "Actions", // Cột chứa các nút
            render: (record) => {
              return (
                <Space size="middle">
                  <Button onClick={() => handleAdd(record)}>Thêm</Button>
                  <Button onClick={() => handleEdit(record)}>Sửa</Button>
                  <Button danger onClick={() => handleDelete(record)}>Xóa</Button>
                </Space>
              );
            },
          },
        ]}
        dataSource={dataSource}
        pagination={{
          pageSize: 10,
        }}
      ></Table>
    </Space>
  );
}
export default Inventory;
