using System.Linq;

namespace TQ.Core.Repository
{
    /// <summary>
    /// Generic repository for reading data from a database.
    /// </summary>
    /// <typeparam name="TEntity">Model being accessed.</typeparam>
    public interface IReadOnlyRepository<out TEntity>
         where TEntity : class
    {
        /// <summary>
        /// Returns an <see cref="IQueryable"/> set of database entries of the provided <see cref="TEntity"/>
        /// </summary>
        /// <returns>An <see cref="IQueryable"/> instance.</returns>
        IQueryable<TEntity> GetQueryable();

        /// <summary>
        /// Returns an <see cref="IQueryable"/> set of local entries of the provided <see cref="TEntity"/>
        /// </summary>
        /// <returns>An <see cref="IQueryable"/> instance.</returns>
        IQueryable<TEntity> GetLocalQueryable();
    }
}