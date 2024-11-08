//using Application.Interfaces;
//using Application.Services;
//using Domain.Entities;
//using Domain.Repository;
//using Moq;

//namespace Tests;

//public class AuthorizationServicesTests
//{
//	private readonly IAuthorizationServices _authorizationServices;
//	private readonly Mock<IAccountRepository> _mockAccountRepository;
//	private readonly Mock<IRoleRepository> _mockRoleRepository;

//	public AuthorizationServicesTests()
//	{
//		_mockAccountRepository = new Mock<IAccountRepository>();
//		_mockRoleRepository = new Mock<IRoleRepository>();
//		_authorizationServices = new AuthorizationServices(_mockAccountRepository.Object, _mockRoleRepository.Object);
//	}

//	[Fact]
//	public async Task UserHasPermission_WithValidPermission_ReturnsTrue()
//	{
//		// Arrange
//		var accountId = 1;
//		var roleId = 1;
//		var permission = "ReadData";

//		var account = new Account
//		{
//			AccountId = accountId,
//			RoleId = roleId,
//			Username = "testuser",
//			Password = "password123",
//			Email = "testuser@example.com",
//			IsActive = "Y",
//			CreatedOn = DateTime.UtcNow,
//			Role = new Role
//			{
//				RoleId = roleId,
//				Name = "TestRole",
//				Permission = new List<string> { "ReadData", "WriteData" }
//			}
//		};

//		var role = new Role
//		{
//			RoleId = roleId,
//			Name = "TestRole",
//			Permission = new List<string> { "ReadData", "WriteData" }
//		};

//		_mockAccountRepository.Setup(repo => repo.GetId(accountId)).ReturnsAsync(account);
//		_mockRoleRepository.Setup(repo => repo.GetById(roleId)).ReturnsAsync(role);

//		// Act
//		var result = await _authorizationServices.UserHasPermission(accountId, permission);

//		// Assert
//		Assert.True(result);
//	}


//	[Fact]
//	public async Task UserHasPermission_WithInvalidPermission_ReturnsFalse()
//	{
//		// Arrange
//		var accountId = 1;
//		var roleId = 1;
//		var permission = "DeleteData";

//		var account = new Account
//		{
//			AccountId = accountId,
//			RoleId = roleId,
//			Username = "testuser",
//			Password = "password123",
//			Email = "testuser@example.com",
//			IsActive = "Y",
//			CreatedOn = DateTime.UtcNow,
//			Role = new Role
//			{
//				RoleId = roleId,
//				Name = "TestRole",
//				Permission = new List<string> { "ReadData", "WriteData" }
//			}
//		};

//		var role = new Role
//		{
//			RoleId = roleId,
//			Name = "TestRole",
//			Permission = new List<string> { "ReadData", "WriteData" }
//		};

//		_mockAccountRepository.Setup(repo => repo.GetId(accountId)).ReturnsAsync(account);
//		_mockRoleRepository.Setup(repo => repo.GetById(roleId)).ReturnsAsync(role);

//		// Act
//		var result = await _authorizationServices.UserHasPermission(accountId, permission);

//		// Assert
//		Assert.False(result);
//	}

//	[Fact]
//	public async Task UserHasPermission_WithNonExistentAccount_ReturnsFalse()
//	{
//		// Arrange
//		var accountId = 1;
//		var permission = "ReadData";

//		_mockAccountRepository.Setup(repo => repo.GetId(accountId)).ReturnsAsync((Account)null);

//		// Act
//		var result = await _authorizationServices.UserHasPermission(accountId, permission);

//		// Assert
//		Assert.False(result);
//	}

//}

