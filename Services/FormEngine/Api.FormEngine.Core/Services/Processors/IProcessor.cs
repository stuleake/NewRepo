using System.Threading.Tasks;

namespace Api.FormEngine.Core.Services.Processors
{
    /// <summary>
    /// Defines an interface to process a data model.
    /// </summary>
    /// <typeparam name="TModel">Model to process</typeparam>
    public interface IProcessor<in TModel>
        where TModel : class
    {
        /// <summary>
        /// Processes the provided <see cref="TModel"/>
        /// </summary>
        /// <param name="entity">Model to process.</param>
        /// <returns>A <see cref="Task"/> when the process is complete.</returns>
        Task ProcessAsync(TModel entity);
    }
}