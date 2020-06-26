using Api.Planner.Core.Commands.Forms;
using Api.Planner.Core.Services.FormEngine;
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
    /// Form Engine Client Test
    /// </summary>
    public class FormEngineClientTest : IDisposable
    {
        private readonly IConfigurationRoot config;
        private readonly ServiceProvider serviceProvider;
        private readonly HttpClient httpClient;

        private readonly FormEngineClient frmEngineclient;
        private readonly FakeHttpMessageHandler messageHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormEngineClientTest"/> class.
        /// </summary>
        public FormEngineClientTest()
        {
            serviceProvider = UnitTestHelper.GiveServiceProvider();
            var logger = serviceProvider.GetRequiredService<ILogger<FormEngineClient>>();

            config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            messageHandler = new FakeHttpMessageHandler();
            httpClient = new HttpClient(messageHandler)
            {
                BaseAddress = new Uri("http://localhost/")
            };

            frmEngineclient = new FormEngineClient(httpClient, logger);
        }

        /// <summary>
        /// Test to get Question Sets
        /// </summary>
        /// <returns>Nothing to return</returns>
        [Fact]
        public async Task Test_GetQuestionSet()
        {
            var response = await frmEngineclient.GetQuestionSetAsync(
                $"{config["ApiUri:FormEngine:GetQuestionSet"]}{"5EBE528C-31D6-4ACB-AF6A-9EB67AFE6333"}",
                "bearer token");

            Assert.Equal(200, response.Code);
            Assert.NotNull(response);
        }

        /// <summary>
        /// Test to create question set response
        /// </summary>
        /// <returns>Nothing to return</returns>
        [Fact]
        public async Task Test_CreateQuestionSetResponse()
        {
            CreateQuestionSetResponse request = new CreateQuestionSetResponse
            {
                AuthToken = "bearer token"
            };

            var response = await frmEngineclient.CreateQuestionSetResponseAsync(
                config["ApiUri:FormEngine:SaveQuestionSetResponse"],
                JObject.Parse(JsonConvert.SerializeObject(request)),
                request.AuthToken,
                "England");

            Assert.Equal(201, response.Code);
            Assert.IsType<Guid>(response.Value);
        }

        /// <summary>
        /// Tets to get question set responses
        /// </summary>
        /// <returns>Nothing to return</returns>
        [Fact]
        public async Task Test_GetQuestionSetResponse()
        {
            GetQuestionSetResponse request = new GetQuestionSetResponse
            {
                AuthToken = "bearer token"
            };

            var response = await frmEngineclient.GetQuestionSetResponseAsync(
                config["ApiUri:FormEngine:GetQuestionSetResponse"],
                JObject.Parse(JsonConvert.SerializeObject(request)),
                request.AuthToken,
                "England").ConfigureAwait(false);

            Assert.Equal(200, response.Code);
            Assert.NotNull(response.Value);
        }

        /// <summary>
        /// Test to validate question set responses
        /// </summary>
        /// <returns>Nothing to return</returns>
        [Fact]
        public async Task Test_ValidateQuestionSetResponse()
        {
            SubmitQuestionSetResponse request = new SubmitQuestionSetResponse
            {
                AuthToken = "bearer token"
            };

            var response = await frmEngineclient.ValidateQuestionSetResponseAsync(
                config["ApiUri:FormEngine:ValidateQuestionSetResponse"],
                JObject.Parse(JsonConvert.SerializeObject(request)),
                request.AuthToken,
                "England");

            Assert.Equal(200, response.Code);
            Assert.Null(response.Value.Errors);
        }

        /// <summary>
        /// Test to delete question set responses
        /// </summary>
        /// <returns>Nothing to return</returns>
        [Fact]
        public async Task Test_DeleteQuestionSetResponse()
        {
            var response = await frmEngineclient.DeleteQuestionSetResponseAsync(
                string.Format(config["ApiUri:FormEngine:DeleteQuestionSetResponse"], "5EBE528C-31D6-4ACB-AF6A-9EB67AFE6333"),
                "bearer token");

            Assert.Equal(200, response.Code);
            Assert.IsType<bool>(response.Value);
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