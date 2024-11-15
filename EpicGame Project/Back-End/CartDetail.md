## Missing Feature
- Function are missing and need to be done
- Service Function

## BUG
- [x] Lỗi khi sửa 1 cartdetailID, vì không có trường trường nhập cartdetailID nên project tự thêm một cartdetailId vào database ✅ 2024-11-16
- [x] Cartdetail/UpdateCartDetail/{cartDetailId} ✅ 2024-11-16
```json
Error: response status is 500
System.InvalidOperationException: The instance of entity type 'Cartdetail' cannot be tracked because another instance with the same key value for {'CartDetailId'} is already being tracked. When attaching existing entities, ensure that only one entity instance with a given key value is attached. Consider using 'DbContextOptionsBuilder.EnableSensitiveDataLogging' to see the conflicting key values.
```

# Complete
- Basic CRUB Function
	- [x] GET: GetAllCartDetail ✅ 2024-11-15
	- [x] GET: GetCartDetail/{id} ✅ 2024-11-15
	- [x] POST: AddCartDetail ✅ 2024-11-15
	- [x] PUT: UpdateCartDetail ✅ 2024-11-15
	- [x] DELETE: DeleteCartDetail/{id} ✅ 2024-11-15
	- [x] POST: Cartdetail/UpdateCartDetail ✅ 2024-11-15
	
