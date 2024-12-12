  import {
    DollarCircleOutlined,
    ShoppingCartOutlined,
    ShoppingOutlined,
    UserOutlined,
  } from "@ant-design/icons";
  import { Card, Space, Statistic } from "antd";
  import { useEffect, useState } from "react";
  import {  GetAllgame,GetAllCartdetal,GetAccount, getOrders, getRevenue, GetAllCart } from "./API";
  import "./dash.css";

  import {
    Chart as ChartJS,
    CategoryScale,
    LinearScale,
    BarElement,
    Title,
    Tooltip,
    Legend,
  } from "chart.js";
  import { Bar } from "react-chartjs-2";

  ChartJS.register(
    CategoryScale,
    LinearScale,
    BarElement,
    Title,
    Tooltip,
    Legend
  );

  function Dashboard() {
    const [orders, setOrders] = useState(0);
    const [game, setGame] = useState(0);
    const [customers, setCustomers] = useState(0);
    const [revenue, setRevenue] = useState(0);

    useEffect(() => {
      GetAllCartdetal().then((res) => {
        setOrders(res.length);
        const total = res.reduce((sum, item) => sum + item.price, 0);
        setRevenue(total);
      });
    
      GetAllgame().then((res) => {
        setGame(res.length);
        
      });
      GetAccount().then((res) => {
        setCustomers(res.length);
      });
    }, []);

    return (
      <Space size={5} direction="vertical">
        
        <Space direction="horizontal" style={{ margin: "50px" }}>
          <DashboardCard
            icon={
              <ShoppingCartOutlined
                style={{
                  color: "green",
                  backgroundColor: "rgba(0,255,0,0.25)",
                  borderRadius: 20,
                  fontSize: 24,
                  padding: 8,
                }}
              />
            }
            title={"Orders"}
            value={orders}
          />
          <DashboardCard
            icon={
              <ShoppingOutlined
                style={{
                  color: "blue",
                  backgroundColor: "rgba(0,0,255,0.25)",
                  borderRadius: 20,
                  fontSize: 24,
                  padding: 8,
                }}
              />
            }
            title={"Game"}
            value={game}
          />
          <DashboardCard
            icon={
              <UserOutlined
                style={{
                  color: "purple",
                  backgroundColor: "rgba(0,255,255,0.25)",
                  borderRadius: 20,
                  fontSize: 24,
                  padding: 8,
                }}
              />
            }
            title={"Customer"}
            value={customers}
          />
          <DashboardCard
            icon={
              <DollarCircleOutlined
                style={{
                  color: "red",
                  backgroundColor: "rgba(255,0,0,0.25)",
                  borderRadius: 20,
                  fontSize: 24,
                  padding: 8,
                }}
              />
            }
            title={"Revenue"}
            value={revenue}
          />
        </Space>
        <Space>
          {/* <RecentOrders /> */}
          <DashboardChart />
        </Space>
      </Space>
    );
  }

  function DashboardCard({ title, value, icon }) {
    return (
      <Card className="card_component">
        <Space direction="horizontal">
          {icon}
          <Statistic title={title} value={value} />
        </Space>
      </Card>
    );
  }
  function DashboardChart() {
    const [reveneuData, setReveneuData] = useState({
      labels: [],
      datasets: [],
    });
    useEffect(() => {
      GetAllCart().then((res) => {
        const labels = res.map((user) => `User-${user.accountId}`);
        const completedCarts = res.map((user) =>
          user.cart.filter(cart => cart.cartStatus === "Completed").length);    
      setReveneuData({
        labels, 
        datasets: [
          {
            label: "Number of Carts",
            data: completedCarts, 
            backgroundColor: "red", 
            barThickness:80, 
          },
        ],
      });
    });
  }, []);

  const options = {
    responsive: true,
    plugins: {
      legend: {
        position: "bottom",
      },
      title: {
        display: true,
        text: "Cart Status Distribution", 
      },
    },
  };

    return (
      <Card style={{ width: 1200, height: "500px", marginLeft: 200 }}>
        <Bar style={{ width: 1200, height: "500px", marginLeft: 100 }} options={options} data={reveneuData} />
      </Card>
    );
  }

  export default Dashboard;
