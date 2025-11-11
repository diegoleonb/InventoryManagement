using InventoryAPI.Data;
using InventoryAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InventoryAPI.Repositories
{
  public abstract class Repository<T> : IRepository<T> where T : class
  {
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    protected Repository(ApplicationDbContext context)
    {
      _context = context;
      _dbSet = context.Set<T>();
    }

    public virtual async Task<T> GetByIdAsync(int id)
    {
      return await _dbSet.FindAsync(id);
    }

    public virtual async Task<T> GetByIdAsync(string id)
    {
      return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
      return await _dbSet.ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
      return await _dbSet.Where(predicate).ToListAsync();
    }

    public virtual async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
      return await _dbSet.SingleOrDefaultAsync(predicate);
    }

    public virtual async Task AddAsync(T entity)
    {
      await _dbSet.AddAsync(entity);
    }

    public virtual void Update(T entity)
    {
      _dbSet.Attach(entity);
      _context.Entry(entity).State = EntityState.Modified;
    }

    public virtual void Remove(T entity)
    {
      _dbSet.Remove(entity);
    }

    public virtual async Task<bool> SaveAllAsync()
    {
      return await _context.SaveChangesAsync() > 0;
    }
  }
}