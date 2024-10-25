using Application.Services;
using Domain.Entities;
using Domain.Repository;
using Moq;

namespace Tests;

public class AuthenticationServicesTests
{
	private readonly AuthenticationServices _authenticationServices;
	private readonly Mock<IAccountRepository> _userRepositoryMock;
	private readonly Mock<IRoleRepository> _roleRepositoryMock;

	public AuthenticationServicesTests()
	{
		_userRepositoryMock = new Mock<IAccountRepository>();
		_roleRepositoryMock = new Mock<IRoleRepository>();

		_authenticationServices = new AuthenticationServices(_userRepositoryMock.Object, _roleRepositoryMock.Object);
	}

	[Fact]
	public async Task LoginUser_ShouldReturnSuccessTrue_WhenCredentialsAreValid()
	{
		// Arrange
		var username = "TestUser";
		var password = "password123";
		var account = new Account { Username = username, Password = password };
		_userRepositoryMock.Setup(repo => repo.GetByUsernameAsync(username))
			.ReturnsAsync(account);

		// Act
		var (success, message) = await _authenticationServices.LoginUser(account);

		// Assert
		Assert.True(success);
		Assert.Equal("generated_token", message);
	}

	[Fact]
	public async Task LoginUser_ShouldReturnSuccessFalse_WhenCredentialsAreInvalid()
	{
		// Arrange
		var username = "TestUser";
		var password = "wrongpassword";
		var account = new Account { Username = username, Password = password };
		_userRepositoryMock.Setup(repo => repo.GetByUsernameAsync(username))
			.ReturnsAsync(new Account { Username = username, Password = "correctpassword" });

		// Act
		var (success, message) = await _authenticationServices.LoginUser(account);

		// Assert
		Assert.False(success);
		Assert.Equal("Invalid password", message);
	}

}