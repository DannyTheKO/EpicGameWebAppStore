import React, { useState, useEffect } from 'react';
import { Table } from 'antd';

import { GetAllCart } from './API';

// Cấu trúc bảng con: Cart
const CartTable = ({ cartData }) => {
  const cartColumns = [
    { title: 'Cart ID', dataIndex: 'cartId', key: 'cartId' },
    { title: 'Payment Method', dataIndex: 'paymentMethodId', key: 'paymentMethodId' },
  ];

  return (
    <Table
      columns={cartColumns}
      dataSource={cartData.map(item => ({
        key: item.cartId,
        cartId: item.cartId,
        paymentMethodId: item.paymentMethodId,
      }))}
      pagination={false}
      expandedRowRender={(cartRecord) => <CartDetailTable cartDetails={cartRecord.cartDetails} />}
    />
  );
};

// Cấu trúc bảng con: Cart Details
const CartDetailTable = ({ cartDetails }) => {
  const cartDetailColumns = [
    { title: 'Game Title', dataIndex: 'title', key: 'title' },
    { title: 'Price', dataIndex: 'price', key: 'price' },
    { title: 'Discount', dataIndex: 'discount', key: 'discount' },
  ];

  return (
    <Table
      columns={cartDetailColumns}
      dataSource={cartDetails.map(item => ({
        key: item.cartDetailId,
        title: item.cartDetail.title,
        price: item.cartDetail.price,
        discount: item.cartDetail.discount,
      }))}
      pagination={false}
    />
  );
};

const AccountTable = () => {
  const [accounts, setAccounts] = useState([]);

  // Lấy dữ liệu từ API khi component mount
  useEffect(() => {
    GetAllCart()
      .then(responseData => {
        setAccounts(responseData);
      })
      .catch(error => {
        console.error('Có lỗi khi lấy dữ liệu:', error);
      });
  }, []);

  // Các cột của bảng Account
  const accountColumns = [
    { title: 'Account Name', dataIndex: 'name', key: 'name' },
    { title: 'Account ID', dataIndex: 'accountId', key: 'accountId' },
  ];

  return (
    <Table
      columns={accountColumns}
      dataSource={accounts.map(account => ({
        key: account.accountId,
        name: account.name,
        accountId: account.accountId,
        cart: account.cart, // Chuyển qua dữ liệu Cart
      }))}
      expandedRowRender={(record) => <CartTable cartData={record.cart} />}
      pagination={false}
    />
  );
};

export default AccountTable;
