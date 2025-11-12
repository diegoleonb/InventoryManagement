using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using InventoryAPI.Controllers;
using InventoryAPI.Interfaces;
using InventoryAPI.DTOs;

namespace InventoryAPI.Tests.Controllers
{
  public class CategoriesControllerTests
  {
    private readonly Mock<ICategoryService> _mockCategoryService;
    private readonly CategoriesController _controller;

    public CategoriesControllerTests()
    {
      _mockCategoryService = new Mock<ICategoryService>();
      _controller = new CategoriesController(_mockCategoryService.Object);
    }

    [Fact]
    public async Task GetCategories_ReturnsOkResult()
    {
      // Arrange
      var categories = new List<CategoryDto>
            {
                new CategoryDto { Id = 1, Name = "Category 1", ProductCount = 5 },
                new CategoryDto { Id = 2, Name = "Category 2", ProductCount = 3 }
            };

      _mockCategoryService.Setup(x => x.GetAllCategoriesAsync())
          .ReturnsAsync(categories);

      // Act
      var result = await _controller.GetCategories();

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(result.Result);
      var returnValue = Assert.IsType<List<CategoryDto>>(okResult.Value);
      Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task GetCategory_WithValidId_ReturnsCategory()
    {
      // Arrange
      var category = new CategoryDto { Id = 1, Name = "Category 1", ProductCount = 5 };

      _mockCategoryService.Setup(x => x.GetCategoryByIdAsync(1))
          .ReturnsAsync(category);

      // Act
      var result = await _controller.GetCategory(1);

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(result.Result);
      var returnValue = Assert.IsType<CategoryDto>(okResult.Value);
      Assert.Equal(1, returnValue.Id);
    }

    [Fact]
    public async Task GetCategory_WithInvalidId_ReturnsNotFound()
    {
      // Arrange
      _mockCategoryService.Setup(x => x.GetCategoryByIdAsync(It.IsAny<int>()))
          .ReturnsAsync((CategoryDto)null);

      // Act
      var result = await _controller.GetCategory(999);

      // Assert
      Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateCategory_WithValidData_ReturnsCreated()
    {
      // Arrange
      var categoryCreateDto = new CategoryCreateDto { Name = "New Category" };
      var categoryDto = new CategoryDto { Id = 1, Name = "New Category", ProductCount = 0 };

      _mockCategoryService.Setup(x => x.CreateCategoryAsync(categoryCreateDto))
          .ReturnsAsync(categoryDto);

      // Act
      var result = await _controller.CreateCategory(categoryCreateDto);

      // Assert
      var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
      var returnValue = Assert.IsType<CategoryDto>(createdAtResult.Value);
      Assert.Equal(1, returnValue.Id);
    }

    [Fact]
    public async Task UpdateCategory_WithValidData_ReturnsOk()
    {
      // Arrange
      var categoryUpdateDto = new CategoryUpdateDto { Name = "Updated Category" };
      var categoryDto = new CategoryDto { Id = 1, Name = "Updated Category", ProductCount = 0 };

      _mockCategoryService.Setup(x => x.UpdateCategoryAsync(1, categoryUpdateDto))
          .ReturnsAsync(categoryDto);

      // Act
      var result = await _controller.UpdateCategory(1, categoryUpdateDto);

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(result.Result);
      var returnValue = Assert.IsType<CategoryDto>(okResult.Value);
      Assert.Equal("Updated Category", returnValue.Name);
    }

    [Fact]
    public async Task UpdateCategory_WithInvalidId_ReturnsNotFound()
    {
      // Arrange
      var categoryUpdateDto = new CategoryUpdateDto();

      _mockCategoryService.Setup(x => x.UpdateCategoryAsync(It.IsAny<int>(), categoryUpdateDto))
          .ThrowsAsync(new KeyNotFoundException());

      // Act
      var result = await _controller.UpdateCategory(999, categoryUpdateDto);

      // Assert
      Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task DeleteCategory_WithValidId_ReturnsNoContent()
    {
      // Arrange
      _mockCategoryService.Setup(x => x.DeleteCategoryAsync(1))
          .ReturnsAsync(true);

      // Act
      var result = await _controller.DeleteCategory(1);

      // Assert
      Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteCategory_WithInvalidId_ReturnsNotFound()
    {
      // Arrange
      _mockCategoryService.Setup(x => x.DeleteCategoryAsync(It.IsAny<int>()))
          .ThrowsAsync(new KeyNotFoundException());

      // Act
      var result = await _controller.DeleteCategory(999);

      // Assert
      Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteCategory_WithProducts_ReturnsBadRequest()
    {
      // Arrange
      _mockCategoryService.Setup(x => x.DeleteCategoryAsync(It.IsAny<int>()))
          .ThrowsAsync(new InvalidOperationException("Cannot delete category with existing products"));

      // Act
      var result = await _controller.DeleteCategory(1);

      // Assert
      var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
      Assert.Equal("Cannot delete category with existing products", badRequestResult.Value);
    }
  }
}