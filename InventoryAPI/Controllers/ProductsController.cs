using InventoryAPI.DTOs;
using InventoryAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAPI.Controllers
{
  /// <summary>
  /// Controller for managing products.
  /// </summary>
  [ApiController]
  [Route("api/[controller]")]
  [Authorize]
  public class ProductsController : ControllerBase
  {
    private readonly IProductService _productService;
    private readonly IAuthService _authService;

    public ProductsController(IProductService productService, IAuthService authService)
    {
      _productService = productService;
      _authService = authService;
    }

    /// <summary>
    /// Gets all products.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "Viewer,Operator,Administrator")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
    {
      var products = await _productService.GetAllProductsAsync();
      return Ok(products);
    }

    /// <summary>
    /// Gets a product by its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [Authorize(Roles = "Viewer,Operator,Administrator")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
      var product = await _productService.GetProductByIdAsync(id);
      if (product == null) return NotFound();
      return Ok(product);
    }

    /// <summary>
    /// Gets products by category ID.
    /// </summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    [HttpGet("category/{categoryId}")]
    [Authorize(Roles = "Viewer,Operator,Administrator")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByCategory(int categoryId)
    {
      var products = await _productService.GetProductsByCategoryAsync(categoryId);
      return Ok(products);
    }

    [HttpGet("low-stock")]
    [Authorize(Roles = "Viewer,Operator,Administrator")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetLowStockProducts([FromQuery] int threshold = 10)
    {
      var products = await _productService.GetLowStockProductsAsync(threshold);
      return Ok(products);
    }

    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="productCreateDto"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "Operator,Administrator")]
    public async Task<ActionResult<ProductDto>> CreateProduct(ProductCreateDto productCreateDto)
    {
      try
      {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
          return Unauthorized();

        var userId = int.Parse(userIdClaim);
        var product = await _productService.CreateProductAsync(productCreateDto, userId);
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }


    /// <summary>
    /// Updates an existing product.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="productUpdateDto"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [Authorize(Roles = "Operator,Administrator")]
    public async Task<ActionResult<ProductDto>> UpdateProduct(int id, ProductUpdateDto productUpdateDto)
    {
      try
      {
        var product = await _productService.UpdateProductAsync(id, productUpdateDto);
        return Ok(product);
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
    /// Deletes a product by its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
      try
      {
        await _productService.DeleteProductAsync(id);
        return NoContent();
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
  }
}