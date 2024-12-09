import React, { useState } from "react";

// Hàm kiểm tra đường dẫn ảnh
const isValidImage = (src) => {
  // Kiểm tra nếu đường dẫn hợp lệ (có thể cải tiến thêm tùy vào yêu cầu)
  return src && src.trim() !== "";
};

// ImageSlider component nhận vào một mảng đường dẫn ảnh
const ImageSlider = ({ imageUrls }) => {
  const [currentIndex, setCurrentIndex] = useState(0);

  // Hàm chuyển đến ảnh kế tiếp
  const nextImage = () => {
    setCurrentIndex((prevIndex) => (prevIndex + 1) % imageUrls.length);
  };

  // Hàm chuyển đến ảnh trước đó
  const prevImage = () => {
    setCurrentIndex(
      (prevIndex) => (prevIndex - 1 + imageUrls.length) % imageUrls.length
    );
  };

  // Kiểm tra nếu mảng imageUrls không trống
  if (!imageUrls || imageUrls.length === 0) {
    return <p>Không có ảnh nào để hiển thị.</p>;
  }

  // Lọc các đường dẫn ảnh hợp lệ
  const validImageUrls = imageUrls.filter((src) => isValidImage(src));

  if (validImageUrls.length === 0) {
    return <p>Không có ảnh hợp lệ.</p>;
  }

  return (
    <div>
      {/* Hiển thị ảnh hiện tại */}
      <div>
        <img
          src={validImageUrls[currentIndex]}
          alt={`Image ${currentIndex + 1}`}
          style={{
            width: "100%",
            maxHeight: "500px",
            objectFit: "cover",
            borderRadius: "8px",
          }}
        />
      </div>

      {/* Nút chuyển ảnh */}
      <button onClick={prevImage}>Previous</button>
      <button onClick={nextImage}>Next</button>

      {/* Hiển thị chỉ số ảnh */}
      <p>
        {currentIndex + 1}/{validImageUrls.length}
      </p>
    </div>
  );
};

// Component cha để kiểm tra slider với các đường dẫn ảnh
const App = () => {
  const imageUrls = [
    "/images/slider1.jpg", // Đường dẫn ảnh hợp lệ
    "/images/slider2.jpg", // Đường dẫn ảnh hợp lệ
    "/images/slider3.jpg", // Đường dẫn ảnh hợp lệ
    "", // Đường dẫn ảnh không hợp lệ
  ];

  return (
    <div>
      <h2>Image Slider Example</h2>
      <ImageSlider imageUrls={imageUrls} />
    </div>
  );
};

export default App;
