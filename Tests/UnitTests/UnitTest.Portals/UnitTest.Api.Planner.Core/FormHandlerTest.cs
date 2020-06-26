using Api.Planner.Core.Commands.Forms;
using Api.Planner.Core.Commands.Forms.QuestionSet;
using Api.Planner.Core.Handlers.Forms;
using Api.Planner.Core.ViewModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnitTest.Helpers.FakeClients;
using Xunit;

namespace UnitTest.Api.Planner.Core
{
    /// <summary>
    /// class to test form handler
    /// </summary>
    public class FormHandlerTest
    {
        private readonly IConfigurationRoot config;
        private readonly FakeFormEngineClient fakeFormEngineClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormHandlerTest"/> class.
        /// </summary>
        public FormHandlerTest()
        {
            config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var questionSets = SetUpQuestionSet();
            var questionSetReponse = SetUpQuestionSetResponse();
            var applicationTypes = SetUpApplciationTypes();
            fakeFormEngineClient = new FakeFormEngineClient(questionSets, questionSetReponse, applicationTypes);
        }

        /// <summary>
        /// Test to get application types
        /// </summary>
        /// <returns>Returns a Task</returns>
        [Fact]
        public async Task Test_GetApplicationTypeHandler()
        {
            GetApplicationType cmd_200 = new GetApplicationType { Country = "England" };
            GetApplicationType cmd_204 = new GetApplicationType { Country = "Wales" };

            GetApplicationTypeHandler handler = new GetApplicationTypeHandler(fakeFormEngineClient.FormEngineClient, config);
            var app_200 = await handler.Handle(cmd_200, System.Threading.CancellationToken.None);
            var app_204 = await handler.Handle(cmd_204, System.Threading.CancellationToken.None);

            Assert.NotNull(app_200);
            Assert.Null(app_204);
        }

        /// <summary>
        /// Test to get question set by application type
        /// </summary>
        /// <returns>Returns a Task</returns>
        [Fact]
        public async Task Test_GetQuestionSetByApplicationTypeHandler()
        {
            GetByApplicationType cmd_200 = new GetByApplicationType { QSCollectionTypeId = Guid.Parse("cc835899-6329-4d30-a387-65f95cab9810") };
            GetByApplicationType cmd_204 = new GetByApplicationType { QSCollectionTypeId = Guid.Parse("cc835899-6329-4d30-a387-65f95cab9811") };

            GetQuestionSetByApplicationTypeHandler handler = new GetQuestionSetByApplicationTypeHandler(fakeFormEngineClient.FormEngineClient, config);
            var app_200 = await handler.Handle(cmd_200, System.Threading.CancellationToken.None);
            var app_204 = await handler.Handle(cmd_204, System.Threading.CancellationToken.None);

            Assert.NotNull(app_200);
            Assert.Null(app_204);
        }

        /// <summary>
        /// Test to get question set
        /// </summary>
        /// <returns>Returns a Task</returns>
        [Fact]
        public async Task Test_GetQuestionSetHandler()
        {
            // Arrange
            GetQuestionSet cmd_200 = new GetQuestionSet { Id = Guid.Parse("5EBE528C-31D6-4ACB-AF6A-9EB67AFE6333") };
            GetQuestionSet cmd_204 = new GetQuestionSet { Id = Guid.Parse("5EBE528C-31D6-4ACB-AF6A-9EB67AFE6335") };

            // Act
            GetQuestionSetHandler handler = new GetQuestionSetHandler(fakeFormEngineClient.FormEngineClient, config);
            var qs_200 = await handler.Handle(cmd_200, System.Threading.CancellationToken.None);
            var qs_204 = await handler.Handle(cmd_204, System.Threading.CancellationToken.None);

            // Assert
            Assert.NotNull(qs_200);
            Assert.Null(qs_204);
        }

