import React from 'react'

import {  Route, Routes } from "react-router-dom";
import Customers from "./Customers";
import Dash from "./Dashboard";
import Admin from './Admin';
import Inventory from "./Inventory";
import Orders from "./Oder";

const Dashboarh=()=> {
  return (
    <Route path="/" element={<Admin />}>
    <Route index element={<Inventory />} /> {/* Mặc định sẽ dẫn đến Inventory */}
    <Route path="inventory" element={<Inventory />} />
    <Route path="orders" element={<Orders />} />
    <Route path="customers" element={<Customers />} />
  </Route>
  )
}
export default Dashboarh



