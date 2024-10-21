using Application.Services;
using Domain.Entities;
using Domain.Repository;
using Moq;

namespace Tests;

public class AuthenticationServiceTests
{
	private readonly AuthenticationService _authenticationService;
	private readonly Mock<IUserRepository> _userRepositoryMock;

	public AuthenticationServiceTests()
	{
		_userRepositoryMock = new Mock<IUserRepository>();
		_authenticationService = new AuthenticationService(_userRepositoryMock.Object);
	}

	[Fact]
	public async Task ValidateUserCredentialsAsync_ShouldReturnTrue_WhenCredentialsAreValid()
	{
		// Arrange
		var username = "TestUser";
		var password = "password123";
		_userRepositoryMock.Setup(repo => repo.GetUserByUserNameAsync(username))
			.ReturnsAsync(new Account { Username = username, Password = password });

		// Act
		var result = await _authenticationService.ValidateUserCredentialAsync(username, password);

		// Assert
		Assert.True(result);
	}

	[Fact]
	public async Task ValidateUserCredentialsAsync_ShouldReturnFalse_WhenCredentialsAreInvalid()
	{
		// Arrange
		var username = "Test";
		var password = "securepass";
		_userRepositoryMock.Setup(repo => repo.GetUserByUserNameAsync(username))
			.ReturnsAsync(new Account { Username = username, Password = "password" });

		// Act
		var result = await _authenticationService.ValidateUserCredentialAsync(username, password);

		// Assert
		Assert.False(result);
	}
}