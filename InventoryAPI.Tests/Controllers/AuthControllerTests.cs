using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using InventoryAPI.Controllers;
using InventoryAPI.Interfaces;
using InventoryAPI.DTOs;

namespace InventoryAPI.Tests.Controllers
{
  public class AuthControllerTests
  {
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly AuthController _authController;

    public AuthControllerTests()
    {
      _mockAuthService = new Mock<IAuthService>();
      _authController = new AuthController(_mockAuthService.Object);
    }

    [Fact]
    public async Task Register_WithValidData_ReturnsOkResult()
    {
      // Arrange
      var registerDto = new UserRegisterDto
      {
        Username = "testuser",
        Email = "test@example.com",
        Password = "password123",
        RoleId = "1"
      };

      var userResponse = new UserResponseDto
      {
        Id = 1,
        Username = "testuser",
        Email = "test@example.com",
        RoleName = "Administrator",
        Token = "jwt_token"
      };

      _mockAuthService.Setup(x => x.Register(registerDto))
          .ReturnsAsync(userResponse);

      // Act
      var result = await _authController.Register(registerDto);

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(result.Result);
      var returnValue = Assert.IsType<UserResponseDto>(okResult.Value);
      Assert.Equal(userResponse.Username, returnValue.Username);
    }

    [Fact]
    public async Task Register_WithExistingUser_ReturnsBadRequest()
    {
      // Arrange
      var registerDto = new UserRegisterDto();

      _mockAuthService.Setup(x => x.Register(registerDto))
          .ThrowsAsync(new ArgumentException("Username or email already exists"));

      // Act
      var result = await _authController.Register(registerDto);

      // Assert
      var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
      Assert.Equal("Username or email already exists", badRequestResult.Value);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsOkResult()
    {
      // Arrange
      var loginDto = new UserLoginDto
      {
        Username = "admin2",
        Password = "admin123"
      };

      var userResponse = new UserResponseDto
      {
        Id = 1,
        Username = "admin2",
        Email = "admin2@gmail.com",
        RoleName = "Administrator",
        Token = "jwt_token"
      };

      _mockAuthService.Setup(x => x.Login(loginDto))
          .ReturnsAsync(userResponse);

      // Act
      var result = await _authController.Login(loginDto);

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(result.Result);
      var returnValue = Assert.IsType<UserResponseDto>(okResult.Value);
      Assert.Equal(userResponse.Username, returnValue.Username);
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
    {
      // Arrange
      var loginDto = new UserLoginDto();

      _mockAuthService.Setup(x => x.Login(loginDto))
          .ThrowsAsync(new UnauthorizedAccessException("Invalid credentials"));

      // Act
      var result = await _authController.Login(loginDto);

      // Assert
      var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result.Result);
      Assert.Equal("Invalid credentials", unauthorizedResult.Value);
    }
  }
}