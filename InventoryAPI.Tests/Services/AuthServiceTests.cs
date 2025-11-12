using Moq;
using Xunit;
using InventoryAPI.Services;
using InventoryAPI.Interfaces;
using InventoryAPI.Models;
using InventoryAPI.DTOs;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace InventoryAPI.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IRoleRepository> _mockRoleRepository;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockRoleRepository = new Mock<IRoleRepository>();
            _mockConfiguration = new Mock<IConfiguration>();

            // Mock configuration
            _mockConfiguration.Setup(x => x["Jwt:Secret"]).Returns("TestSecretKeyThatIsLongEnoughForTesting12345");
            _mockConfiguration.Setup(x => x["Jwt:Issuer"]).Returns("TestIssuer");
            _mockConfiguration.Setup(x => x["Jwt:Audience"]).Returns("TestAudience");

            _authService = new AuthService(_mockUserRepository.Object, _mockRoleRepository.Object, _mockConfiguration.Object);
        }

        [Fact]
        public async Task Register_WithExistingUser_ThrowsException()
        {
            // Arrange
            var registerDto = new UserRegisterDto
            {
                Username = "existinguser",
                Email = "existing@example.com",
                Password = "password123",
                RoleId = "1"
            };

            _mockUserRepository.Setup(x => x.UserExists(registerDto.Username, registerDto.Email))
                .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _authService.Register(registerDto));
        }

        [Fact]
        public async Task Register_WithInvalidRole_ThrowsException()
        {
            // Arrange
            var registerDto = new UserRegisterDto
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "password123",
                RoleId = "999"
            };

            _mockUserRepository.Setup(x => x.UserExists(registerDto.Username, registerDto.Email))
                .ReturnsAsync(false);
            _mockRoleRepository.Setup(x => x.GetByIdAsync(registerDto.RoleId))
                .ReturnsAsync((Role)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _authService.Register(registerDto));
        }

        [Fact]
        public async Task Login_WithInvalidUsername_ThrowsUnauthorizedException()
        {
            // Arrange
            var loginDto = new UserLoginDto
            {
                Username = "nonexistent",
                Password = "password123"
            };

            _mockUserRepository.Setup(x => x.GetUserByUsernameAsync(loginDto.Username))
                .ReturnsAsync((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authService.Login(loginDto));
        }

        [Fact]
        public async Task Login_WithInvalidPassword_ThrowsUnauthorizedException()
        {
            // Arrange
            var loginDto = new UserLoginDto
            {
                Username = "testuser",
                Password = "wrongpassword"
            };

            var user = new User
            {
                Id = 1,
                Username = "testuser",
                Email = "test@example.com"
            };

            // Create password hash with different password
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            user.PasswordSalt = hmac.Key;
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("correctpassword"));

            _mockUserRepository.Setup(x => x.GetUserByUsernameAsync(loginDto.Username))
                .ReturnsAsync(user);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authService.Login(loginDto));
        }


        [Fact]
        public void GetUserIdFromToken_WithInvalidToken_ReturnsNull()
        {
            // Arrange
            var invalidToken = "invalid.token.here";

            // Act
            var userId = _authService.GetUserIdFromToken(invalidToken);

            // Assert
            Assert.Null(userId);
        }

        [Fact]
        public async Task UserExists_ReturnsResultFromRepository()
        {
            // Arrange
            var username = "testuser";
            var email = "test@example.com";
            
            _mockUserRepository.Setup(x => x.UserExists(username, email))
                .ReturnsAsync(true);

            // Act
            var result = await _authService.UserExists(username, email);

            // Assert
            Assert.True(result);
        }
    }
}