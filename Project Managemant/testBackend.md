#### Cartdetail
- [ ] Chưa làm xong
#### GetUserByID
- [ ] Lỗi xem 1 tài khoản
#### Cart
- [ ] Create lỗi 
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

- /Cart/GetCart/{cartId}: 

```
|TypeError: Failed to execute 'fetch' on 'Window': Request with GET/HEAD method cannot have body.|
```
