using Application.Services;
using Domain.Entities;
using Domain.Repository;
using Moq;

namespace Tests;

public class AuthenticationServicesTests
{
	private readonly AuthenticationServices _authenticationServices;
	private readonly Mock<IAccountRepository> _userRepositoryMock;

	public AuthenticationServicesTests()
	{
		_userRepositoryMock = new Mock<IAccountRepository>();
		_authenticationServices = new AuthenticationServices(_userRepositoryMock.Object);
	}

	[Fact]
	public async Task ValidateUserCredentialsAsync_ShouldReturnTrue_WhenCredentialsAreValid()
	{
		// Arrange
		var username = "TestUser";
		var password = "password123";
		_userRepositoryMock.Setup(repo => repo.GetByUsernameAsync(username))
			.ReturnsAsync(new Account { Username = username, Password = password });

		// Act
		var result = await _authenticationServices.ValidateUserCredentialAsync(username, password);

		// Assert
		Assert.True(result);
	}

	[Fact]
	public async Task ValidateUserCredentialsAsync_ShouldReturnFalse_WhenCredentialsAreInvalid()
	{
		// Arrange
		var username = "Test";
		var password = "securepass";
		_userRepositoryMock.Setup(repo => repo.GetByUsernameAsync(username))
			.ReturnsAsync(new Account { Username = username, Password = "password" });

		// Act
		var result = await _authenticationServices.ValidateUserCredentialAsync(username, password);

		// Assert
		Assert.False(result);
	}
}