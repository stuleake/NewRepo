using Api.Planner.Core.ViewModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTest.Helpers.FakeClients
{
    /// <summary>
    /// Handler class for Fake Http Message
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly IConfigurationRoot config;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeHttpMessageHandler"/> class.
        /// </summary>
        public FakeHttpMessageHandler()
        {
            config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }

        /// <summary>
        /// Http send request
        /// </summary>
        /// <param name="request">http request</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>""</returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            HttpResponseMessage result = null;
            if (request.RequestUri.AbsolutePath.Substring(1).Equals($"{config["ApiUri:FormEngine:GetQuestionSet"]}{"5EBE528C-31D6-4ACB-AF6A-9EB67AFE6333"}", StringComparison.OrdinalIgnoreCase))
            {
                result = GetQuestionSet();
            }
            else if (request.RequestUri.AbsolutePath.Substring(1).Equals(config["ApiUri:FormEngine:SaveQuestionSetResponse"], StringComparison.OrdinalIgnoreCase))
            {
                result = CreateQuestionSetResponse();
            }
            else if (request.RequestUri.AbsolutePath.Substring(1).Equals(config["ApiUri:FormEngine:GetQuestionSetResponse"], StringComparison.OrdinalIgnoreCase))
            {
                result = GetQuestionSetResponse();
            }
            else if (request.RequestUri.AbsolutePath.Substring(1).Equals(config["ApiUri:FormEngine:ValidateQuestionSetResponse"], StringComparison.OrdinalIgnoreCase))
            {
                result = ValidateQuestionSetResponse();
            }
            else if (request.RequestUri.AbsolutePath.Substring(1)
                .Equals(string.Format(config["ApiUri:FormEngine:DeleteQuestionSetResponse"], "5EBE528C-31D6-4ACB-AF6A-9EB67AFE6333"), StringComparison.OrdinalIgnoreCase))
            {
                result = DeleteQuestionSetResponse();
            }
            else if (request.RequestUri.AbsolutePath.Substring(1).Equals(string.Format(config["ApiUri:PP2:SubmitQuestionSetResponse"]), StringComparison.OrdinalIgnoreCase))
            {
                result = SubmitQuestionSetResponse();
            }

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        private HttpResponseMessage GetQuestionSet()
        {
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    JsonConvert.SerializeObject(new QuestionSet
                    {
                        Definition = "{}",
                        QuestionSetId = Guid.Parse("5EBE528C-31D6-4ACB-AF6A-9EB67AFE6335")
                    }),
                    Encoding.UTF8,
                    "application/json")
            };
        }

        private HttpResponseMessage CreateQuestionSetResponse()
        {
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Created,
                Content = new StringContent(JsonConvert.SerializeObject(Guid.NewGuid()), Encoding.UTF8, "application/json")
            };
        }

        private HttpResponseMessage GetQuestionSetResponse()
        {
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    JsonConvert.SerializeObject(new QuestionSetResponse
                    {
                        QuestionSetId = Guid.Parse("5EBE528C-31D6-4ACB-AF6A-9EB67AFE6333"),
                        QuestionSetResponseId = Guid.NewGuid(),
                        Response = "{}"
                    }),
                    Encoding.UTF8,
                    "application/json")
            };
        }

        private HttpResponseMessage ValidateQuestionSetResponse()
        {
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    JsonConvert.SerializeObject(new QuestionSetSubmit
                    {
                        QuestionSetId = Guid.Parse("5EBE528C-31D6-4ACB-AF6A-9EB67AFE6333"),
                        QuestionSetResponseId = Guid.NewGuid(),
                        Response = "{}"
                    }),
                    Encoding.UTF8,
                    "application/json")
            };
        }

        private HttpResponseMessage DeleteQuestionSetResponse()
        {
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(true), Encoding.UTF8, "application/json")
            };
        }

        private HttpResponseMessage SubmitQuestionSetResponse()
        {
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Created,
                Content = new StringContent(JsonConvert.SerializeObject(Guid.NewGuid()), Encoding.UTF8, "application/json")
            };
        }
    }
}