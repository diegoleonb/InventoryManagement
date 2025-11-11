using InventoryAPI.Data;
using InventoryAPI.Interfaces;
using InventoryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Repositories
{
  public class CategoryRepository : Repository<Category>, ICategoryRepository
  {
    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Category> GetCategoryWithProductsAsync(int categoryId)
    {
      return await _context.Categories
          .Include(c => c.Products)
          .FirstOrDefaultAsync(c => c.Id == categoryId);
    }

    public async Task<IEnumerable<Category>> GetCategoriesWithProductsAsync()
    {
      return await _context.Categories
          .Include(c => c.Products)
          .ToListAsync();
    }
  }
}