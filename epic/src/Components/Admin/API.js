import axios from 'axios';
 const token =localStorage.getItem('authToken');
const apiClient = axios.create({
  baseURL: 'http://localhost:5084',
  headers: {
    'Content-Type': 'application/json',
  'Authorization': `Bearer ${token}`

  },
});
export const AddPublisher = async (Publisher) => {
  try {
    const response = await apiClient.post('/Store/Dashboard/Publisher/Create', Publisher); 
    return response.data;
} catch (error) {
    console.error("Error adding game:", error.response || error.message);
    return null; 
}
};
export const AddAccountu = async (Account) => {
  try {
    const response = await apiClient.post('/Store/Dashboard/Account/Add', Account);

    if (response.status === 200) {
      console.log("API Success:", response.data);
      return response.data;
    }
  } catch (error) {
    if (error.response) {
      if (error.response.status === 400) {
        console.error("API Error:", error.response.data.message);
        return {
          success: false,
          message: error.response.data.message 
        };
        }
    }
  }
};

export const AddAccountgame = async (AccountGame) => {
  try {
    const response = await apiClient.post('/Store/Dashboard/AccountGame/Add', AccountGame);

    if (response.status === 200) {
      return {
        success: true,
        message: response.data.message || "Thêm tài khoản game thành công.",
      };
    } else if (response.status === 400) {
      return {
        success: false,
        message: response.data.message || "Dữ liệu không hợp lệ, vui lòng kiểm tra lại.",
      };
    }
  } catch (error) {
    console.error("Error adding account game:", error.response || error.message);
    return {
      success: false,
      message: error.response?.data?.message || "Có lỗi xảy ra trong quá trình thêm tài khoản game.",
    };
  }
};

export const AddDiscount = async (Discount) => {
  try {
    const response = await apiClient.post('/Store/Dashboard/Discount/Add', Discount);
    if (response.status === 200) {
      console.log("API Success:", response.data);
      return response.data;
    }
  } catch (error) {
    if (error.response) {
      if (error.response.status === 400) {
        console.error("API Error:", error.response.data.message);
        return {
          success: false,
          message: error.response.data.message || "Dữ liệu không hợp lệ, vui lòng kiểm tra lại."
        };
      }
      if (error.response.status === 500) {
        console.error("Server Error:", error.response.data.message);
        return {
          success: false,
          message: "Đã xảy ra lỗi từ phía máy chủ. Vui lòng thử lại sau."
        };
      }
    }
    console.error("Unexpected Error:", error.message);
    return {
      success: false,
      message: error.message || "Có lỗi xảy ra. Vui lòng kiểm tra lại kết nối."
    };
  }
};


export const GetAllGenre = async () => {
try {
  const response = await apiClient.get('/Genre/GetAllGenre'); 
  return response.data;
} catch (error) {
  console.error("Error :", error.response || error.message);
  return [];
}
};
export const GetIMGbygameid = async (gameif) => {
  try {
    const response = await apiClient.get(`/Store/Dashboard/Image/GetByGameId/${gameif}`);
    return response.data;
  } catch (error) {
    console.error("Error :", error.response || error.message);
    return [];
  }
  };
export const GetAllDiscount =  async () => {
  try {
    const response = await apiClient.get('/Store/Dashboard/Discount/GetAll');
    return response.data; 
  } catch (error) {
    console.error("Error :", error.response || error.message);
    return [];
  }
};
export const GetAllCart = async() => {
  try {
    const response = await apiClient.get('/Store/Dashboard/Cart/GetAll'); 
    return response.data;
  } catch (error) {
    console.error("Error :", error.response || error.message);
    return [];
  }
  };
  export const GetImgGame = async (idGame) => {
    try {
      const response = await apiClient.get(`/Store/Dashboard/Image/GetByGameId/${idGame}`); 
      console.log(response);
      return response.data;
    } catch (error) {
      console.error("Error:", error.response || error.message);
      return [];
    }
  };
  
export const GetAllPublisher = async() => {
try {
  const response = await apiClient.get('/Store/Dashboard/Publisher/GetAll'); 
  return response.data; 
} catch (error) {
  console.error("Error :", error.response || error.message);
  return [];
}
};

export const GetAllAccountgame = async() => {
try {
  const response = await apiClient.get('/Store/Dashboard/AccountGame/GetAll'); 
  return response.data; 
} catch (error) {
  console.error("Error :", error.response || error.message);
  return [];
}
};
export const GetAllCartdetal = async() => {
  try {
    const response = await apiClient.get('/Cartdetail/GetAllCartDetail'); 
    return response.data;
  } catch (error) {
    console.error("Error :", error.response || error.message);
    return [];
  }
  };

export const GetAccount = async() => {
try {
  const response = await apiClient.get('/Store/Dashboard/Account/GetAll'); 
  return response.data; 
} catch (error) {
  console.error("Error :", error.response || error.message);
  return [];
}
};

export const GetRole = async () => {
  try {
    const response = await apiClient.get('/Role/GetAll'); 
    return response.data; 
  } catch (error) {
    console.error("Error :", error.response || error.message);
    return [];
  }
};


export const GetAllgame = async () => {
  try {
    const response = await apiClient.get('/Store/Dashboard/Game/GetAll'); 
    return response.data; 
  } catch (error) {
    console.error("Error :", error.response || error.message);
    return [];
  }
};

