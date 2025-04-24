using Platform.Core.Entities;

namespace Platform.Core.Interfaces
{
  public interface IRepository<T> where T : BaseEntity
  {
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<T> AddAsync(T entity);
    T Update(T entity);
    Task DeleteAsync(int id);
    Task DisableAsync(int id);
    IQueryable<T> Query { get; }

  }
}
