using InventoryAPI.Models;

namespace InventoryAPI.Interfaces
{
  public interface IProductRepository : IRepository<Product>
  {
    /// <summary>
    /// Gets all products with their details.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Product>> GetProductsWithDetailsAsync();
    /// <summary>
    /// Gets a product by its ID with details.
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    Task<Product> GetProductWithDetailsAsync(int productId);
    /// <summary>
    /// Gets products by category ID.
    /// </summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
    /// <summary>
    /// Gets products with stock below a certain threshold.
    /// </summary>
    /// <param name="threshold"></param>
    /// <returns></returns>
    Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold = 10);
  }
}