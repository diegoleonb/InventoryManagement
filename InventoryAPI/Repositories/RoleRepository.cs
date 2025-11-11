using InventoryAPI.Data;
using InventoryAPI.Interfaces;
using InventoryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Repositories
{
  public class RoleRepository : Repository<Role>, IRoleRepository
  {
    public RoleRepository(ApplicationDbContext context) : base(context){}

    public async Task<Role> GetRoleByNameAsync(string roleName)
    {
      return await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
    }
  }
}