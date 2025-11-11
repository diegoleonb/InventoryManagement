using InventoryAPI.DTOs;

namespace InventoryAPI.Interfaces
{
  public interface IProductService
  {
    /// <summary>
    /// Gets all products.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<ProductDto>> GetAllProductsAsync();
    /// <summary>
    /// Gets a product by its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ProductDto> GetProductByIdAsync(int id);
    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="productCreateDto"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<ProductDto> CreateProductAsync(ProductCreateDto productCreateDto, int userId);
    /// <summary>
    /// Updates an existing product.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="productUpdateDto"></param>
    /// <returns></returns>
    Task<ProductDto> UpdateProductAsync(int id, ProductUpdateDto productUpdateDto);
    /// <summary>
    /// Deletes a product by its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> DeleteProductAsync(int id);
    /// <summary>
    /// Gets products by category ID.
    /// </summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId);
    /// <summary>
    /// Gets products with stock below a certain threshold.
    /// </summary>
    /// <param name="threshold"></param>
    /// <returns></returns>
    Task<IEnumerable<ProductDto>> GetLowStockProductsAsync(int threshold = 10);
  }
}