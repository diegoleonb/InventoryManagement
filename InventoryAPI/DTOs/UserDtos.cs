namespace InventoryAPI.DTOs
{
  /// <summary>
  /// User Login Data Transfer Object
  /// </summary>
  public class UserLoginDto
  {
    public string Username { get; set; }
    public string Password { get; set; }
  }

  /// <summary>
  /// User Registration Data Transfer Object
  /// </summary>
  public class UserRegisterDto
  {
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string RoleId { get; set; }
  }

  /// <summary>
  /// Response Data Transfer Object for User after authentication
  /// </summary>
  public class UserResponseDto
  {
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string RoleId { get; set; }
    public string RoleName { get; set; }
    public string Token { get; set; }
  }
}