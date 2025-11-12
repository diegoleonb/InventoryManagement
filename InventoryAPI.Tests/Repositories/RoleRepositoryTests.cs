using Microsoft.EntityFrameworkCore;
using InventoryAPI.Data;
using InventoryAPI.Models;
using InventoryAPI.Repositories;
using Xunit;

namespace InventoryAPI.Tests.Repositories
{
  public class RoleRepositoryTests : IDisposable
  {
    private readonly ApplicationDbContext _context;
    private readonly RoleRepository _repository;

    public RoleRepositoryTests()
    {
      var options = new DbContextOptionsBuilder<ApplicationDbContext>()
          .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
          .Options;

      _context = new ApplicationDbContext(options);
      _repository = new RoleRepository(_context);

      SeedData();
    }

    private void SeedData()
    {
      var roles = new List<Role>
            {
                new Role { Id = "1", Name = "Administrator" },
                new Role { Id = "2", Name = "Operator" }
            };

      _context.Roles.AddRange(roles);
      _context.SaveChanges();
    }

    [Fact]
    public async Task GetRoleByNameAsync_WithExistingName_ReturnsRole()
    {
      // Act
      var result = await _repository.GetRoleByNameAsync("Administrator");

      // Assert
      Assert.NotNull(result);
      Assert.Equal("Administrator", result.Name);
    }

    [Fact]
    public async Task GetRoleByNameAsync_WithNonExistingName_ReturnsNull()
    {
      // Act
      var result = await _repository.GetRoleByNameAsync("Nonexistent");

      // Assert
      Assert.Null(result);
    }

    public void Dispose()
    {
      _context?.Dispose();
    }
  }
}