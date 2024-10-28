import axios from 'axios';
const apiClient = axios.create({
  baseURL: 'https://localhost:5001/api', // Địa chỉ URL của ASP.NET Core API
  headers: {
    'Content-Type': 'application/json',
  },
});
export const getOrders = () => {
    return fetch("https://dummyjson.com/carts/1").then((res) => res.json());
  };
  
  export const getRevenue = () => {
    return fetch("https://dummyjson.com/carts").then((res) => res.json());
  };
  
  export const getInventory = async () => {
    try {
      const response = await apiClient.get('/products');
      return response.data.products; // Lấy danh sách sản phẩm từ response
    } catch (error) {
      console.error("Error fetching inventory:", error);
      return [];
    }
  };
  
  export const getCustomers = () => {
    return fetch("https://dummyjson.com/users").then((res) => res.json());
  };
  export const getComments = () => {
    return fetch("https://dummyjson.com/comments").then((res) => res.json());
  };
  