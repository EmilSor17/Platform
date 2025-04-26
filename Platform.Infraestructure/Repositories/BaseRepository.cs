using Microsoft.EntityFrameworkCore;
using Platform.Core.Entities;
using Platform.Core.Interfaces;
using Platform.Infraestructure.Data;

namespace Platform.Infraestructure.Repositories;

public class BaseRepository<T> : IRepository<T> where T : BaseEntity
{
  protected readonly PlatformContext _context;
  public BaseRepository(PlatformContext context)
  {
    _context = context;
  }
  public virtual async Task<IEnumerable<T>> GetAllAsync()
  {
    return await _context.Set<T>().ToListAsync();
  }
  public async Task<T> GetByIdAsync(int id)
  {
    return await _context.Set<T>().FindAsync(id);
  }
  public virtual async Task<T> AddAsync(T entity)
  {
    await _context.Set<T>().AddAsync(entity);
    return entity;
  }
  public virtual T Update(T entity)
  {
    _context.Set<T>().Update(entity);
    return entity;
  }
  public async Task DeleteAsync(int id)
  {
    var entity = await _context.Set<T>().FindAsync(id);
    if (entity != null)
      _context.Set<T>().Remove(entity);
  }
  public async Task DisableAsync(int id)
  {
    var entity = await _context.Set<T>().FindAsync(id);
    if (entity != null)
      entity.Status = false;
      _context.Set<T>().Update(entity);
  }
  public virtual IQueryable<T> Query => _context.Set<T>();
}
