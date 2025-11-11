using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace InventoryAPI.Models
{
  /// <summary>
  /// Represents an application user with credentials and role membership.
  /// </summary>
  /// <remarks>
  /// This class defines the properties required to manage user authentication
  /// </remarks>
  public class User
  {
    /// <summary>
    /// Unique identifier of the user.
    /// </summary>
    [Key]
    public int Id { get; set; }
    /// <summary>
    /// Username of the user for login purposes.
    /// </summary>
    [Required,StringLength(50)]
    public string Username { get; set; }
    /// <summary>
    /// Email address of the user.
    /// </summary>
    [Required,StringLength(100)]
    public string Email { get; set; }
    /// <summary>
    /// Password hash for secure authentication.
    /// </summary>
    [Required]
    public byte[] PasswordHash { get; set; }
    /// <summary>
    /// Password salt used in hashing the password.
    /// </summary>
    [Required]
    public byte[] PasswordSalt { get; set; }
    /// <summary>
    /// Role identifier linking to the user's role.
    /// </summary>
    [Required]
    public string RoleId { get; set; }
    /// <summary>
    /// Role associated with the user.
    /// </summary>
    [Required]
    public Role Role { get; set; }
  }
}
