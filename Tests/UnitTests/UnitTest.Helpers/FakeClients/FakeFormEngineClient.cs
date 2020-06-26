using Api.Planner.Core.Commands.Forms;
using Api.Planner.Core.Services.FormEngine;
using Api.Planner.Core.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Core.Models;

namespace UnitTest.Helpers.FakeClients
{
    /// <summary>
    /// Fake Form Engine Client
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class FakeFormEngineClient
    {
        /// <summary>
        /// Gets the form engine client
        /// </summary>
        public IFormEngineClient FormEngineClient { get; private set; }

        private readonly IEnumerable<QuestionSet> questionSets;
        private readonly IEnumerable<QuestionSetResponse> questionSetResponses;
        private readonly IEnumerable<ApplicationTypeModel> applicationTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeFormEngineClient"/> class.
        /// </summary>
        /// <param name="lstQs">List of Question sets</param>
        /// <param name="qsr">List of question set rules</param>
        /// <param name="applicationTypes">List of application types</param>
        public FakeFormEngineClient(IEnumerable<QuestionSet> lstQs, IEnumerable<QuestionSetResponse> qsr, IEnumerable<ApplicationTypeModel> applicationTypes)
        {
            FormEngineClient = Mock.Create<IFormEngineClient>();
            questionSets = lstQs;
            questionSetResponses = qsr;
            this.applicationTypes = applicationTypes;

            Mock.Arrange(() => FormEngineClient.GetQuestionSetAsync(Arg.IsAny<string>(), Arg.IsAny<string>()))
                .Returns((string requestUrl, string authToken) => GetQuestionSetAsync(requestUrl));
            Mock.Arrange(() => FormEngineClient.CreateQuestionSetResponseAsync(Arg.IsAny<string>(), Arg.IsAny<JObject>(), Arg.IsAny<string>(), Arg.IsAny<string>()))
                .Returns((string requestUrl, JObject obj, string authToken) => CreateQuestionSetResponseAsync(obj));
            Mock.Arrange(() => FormEngineClient.GetQuestionSetResponseAsync(Arg.IsAny<string>(), Arg.IsAny<JObject>(), Arg.IsAny<string>(), Arg.IsAny<string>()))
                .Returns((string requestUrl, JObject obj, string authToken) => GetQuestionSetResponseAsync(obj));
            Mock.Arrange(() => FormEngineClient.ValidateQuestionSetResponseAsync(Arg.IsAny<string>(), Arg.IsAny<JObject>(), Arg.IsAny<string>(), Arg.IsAny<string>()))
                .Returns((string requestUrl, JObject obj, string authToken) => ValidateQuestionSetResponseAsync());
            Mock.Arrange(() => FormEngineClient.DeleteQuestionSetResponseAsync(Arg.IsAny<string>(), Arg.IsAny<string>()))
                .Returns((string requestUrl, string authToken) => DeleteQuestionResponseAsync());
            Mock.Arrange(() => FormEngineClient.GetApplicationTypesAsync(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<string>()))
                .Returns((string requestUrl, string authToken, string country, string product) => GetApplicationTypesAsync(country));
            Mock.Arrange(() => FormEngineClient.GetQuestionSetsByApplicationTypeAsync(Arg.IsAny<string>(), Arg.IsAny<string>()))
            .Returns((string requestUrl, string authToken) => GetQuestionSetByApplicationTypesAsync(requestUrl));
        }

        private async Task<ServiceResponse<List<ApplicationTypeModel>>> GetApplicationTypesAsync(string country)
        {
            var result = new ServiceResponse<List<ApplicationTypeModel>>
            {
                Code = country.Equals("england", StringComparison.OrdinalIgnoreCase) ? 200 : 204,
                Value = country.Equals("england", StringComparison.OrdinalIgnoreCase) ? applicationTypes.ToList() : null
            };

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        private async Task<ServiceResponse<List<QuestionSet>>> GetQuestionSetByApplicationTypesAsync(string requestUrl)
        {
            var applicationtypeId = Guid.Parse(requestUrl.Split('/').Last());
            var result = new ServiceResponse<List<QuestionSet>>
            {
                Code = applicationtypeId == Guid.Parse("cc835899-6329-4d30-a387-65f95cab9810") ? 200 : 204,
                Value = applicationtypeId == Guid.Parse("cc835899-6329-4d30-a387-65f95cab9810") ? questionSets.ToList() : null
            };

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        private async Task<ServiceResponse<QuestionSet>> GetQuestionSetAsync(string requestUrl)
        {
            Guid id = Guid.Parse(requestUrl.Split('/').Last());
            var qs = questionSets.FirstOrDefault(q => q.QuestionSetId.Equals(id));
            var result = new ServiceResponse<QuestionSet> { Code = qs == null ? 204 : 200, Value = qs };

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        private async Task<ServiceResponse<Guid>> CreateQuestionSetResponseAsync(JObject obj)
        {
            Guid id = JsonConvert.DeserializeObject<CreateQuestionSetResponse>(obj.ToString()).QuestionSetId;
            var result = id == Guid.Parse("5EBE528C-31D6-4ACB-AF6A-9EB67AFE6333")
                ? new ServiceResponse<Guid> { Code = 201, Value = Guid.NewGuid() }
                : new ServiceResponse<Guid> { Code = 404, Message = "Error" };

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        private async Task<ServiceResponse<QuestionSetResponse>> GetQuestionSetResponseAsync(JObject obj)
        {
            Guid id = JsonConvert.DeserializeObject<GetQuestionSetResponse>(obj.ToString()).QuestionSetId;
            var qs = questionSetResponses.FirstOrDefault(q => q.QuestionSetId.Equals(id));
            var result = new ServiceResponse<QuestionSetResponse> { Code = qs == null ? 204 : 200, Value = qs };

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        private async Task<ServiceResponse<QuestionSetSubmit>> ValidateQuestionSetResponseAsync()
        {
            var result = new ServiceResponse<QuestionSetSubmit> { Code = 200, Value = new QuestionSetSubmit { Errors = null, UserApplicationId = Guid.NewGuid() } };

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        private async Task<ServiceResponse<bool>> DeleteQuestionResponseAsync()
        {
            var result = new ServiceResponse<bool> { Code = 200, Value = true };

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}