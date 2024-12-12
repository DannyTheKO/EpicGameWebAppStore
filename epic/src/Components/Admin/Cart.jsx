import React, { useState, useEffect } from 'react';
import { Table } from 'antd';
import { GetAllCart } from './API'; 
const accountColumns = [
  { title: 'Account Name', dataIndex: 'name', key: 'name' },
  { title: 'Account ID', dataIndex: 'accountId', key: 'accountId' },
];

const cartColumns = [
  { title: 'Cart ID', dataIndex: 'cartId', key: 'cartId' },
  { title: 'Payment Method', dataIndex: 'paymentMethod', key: 'paymentMethod' },
  { title: 'Create On', dataIndex: 'createOn', key: 'createOn' },
  { title: 'Status', dataIndex: 'cartStatus', key: 'cartStatus' },

];
const cartDetailColumns = [
  { title: 'Game Title', dataIndex: ['cartDetail', 'title'], key: 'title' },
  { title: 'Price', dataIndex: ['cartDetail', 'price'], key: 'price' },
  { title: 'Discount', dataIndex: ['cartDetail', 'discount'], key: 'discount' },
  
];

const AccountTable = () => {
  const [accounts, setAccounts] = useState([]);

  useEffect(() => {
    GetAllCart()
      .then(setAccounts)
      .catch((error) => console.error('Error fetching data:', error));
  }, []);

  return (
    <Table
      columns={accountColumns}
      dataSource={accounts.map((account) => ({
        key: account.accountId,
        ...account,
      }))}
      scroll={{ y :700}}
      pagination={false}
      expandedRowRender={(account) => (
        <CartTable cartData={account.cart} />
      )}
    />
  );
};

const CartTable = ({ cartData }) => (
  <Table
    columns={cartColumns}
    dataSource={cartData.map((cart) => ({
      key: cart.cartId,
      ...cart,
    }))}
    pagination={false}
    expandedRowRender={(cart) => (
      <CartDetailTable cartDetails={cart.cartDetails} />
    )}
  />
);

const CartDetailTable = ({ cartDetails }) => (
  <Table
    columns={cartDetailColumns}
    dataSource={cartDetails.map((detail) => ({
      key: detail.cartDetailId,
      ...detail,
    }))}
    pagination={false}
  />
);

export default AccountTable;
