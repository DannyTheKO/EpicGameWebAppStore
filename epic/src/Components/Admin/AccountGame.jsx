import {Button , Space, Table } from "antd";
import { useEffect, useState } from "react";
import {  getOrders } from "./API";
import "./table.css";

function Orders() {
  const [loading, setLoading] = useState(false);
  const [dataSource, setDataSource] = useState([]);
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
  useEffect(() => {
    setLoading(true);
    getOrders().then((res) => {
      setDataSource(res.products);
      setLoading(false);
    });
  }, []);

  return (
    <Space className="size_table" size={20} direction="vertical">

      <Table
        className="data"
        loading={loading}
        columns={[
          {
            title: "ID Account",
            dataIndex: "AccountID",
          },
          {
            title: "ID Game",
            dataIndex: "GaneID",
           
          },
          {
            title: "Date Addrd",
            dataIndex: "DateAdded",
            
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
export default Orders;