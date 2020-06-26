using Api.FormEngine.Core.Commands.Forms;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Api.FormEngine.Examples
{
    /// <summary>
    /// A class to get Create Question Set Response
    /// </summary>
    public class CreateQuestionSetResponseExample : IExamplesProvider<CreateQuestionSetResponse>
    {
        /// <inheritdoc/>
        public CreateQuestionSetResponse GetExamples()
        {
            return new CreateQuestionSetResponse
            {
                ApplicationName = "Application_Name",
                QuestionSetId = Guid.Parse("5EBE528C-31D6-4ACB-AF6A-9EB67AFE6333"),
                Response = "{\"id\":\"497d90a3-60c5-45f4-8426-b28d56192e5a\"," +
                "\"sections\":[{\"id\":\"2eb24c0e-4643-4366-8d41-384fc46f7cbb\"," +
                "\"fields\":[{\"id\":\"e169e055-d75f-4890-b679-c6481560e34a\"," +
                "\"answer\":\"k21\"},{\"id\":\"d2798c1b-2d32-40d3-a33e-1a9318989e62\"," +
                "\"answer\":\"k21\"},{\"id\":\"b7952873-b6ec-4cf9-bd86-0118f84d6521\"," +
                "\"answer\":\"k21\"}]},{\"id\":\"d0d1cc29-9677-4af2-9dc4-b3c795758b88\"," +
                "\"fields\":[{\"id\":\"6143d1bb-9dc8-4d36-9413-f44991305de2\"," +
                "\"answer\":\"k21\"}]}]}",
            };
        }
    }
}