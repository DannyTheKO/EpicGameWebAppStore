import React, { useState, useEffect } from 'react';
import { Table } from 'antd';
import { GetAllCart } from './API';

const AccountTable = () => {
  const [data, setData] = useState([]);

  // Lấy dữ liệu từ hàm GetAllCart
  useEffect(() => {
    GetAllCart()
      .then(responseData => {
        setData(responseData);
      })
      .catch(error => {
        console.error('Có lỗi khi lấy dữ liệu:', error);
      });
  }, []); // Chạy khi component mount

  // Columns cho bảng Account
  const accountColumns = [
    {
      title: 'Account Name',
      dataIndex: 'name',
      key: 'name',
    },
    {
      title: 'Account ID',
      dataIndex: 'accountId',
      key: 'accountId',
    },
  ];

  // Expanded row renderer cho Cart
  const expandedRowRenderCart = (record) => {
    return (
      <Table
        columns={[
          { title: 'Cart ID', dataIndex: 'cartId', key: 'cartId' },
          { title: 'Payment Method ID', dataIndex: 'paymentMethodId', key: 'paymentMethodId' },
        ]}
        dataSource={record.cart.map(cart => ({
          key: cart.cartId,
          cartId: cart.cartId,
          paymentMethodId: cart.paymentMethodId,
        }))}
        pagination={false}
        expandedRowRender={(cartRecord) => expandedRowRenderCartDetails(cartRecord)}
      />
    );
  };

  // Expanded row renderer cho Cart Details
  const expandedRowRenderCartDetails = (cartRecord) => {
    return (
      <Table
        columns={[
          { title: 'Game Title', dataIndex: 'title', key: 'title' },
          { title: 'Price', dataIndex: 'price', key: 'price' },
          { title: 'Discount', dataIndex: 'discount', key: 'discount' },
        ]}
        dataSource={cartRecord.map(item => ({
          key: item.cartDetailId,
          title: item.cartDetail.title,
          price: item.cartDetail.price,
          discount: item.cartDetail.discount,
        }))}
        pagination={false}
      />
    );
  };

  return (
    <Table
      columns={accountColumns}
      dataSource={data.map(item => ({ ...item, key: item.accountId }))}
      expandedRowRender={expandedRowRenderCart}
      pagination={false}
    />
  );
};

export default AccountTable;
