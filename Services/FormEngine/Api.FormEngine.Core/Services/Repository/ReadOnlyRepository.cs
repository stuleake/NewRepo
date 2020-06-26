using Microsoft.EntityFrameworkCore;
using System.Linq;
using TQ.Core.Repository;
using TQ.Data.FormEngine;

namespace Api.FormEngine.Core.Services.Repository
{
    /// <summary>
    /// Read only repository for querying the forms engine database.
    /// </summary>
    /// <typeparam name="TEntity">Entity to query.</typeparam>
    public class ReadOnlyRepository<TEntity> : IReadOnlyRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Instance of the database context.
        /// </summary>
        protected readonly FormsEngineContext dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyRepository{TEntity}"/> class.
        /// </summary>
        /// <param name="context">Database context to query.</param>
        public ReadOnlyRepository(FormsEngineContext context)
        {
            this.dbContext = context;
        }

        /// <inheritdoc/>
        public IQueryable<TEntity> GetLocalQueryable()
        {
            return dbContext.Set<TEntity>()
                .Local
                .AsQueryable();
        }

        /// <inheritdoc/>
        public IQueryable<TEntity> GetQueryable()
        {
            return dbContext.Set<TEntity>()
                .AsNoTracking()
                .AsQueryable();
        }
    }
}