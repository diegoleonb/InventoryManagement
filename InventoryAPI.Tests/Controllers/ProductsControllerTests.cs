using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using InventoryAPI.Controllers;
using InventoryAPI.Interfaces;
using InventoryAPI.DTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace InventoryAPI.Tests.Controllers
{
  public class ProductsControllerTests
  {
    private readonly Mock<IProductService> _mockProductService;
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
      _mockProductService = new Mock<IProductService>();
      _mockAuthService = new Mock<IAuthService>();
      _controller = new ProductsController(_mockProductService.Object, _mockAuthService.Object);

      // Mock user identity
      var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
      {
                new Claim(ClaimTypes.Name, "testuser"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
      }, "mock"));

      _controller.ControllerContext = new ControllerContext()
      {
        HttpContext = new DefaultHttpContext() { User = user }
      };
    }

    [Fact]
    public async Task GetProducts_ReturnsOkResult()
    {
      // Arrange
      var products = new List<ProductDto>
            {
                new ProductDto { Id = 1, Name = "Product 1" },
                new ProductDto { Id = 2, Name = "Product 2" }
            };

      _mockProductService.Setup(x => x.GetAllProductsAsync())
          .ReturnsAsync(products);

      // Act
      var result = await _controller.GetProducts();

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(result.Result);
      var returnValue = Assert.IsType<List<ProductDto>>(okResult.Value);
      Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task GetProduct_WithValidId_ReturnsProduct()
    {
      // Arrange
      var product = new ProductDto { Id = 1, Name = "Product 1" };

      _mockProductService.Setup(x => x.GetProductByIdAsync(1))
          .ReturnsAsync(product);

      // Act
      var result = await _controller.GetProduct(1);

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(result.Result);
      var returnValue = Assert.IsType<ProductDto>(okResult.Value);
      Assert.Equal(1, returnValue.Id);
    }

    [Fact]
    public async Task GetProduct_WithInvalidId_ReturnsNotFound()
    {
      // Arrange
      _mockProductService.Setup(x => x.GetProductByIdAsync(It.IsAny<int>()))
          .ReturnsAsync((ProductDto)null);

      // Act
      var result = await _controller.GetProduct(999);

      // Assert
      Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateProduct_WithValidData_ReturnsCreated()
    {
      // Arrange
      var productCreateDto = new ProductCreateDto
      {
        Name = "New Product",
        Description = "Description",
        StockQuantity = 10,
        Price = 99.99m,
        PictureUrl = "",
        CategoryId = 1
      };

      var productDto = new ProductDto { Id = 1, Name = "New Product" };

      _mockAuthService.Setup(x => x.GetUserIdFromToken(It.IsAny<string>()))
          .Returns(1);
      _mockProductService.Setup(x => x.CreateProductAsync(productCreateDto, 1))
          .ReturnsAsync(productDto);

      // Act
      var result = await _controller.CreateProduct(productCreateDto);

      // Assert
      var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
      var returnValue = Assert.IsType<ProductDto>(createdAtResult.Value);
      Assert.Equal(1, returnValue.Id);
    }


    [Fact]
    public async Task UpdateProduct_WithValidData_ReturnsOk()
    {
      // Arrange
      var productUpdateDto = new ProductUpdateDto
      {
        Name = "Updated Product",
        Description = "Updated Description",
        StockQuantity = 20,
        Price = 199.99m,
        PictureUrl = "",
        CategoryId = 2
      };

      var productDto = new ProductDto { Id = 1, Name = "Updated Product" };

      _mockProductService.Setup(x => x.UpdateProductAsync(1, productUpdateDto))
          .ReturnsAsync(productDto);

      // Act
      var result = await _controller.UpdateProduct(1, productUpdateDto);

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(result.Result);
      var returnValue = Assert.IsType<ProductDto>(okResult.Value);
      Assert.Equal("Updated Product", returnValue.Name);
    }

    [Fact]
    public async Task UpdateProduct_WithInvalidId_ReturnsNotFound()
    {
      // Arrange
      var productUpdateDto = new ProductUpdateDto();

      _mockProductService.Setup(x => x.UpdateProductAsync(It.IsAny<int>(), productUpdateDto))
          .ThrowsAsync(new KeyNotFoundException());

      // Act
      var result = await _controller.UpdateProduct(999, productUpdateDto);

      // Assert
      Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task DeleteProduct_WithValidId_ReturnsNoContent()
    {
      // Arrange
      _mockProductService.Setup(x => x.DeleteProductAsync(1))
          .ReturnsAsync(true);

      // Act
      var result = await _controller.DeleteProduct(1);

      // Assert
      Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteProduct_WithInvalidId_ReturnsNotFound()
    {
      // Arrange
      _mockProductService.Setup(x => x.DeleteProductAsync(It.IsAny<int>()))
          .ThrowsAsync(new KeyNotFoundException());

      // Act
      var result = await _controller.DeleteProduct(999);

      // Assert
      Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetProductsByCategory_ReturnsProducts()
    {
      // Arrange
      var products = new List<ProductDto>
            {
                new ProductDto { Id = 1, Name = "Product 1", CategoryId = 1 },
                new ProductDto { Id = 2, Name = "Product 2", CategoryId = 1 }
            };

      _mockProductService.Setup(x => x.GetProductsByCategoryAsync(1))
          .ReturnsAsync(products);

      // Act
      var result = await _controller.GetProductsByCategory(1);

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(result.Result);
      var returnValue = Assert.IsType<List<ProductDto>>(okResult.Value);
      Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task GetLowStockProducts_ReturnsLowStockProducts()
    {
      // Arrange
      var products = new List<ProductDto>
            {
                new ProductDto { Id = 1, Name = "Low Stock Product", StockQuantity = 5 }
            };

      _mockProductService.Setup(x => x.GetLowStockProductsAsync(10))
          .ReturnsAsync(products);

      // Act
      var result = await _controller.GetLowStockProducts(10);

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(result.Result);
      var returnValue = Assert.IsType<List<ProductDto>>(okResult.Value);
      Assert.Single(returnValue);
    }
  }
}