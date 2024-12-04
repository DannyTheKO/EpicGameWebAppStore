import axios from 'axios';
 const token =localStorage.getItem('authToken');
const apiClient = axios.create({
  baseURL: 'http://localhost:5084', // Địa chỉ URL của ASP.NET Core API
  headers: {
    'Content-Type': 'application/json',
   'Authorization': `Bearer ${token}`
  },
});
export const AddGame = async (game, img) => {
  try {
    const formData = new FormData();
    
    // Thêm dữ liệu game vào FormData
    formData.append('gameFormModel', JSON.stringify(game)); // Chuyển đối tượng game thành chuỗi JSON
    formData.append('imageFile', img); // Thêm tệp hình ảnh vào FormData
    if (img) {
      formData.append('imageFile', img); // Thêm tệp hình ảnh vào FormData
    } else {
      console.error('No image selected!');
      return null;
    }
    

    // In formData ra console để kiểm tra
    console.log('FormData content:');
    formData.forEach((value, key) => {
      console.log(key + ':', value); // In key và value của từng phần tử trong FormData
    });
    const response = await apiClient.post('/Game/CreateGame', formData);

    // Trả về dữ liệu phản hồi
    return response.data;
  } catch (error) {
    console.error("Error adding game:", error.response || error.message);
    return null; // Hoặc bạn có thể xử lý theo cách khác
  }
};

export const AddPublisher = async (Publisher) => {
  try {
    const response = await apiClient.post('/Publisher/createpublisher', Publisher); // Gửi yêu cầu POST đến API
    return response.data; // Trả về dữ liệu phản hồi
} catch (error) {
    console.error("Error adding game:", error.response || error.message);
    return null; // Hoặc bạn có thể xử lý theo cách khác
}
};


export const AddAccountu = async (Account) => {
  try {
    const response = await apiClient.post('/Account/AddAccount', Account); // Gửi yêu cầu POST đến API
    return response.data; // Trả về dữ liệu phản hồi
} catch (error) {
    console.error("Error adding game:", error.response || error.message);
    return null; // Hoặc bạn có thể xử lý theo cách khác
}
};
export const AddAccountgame = async (AccountGame) => {
  try {
    const response = await apiClient.post('/AccountGame/AddAccountgame', AccountGame); // Gửi yêu cầu POST đến API
    return response.data; // Trả về dữ liệu phản hồi
} catch (error) {
    console.error("Error adding game:", error.response || error.message);
    return null; // Hoặc bạn có thể xử lý theo cách khác
}
};
export const AddDiscount = async (Discount) => {
  try {
    console.log(Discount);
    const response = await apiClient.post('/Discount/addDiscount', Discount); // Gửi yêu cầu POST đến API
    return response.data; // Trả về dữ liệu phản hồi
} catch (error) {
    console.error("Error adding game:", error.response || error.message);
    return null; // Hoặc bạn có thể xử lý theo cách khác
}
};
export const GetAllGenre = async () => {
try {
  const response = await apiClient.get('/Genre/GetAllGenre'); // Đường dẫn chính xác đến endpoint
  console.log("API Response:", response.data); // In toàn bộ dữ liệu phản hồi ra console
  return response.data; // Trả về danh sách sản phẩm
} catch (error) {
  console.error("Error :", error.response || error.message);
  return [];
}
};
export const GetAllDiscount =  async () => {
  try {
    const response = await apiClient.get('/Discount/GetAll'); // Đường dẫn chính xác đến endpoint
    console.log("API Response:", response.data); // In toàn bộ dữ liệu phản hồi ra console
    return response.data; // Trả về danh sách sản phẩm
  } catch (error) {
    console.error("Error :", error.response || error.message);
    return [];
  }
};
export const GetAllCart = async() => {
  // Code để lấy tất cả thể loại
  try {
    const response = await apiClient.get('/Cart/GetAll'); // Đường dẫn chính xác đến endpoint
    console.log("API Response:", response.data); // In toàn bộ dữ liệu phản hồi ra console
    return response.data; // Trả về danh sách sản phẩm
  } catch (error) {
    console.error("Error :", error.response || error.message);
    return [];
  }
  };
  
export const GetAllPublisher = async() => {
// Code để lấy tất cả thể loại
try {
  const response = await apiClient.get('/Publisher/GetAll'); // Đường dẫn chính xác đến endpoint
  console.log("API Response:", response.data); // In toàn bộ dữ liệu phản hồi ra console
  return response.data; // Trả về danh sách sản phẩm
} catch (error) {
  console.error("Error :", error.response || error.message);
  return [];
}
};

export const GetAllAccountgame = async() => {
// Code để lấy tất cả thể loại
try {
  const response = await apiClient.get('/AccountGame/GetAllAccountGame'); // Đường dẫn chính xác đến endpoint
  console.log("API Response:", response.data); // In toàn bộ dữ liệu phản hồi ra console
  return response.data; // Trả về danh sách sản phẩm
} catch (error) {
  console.error("Error :", error.response || error.message);
  return [];
}
};
export const GetAllCartdetal = async() => {
  // Code để lấy tất cả thể loại
  try {
    const response = await apiClient.get('/Cartdetail/GetAllCartDetail'); // Đường dẫn chính xác đến endpoint
    console.log("API Response:", response.data); // In toàn bộ dữ liệu phản hồi ra console
    return response.data; // Trả về danh sách sản phẩm
  } catch (error) {
    console.error("Error :", error.response || error.message);
    return [];
  }
  };


