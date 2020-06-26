using System.Collections.Generic;

namespace Api.FormEngine.Core.Services.Validators
{
    /// <summary>
    /// Class indicating the status of a validation, containing the status and error messages, if applicable. This class is immutable.
    /// </summary>
    ///  <typeparam name="TModel">Model that was validated.</typeparam>
    public sealed class ValidationResult<TModel>
        where TModel : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult{TModel}"/> class, with the model flagged as valid.
        /// </summary>
        /// <param name="model">Model that was validated</param>
        public ValidationResult(TModel model)
        {
            IsValid = true;
            Model = model;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult{TModel}"/> class, with the model flagged as invalid and error messages attached.
        /// </summary>
        /// <param name="model">Model that was validated.</param>
        /// <param name="messages">Error messages attached to the model.</param>
        public ValidationResult(TModel model, IEnumerable<string> messages)
        {
            IsValid = false;
            Messages = new List<string>(messages).AsReadOnly();
            Model = model;
        }

        /// <summary>
        /// Gets a value indicating whether the model is valid or not.
        /// </summary>
        public bool IsValid { get; private set; }

        /// <summary>
        /// Gets the messages generated during validation.
        /// </summary>
        public IReadOnlyCollection<string> Messages { get; private set; }

        /// <summary>
        /// Gets the model that was validated.
        /// </summary>
        public TModel Model { get; private set; }
    }
}