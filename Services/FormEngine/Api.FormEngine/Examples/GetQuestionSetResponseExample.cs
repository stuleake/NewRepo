using Api.FormEngine.Core.Commands.Forms;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Api.FormEngine.Examples
{
    /// <summary>
    /// A class to get GetQuestionSetResponse
    /// </summary>
    public class GetQuestionSetResponseExample : IExamplesProvider<GetQuestionSetResponse>
    {
        /// <inheritdoc/>
        public GetQuestionSetResponse GetExamples()
        {
            return new GetQuestionSetResponse
            {
                ApplicationName = "Application_name",
                QuestionSetId = Guid.Parse("5EBE528C-31D6-4ACB-AF6A-9EB67AFE6333"),
            };
        }
    }
}