using Api.Planner.Core.HttpModel;
using Api.Planner.Core.Services.PP2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using UnitTest.Helpers;
using UnitTest.Helpers.FakeClients;
using Xunit;

namespace UnitTest.Api.Planner.Core
{
    /// <summary>
    /// PP2 client Test
    /// </summary>
    public class PP2HttpClientTest : IDisposable
    {
        private readonly IConfigurationRoot config;
        private readonly ServiceProvider serviceProvider;
        private readonly HttpClient httpClient;

        private readonly PP2HttpClient pp2EngineClient;
        private readonly FakeHttpMessageHandler messageHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="PP2HttpClientTest"/> class.
        /// </summary>
        public PP2HttpClientTest()
        {
            serviceProvider = UnitTestHelper.GiveServiceProvider();
            var logger = serviceProvider.GetRequiredService<ILogger<PP2HttpClient>>();

            config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            messageHandler = new FakeHttpMessageHandler();
            httpClient = new HttpClient(messageHandler)
            {
                BaseAddress = new Uri("http://localhost/")
            };

            pp2EngineClient = new PP2HttpClient(httpClient, logger);
        }

        /// <summary>
        /// Test to submit question set
        /// </summary>
        /// <returns>Nothing to return</returns>
        [Fact]
        public async Task Test_SubmitQuestionSet()
        {
            QuestionSetResponseModel request = new QuestionSetResponseModel
            {
                ApplicationId = Guid.NewGuid(),
                QuestionSetId = Guid.NewGuid(),
                Response = "{}"
            };

            var response = await pp2EngineClient.CreateQuestionSetResponseAsync(
                config["ApiUri:PP2:SubmitQuestionSetResponse"],
                JObject.Parse(JsonConvert.SerializeObject(request)),
                "bearer token",
                "england");

            Assert.Equal(201, response.Code);
            Assert.IsType<Guid>(response.Value);
        }

        /// <summary>
        /// Disposes the object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Override for Dispose
        /// </summary>
        /// <param name="disposing">bool to indicate dispose items or not</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (httpClient != null)
                {
                    httpClient.Dispose();
                }

                if (messageHandler != null)
                {
                    messageHandler.Dispose();
                }
            }
        }
    }
}