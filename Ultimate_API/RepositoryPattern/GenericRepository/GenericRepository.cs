namespace Ultimate_API.RepositoryPattern.GenericRepository
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Ultimate_API.ApplicationDbContext;
    using Ultimate_API.Models;

    public class GenericRepository<TEntity, TType> : IGenericRepository<TEntity, TType> where TEntity : class
    {

        private readonly UltimateContext _context;
        private readonly DbSet<TEntity> _dbSet;


        public GenericRepository(UltimateContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(TType id)
        {
            return await _dbSet.FindAsync(id);
        }
        //public async Task<TEntity> _GetByIdAsync(TType id)
        //{
        //    if (id is int)
        //    {
        //        return await _dbSet.FindAsync(id);
        //    }
        //    else if (id is string)
        //    {
        //        return await _dbSet.FindAsync(id);
        //    }
        //    else
        //    {
        //        // Handle other cases if needed, or throw an exception
        //        throw new ArgumentException("Unsupported ID type");
        //    }
        //}



        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TType id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TEntity>> GetAllIncludingAsync(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _dbSet;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.ToListAsync();
        }
    }
}
