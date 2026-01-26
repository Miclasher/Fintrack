using Fintrack.Domain.Entities;
using Fintrack.Services;
using Fintrack.Services.Abstractions;
using Fintrack.Services.Utilities;
using Fintrack.Shared;
using Moq;

namespace Fintrack.Tests;

[TestClass]
public class AuthServiceTests : ServiceTestsBase
{
    private Mock<IJwtUtility> _mockJwtUtility = null!;
    private AuthService _authService = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _mockJwtUtility = new Mock<IJwtUtility>();
        _authService = new AuthService(_mockJwtUtility.Object, _mockRepositoryManager.Object);
    }

    [TestMethod]
    public async Task LoginAsyncTest1()
    {
        var userLoginDto = new UserLoginDTO { Username = "testuser", Password = "password" };
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "testuser",
            PasswordHash = HashUtility.HashPassword("password", Convert.FromBase64String("somesalt")),
            Salt = "somesalt"
        };

        _mockRepositoryManager.Setup(r => r.User.GetByUsernameAsync(userLoginDto.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _mockJwtUtility.Setup(j => j.GenerateAccessToken(user.Id))
            .Returns("accessToken");
        _mockJwtUtility.Setup(j => j.GenerateRefreshToken())
            .Returns("refreshToken");

        var result = await _authService.LoginAsync(userLoginDto);

        Assert.IsNotNull(result);
        Assert.AreEqual("accessToken", result.AccessToken);
        Assert.AreEqual("refreshToken", result.RefreshToken);
    }

    [TestMethod]
    public async Task LoginAsyncTest2()
    {
        var userLoginDto = new UserLoginDTO { Username = "testuser", Password = "wrongpassword" };
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "testuser",
            PasswordHash = HashUtility.HashPassword("password", Convert.FromBase64String("somesalt")),
            Salt = "somesalt",
        };

        _mockRepositoryManager.Setup(r => r.User.GetByUsernameAsync(userLoginDto.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        await Assert.ThrowsAsync<InvalidDataException>(() => _authService.LoginAsync(userLoginDto));
    }

    [TestMethod]
    public async Task RegisterAsyncTest1()
    {
        var userRegisterDto = new UserRegisterDTO
        {
            Username = "newuser",
            Email = "newuser@example.com",
            PlainPassword = "password",
            DisplayName = "New User",
            MonobankApiToken = "token"
        };

        _mockRepositoryManager.Setup(r => r.User.UsernameExistsAsync(userRegisterDto.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _mockRepositoryManager.Setup(r => r.TransactionTypeTemplate.GetAllWithMccAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<TransactionTypeTemplate>());
        _mockJwtUtility.Setup(j => j.GenerateAccessToken(It.IsAny<Guid>()))
            .Returns("accessToken");
        _mockJwtUtility.Setup(j => j.GenerateRefreshToken())
            .Returns("refreshToken");

        var result = await _authService.RegisterAsync(userRegisterDto);

        Assert.IsNotNull(result);
        Assert.AreEqual("accessToken", result.AccessToken);
        Assert.AreEqual("refreshToken", result.RefreshToken);
    }

    [TestMethod]
    public async Task RegisterAsyncTest2()
    {
        var userRegisterDto = new UserRegisterDTO
        {
            Username = "existinguser",
            Email = "existinguser@example.com",
            PlainPassword = "password",
            DisplayName = "Existing User"
        };

        _mockRepositoryManager.Setup(r => r.User.UsernameExistsAsync(userRegisterDto.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _authService.RegisterAsync(userRegisterDto));
    }
}
