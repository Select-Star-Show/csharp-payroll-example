using System.Linq.Expressions;

namespace Payroll.Library;

public interface IRepository<TEntity, TId> where TEntity : class
{
    // CREATE
    Task<TEntity> AddAsync(TEntity entity);

    // READ
    Task<TEntity?> GetByIdAsync(TId id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

    // UPDATE
    Task UpdateAsync(TEntity entity);

    // DELETE
    Task DeleteAsync(TId id);

    // Optional: Save changes if needed outside unit-of-work
    Task<int> SaveChangesAsync();
}