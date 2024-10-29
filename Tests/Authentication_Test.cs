using Application.Services;
using Domain.Entities;
using Domain.Repository;
using Moq;

namespace Tests;

public class AuthenticationServicesTests
{
	private readonly AuthenticationServices _authenticationServices;
	private readonly Mock<IRoleRepository> _roleRepositoryMock;
	private readonly Mock<IAccountRepository> _userRepositoryMock;

	public AuthenticationServicesTests()
	{
		_userRepositoryMock = new Mock<IAccountRepository>();
		_roleRepositoryMock = new Mock<IRoleRepository>();
		_authenticationServices = new AuthenticationServices(_userRepositoryMock.Object, _roleRepositoryMock.Object);
	}

	[Fact]
	public async Task GetAllUser_ShouldReturnAllUsers()
	{
		// Arrange
		var users = new List<Account>
		{
			new() { AccountId = 1, Username = "user1" },
			new() { AccountId = 2, Username = "user2" }
		};
		_userRepositoryMock.Setup(repo => repo.GetAllUserAsync()).ReturnsAsync(users);

		// Act
		var result = await _authenticationServices.GetAllUser();

		// Assert
		Assert.Equal(2, result.Count());
		Assert.Contains(result, u => u.Username == "user1");
		Assert.Contains(result, u => u.Username == "user2");
	}

	[Fact]
	public async Task DeleteUser_ShouldDeleteUser()
	{
		// Arrange
		var userId = 1;
		var account = new Account { AccountId = userId };
		_userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(account);
		_userRepositoryMock.Setup(repo => repo.DeleteUserAsync(userId)).Returns(Task.CompletedTask);

		// Act
		await _authenticationServices.DeleteUser(userId);

		// Assert
		_userRepositoryMock.Verify(repo => repo.DeleteUserAsync(userId), Times.Once);
	}


	[Fact]
	public async Task GetUserId_ShouldReturnUser()
	{
		// Arrange
		var userId = 1;
		var user = new Account { AccountId = userId, Username = "testUser" };
		_userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(user);

		// Act
		var result = await _authenticationServices.GetUserId(userId);

		// Assert
		Assert.Equal(userId, result.AccountId);
		Assert.Equal("testUser", result.Username);
	}

	[Fact]
	public async Task GetAccountByUsername_ShouldReturnAccount()
	{
		// Arrange
		var username = "testUser";
		var account = new Account { AccountId = 1, Username = username };
		_userRepositoryMock.Setup(repo => repo.GetByUsernameAsync(username)).ReturnsAsync(account);

		// Act
		var result = await _authenticationServices.GetAccountByUsername(username);

		// Assert
		Assert.Equal(username, result.Username);
	}

	[Fact]
	public async Task GetAccountByEmail_ShouldReturnAccount()
	{
		// Arrange
		var email = "test@example.com";
		var account = new Account { AccountId = 1, Email = email };
		_userRepositoryMock.Setup(repo => repo.GetByEmailAsync(email)).ReturnsAsync(account);

		// Act
		var result = await _authenticationServices.GetAccountByEmail(email);

		// Assert
		Assert.Equal(email, result.Email);
	}

	[Fact]
	public async Task ValidateUserCredentialAsync_ShouldReturnTrue_WhenCredentialsAreValid()
	{
		// Arrange
		var username = "testUser";
		var password = "password123";
		var account = new Account { Username = username, Password = password };
		_userRepositoryMock.Setup(repo => repo.GetByUsernameAsync(username)).ReturnsAsync(account);

		// Act
		var result = await _authenticationServices.ValidateUserCredentialAsync(username, password);

		// Assert
		Assert.True(result);
	}

	[Fact]
	public async Task GenerateTokenAsync_ShouldReturnToken()
	{
		// Arrange
		var username = "testUser";

		// Act
		var result = await _authenticationServices.GenerateTokenAsync(username);

		// Assert
		Assert.Equal("generated_token", result);
	}

	[Fact]
	public async Task RegisterUser_ShouldReturnSuccessTrue_WhenRegistrationIsSuccessful()
	{
		// Arrange
		var account = new Account { Username = "newUser", Email = "new@example.com", Password = "password123" };
		var confirmPassword = "password123";
		_userRepositoryMock.Setup(repo => repo.GetByUsernameAsync(account.Username)).ReturnsAsync((Account)null);
		_userRepositoryMock.Setup(repo => repo.GetByEmailAsync(account.Email)).ReturnsAsync((Account)null);

		// Act
		var (success, message) = await _authenticationServices.RegisterUser(account, confirmPassword);

		// Assert
		Assert.True(success);
		Assert.Empty(message);
	}

	[Fact]
	public async Task LoginUser_ShouldReturnSuccessTrue_WhenCredentialsAreValid()
	{
		// Arrange
		var account = new Account { Username = "testUser", Password = "password123" };
		_userRepositoryMock.Setup(repo => repo.GetByUsernameAsync(account.Username)).ReturnsAsync(account);

		// Act
		var (success, result, accountId) = await _authenticationServices.LoginUser(account);

		// Assert
		Assert.True(success);
		Assert.Equal("generated_token", result);
	}

	[Fact]
	public async Task UpdateUser_ShouldUpdateUser()
	{
		// Arrange
		var account = new Account { AccountId = 1, Username = "updatedUser", Email = "updated@example.com" };
		_userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(account.AccountId)).ReturnsAsync(account);
		_userRepositoryMock.Setup(repo => repo.UpdateUserAsync(account)).Returns(Task.CompletedTask);

		// Act
		var result = await _authenticationServices.UpdateUser(account);

		// Assert
		Assert.Equal(account.Username, result.Username);
		Assert.Equal(account.Email, result.Email);
	}
}