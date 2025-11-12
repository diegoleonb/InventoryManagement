using Microsoft.EntityFrameworkCore;
using InventoryAPI.Data;
using InventoryAPI.Models;
using InventoryAPI.Repositories;
using Xunit;

namespace InventoryAPI.Tests.Repositories
{
  public class CategoryRepositoryTests : IDisposable
  {
    private readonly ApplicationDbContext _context;
    private readonly CategoryRepository _repository;

    public CategoryRepositoryTests()
    {
      var options = new DbContextOptionsBuilder<ApplicationDbContext>()
          .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
          .Options;

      _context = new ApplicationDbContext(options);
      _repository = new CategoryRepository(_context);

      SeedData();
    }

    private void SeedData()
    {
      var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Category 1" },
                new Category { Id = 2, Name = "Category 2" }
            };

      _context.Categories.AddRange(categories);

      var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", CategoryId = 1, Description = "Description 1", PictureUrl= ""},
                new Product { Id = 2, Name = "Product 2", CategoryId = 1, Description = "Description 2", PictureUrl= "" },
                new Product { Id = 3, Name = "Product 3", CategoryId = 2, Description = "Description 3", PictureUrl= "" }
            };

      _context.Products.AddRange(products);
      _context.SaveChanges();
    }

    [Fact]
    public async Task GetCategoryWithProductsAsync_WithExistingId_ReturnsCategoryWithProducts()
    {
      // Act
      var result = await _repository.GetCategoryWithProductsAsync(1);

      // Assert
      Assert.NotNull(result);
      Assert.Equal(1, result.Id);
      Assert.NotNull(result.Products);
      Assert.Equal(2, result.Products.Count);
    }

    [Fact]
    public async Task GetCategoryWithProductsAsync_WithNonExistingId_ReturnsNull()
    {
      // Act
      var result = await _repository.GetCategoryWithProductsAsync(999);

      // Assert
      Assert.Null(result);
    }

    [Fact]
    public async Task GetCategoriesWithProductsAsync_ReturnsCategoriesWithProducts()
    {
      // Act
      var result = await _repository.GetCategoriesWithProductsAsync();

      // Assert
      Assert.NotNull(result);
      Assert.Equal(2, result.Count());
      Assert.All(result, c => Assert.NotNull(c.Products));
    }

    public void Dispose()
    {
      _context?.Dispose();
    }
  }
}