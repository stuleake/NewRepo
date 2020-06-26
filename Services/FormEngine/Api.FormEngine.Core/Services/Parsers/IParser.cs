using Api.FormEngine.Core.Services.Parsers.Models;

namespace Api.FormEngine.Core.Services.Parsers
{
    /// <summary>
    /// Defines an interface for parsing a model into a different model.
    /// </summary>
    /// <typeparam name="TModel">Model to parse.</typeparam>
    /// <typeparam name="TResult">Type to return.</typeparam>
    public interface IParser<in TModel, TResult>
        where TResult : class
    {
        /// <summary>
        /// Parses a <see cref="TModel"/> into a <see cref="TResult"/>.
        /// </summary>
        /// <param name="model">Model to parse</param>
        /// <returns>A <see cref="TResult"/> object</returns>
        ParseResult<TResult> Parse(TModel model);
    }
}