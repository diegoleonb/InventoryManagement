namespace InventoryAPI.DTOs
{
  /// <summary>
  /// Product Data Transfer Object
  /// </summary>
  public class ProductDto
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int StockQuantity { get; set; }
    public decimal Price { get; set; }
    public string PictureUrl { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public string CreatedByUser { get; set; }
  }

  /// <summary>
  /// product Creation Data Transfer Object
  /// </summary>
  public class ProductCreateDto
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public int StockQuantity { get; set; }
    public decimal Price { get; set; }
    public string PictureUrl { get; set; }
    public int CategoryId { get; set; }
  }

  /// <summary>
  /// Product Update Data Transfer Object
  /// </summary>
  public class ProductUpdateDto
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public int StockQuantity { get; set; }
    public decimal Price { get; set; }
    public string PictureUrl { get; set; }
    public int CategoryId { get; set; }
  }
}