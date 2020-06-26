using Swashbuckle.AspNetCore.Filters;
using System;

namespace Api.FormEngine.Examples
{
    /// <summary>
    /// A class to get QuestionId
    /// </summary>
    public class GuidExample : IExamplesProvider<Guid>
    {
        /// <inheritdoc/>
        public Guid GetExamples()
        {
            return Guid.NewGuid();
        }
    }
}