import { Button ,Avatar, Rate, Space, Table, Typography } from "antd";
import { useEffect, useState } from "react";
import { getCustomers, getInventory } from "./API";
import "./table.css";
function Customers() {
  const [loading, setLoading] = useState(false);
  const [dataSource, setDataSource] = useState([]);

  useEffect(() => {
    setLoading(true);
    getCustomers().then((res) => {
      setDataSource(res.users);
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
            title: "Photo",
            dataIndex: "image",
            render: (link) => {
              return <Avatar src={link} />;
            },
          },
          {
            title: "First Name",
            dataIndex: "firstName",
          },
          {
            title: "LastName",
            dataIndex: "lastName",
          },
          {
            title: "Email",
            dataIndex: "email",
          },
          {
            title: "Phone",
            dataIndex: "phone",
          },

          {
            title: "address",
            dataIndex: "address",
            render: (address) => {
              return (
                <span>
                  {address.address}, {address.city}
                </span>
              );
            },
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
export default Customers;
