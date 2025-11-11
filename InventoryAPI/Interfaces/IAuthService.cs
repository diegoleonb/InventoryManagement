using InventoryAPI.DTOs;
using InventoryAPI.Models;

namespace InventoryAPI.Interfaces
{
  public interface IAuthService
  {
    /// <summary>
    /// Register a new user.
    /// </summary>
    /// <param name="registerDto"></param>
    /// <returns></returns>
    Task<UserResponseDto> Register(UserRegisterDto registerDto);
    /// <summary>
    /// Login an existing user.
    /// </summary>
    /// <param name="loginDto"></param>
    /// <returns></returns>
    Task<UserResponseDto> Login(UserLoginDto loginDto);
    /// <summary>
    /// Generate JWT token for a user.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    string GenerateJwtToken(User user);
    /// <summary>
    /// Returns true if a user with the given username or email already exists.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<bool> UserExists(string username, string email);
    /// <summary>
    /// Get User ID from JWT token.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    int? GetUserIdFromToken(string token);
  }
}