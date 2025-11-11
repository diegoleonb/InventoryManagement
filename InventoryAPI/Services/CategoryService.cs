using InventoryAPI.DTOs;
using InventoryAPI.Interfaces;
using InventoryAPI.Models;

namespace InventoryAPI.Services
{
  public class CategoryService : ICategoryService
  {
    private readonly ICategoryRepository _categoryRepository;

    /// <summary>
    /// Constructor for CategoryService
    /// </summary>
    /// <param name="categoryRepository"></param>
    public CategoryService(ICategoryRepository categoryRepository)
    {
      _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
    {
      var categories = await _categoryRepository.GetCategoriesWithProductsAsync();
      return categories.Select(MapToCategoryDto);
    }

    public async Task<CategoryDto> GetCategoryByIdAsync(int id)
    {
      var category = await _categoryRepository.GetCategoryWithProductsAsync(id);
      if (category == null) return null;

      return MapToCategoryDto(category);
    }

    public async Task<CategoryDto> CreateCategoryAsync(CategoryCreateDto categoryCreateDto)
    {
      var category = new Category
      {
        Name = categoryCreateDto.Name
      };

      await _categoryRepository.AddAsync(category);
      await _categoryRepository.SaveAllAsync();

      return MapToCategoryDto(category);
    }

    public async Task<CategoryDto> UpdateCategoryAsync(int id, CategoryUpdateDto categoryUpdateDto)
    {
      var category = await _categoryRepository.GetByIdAsync(id);
      if (category == null)
        throw new KeyNotFoundException("Category not found");

      category.Name = categoryUpdateDto.Name;

      _categoryRepository.Update(category);
      await _categoryRepository.SaveAllAsync();

      await _categoryRepository.GetCategoryWithProductsAsync(id);

      return MapToCategoryDto(category);
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
      var category = await _categoryRepository.GetCategoryWithProductsAsync(id);
      if (category == null)
        throw new KeyNotFoundException("Category not found");

      if (category.Products.Any())
        throw new InvalidOperationException("Cannot delete category with existing products");

      _categoryRepository.Remove(category);
      return await _categoryRepository.SaveAllAsync();
    }

    private CategoryDto MapToCategoryDto(Category category)
    {
      return new CategoryDto
      {
        Id = category.Id,
        Name = category.Name,
        ProductCount = category.Products?.Count ?? 0
      };
    }
  }
}