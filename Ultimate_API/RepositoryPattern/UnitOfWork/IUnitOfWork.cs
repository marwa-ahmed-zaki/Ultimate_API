using Microsoft.EntityFrameworkCore;
using Ultimate_API.RepositoryPattern.GenericRepository;

namespace Ultimate_API.RepositoryPattern.UnitOfWork
{
   

    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity, TType> Repository<TEntity, TType>() where TEntity : class;

        Task<int> CompleteAsync();
    }
}
