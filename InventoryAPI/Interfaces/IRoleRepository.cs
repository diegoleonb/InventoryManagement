using InventoryAPI.Models;

namespace InventoryAPI.Interfaces
{
  public interface IRoleRepository : IRepository<Role>
  {
    /// <summary>
    /// Gets a role by its name.
    /// </summary>
    /// <param name="roleName"></param>
    /// <returns></returns>
    Task<Role> GetRoleByNameAsync(string roleName);
  }
}