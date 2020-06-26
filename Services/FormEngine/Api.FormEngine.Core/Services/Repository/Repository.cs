using System.Collections.Generic;
using System.Threading.Tasks;
using TQ.Core.Repository;
using TQ.Data.FormEngine;

namespace Api.FormEngine.Core.Services.Repository
{
    /// <summary>
    /// Repository for accessing read/write operations of the forms engine database.
    /// </summary>
    /// <typeparam name="TEntity">Entity to query.</typeparam>
    public class Repository<TEntity> : ReadOnlyRepository<TEntity>, IRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TEntity}"/> class.
        /// </summary>
        /// <param name="context">DbContext to query.</param>
        public Repository(FormsEngineContext context) : base(context)
        {
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(TEntity entity)
        {
            dbContext.Set<TEntity>().Update(entity);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task AddAsync(TEntity entity)
        {
            dbContext.Set<TEntity>().Add(entity);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            dbContext.Set<TEntity>().AddRange(entities);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            dbContext.Set<TEntity>().UpdateRange(entities);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(TEntity entity)
        {
            dbContext.Set<TEntity>().Remove(entity);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task DeleteAllAsync(IEnumerable<TEntity> entities)
        {
            dbContext.Set<TEntity>().RemoveRange(entities);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}