import {
    AppstoreOutlined,
    ShopOutlined,
    ShoppingCartOutlined,
    UserOutlined,
  } from "@ant-design/icons";
  import { MdOutlineLogout ,MdDiscount, MdOutlineWarehouse  } from "react-icons/md";
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
  
    const navigate = useNavigate();
    return (
      <div className="SideMenu">
        <Menu
          className="SideMenuVertical"
          mode="vertical"
          onClick={(item) => {
            //item.key
            navigate(item.key);
          }}
          selectedKeys={[selectedKeys]}
          items={[
            {
              label: "Dashboard",
              icon: <AppstoreOutlined />,
              key: "/",
            },
            {
              label: "Game",
              key: "/game",
              icon: <ShopOutlined />,
            },
            {
              label: "Acount game",
              key: "/accountgame",
              icon: <ShoppingCartOutlined />,
            },
            {
              label: "Account",
              key: "/account",
              icon: <UserOutlined />,
            },
            {
              label: "Discount",
              key: "/discount",
              icon: <MdDiscount />,
            },
            {
              label: "Publisher",
              key: "/publisher",
              icon: <MdOutlineWarehouse />,
            },
            {
              label: "Logout",
              key: "/index",
              icon: <MdOutlineLogout />,
            },
          ]}
        ></Menu>
      </div>
    );
  }
  export default SideMenu;
  