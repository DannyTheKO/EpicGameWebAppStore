import { Space, Typography, Dropdown, Menu } from "antd";
import "./header.css";
import { MdAccountCircle } from "react-icons/md";

function Header() {
  const handleLogout = () => {
    localStorage.removeItem('authToken');
 
    window.location.href = "/"; // Ví dụ chuyển hướng về trang chủ sau khi đăng xuất
  };
  const menu = (
    <Menu>
      <Menu.Item>
        <a href="thông tin cá nhân ">Thông tin tài khoản</a>
      </Menu.Item>
      <Menu.Item onClick={handleLogout}>
        
        <a href="/">Đăng xuất</a>
      </Menu.Item>
    </Menu>
  );
  return (
    <div className="AppHeader">
      <img className="logoad" src="../Asset/logo.png"></img>
      <Typography.Title>EPIC GAMING</Typography.Title>
      <Space>
        <Dropdown overlay={menu} trigger={["hover"]}>
          <MdAccountCircle className="client" />
        </Dropdown>
      </Space>
    </div>
  );
  
}
export default Header;
