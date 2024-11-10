  import {
      DollarCircleOutlined,
      ShoppingCartOutlined,
      ShoppingOutlined,
      UserOutlined,
    } from "@ant-design/icons";
    import { Card, Space, Statistic, Table, Typography } from "antd";
    import { useEffect, useState } from "react";
    import { getCustomers, GetAllgame, getOrders, getRevenue } from "./API";
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
        getOrders().then((res) => {
          setOrders(res.total);
          setRevenue(res.discountedTotal);
        });
        GetAllgame().then((res) => {
          setGame(res.length);
          console.log(res.total);
        });
        getCustomers().then((res) => {
          setCustomers(res.total);
        });
      }, []);
    
      return (
        <Space size={5} direction="vertical">
          <Space direction="horizontal">
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
            <RecentOrders />
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
    function RecentOrders() {
      const [dataSource, setDataSource] = useState([]);
      const [loading, setLoading] = useState(false);
    
      useEffect(() => {
        setLoading(true);
        getOrders().then((res) => {
          setDataSource(res.products);
          setLoading(false);
        });
      }, []);
    
      // return (
      //   <>
      //     <Typography.Text>Recent Orders</Typography.Text>
      //     <Table className="table_recentorder"
      //       columns={[
      //         {
      //           title: "Title",
      //           dataIndex: "title",
      //         },
      //         {
      //           title: "Quantity",
      //           dataIndex: "quantity",
      //         },
      //         {
      //           title: "Price",
      //           dataIndex: "discountedPrice",
      //         },
      //       ]}
      //       loading={loading}
      //       dataSource={dataSource}
      //       pagination={false}
      //     ></Table>
      //   </>
      // );
    }function DashboardChart() {
      const [reveneuData, setReveneuData] = useState({
        labels: [],
        datasets: [],
      });
      const [tableData, setTableData] = useState([]); // State để lưu dữ liệu bảng
    
      useEffect(() => {
        getRevenue().then((res) => {
          const labels = res.carts.map((cart) => {
            return `User-${cart.userId}`;
          });
          const data = res.carts.map((cart) => {
            return cart.discountedTotal;
          });
    
          const dataSource = {
            labels,
            datasets: [
              {
                label: "Revenue",
                data: data,
                backgroundColor: "rgba(255, 0, 0, 1)",
              },
            ],
          };
    
          setReveneuData(dataSource);
    
      
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
            text: "Order Revenue",
          },
        },
      };
    
     
      return (
        <Card style={{ width: 1200, height: 'auto' }}>
          <Bar options={options} data={reveneuData} />
          
            
        </Card>
      );
    }
    
    export default Dashboard;
    