using InventoryAPI.DTOs;
using InventoryAPI.Interfaces;
using InventoryAPI.Models;

namespace InventoryAPI.Services
{
  /// <summary>
  /// Service for managing products.
  /// </summary>
  public class ProductService : IProductService
  {
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUserRepository _userRepository;

    public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository, IUserRepository userRepository)
    {
      _productRepository = productRepository;
      _categoryRepository = categoryRepository;
      _userRepository = userRepository;
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
      var products = await _productRepository.GetProductsWithDetailsAsync();
      return products.Select(MapToProductDto);
    }

    public async Task<ProductDto> GetProductByIdAsync(int id)
    {
      var product = await _productRepository.GetProductWithDetailsAsync(id);
      if (product == null) return null;

      return MapToProductDto(product);
    }

    public async Task<ProductDto> CreateProductAsync(ProductCreateDto productCreateDto, int userId)
    {
      // Validate category exists
      var category = await _categoryRepository.GetByIdAsync(productCreateDto.CategoryId);
      if (category == null)
        throw new ArgumentException("Category not found");

      // Validate user exists
      var user = await _userRepository.GetByIdAsync(userId);
      if (user == null)
        throw new ArgumentException("User not found");

      var product = new Product
      {
        Name = productCreateDto.Name,
        Description = productCreateDto.Description,
        StockQuantity = productCreateDto.StockQuantity,
        Price = productCreateDto.Price,
        CategoryId = productCreateDto.CategoryId,
        CreatedByUserId = userId,
        CreatedAt = DateTime.UtcNow,
        LastUpdatedAt = DateTime.UtcNow
      };

      await _productRepository.AddAsync(product);
      await _productRepository.SaveAllAsync();

      // Load related data for response
      product.Category = category;
      product.CreatedByUser = user;

      return MapToProductDto(product);
    }

    public async Task<ProductDto> UpdateProductAsync(int id, ProductUpdateDto productUpdateDto)
    {
      var product = await _productRepository.GetByIdAsync(id);
      if (product == null)
        throw new KeyNotFoundException("Product not found");

      // Validate category exists
      var category = await _categoryRepository.GetByIdAsync(productUpdateDto.CategoryId);
      if (category == null)
        throw new ArgumentException("Category not found");

      product.Name = productUpdateDto.Name;
      product.Description = productUpdateDto.Description;
      product.StockQuantity = productUpdateDto.StockQuantity;
      product.Price = productUpdateDto.Price;
      product.CategoryId = productUpdateDto.CategoryId;
      product.LastUpdatedAt = DateTime.UtcNow;

      _productRepository.Update(product);
      await _productRepository.SaveAllAsync();

      // Load related data for response
      product.Category = category;
      product.CreatedByUser = await _userRepository.GetByIdAsync(product.CreatedByUserId.Value);

      return MapToProductDto(product);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
      var product = await _productRepository.GetByIdAsync(id);
      if (product == null)
        throw new KeyNotFoundException("Product not found");

      _productRepository.Remove(product);
      return await _productRepository.SaveAllAsync();
    }

    public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId)
    {
      var products = await _productRepository.GetProductsByCategoryAsync(categoryId);
      return products.Select(MapToProductDto);
    }

    public async Task<IEnumerable<ProductDto>> GetLowStockProductsAsync(int threshold = 10)
    {
      var products = await _productRepository.GetLowStockProductsAsync(threshold);
      return products.Select(MapToProductDto);
    }

    private ProductDto MapToProductDto(Product product)
    {
      return new ProductDto
      {
        Id = product.Id,
        Name = product.Name,
        Description = product.Description,
        StockQuantity = product.StockQuantity,
        Price = product.Price,
        CategoryId = product.CategoryId,
        CategoryName = product.Category?.Name,
        CreatedAt = product.CreatedAt,
        LastUpdatedAt = product.LastUpdatedAt,
        CreatedByUser = product.CreatedByUser?.Username
      };
    }
  }
}