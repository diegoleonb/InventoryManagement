using InventoryAPI.DTOs;

namespace InventoryAPI.Interfaces
{
  public interface ICategoryService
  {
    /// <summary>
    /// Gets all categories.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
    /// <summary>
    /// Gets a category by its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<CategoryDto> GetCategoryByIdAsync(int id);
    /// <summary>
    /// Creates a new category.
    /// </summary>
    /// <param name="categoryCreateDto"></param>
    /// <returns></returns>
    Task<CategoryDto> CreateCategoryAsync(CategoryCreateDto categoryCreateDto);
    /// <summary>
    /// Updates an existing category.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="categoryUpdateDto"></param>
    /// <returns></returns>
    Task<CategoryDto> UpdateCategoryAsync(int id, CategoryUpdateDto categoryUpdateDto);
    /// <summary>
    /// Deletes a category by its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> DeleteCategoryAsync(int id);
  }
}