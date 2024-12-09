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
    return response.data; 
} catch (error) {
    console.error("Error adding game:", error.response || error.message);
    return null; 
}
};
export const AddAccountgame = async (AccountGame) => {
  try {
    const response = await apiClient.post('/Store/Dashboard/AccountGame/Add', AccountGame); 
    return response.data; 
} catch (error) {
    console.error("Error adding game:", error.response || error.message);
    return null; 
}
};
export const AddDiscount = async (Discount) => {
  try {
    const response = await apiClient.post('/Store/Dashboard/Discount/Add', Discount); 
    return response.data; 
} catch (error) {
    console.error("Error adding game:", error.response || error.message);
    return null; 
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
export const DeleteGame = async (gameId) => {
try {
  const response = await apiClient.delete(`/Store/Dashboard/Game/Delete/${gameId}`);
  return response.data;
} catch (error) {
  console.error("Lỗi khi xóa sản phẩm:", error.response || error.message);
  throw new Error("Xóa không thành công");
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

export const UpdateGame = async (gameId,updatedGameData) => {
  try {
    const response = await apiClient.put(`/Store/Dashboard/Game/Update/${gameId}`, updatedGameData); 
    return response.data; 
  } catch (error) {
    console.error("Error updating game:", error.response || error.message);
    return null; 
  }
};
export const UpdateAccount = async (Accountid,updatedAccount) => {
  try {
    const response = await apiClient.put(`/Store/Dashboard/Account/Update/${Accountid}`, updatedAccount); 
    return response.data;
  } catch (error) {
    console.error("Error updating game:", error.response || error.message);
    return null; 
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
export const UpdateDiscount = async (DiscountID,UpdateDiscount) => {
  try {
    const response = await apiClient.put(`/Store/Dashboard/Discount/Update/${DiscountID}`, UpdateDiscount); 
    return response.data; 
  } catch (error) {
    console.error("Error updating game:", error.response || error.message);
    return null; 
  }
};
export const UpdatePublisher = async (publisher,UpdatePublisher) => {
  try {
    const response = await apiClient.put(`/Store/Dashboard/Discount/Update/${publisher}`, UpdatePublisher); 
    return response.data; 
  } catch (error) {
    console.error("Error updating game:", error.response || error.message);
    return null; 
  }
};
export const DeleteDiscount = async (DiscountID) => {
  try {
    const response = await apiClient.delete(`/Store/Dashboard/DeleteDiscount/${DiscountID}`);
    return response.data; 
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