using System.Collections.Generic;
using System.Threading.Tasks;

namespace TQ.Core.Repository
{
    /// <summary>
    /// Generic repository for reading and writing data to and from a database.
    /// </summary>
    /// <typeparam name="TEntity">Model being accessed.</typeparam>
    public interface IRepository<TEntity> : IReadOnlyRepository<TEntity>
       where TEntity : class
    {
        /// <summary>
        /// Inserts the entry into the database.
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        /// <returns>A <see cref="Task"/> for awaiting data insertion.</returns>
        Task AddAsync(TEntity entity);

        /// <summary>
        /// Inserts all entries into the database.
        /// </summary>
        /// <param name="entities">Entities to update.</param>
        /// <returns>A <see cref="Task"/> for awaiting data insertion.</returns>
        Task AddRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Updates the provided entity in the database.
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        /// <returns>A <see cref="Task"/> for awaiting data updates.</returns>
        Task UpdateAsync(TEntity entity);

        /// <summary>
        /// Updates the provided entities in the database.
        /// </summary>
        /// <param name="entities">Entities to update.</param>
        /// <returns>A <see cref="Task"/> for awaiting data updates.</returns>
        Task UpdateRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Deletes the provided entity from the database.
        /// </summary>
        /// <param name="entity">Entity to delete.</param>
        /// <returns>A <see cref="Task"/> for awaiting data deletion.</returns>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// Deletes all provided entities from the database.
        /// </summary>
        /// <param name="entities">Entities to delete.</param>
        /// <returns>A <see cref="Task"/> for awaiting data deletion.</returns>
        Task DeleteAllAsync(IEnumerable<TEntity> entities);
    }
}