export const GetAllUsername = () => {
// Code để lấy tất cả thể loại
};

export const GetAllTitle = () => {
// Code để lấy tất cả thể loại
};

export const GetAccount = async() => {
// Code để lấy tất cả thể loại
try {
  const response = await apiClient.get('/Account/GetAll'); // Đường dẫn chính xác đến endpoint
  console.log("API Response:", response.data); // In toàn bộ dữ liệu phản hồi ra console
  return response.data; // Trả về danh sách sản phẩm
} catch (error) {
  console.error("Error :", error.response || error.message);
  return [];
}
};

export const GetRole = async () => {
  try {
    const response = await apiClient.get('/Role/GetAll'); // Đường dẫn chính xác đến endpoint
    console.log("API Response:", response.data); // In toàn bộ dữ liệu phản hồi ra console
    return response.data; // Trả về danh sách sản phẩm
  } catch (error) {
    console.error("Error :", error.response || error.message);
    return [];
  }
};


export const GetAllgame = async () => {
  try {
    const response = await apiClient.get('/Game/GetAll'); // Đường dẫn chính xác đến endpoint
    console.log("API Response:", response.data); // In toàn bộ dữ liệu phản hồi ra console
    return response.data; // Trả về danh sách sản phẩm
  } catch (error) {
    console.error("Error :", error.response || error.message);
    return [];
  }
};

// API.js
export const DeleteGame = async (gameId) => {
try {
  const response = await apiClient.delete(`/Game/DeleteGame/${gameId}`); // Xóa game dựa trên gameId
  console.log("Xóa thành công gameId:", gameId);
  return response.data; // Trả về dữ liệu phản hồi (có thể là game đã xóa)
} catch (error) {
  console.error("Lỗi khi xóa sản phẩm:", error.response || error.message);
  throw new Error("Xóa không thành công");
}
};

// API.js
export const DeleteAccountgame = async (AccountGameID) => {
  try {
    const response = await apiClient.delete(`/Accountgame/DeleteAccountGame/${AccountGameID}`); // Xóa game dựa trên gameId
    console.log("Xóa thành công AccountgameID:", AccountGameID);
    return response.data; // Trả về dữ liệu phản hồi (có thể là game đã xóa)
  } catch (error) {
    console.error("Lỗi khi xóa sản phẩm:", error.response || error.message);
    throw new Error("Xóa không thành công");
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

export const UpdateGame = async (gameId,updatedGameData) => {
  try {
    // Sử dụng PUT để cập nhật thông tin game
    const response = await apiClient.put(`/Game/UpdateGame/${gameId}`, updatedGameData); 
    return response.data; // Trả về dữ liệu phản hồi sau khi cập nhật
  } catch (error) {
    console.error("Error updating game:", error.response || error.message);
    return null; // Trả về null hoặc xử lý lỗi theo cách khác
  }
};
export const UpdateAccount = async (Accountid,updatedAccount) => {
  try {
    // Sử dụng PUT để cập nhật thông tin game
    const response = await apiClient.put(`/Account/UpdateAccount/${Accountid}`, updatedAccount); 
    return response.data; // Trả về dữ liệu phản hồi sau khi cập nhật
  } catch (error) {
    console.error("Error updating game:", error.response || error.message);
    return null; // Trả về null hoặc xử lý lỗi theo cách khác
  }
};
export const UpdateDiscount = async (DiscountID,UpdateDiscount) => {
  try {
    // Sử dụng PUT để cập nhật thông tin game
    const response = await apiClient.put(`/Discount/UpdateDiscount/${DiscountID}`, UpdateDiscount); 
    return response.data; // Trả về dữ liệu phản hồi sau khi cập nhật
  } catch (error) {
    console.error("Error updating game:", error.response || error.message);
    return null; // Trả về null hoặc xử lý lỗi theo cách khác
  }
};
export const UpdatePublisher = async (publisher,UpdatePublisher) => {
  try {
    // Sử dụng PUT để cập nhật thông tin game
    const response = await apiClient.put(`/Publisher/UpdatePublisher/${publisher}`, UpdatePublisher); 
    return response.data; // Trả về dữ liệu phản hồi sau khi cập nhật
  } catch (error) {
    console.error("Error updating game:", error.response || error.message);
    return null; // Trả về null hoặc xử lý lỗi theo cách khác
  }
};
export const DeleteDiscount = async (DiscountID) => {
  try {
    const response = await apiClient.delete(`/Discount/DeleteDiscount/${DiscountID}`); // Xóa game dựa trên gameId
    console.log("Xóa thành công discount:", DiscountID);
    return response.data; // Trả về dữ liệu phản hồi (có thể là game đã xóa)
  } catch (error) {
    console.error("Lỗi khi xóa điscount:", error.response || error.message);
    throw new Error("Xóa không thành công");
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