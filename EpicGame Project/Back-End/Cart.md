### POST: Cart/CreateCart
- [x] Create lỗi ✅ 2024-11-15
```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "cart": [
      "The cart field is required."
    ],
    "$.createdOn": [
      "The JSON value could not be converted to System.Nullable`1[System.DateTime]. Path: $.createdOn | LineNumber: 5 | BytePositionInLine: 37."
    ]
  },
  "traceId": "00-e9beaa40c0d0a9635f5b325c3dad6601-5d9811f3fd622196-00"
}
```

- [x] /Cart/GetCart/{cartId}: ✅ 2024-11-15
```shell
TypeError: Failed to execute 'fetch' on 'Window': Request with GET/HEAD method cannot have body.
```

### PUT: Update Cart

- [x] Cart/UpdateCart/{id} ✅ 2024-11-15
```
Error: response status is 400

##### Response body

Download

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Cartdetails[0].Cart": [
      "The Cart field is required."
    ],
    "Cartdetails[0].Game": [
      "The Game field is required."
    ],
    "Cartdetails[1].Cart": [
      "The Cart field is required."
    ],
    "Cartdetails[1].Game": [
      "The Game field is required."
    ]
  },
  "traceId": "00-d901c6902fddf718162afd5d973cabfd-55a8a2b75dd3c365-00"
}
```

### DELETE: Delete Cart
- [x] /Cart/DeleteCart/{id}: ✅ 2024-11-15
 ```
 Error: response status is 400
  ```json
{
  "success": false,
  "message": "Requested Cart do not existed or already deleted"
}
```


