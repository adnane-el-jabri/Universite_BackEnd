using System.Linq.Expressions;
namespace UniversiteDomain.DataAdapters
{
    public interface IRepository<T> where T : class
    {
        Task<T> CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(long id);
        Task DeleteAsync(T entity);
        Task<T?> FindAsync(long id);
        Task<T?> FindAsync(params object[] keyValues);
        Task<List<T>> FindByConditionAsync(Expression<Func<T, bool>> condition);
        Task<List<T>> FindAllAsync();
        Task SaveChangesAsync();
    }
}