// API.js
export const 
DeleteGame = async (gameId) => {
  try {
    const response = await apiClient.delete(`/Store/Dashboard/Game/Delete/${gameId}`);
    
    if (response.status === 200) {
      return {
        success: true,
        message: response.data.message || "Game deleted successfully.",
      };
    } else if (response.status === 400) {
      return {
        success: false,
        message: response.data.message || "Failed to delete the game. Please check the data.",
      };
    } else {
      return {
        success: false,
        message: "Unexpected server response.",
      };
    }
  } catch (error) {
    console.error("Lỗi khi xóa sản phẩm:", error.response || error.message);
    return {
      success: false,
      message: error.response?.data?.message || "Failed to delete the game. Please try again.",
    };
  }
};

export const DeleteIMG = async (imgid) => {
  try {
    const response = await apiClient.delete(`/Store/Dashboard/Image/Delete/${imgid}`);
    return response.data;
  } catch (error) {
    console.error("Lỗi khi xóa sản phẩm:", error.response || error.message);
    throw new Error("Xóa không thành công");
  }
  };

// API.js
export const DeleteAccountgame = async (AccountGameID) => {
  try {
    const response = await apiClient.delete(`/Store/Dashboard/AccountGame/Delete/${AccountGameID}`); // Xóa game dựa trên gameId
    return response.data; 
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

export const UpdateGame = async (gameId, updatedGameData) => {
  try {
    const response = await apiClient.put(`/Store/Dashboard/Game/Update/${gameId}`, updatedGameData);

    if (response.status === 200) {
      return {
        success: true,
        message: response.data.message || "Game updated successfully.",
      };
    } else if (response.status === 400) {
      return {
        success: false,
        message: response.data.message || "Invalid data provided. Please check and try again.",
      };
    } else {
      return {
        success: false,
        message: "Unexpected server response.",
      };
    }
  } catch (error) {
    console.error("Error updating game:", error.response || error.message);
    return {
      success: false,
      message: error.response?.data?.message || "Failed to update the game. Please try again.",
    };
  }
};

export const UpdateAccount = async (Accountid, updatedAccount) => {
  try {
    const response = await apiClient.put(`/Store/Dashboard/Account/Update/${Accountid}`, updatedAccount);

    if (response.status === 200) {
      return {
        success: true,
        message: response.data.message || "Cập nhật tài khoản thành công.",
      };
    } else if (response.status === 400) {
      return {
        success: false,
        message: response.data.message || "Dữ liệu không hợp lệ, vui lòng kiểm tra lại.",
      };
    }
  } catch (error) {
    console.error("Error updating account:", error.response || error.message);
    return {
      success: false,
      message: error.response?.data?.message || "Có lỗi xảy ra trong quá trình cập nhật.",
    };
  }
};

export const UpdateIMG = async (gameid,img) => {
  try {
    const response = await apiClient.put(`/Store/Dashboard/Image/Update/${gameid}`, img); 
    return response.data;
  } catch (error) {
    console.error("Error updating game:", error.response || error.message);
    return null; 
  }
};
export const UpdateDiscount = async (DiscountID, updatedDiscount) => {
  try {
    const response = await apiClient.put(`/Store/Dashboard/Discount/Update/${DiscountID}`, updatedDiscount);
    console.log(DiscountID,updatedDiscount);
    if (response.status === 200) {
      return {
        success: true,
        message: response.data.message || "Cập nhật giảm giá thành công.",
      };
    } else if (response.status === 400) {
      return {
        success: false,
        message: response.data.message || "Dữ liệu không hợp lệ, vui lòng kiểm tra lại.",
      };
    }
  } catch (error) {
    console.error("Error updating discount:", error.response || error.message);
    return {
      success: false,
      message: error.response?.data?.message || "Có lỗi xảy ra trong quá trình cập nhật.",
    };
  }
};

export const UpdatePublisher = async (publisher,UpdatePublisher) => {
  try {
    const response = await apiClient.put(`/Store/Dashboard/Publisher/Update/${publisher}`, UpdatePublisher); 
    return response.data; 
  } catch (error) {
    console.error("Error updating publisher:", error.response || error.message);
    return null; 
  }
};export const DeleteDiscount = async (DiscountID) => {
  try {
    const response = await apiClient.delete(`/Store/Dashboard/Discount/Delete/${DiscountID}`);
    
    // Kiểm tra nếu thành công
    if (response.status === 200) {
      console.log("API Success:", response.data);
      return { success: true, message: "Discount deleted successfully", data: response.data };
    } else {
      // Xử lý nếu API không trả về 200
      return { success: false, message: "Failed to delete discount" };
    }
  } catch (error) {
    if (error.response) {
      // Xử lý lỗi dựa trên mã lỗi HTTP
      if (error.response.status === 400) {
        console.error("API Error:", error.response.data.message);
        return {
          success: false,
          message: error.response.data.message || "Dữ liệu không hợp lệ, vui lòng kiểm tra lại."
        };
      } else if (error.response.status === 500) {
        console.error("Server Error:", error.response.data.message);
        return {
          success: false,
          message: "Đã xảy ra lỗi từ phía máy chủ. Vui lòng thử lại sau."
        };
      }
    } else {
      // Xử lý khi không có phản hồi từ server (lỗi mạng hoặc cấu hình API sai)
      console.error("Unexpected Error:", error.message);
      return {
        success: false,
        message: "Có lỗi xảy ra. Vui lòng kiểm tra kết nối mạng hoặc cấu hình API."
      };
    }
  }
};

export const getOrders = async () => {
  try {
    const response = await fetch("https://dummyjson.com/carts/1");
    return await response.json();
  } catch (error) {
    console.error("Error fetching orders:", error);
    return null;
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