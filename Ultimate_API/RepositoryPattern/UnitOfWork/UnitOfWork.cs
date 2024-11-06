using Microsoft.EntityFrameworkCore;
using System.Collections;
using Ultimate_API.Models;
using Ultimate_API.RepositoryPattern.GenericRepository;

namespace Ultimate_API.RepositoryPattern.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UltimateContext _context;
        //private Hashtable _repositories;
        private readonly IDictionary<string, object> _repositories = new Dictionary<string, object>();



        public UnitOfWork(UltimateContext context)
        {
            _context = context;
        }

        public UltimateContext DbContext { get { return _context; } }

        public IGenericRepository<TEntity, TType> Repository<TEntity, TType>() where TEntity : class
        {
            var entityTypeName = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(entityTypeName))
            {
                // Use an open generic type here
                var repositoryType = typeof(GenericRepository<,>);

                // Use MakeGenericType to specify the generic type arguments
                var repositoryInstance = Activator.CreateInstance(
                    repositoryType.MakeGenericType(typeof(TEntity), typeof(TType)), _context);

                _repositories.Add(entityTypeName, repositoryInstance);
            }

            return (IGenericRepository<TEntity, TType>)_repositories[entityTypeName];
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

      
    }
}
