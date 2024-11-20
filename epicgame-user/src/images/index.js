// src/assets/index.js
const importAll = (r) => {
    let images = {};
    r.keys().forEach((item) => {
        images[item.replace('./', '')] = r(item);
    });
    return images;
};

const images = importAll(require.context('.', false, /\.(png|jpe?g|svg)$/));

export default images;
