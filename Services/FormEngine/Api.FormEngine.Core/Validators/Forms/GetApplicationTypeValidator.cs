using Api.FormEngine.Core.Commands.Forms;
using FluentValidation;

namespace Api.FormEngine.Core.Validators.Forms
{
    /// <summary>
    /// Validation Rules for GetApplicationType Command
    /// </summary>
    public class GetApplicationTypeValidator : AbstractValidator<GetApplicationType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetApplicationTypeValidator"/> class with rules.
        /// </summary>
        public GetApplicationTypeValidator()
        {
            RuleFor(command => command.Country)
                 .NotEmpty().WithMessage(command => $"{nameof(command.Country)} is not specified");

            RuleFor(command => command.Product)
                .NotEmpty().WithMessage(command => $"{nameof(command.Product)} is not specified");
        }
    }
}