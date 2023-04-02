using System.Linq.Expressions;

namespace CliverSystem.Core.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> FindById<TId>(TId id);
        IQueryable<T> Find(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string includeProperties = "", bool trackChanges = true);
        Task<bool> Add(T entity);
        Task<bool> Upsert(T entity);
        Task<bool> Delete<TId>(TId id);
        void Delete(T entityToDelete);
    }
}
