using InventoryAPI.Models;

namespace InventoryAPI.Interfaces
{
  public interface ICategoryRepository : IRepository<Category>
  {
    /// <summary>
    /// Gets a category by its ID including its products.
    /// </summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    Task<Category> GetCategoryWithProductsAsync(int categoryId);
    /// <summary>
    /// Gets all categories including their products.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Category>> GetCategoriesWithProductsAsync();
  }
}