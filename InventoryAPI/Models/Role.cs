using System.ComponentModel.DataAnnotations;

namespace InventoryAPI.Models
{
  /// <summary>
  /// Role assigned to users for authorization purposes.
  /// </summary>
  public class Role
  {
    /// <summary>
    /// Identifier of the role.
    /// </summary>
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    /// <summary>
    /// Name of the role.
    /// </summary>

    [Required,StringLength(50)]
    public string Name { get; set; }
    /// <summary>
    /// Users associated with this role.
    /// </summary>

    public ICollection<User> Users { get; set; }
  }
}