        /// <summary>
        /// Test to create question set response
        /// </summary>
        /// <returns>Returns a Task</returns>
        [Fact]
        public async Task Test_CreateQuestionSetResponseHandler()
        {
            // Arrange
            CreateQuestionSetResponse cmd_Exception = new CreateQuestionSetResponse
            {
                AuthToken = string.Empty,
                QuestionSetId = Guid.Parse("5EBE528C-31D6-4ACB-AF6A-9EB67AFE6335"),
                ApplicationName = "application_unittest",
                Response = "{}"
            };
            CreateQuestionSetResponse cmd_200 = new CreateQuestionSetResponse
            {
                AuthToken = string.Empty,
                QuestionSetId = Guid.Parse("5EBE528C-31D6-4ACB-AF6A-9EB67AFE6333"),
                ApplicationName = "application_unittest",
                Response = "{}"
            };

            // Act
            CreateQuestionSetResponseHandler handler = new CreateQuestionSetResponseHandler(fakeFormEngineClient.FormEngineClient, config);

            var qsr_200 = await handler.Handle(cmd_200, System.Threading.CancellationToken.None);

            Exception error = null;
            try
            {
                _ = handler.Handle(cmd_Exception, System.Threading.CancellationToken.None).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                error = ex;
            }

            // Assert
            Assert.NotNull(error);
            Assert.IsType<Guid>(qsr_200);
        }

        /// <summary>
        /// Test to get the response set
        /// </summary>
        /// <returns>Returns a Task</returns>
        [Fact]
        public async Task Test_GetQuestionSetResponseHandler()
        {
            // Arrange
            GetQuestionSetResponse cmd_200 = new GetQuestionSetResponse
            {
                AuthToken = string.Empty,
                QuestionSetId = Guid.Parse("5EBE528C-31D6-4ACB-AF6A-9EB67AFE6333"),
                ApplicationName = "application_unittest"
            };
            GetQuestionSetResponse cmd_204 = new GetQuestionSetResponse
            {
                AuthToken = string.Empty,
                QuestionSetId = Guid.Parse("5EBE528C-31D6-4ACB-AF6A-9EB67AFE6332"),
                ApplicationName = "application_unittest"
            };

            // Act
            GetQuestionSetResponseHandler handler = new GetQuestionSetResponseHandler(fakeFormEngineClient.FormEngineClient, config);
            var qsr_200 = await handler.Handle(cmd_200, System.Threading.CancellationToken.None);
            var qsr_204 = await handler.Handle(cmd_204, System.Threading.CancellationToken.None);

            // Assert
            Assert.NotNull(qsr_200);
            Assert.Null(qsr_204);
        }

        /// <summary>
        /// Helper Method to setup the Question Set.
        /// </summary>
        /// <returns>Returns a list of question sets</returns>
        private List<QuestionSet> SetUpQuestionSet()
        {
            var qs = new List<QuestionSet>
            {
                new QuestionSet { Definition = "{}", QuestionSetId = Guid.Parse("5EBE528C-31D6-4ACB-AF6A-9EB67AFE6333") }
            };

            return qs;
        }

        /// <summary>
        /// Helper Method to Setup the QuestionSet Response.
        /// </summary>
        /// <returns>Returns a list of question set responses</returns>
        private List<QuestionSetResponse> SetUpQuestionSetResponse()
        {
            var qs = new List<QuestionSetResponse>
            {
                new QuestionSetResponse
                {
                    QuestionSetId = Guid.Parse("5EBE528C-31D6-4ACB-AF6A-9EB67AFE6333"),
                    UserApplicationId = Guid.NewGuid(),
                    QuestionSetResponseId = Guid.NewGuid()
                }
            };

            return qs;
        }

        /// <summary>
        /// Test to set the application type
        /// </summary>
        /// <returns>Returns a Task</returns>
        private List<ApplicationTypeModel> SetUpApplciationTypes()
        {
            var applicationTypes = new List<ApplicationTypeModel>
            {
                new ApplicationTypeModel
                {
                    QSCollectionTypeId = Guid.NewGuid(),
                    Label = "PP2",
                }
            };

            return applicationTypes;
        }
    }
}