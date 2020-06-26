using Api.FormEngine.Core.Commands.Forms;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Api.FormEngine.Examples
{
    /// <summary>
    /// A class to get ValidateQuestionSet Response
    /// </summary>
    public class ValidateQuestionSetResponseExample : IExamplesProvider<ValidateQuestionSetResponse>
    {
        /// <inheritdoc/>
        public ValidateQuestionSetResponse GetExamples()
        {
            return new ValidateQuestionSetResponse
            {
                ApplicationName = "Application_Name",
                QuestionSetId = Guid.Parse("5EBE528C-31D6-4ACB-AF6A-9EB67AFE6333"),
            };
        }
    }
}