namespace Api.FormEngine.Core.Services.Parsers.Models
{
    /// <summary>
    /// Object representing a collection of results returned from a parse operation.
    /// </summary>
    /// <typeparam name="TModel">Model of the results</typeparam>
    public class ParseResult<TModel>
    {
        /// <summary>
        /// Gets or sets the collection of <see cref="TModel"/> that has been added.
        /// </summary>
        public TModel Added { get; set; } = default;

        /// <summary>
        /// Gets or sets the collection of <see cref="TModel"/> that has been updated.
        /// </summary
        public TModel Updated { get; set; } = default;

        /// <summary>
        /// Gets or sets the collection of <see cref="TModel"/> that has been deleted.
        /// </summary
        public TModel Deleted { get; set; } = default;
    }
}