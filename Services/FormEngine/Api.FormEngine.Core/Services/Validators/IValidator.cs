namespace Api.FormEngine.Core.Services.Validators
{
    /// <summary>
    /// Interface to perform validation of the specified type.
    /// </summary>
    /// <typeparam name="TModel">Model to validate.</typeparam>
    public interface IValidator<TModel>
        where TModel : class
    {
        /// <summary>
        /// Returns a <see cref="ValidationResult{TModel}"/> describing the validity of the provided model.
        /// </summary>
        /// <param name="model">Model to validate.</param>
        /// <returns>Validation status of the model.</returns>
        ValidationResult<TModel> Validate(TModel model);
    }
}