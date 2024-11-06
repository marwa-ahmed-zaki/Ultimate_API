using System.Linq.Expressions;

namespace Ultimate_API.RepositoryPattern.GenericRepository
{
    public interface IGenericRepository<TEntity, TType> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(TType id);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TType id);
        Task<IEnumerable<TEntity>> GetAllIncludingAsync(params Expression<Func<TEntity, object>>[] includeProperties);


    }
}
