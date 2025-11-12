using Microsoft.EntityFrameworkCore;
using InventoryAPI.Data;
using InventoryAPI.Models;
using InventoryAPI.Repositories;

namespace InventoryAPI.Tests.Repositories
{
  public class ProductRepositoryTests : IDisposable
  {
    private readonly ApplicationDbContext _context;
    private readonly ProductRepository _repository;

    public ProductRepositoryTests()
    {
      var options = new DbContextOptionsBuilder<ApplicationDbContext>()
          .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
          .Options;

      _context = new ApplicationDbContext(options);
      _repository = new ProductRepository(_context);

      // Seed data
      SeedData();
    }

    private void SeedData()
    {
      var category = new Category { Id = 1, Name = "Test Category" };
      var user = new User
      {
        Id = 1,
        Username = "testuser",
        Email = "test@example.com",
        PasswordHash = new byte[0],
        PasswordSalt = new byte[0],
        RoleId = "1",
        Role = new Role { Id = "1", Name = "Administrator" }
      };

      _context.Categories.Add(category);
      _context.Users.Add(user);

      var products = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "Product 1",
                    Description = "Description 1",
                    StockQuantity = 5,
                    CategoryId = 1,
                    PictureUrl = "http://example.com/product1.jpg",
                    CreatedByUserId = 1
                },
                new Product
                {
                    Id = 2,
                    Name = "Product 2",
                    Description = "Description 2",
                    StockQuantity = 15,
                    CategoryId = 1,
                    PictureUrl = "http://example.com/product2.jpg",
                    CreatedByUserId = 1
                }
            };

      _context.Products.AddRange(products);
      _context.SaveChanges();
    }

    [Fact]
    public async Task GetProductsWithDetailsAsync_ReturnsProductsWithDetails()
    {
      // Act
      var result = await _repository.GetProductsWithDetailsAsync();

      // Assert
      Assert.NotNull(result);
      Assert.Equal(2, result.Count());
      Assert.All(result, p => Assert.NotNull(p.Category));
      Assert.All(result, p => Assert.NotNull(p.CreatedByUser));
    }

    [Fact]
    public async Task GetProductWithDetailsAsync_WithValidId_ReturnsProduct()
    {
      // Act
      var result = await _repository.GetProductWithDetailsAsync(1);

      // Assert
      Assert.NotNull(result);
      Assert.Equal(1, result.Id);
      Assert.NotNull(result.Category);
      Assert.NotNull(result.CreatedByUser);
    }

    [Fact]
    public async Task GetLowStockProductsAsync_ReturnsOnlyLowStockProducts()
    {
      // Act
      var result = await _repository.GetLowStockProductsAsync(10);

      // Assert
      Assert.NotNull(result);
      Assert.Single(result);
      Assert.Equal(1, result.First().Id); // Product 1 has stock 5
    }

    [Fact]
    public async Task GetProductsByCategoryAsync_ReturnsCategoryProducts()
    {
      // Act
      var result = await _repository.GetProductsByCategoryAsync(1);

      // Assert
      Assert.NotNull(result);
      Assert.Equal(2, result.Count());
      Assert.All(result, p => Assert.Equal(1, p.CategoryId));
    }

    public void Dispose()
    {
      _context?.Dispose();
    }
  }
}