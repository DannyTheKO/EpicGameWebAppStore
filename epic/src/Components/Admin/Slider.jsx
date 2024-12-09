import React from "react";
import Slider from "react-slick";
import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";

const ImageSlider = ({ images }) => {
  const settings = {
    dots: true, // Hiển thị nút điều hướng
    infinite: true, // Vòng lặp vô hạn
    speed: 500, // Thời gian chuyển ảnh
    slidesToShow: 3, // Số ảnh hiển thị cùng lúc
    slidesToScroll: 1, // Số ảnh lướt qua mỗi lần
    nextArrow: <SampleNextArrow />,
    prevArrow: <SamplePrevArrow />,
  };

  // Các nút tùy chỉnh (tùy chọn)
  function SampleNextArrow(props) {
    const { className, style, onClick } = props;
    return (
      <div
        className={className}
        style={{ ...style, display: "block", right: 10 }}
        onClick={onClick}
      />
    );
  }

  function SamplePrevArrow(props) {
    const { className, style, onClick } = props;
    return (
      <div
        className={className}
        style={{ ...style, display: "block", left: 10 }}
        onClick={onClick}
      />
    );
  }

  return (
    <div>
      <Slider {...settings}>
        {images.map((img, index) => (
          <div key={index}>
            <img
              src={img.src}
              alt={img.alt || `Image ${index + 1}`}
              style={{
                width: "100%",
                height: "200px",
                borderRadius: "10px",
                objectFit: "cover",
              }}
            />
          </div>
        ))}
      </Slider>
    </div>
  );
};

export default ImageSlider;
