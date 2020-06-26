using Api.FormEngine.Core.Commands.Forms;
using FluentValidation;
using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TQ.Data.FormEngine;

namespace Api.FormEngine.Core.Validators.Forms
{
    /// <summary>
    /// Validation Rules for CreateQuestionSetResponseValidator Command
    /// </summary>
    public class CreateQuestionSetResponseValidator : AbstractValidator<CreateQuestionSetResponse>
    {
        private readonly FormsEngineContext formsEngineContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateQuestionSetResponseValidator"/> class with rules.
        /// </summary>
        /// <param name="formsEngineContext">object of formsEngineContext for validation against database</param>
        public CreateQuestionSetResponseValidator(FormsEngineContext formsEngineContext)
        {
            this.formsEngineContext = formsEngineContext;

            RuleFor(command => command.Country)
                 .NotEmpty().WithMessage(command => $"{nameof(command.Country)} is not specified");

            RuleFor(command => command.Product)
                .NotEmpty().WithMessage(command => $"{nameof(command.Product)} is not specified");

            RuleFor(command => command.Response)
                .NotEmpty().WithMessage(command => $"{nameof(command.Response)} is empty");

            RuleFor(command => command.ApplicationName)
               .NotEmpty().WithMessage(command => $"{nameof(command.ApplicationName)} is not specified");

            RuleFor(command => command.QuestionSetId).CustomAsync((questionSetId, context, cancellationToken) => ValidateQuestionSetIdAsync(questionSetId, context));
        }

        private async Task ValidateQuestionSetIdAsync(Guid questionSetId, CustomContext context)
        {
            var questionSet = await formsEngineContext.QS.FirstOrDefaultAsync(qs => qs.QSId.Equals(questionSetId)).ConfigureAwait(false);

            if (questionSet == null)
            {
                context.AddFailure("Invalid question set Id.");
            }
        }
    }
}