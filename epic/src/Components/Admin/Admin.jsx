import AppHeader from "./Header";
import PageContent from "./PageContent";
import SideMenu from "./SideMenu";
import "./managa.css";

function Admin() {
  return (
    <div className="App">
      <AppHeader />
      <div className="SideMenuAndPageContent">
        <SideMenu/>
        <PageContent/>
      </div>
    </div>
  );
}
export default Admin;

