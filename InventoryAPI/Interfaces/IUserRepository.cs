using InventoryAPI.Models;

namespace InventoryAPI.Interfaces
{
  public interface IUserRepository : IRepository<User>
  {
    /// <summary>
    /// Gets a user by their username.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    Task<User> GetUserByUsernameAsync(string username);
    /// <summary>
    /// Gets a user by their email.
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<User> GetUserByEmailAsync(string email);
    /// <summary>
    /// Gets whether a user with the given username or email exists.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<bool> UserExists(string username, string email);
    /// <summary>
    /// Gets all users along with their roles.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<User>> GetUsersWithRolesAsync();
  }
}