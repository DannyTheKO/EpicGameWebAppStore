### Basic Function
- [x] Create ✅ 2024-11-18
- [x] Read ✅ 2024-11-18
- [x] Read specific game by selecting game ID ✅ 2024-11-18
- [x] Update ✅ 2024-11-18
- [x] Delete ✅ 2024-11-18

## Feature Function
 - [x] Add Image function when creating a game ✅ 2024-11-25

### BUG
- [x] Game/CreateGame, Game/UpdateGame/{id} ✅ 2024-11-18
```
{
  "gameId": 0,
  "publisherId": 0,
  "genreId": 0,
  "title": "string",
  "price": 0,
  "author": "string",
  "release": "2024-11-15T17:46:39.937Z",
  "rating": 0,
  "description": "string",
  >> dư trường giá trị cartdetail
  "cartdetails": [
    {
      "cartDetailId": 0,
      "cartId": 0,
      "gameId": 0,
      "quantity": 0,
      "price": 0,
      "discount": 0
    }
  ],
  "discounts": [
    {
      "discountId": 0,
      "gameId": 0,
      "percent": 0,
      "code": "string",
      "startOn": "2024-11-15T17:46:39.937Z",
      "endOn": "2024-11-15T17:46:39.937Z",
      "game": "string"
    }
  ],
  "genre": {
    "genreId": 0,
    "name": "string"
  },
  "publisher": {
    "publisherId": 0,
    "name": "string",
    "address": "string",
    "email": "string",
    "phone": "string",
    "website": "string"
  }
}
```

 - [x] Game/DeleteConfirm/{id} ✅ 2024-11-18
 ```json
 Error: response status is 400

##### Response body

Download

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "Bad Request",
  "status": 400,
  "traceId": "00-dedc37543491fa3b0fc5dd8eabca46c9-9e1beb7d461cf314-00"
}
```

