using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryAPI.Models
{

  /// <summary>
  /// Represents an item, including inventory and pricing information.
  /// </summary>
  /// <remarks>
  /// This entity is used by inventory management.
  /// </remarks>
  public class Product
  {
    /// <summary>
    /// Unique identifier of the product.
    /// </summary>
    [Key]
    public int Id { get; set; }
    [Required,StringLength(100)]
    /// <summary>
    /// Product name.
    /// </summary>
    public string Name { get; set; }
    [StringLength(250)]

    /// <summary>
    /// Detailed description of the product.
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// Quantity of the product in stock.
    /// </summary>
    [Required,Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }
    /// <summary>
    /// Price of the product.
    /// </summary>
    [Required,Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    /// <summary>
    /// Picture URL of the product.
    /// </summary>
    [StringLength(250)]
    public string PictureUrl { get; set; }
    /// <summary>
    /// Category identifier the product belongs to.
    /// </summary>
    public int CategoryId { get; set; }
    /// <summary>
    /// Category the product belongs to.
    /// </summary>
    public Category Category { get; set; }
    /// <summary>
    /// Creation timestamp of the product record.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// Last update timestamp of the product record.
    /// </summary>
    public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// User ID who created the product.
    /// </summary>
    public int? CreatedByUserId { get; set; }
    /// <summary>
    /// User who created the product.
    /// </summary>
    public User CreatedByUser { get; set; }
  }
}
