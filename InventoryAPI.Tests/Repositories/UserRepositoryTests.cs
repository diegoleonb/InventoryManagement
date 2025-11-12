using Microsoft.EntityFrameworkCore;
using InventoryAPI.Data;
using InventoryAPI.Models;
using InventoryAPI.Repositories;
using Xunit;

namespace InventoryAPI.Tests.Repositories
{
  public class UserRepositoryTests : IDisposable
  {
    private readonly ApplicationDbContext _context;
    private readonly UserRepository _repository;

    public UserRepositoryTests()
    {
      var options = new DbContextOptionsBuilder<ApplicationDbContext>()
          .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
          .Options;

      _context = new ApplicationDbContext(options);
      _repository = new UserRepository(_context);

      SeedData();
    }

    private void SeedData()
    {
      var role = new Role { Id = "1", Name = "Administrator" };
      _context.Roles.Add(role);

      var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Username = "user1",
                    Email = "user1@example.com",
                    PasswordHash = new byte[] { 1, 2, 3 },
                    PasswordSalt = new byte[] { 4, 5, 6 },
                    RoleId = "1",
                    Role = role
                },
                new User
                {
                    Id = 2,
                    Username = "user2",
                    Email = "user2@example.com",
                    PasswordHash = new byte[] { 7, 8, 9 },
                    PasswordSalt = new byte[] { 10, 11, 12 },
                    RoleId = "1",
                    Role = role
                }
            };

      _context.Users.AddRange(users);
      _context.SaveChanges();
    }

    [Fact]
    public async Task GetUserByUsernameAsync_WithExistingUsername_ReturnsUser()
    {
      // Act
      var result = await _repository.GetUserByUsernameAsync("user1");

      // Assert
      Assert.NotNull(result);
      Assert.Equal("user1", result.Username);
      Assert.NotNull(result.Role);
    }

    [Fact]
    public async Task GetUserByUsernameAsync_WithNonExistingUsername_ReturnsNull()
    {
      // Act
      var result = await _repository.GetUserByUsernameAsync("nonexistent");

      // Assert
      Assert.Null(result);
    }

    [Fact]
    public async Task GetUserByEmailAsync_WithExistingEmail_ReturnsUser()
    {
      // Act
      var result = await _repository.GetUserByEmailAsync("user1@example.com");

      // Assert
      Assert.NotNull(result);
      Assert.Equal("user1@example.com", result.Email);
    }

    [Fact]
    public async Task GetUserByEmailAsync_WithNonExistingEmail_ReturnsNull()
    {
      // Act
      var result = await _repository.GetUserByEmailAsync("nonexistent@example.com");

      // Assert
      Assert.Null(result);
    }

    [Fact]
    public async Task UserExists_WithExistingUsernameOrEmail_ReturnsTrue()
    {
      // Act
      var result = await _repository.UserExists("user1", "nonexistent@example.com");

      // Assert
      Assert.True(result);
    }

    [Fact]
    public async Task UserExists_WithNonExistingUsernameAndEmail_ReturnsFalse()
    {
      // Act
      var result = await _repository.UserExists("nonexistent", "nonexistent@example.com");

      // Assert
      Assert.False(result);
    }

    [Fact]
    public async Task GetUsersWithRolesAsync_ReturnsUsersWithRoles()
    {
      // Act
      var result = await _repository.GetUsersWithRolesAsync();

      // Assert
      Assert.NotNull(result);
      Assert.Equal(2, result.Count());
      Assert.All(result, u => Assert.NotNull(u.Role));
    }

    public void Dispose()
    {
      _context?.Dispose();
    }
  }
}