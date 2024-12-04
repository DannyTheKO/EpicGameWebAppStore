import { Space, Typography, Dropdown, Menu } from "antd";
import "./header.css";
import { MdAccountCircle } from "react-icons/md";

function Header() {
  
  const menu = (
    <Menu>
      <Menu.Item>
        <a href="/userprofile">Thông tin tài khoản</a>
      </Menu.Item>
      <Menu.Item>
        <a href="/">Chuyển đến trang user</a>
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
