using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InventoryAPI.Models
{
  /// <summary>
  /// Category of products.
  /// </summary>
  public class Category
  {
    /// <summary>
    /// Primary key identifier for the category.
    /// </summary>
    [Key]
    public int Id { get; set; }
    /// <summary>
    /// Name of the category.
    /// </summary>
    [Required,StringLength(100)]
    public string Name { get; set; }
    /// <summary>
    /// Products associated with this category.
    /// </summary>
    public ICollection<Product> Products { get; set; }
  }
}
