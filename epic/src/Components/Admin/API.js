  import axios from 'axios';

  const apiClient = axios.create({
    baseURL: 'http://localhost:5084/api/Game', // Địa chỉ URL của ASP.NET Core API
    headers: {
      'Content-Type': 'application/json',
    },
  });
  export const AddGame = async (game) => {
    try {
      const response = await apiClient.post('/', game); // Gửi yêu cầu POST đến API
      return response.data; // Trả về dữ liệu phản hồi
  } catch (error) {
      console.error("Error adding game:", error.response || error.message);
      return null; // Hoặc bạn có thể xử lý theo cách khác
  }
};

  export const GetAllgame = async () => {
    try {
      const response = await apiClient.get('/'); // Đường dẫn chính xác đến endpoint
      console.log("API Response:", response.data); // In toàn bộ dữ liệu phản hồi ra console
      return response.data; // Trả về danh sách sản phẩm
    } catch (error) {
      console.error("Error :", error.response || error.message);
      return [];
    }
  };
  // API.js
export const Deletegame = async (gameId) => {
  try {
      const response = await apiClient.delete(`/${gameId}`); // Điều chỉnh theo endpoint API của bạn
      return response.data; // Trả về dữ liệu từ phản hồi
  } catch (error) {
      console.error("Lỗi khi xóa game:", error);
      throw error; // Ném lỗi để xử lý ở nơi gọi hàm
  }
};

  



  export const getCustomers = async () => {
    try {
      const response = await fetch("https://dummyjson.com/users");
      return await response.json();
    } catch (error) {
      console.error("Error fetching customers:", error);
      return null;
    }
  };

  export const getComments = async () => {
    try {
      const response = await fetch("https://dummyjson.com/comments");
      return await response.json();
    } catch (error) {
      console.error("Error fetching comments:", error);
      return null;
    }
  };
  export const getOrders = async () => {
    try {
      const response = await fetch("https://dummyjson.com/carts/1");
      return await response.json();
    } catch (error) {
      console.error("Error fetching orders:", error);
      return null; // Hoặc return [] nếu bạn muốn trả về mảng rỗng
    }
  };

  export const getRevenue = async () => {
    try {
      const response = await fetch("https://dummyjson.com/carts");
      return await response.json();
    } catch (error) {
      console.error("Error fetching revenue:", error);
      return null;
    }
  };