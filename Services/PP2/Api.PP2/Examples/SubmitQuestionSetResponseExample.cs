using Api.PP2.Core.Commands.Forms;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Api.PP2.Examples
{
    /// <summary>
    /// A class to get Submit Question set Response
    /// </summary>
    public class SubmitQuestionSetResponseExample : IExamplesProvider<SubmitQuestionSetResponse>
    {
        /// <inheritdoc/>
        public SubmitQuestionSetResponse GetExamples()
        {
            return new SubmitQuestionSetResponse
            {
                ApplicationId = Guid.Parse("7b400618-b055-4061-99db-08d777b55ae8"),
                QuestionSetId = Guid.Parse("5ebe528c-31d6-4acb-af6a-9eb67afe6333"),
                Response = "{\"id\":\"497d90a3-60c5-45f4-8426-b28d56192e5a\"," +
                "\"sections\":[{\"id\":\"2eb24c0e-4643-4366-8d41-384fc46f7cbb\"," +
                "\"fields\":[{\"id\":\"e169e055-d75f-4890-b679-c6481560e34a\"," +
                "\"answer\":\"k21\"},{\"id\":\"d2798c1b-2d32-40d3-a33e-1a9318989e62\"," +
                "\"answer\":\"k21\"},{\"id\":\"b7952873-b6ec-4cf9-bd86-0118f84d6521\"," +
                "\"answer\":\"k21@email.com\"}]},{\"id\":\"d0d1cc29-9677-4af2-9dc4-b3c795758b88\"," +
                "\"fields\":[{\"id\":\"6143d1bb-9dc8-4d36-9413-f44991305de2\"," +
                "\"answer\":\"k21\"}]}]}",
            };
        }
    }
}