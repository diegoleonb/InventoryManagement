using InventoryAPI.DTOs;
using InventoryAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAPI.Controllers
{
  /// <summary>
  /// Controller for managing categories.
  /// </summary>
  [ApiController]
  [Route("api/[controller]")]
  [Authorize]
  public class CategoriesController : ControllerBase
  {
    private readonly ICategoryService _categoryService;

    /// <summary>
    /// Constructor for CategoriesController
    /// </summary>
    /// <param name="categoryService"></param>
    public CategoriesController(ICategoryService categoryService)
    {
      _categoryService = categoryService;
    }

    /// <summary>
    /// Gets all categories.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "Viewer,Operator,Administrator")]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
    {
      var categories = await _categoryService.GetAllCategoriesAsync();
      return Ok(categories);
    }

    /// <summary>
    /// Gets a category by its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [Authorize(Roles = "Viewer,Operator,Administrator")]
    public async Task<ActionResult<CategoryDto>> GetCategory(int id)
    {
      var category = await _categoryService.GetCategoryByIdAsync(id);
      if (category == null) return NotFound();
      return Ok(category);
    }

    /// <summary>
    /// Creates a new category.
    /// </summary>
    /// <param name="categoryCreateDto"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<CategoryDto>> CreateCategory(CategoryCreateDto categoryCreateDto)
    {
      try
      {
        var category = await _categoryService.CreateCategoryAsync(categoryCreateDto);
        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    /// <summary>
    /// Updates an existing category.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="categoryUpdateDto"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<CategoryDto>> UpdateCategory(int id, CategoryUpdateDto categoryUpdateDto)
    {
      try
      {
        var category = await _categoryService.UpdateCategoryAsync(id, categoryUpdateDto);
        return Ok(category);
      }
      catch (KeyNotFoundException)
      {
        return NotFound();
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    /// <summary>
    /// Deletes a category by its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult> DeleteCategory(int id)
    {
      try
      {
        await _categoryService.DeleteCategoryAsync(id);
        return NoContent();
      }
      catch (KeyNotFoundException)
      {
        return NotFound();
      }
      catch (InvalidOperationException ex)
      {
        return BadRequest(ex.Message);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }
  }
}