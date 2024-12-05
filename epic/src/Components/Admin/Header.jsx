import { Space, Typography, Dropdown, Menu } from "antd";
import "./header.css";
import { useNavigate } from "react-router-dom";
import { MdAccountCircle } from "react-icons/md";
import EpicGamesLogo from '../../assets/EpicGames_Logo.png';
function Header() {
  const navigate = useNavigate();
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
    <img 
  src={EpicGamesLogo} 
  alt="Epic Games Logo" 
  className="logo-image" 
  onClick={() => navigate("/")} 
  style={{ width: '100px', height: 'auto',marginLeft:"90px" }} 
/>
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
