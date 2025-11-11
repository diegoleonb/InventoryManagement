using InventoryAPI.Data;
using InventoryAPI.Interfaces;
using InventoryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Repositories
{
  public class ProductRepository : Repository<Product>, IProductRepository
  {
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Product>> GetProductsWithDetailsAsync()
    {
      return await _context.Products
          .Include(p => p.Category)
          .Include(p => p.CreatedByUser)
          .ThenInclude(u => u.Role)
          .ToListAsync();
    }

    public async Task<Product> GetProductWithDetailsAsync(int productId)
    {
      return await _context.Products
          .Include(p => p.Category)
          .Include(p => p.CreatedByUser)
          .ThenInclude(u => u.Role)
          .FirstOrDefaultAsync(p => p.Id == productId);
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
    {
      return await _context.Products
          .Include(p => p.Category)
          .Include(p => p.CreatedByUser)
          .ThenInclude(u => u.Role)
          .Where(p => p.CategoryId == categoryId)
          .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold = 10)
    {
      return await _context.Products
          .Include(p => p.Category)
          .Where(p => p.StockQuantity <= threshold)
          .ToListAsync();
    }
  }
}