using Moq;
using Xunit;
using InventoryAPI.Services;
using InventoryAPI.Interfaces;
using InventoryAPI.Models;
using InventoryAPI.DTOs;

namespace InventoryAPI.Tests.Services
{
  public class ProductServiceTests
  {
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<ICategoryRepository> _mockCategoryRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
      _mockProductRepository = new Mock<IProductRepository>();
      _mockCategoryRepository = new Mock<ICategoryRepository>();
      _mockUserRepository = new Mock<IUserRepository>();
      _productService = new ProductService(
          _mockProductRepository.Object,
          _mockCategoryRepository.Object,
          _mockUserRepository.Object
      );
    }

    [Fact]
    public async Task GetAllProductsAsync_ReturnsProducts()
    {
      // Arrange
      var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Category = new Category { Name = "Category 1" }, CreatedByUser = new User { Username = "user1" } },
                new Product { Id = 2, Name = "Product 2", Category = new Category { Name = "Category 2" }, CreatedByUser = new User { Username = "user2" } }
            };

      _mockProductRepository.Setup(x => x.GetProductsWithDetailsAsync())
          .ReturnsAsync(products);

      // Act
      var result = await _productService.GetAllProductsAsync();

      // Assert
      Assert.NotNull(result);
      Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetProductByIdAsync_WithValidId_ReturnsProduct()
    {
      // Arrange
      var product = new Product
      {
        Id = 1,
        Name = "Test Product",
        Category = new Category { Name = "Test Category" },
        CreatedByUser = new User { Username = "testuser" }
      };

      _mockProductRepository.Setup(x => x.GetProductWithDetailsAsync(1))
          .ReturnsAsync(product);

      // Act
      var result = await _productService.GetProductByIdAsync(1);

      // Assert
      Assert.NotNull(result);
      Assert.Equal(product.Id, result.Id);
      Assert.Equal(product.Name, result.Name);
    }

    [Fact]
    public async Task GetProductByIdAsync_WithInvalidId_ReturnsNull()
    {
      // Arrange
      _mockProductRepository.Setup(x => x.GetProductWithDetailsAsync(It.IsAny<int>()))
          .ReturnsAsync((Product)null);

      // Act
      var result = await _productService.GetProductByIdAsync(999);

      // Assert
      Assert.Null(result);
    }

    [Fact]
    public async Task CreateProductAsync_WithValidData_ReturnsProduct()
    {
      // Arrange
      var productCreateDto = new ProductCreateDto
      {
        Name = "Product Test",
        Description = "Product Description Test",
        StockQuantity = 10,
        Price = 99.99m,
        PictureUrl = "",
        CategoryId = 1
      };

      var category = new Category { Id = 1, Name = "Test Category" };
      var user = new User { Id = 1, Username = "testuser" };

      _mockCategoryRepository.Setup(x => x.GetByIdAsync(productCreateDto.CategoryId))
          .ReturnsAsync(category);
      _mockUserRepository.Setup(x => x.GetByIdAsync(1))
          .ReturnsAsync(user);
      _mockProductRepository.Setup(x => x.AddAsync(It.IsAny<Product>()));
      _mockProductRepository.Setup(x => x.SaveAllAsync()).ReturnsAsync(true);

      // Act
      var result = await _productService.CreateProductAsync(productCreateDto, 1);

      // Assert
      Assert.NotNull(result);
      Assert.Equal(productCreateDto.Name, result.Name);
      Assert.Equal(productCreateDto.Description, result.Description);
      Assert.Equal(category.Name, result.CategoryName);
    }

    [Fact]
    public async Task CreateProductAsync_WithInvalidCategory_ThrowsException()
    {
      // Arrange
      var productCreateDto = new ProductCreateDto { CategoryId = 999 };

      _mockCategoryRepository.Setup(x => x.GetByIdAsync(productCreateDto.CategoryId))
          .ReturnsAsync((Category)null);

      // Act & Assert
      await Assert.ThrowsAsync<ArgumentException>(() => _productService.CreateProductAsync(productCreateDto, 1));
    }

    [Fact]
    public async Task CreateProductAsync_WithInvalidUser_ThrowsException()
    {
      // Arrange
      var productCreateDto = new ProductCreateDto { CategoryId = 1 };

      _mockCategoryRepository.Setup(x => x.GetByIdAsync(productCreateDto.CategoryId))
          .ReturnsAsync(new Category());
      _mockUserRepository.Setup(x => x.GetByIdAsync(1))
          .ReturnsAsync((User)null);

      // Act & Assert
      await Assert.ThrowsAsync<ArgumentException>(() => _productService.CreateProductAsync(productCreateDto, 1));
    }

    [Fact]
    public async Task UpdateProductAsync_WithValidData_ReturnsUpdatedProduct()
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

      var existingProduct = new Product
      {
        Id = 1,
        Name = "Original Product",
        CreatedByUserId = 1
      };

      var category = new Category { Id = 2, Name = "Updated Category" };
      var user = new User { Id = 1, Username = "testuser" };

      _mockProductRepository.Setup(x => x.GetByIdAsync(1))
          .ReturnsAsync(existingProduct);
      _mockCategoryRepository.Setup(x => x.GetByIdAsync(productUpdateDto.CategoryId))
          .ReturnsAsync(category);
      _mockUserRepository.Setup(x => x.GetByIdAsync(1))
          .ReturnsAsync(user);
      _mockProductRepository.Setup(x => x.SaveAllAsync()).ReturnsAsync(true);

      // Act
      var result = await _productService.UpdateProductAsync(1, productUpdateDto);

      // Assert
      Assert.NotNull(result);
      Assert.Equal(productUpdateDto.Name, result.Name);
      Assert.Equal(productUpdateDto.Description, result.Description);
    }

    [Fact]
    public async Task UpdateProductAsync_WithInvalidProductId_ThrowsException()
    {
      // Arrange
      var productUpdateDto = new ProductUpdateDto();

      _mockProductRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
          .ReturnsAsync((Product)null);

      // Act & Assert
      await Assert.ThrowsAsync<KeyNotFoundException>(() => _productService.UpdateProductAsync(999, productUpdateDto));
    }

    [Fact]
    public async Task DeleteProductAsync_WithValidId_ReturnsTrue()
    {
      // Arrange
      var product = new Product { Id = 1 };

      _mockProductRepository.Setup(x => x.GetByIdAsync(1))
          .ReturnsAsync(product);
      _mockProductRepository.Setup(x => x.SaveAllAsync()).ReturnsAsync(true);

      // Act
      var result = await _productService.DeleteProductAsync(1);

      // Assert
      Assert.True(result);
    }

    [Fact]
    public async Task DeleteProductAsync_WithInvalidId_ThrowsException()
    {
      // Arrange
      _mockProductRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
          .ReturnsAsync((Product)null);

      // Act & Assert
      await Assert.ThrowsAsync<KeyNotFoundException>(() => _productService.DeleteProductAsync(999));
    }

    [Fact]
    public async Task GetLowStockProductsAsync_ReturnsLowStockProducts()
    {
      // Arrange
      var lowStockProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Low Stock Product", StockQuantity = 5, Category = new Category { Name = "Category 1" } }
            };

      _mockProductRepository.Setup(x => x.GetLowStockProductsAsync(10))
          .ReturnsAsync(lowStockProducts);

      // Act
      var result = await _productService.GetLowStockProductsAsync(10);

      // Assert
      Assert.NotNull(result);
      Assert.Single(result);
      Assert.Equal(5, result.First().StockQuantity);
    }
  }
}