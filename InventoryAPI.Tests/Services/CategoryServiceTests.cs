using Moq;
using Xunit;
using InventoryAPI.Services;
using InventoryAPI.Interfaces;
using InventoryAPI.Models;
using InventoryAPI.DTOs;

namespace InventoryAPI.Tests.Services
{
  public class CategoryServiceTests
  {
    private readonly Mock<ICategoryRepository> _mockCategoryRepository;
    private readonly CategoryService _categoryService;

    public CategoryServiceTests()
    {
      _mockCategoryRepository = new Mock<ICategoryRepository>();
      _categoryService = new CategoryService(_mockCategoryRepository.Object);
    }

    [Fact]
    public async Task GetAllCategoriesAsync_ReturnsCategories()
    {
      // Arrange
      var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Category 1", Products = new List<Product>() },
                new Category { Id = 2, Name = "Category 2", Products = new List<Product>() }
            };

      _mockCategoryRepository.Setup(x => x.GetCategoriesWithProductsAsync())
          .ReturnsAsync(categories);

      // Act
      var result = await _categoryService.GetAllCategoriesAsync();

      // Assert
      Assert.NotNull(result);
      Assert.Equal(2, result.Count());
      Assert.Equal(0, result.First().ProductCount);
    }

    [Fact]
    public async Task GetCategoryByIdAsync_WithValidId_ReturnsCategory()
    {
      // Arrange
      var category = new Category
      {
        Id = 1,
        Name = "Test Category",
        Products = new List<Product>()
      };

      _mockCategoryRepository.Setup(x => x.GetCategoryWithProductsAsync(1))
          .ReturnsAsync(category);

      // Act
      var result = await _categoryService.GetCategoryByIdAsync(1);

      // Assert
      Assert.NotNull(result);
      Assert.Equal(category.Id, result.Id);
      Assert.Equal(category.Name, result.Name);
    }

    [Fact]
    public async Task GetCategoryByIdAsync_WithInvalidId_ReturnsNull()
    {
      // Arrange
      _mockCategoryRepository.Setup(x => x.GetCategoryWithProductsAsync(It.IsAny<int>()))
          .ReturnsAsync((Category)null);

      // Act
      var result = await _categoryService.GetCategoryByIdAsync(999);

      // Assert
      Assert.Null(result);
    }

    [Fact]
    public async Task CreateCategoryAsync_WithValidData_ReturnsCategory()
    {
      // Arrange
      var categoryCreateDto = new CategoryCreateDto
      {
        Name = "New Category"
      };

      _mockCategoryRepository.Setup(x => x.AddAsync(It.IsAny<Category>()));
      _mockCategoryRepository.Setup(x => x.SaveAllAsync()).ReturnsAsync(true);

      // Act
      var result = await _categoryService.CreateCategoryAsync(categoryCreateDto);

      // Assert
      Assert.NotNull(result);
      Assert.Equal(categoryCreateDto.Name, result.Name);
      Assert.Equal(0, result.ProductCount);
    }

    [Fact]
    public async Task UpdateCategoryAsync_WithValidData_ReturnsUpdatedCategory()
    {
      // Arrange
      var categoryUpdateDto = new CategoryUpdateDto
      {
        Name = "Updated Category"
      };

      var existingCategory = new Category
      {
        Id = 1,
        Name = "Original Category",
        Products = new List<Product>()
      };

      _mockCategoryRepository.Setup(x => x.GetByIdAsync(1))
          .ReturnsAsync(existingCategory);
      _mockCategoryRepository.Setup(x => x.SaveAllAsync()).ReturnsAsync(true);

      // Act
      var result = await _categoryService.UpdateCategoryAsync(1, categoryUpdateDto);

      // Assert
      Assert.NotNull(result);
      Assert.Equal(categoryUpdateDto.Name, result.Name);
    }

    [Fact]
    public async Task UpdateCategoryAsync_WithInvalidId_ThrowsException()
    {
      // Arrange
      var categoryUpdateDto = new CategoryUpdateDto();

      _mockCategoryRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
          .ReturnsAsync((Category)null);

      // Act & Assert
      await Assert.ThrowsAsync<KeyNotFoundException>(() => _categoryService.UpdateCategoryAsync(999, categoryUpdateDto));
    }

    [Fact]
    public async Task DeleteCategoryAsync_WithValidIdAndNoProducts_ReturnsTrue()
    {
      // Arrange
      var category = new Category
      {
        Id = 1,
        Name = "Test Category",
        Products = new List<Product>() // No products
      };

      _mockCategoryRepository.Setup(x => x.GetCategoryWithProductsAsync(1))
          .ReturnsAsync(category);
      _mockCategoryRepository.Setup(x => x.SaveAllAsync()).ReturnsAsync(true);

      // Act
      var result = await _categoryService.DeleteCategoryAsync(1);

      // Assert
      Assert.True(result);
    }

    [Fact]
    public async Task DeleteCategoryAsync_WithInvalidId_ThrowsException()
    {
      // Arrange
      _mockCategoryRepository.Setup(x => x.GetCategoryWithProductsAsync(It.IsAny<int>()))
          .ReturnsAsync((Category)null);

      // Act & Assert
      await Assert.ThrowsAsync<KeyNotFoundException>(() => _categoryService.DeleteCategoryAsync(999));
    }

    [Fact]
    public async Task DeleteCategoryAsync_WithProducts_ThrowsException()
    {
      // Arrange
      var category = new Category
      {
        Id = 1,
        Name = "Test Category",
        Products = new List<Product> { new Product() } // Has products
      };

      _mockCategoryRepository.Setup(x => x.GetCategoryWithProductsAsync(1))
          .ReturnsAsync(category);

      // Act & Assert
      await Assert.ThrowsAsync<InvalidOperationException>(() => _categoryService.DeleteCategoryAsync(1));
    }
  }
}