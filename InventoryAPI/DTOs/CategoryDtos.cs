namespace InventoryAPI.DTOs
{
  /// <summary>
  /// Category Data Transfer Object
  /// </summary>
  public class CategoryDto
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int ProductCount { get; set; }
  }

  /// <summary>
  /// Category Creation Data Transfer Object
  /// </summary>
  public class CategoryCreateDto
  {
    public string Name { get; set; }
  }

  /// <summary>
  /// Category Update Data Transfer Object
  /// </summary>
  public class CategoryUpdateDto
  {
    public string Name { get; set; }
  }
}
