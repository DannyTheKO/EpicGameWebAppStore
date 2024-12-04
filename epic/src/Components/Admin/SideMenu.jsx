import {
  AppstoreOutlined,
  ShopOutlined,
  ShoppingCartOutlined,
  UserOutlined,
} from "@ant-design/icons";

import {
  MdOutlineLogout,
  MdDiscount,
  MdOutlineWarehouse,
  MdOutlineShoppingCart,
} from "react-icons/md";
import { Menu } from "antd";
import { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";

function SideMenu() {
  const location = useLocation();
  const [selectedKeys, setSelectedKeys] = useState("/");

  useEffect(() => {
    const pathName = location.pathname;
    setSelectedKeys(pathName);
  }, [location.pathname]);
  const handleLogout = () => {
    localStorage.clear();
    navigate("/"); 
  };

  const navigate = useNavigate();
  return (
    <div className="SideMenu">
      <Menu
        className="SideMenuVertical"
        mode="vertical"
        onClick={(item) => {

          if (item.key === "/") {
            handleLogout(); 
          } else {
            navigate(item.key); 
          }
        }}
        selectedKeys={[selectedKeys]}
        items={[
          {
            label: "Dashboard",
            icon: <AppstoreOutlined />,
            key: "/admin",
          },
          {
            label: "Game",
            key: "/admin/admingame",
            icon: <ShopOutlined />,
          },
          {
            label: "Acount game",
            key: "/admin/adminaccountgame",
            icon: <ShoppingCartOutlined />,
          },
          {
            label: "Account",
            key: "/admin/adminaccount",
            icon: <UserOutlined />,
          },
          {
            label: "Discount",
            key: "/admin/admindiscount",
            icon: <MdDiscount />,
          },
          {
            label: "Publisher",
            key: "/admin/adminpublisher",
            icon: <MdOutlineWarehouse />,
          },
          {
            label: "Cart",
            key: "/admin/admincard",
            icon: <MdOutlineShoppingCart />,
          },
          {
            label: "Logout",
            key: "/",
            icon: <MdOutlineLogout />,
          },
        ]}
      ></Menu>
    </div>
  );
}
export default SideMenu;
