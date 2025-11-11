using InventoryAPI.Data;
using InventoryAPI.Interfaces;
using InventoryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Repositories
{
  public class UserRepository : Repository<User>, IUserRepository
  {
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
      return await _context.Users
          .Include(u => u.Role)
          .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
      return await _context.Users
          .Include(u => u.Role)
          .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> UserExists(string username, string email)
    {
      return await _context.Users.AnyAsync(u => u.Username == username || u.Email == email);
    }

    public async Task<IEnumerable<User>> GetUsersWithRolesAsync()
    {
      return await _context.Users
          .Include(u => u.Role)
          .ToListAsync();
    }
  }
}