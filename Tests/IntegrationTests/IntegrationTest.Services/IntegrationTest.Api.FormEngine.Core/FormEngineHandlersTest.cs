using Api.FormEngine.Core.Commands.Forms;
using Api.FormEngine.Core.Commands.Forms.QuestionSet;
using Api.FormEngine.Core.Handlers.Forms;
using AutoMapper;
using IntegrationTest.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using TQ.Core.Constants;
using TQ.Data.FormEngine;
using Xunit;

namespace IntegrationTest.Api.FormEngine.Core
{
    /// <summary>
    /// class for form engine handler unit test
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class FormEngineHandlersTest
    {
        private readonly FormsEngineContext formsEngineContext;
        private readonly IMapper mapper;
        private readonly string response;
        private readonly string responseFail;
        private readonly Guid questionSetId;
        private readonly Guid questionSetIdFail;
        private readonly string applicationName;

        private Guid questionSetResponseId;
        private Guid qscollectionId;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormEngineHandlersTest"/> class.
        /// </summary>
        public FormEngineHandlersTest()
        {
            formsEngineContext = IntegrationHelper.GiveServiceProvider().GetRequiredService<FormsEngineContext>();
            mapper = IntegrationHelper.GiveServiceProvider().GetRequiredService<IMapper>();
            response = "{\"90b9d5c0-c1b5-4784-e9e9-08d7c66278f5-1\":\"Yes\",\"3c276ed0-14de-4627-e9ea-08d7c66278f5-2\":\"Other person\",\"02c7b534-36ba-4021-e9eb-08d7c66278f5-3\":\"Ms\"," +
                "\"9b5b8f2b-fe37-418c-e9ec-08d7c66278f5-4\":\"Test\",\"d5adb775-2dd5-437f-e9ed-08d7c66278f5-5\":\"T\"" +
                ",\"79971e80-503e-4616-e9ee-08d7c66278f5-6\":\"12345678\",\"b3bd3381-bb85-47c6-e9ef-08d7c66278f5-7\":\"test@yopmail.com\"}";
            responseFail = "{\"DB5E6B9A-D7E0-4D53-E9F9-08D7C66278F5-1\":\"30\",\"407D251A-46B2-48B0-E9FA-08D7C66278F5-2\":\"sqmtrs\"}";
            questionSetId = Guid.Parse("093E1C31-4F62-49AF-273F-08D7C662BC83");
            questionSetIdFail = Guid.Parse("28423277-F54F-41C7-2742-08D7C662BC83");
            applicationName = "application_integrationtest";
        }

        /// <summary>
        /// method for Question set unit test
        /// </summary>
        /// <returns>true if success</returns>
        [Fact]
        public async Task QuestionSet_Test()
        {
            // Get Application Types
            var cmdGetApplicationTypes = new GetApplicationType { Country = CountryConstants.England, Product = ProductConstants.PP2 };
            var handlerGetApplicationTypes = new GetApplicationTypeHandler(formsEngineContext, mapper);
            var applicationTypes = await handlerGetApplicationTypes.Handle(cmdGetApplicationTypes, System.Threading.CancellationToken.None).ConfigureAwait(false);
            Assert.True(applicationTypes?.Any());

            // Get By Application Types
            var cmdGetByApplicationType = new GetByApplicationType { Country = CountryConstants.England, QSCollectionTypeId = applicationTypes.ElementAt(0).QSCollectionTypeId };
            var handlerGetQuestionSetByApplicationType = new GetQuestionSetByApplicationTypeHandler(formsEngineContext);
            var questionSet = await handlerGetQuestionSetByApplicationType.Handle(cmdGetByApplicationType, System.Threading.CancellationToken.None).ConfigureAwait(false);
            Assert.NotNull(questionSet);

            // Get QS
            var cmdGetQSet = new GetQuestionSetRequest { Id = questionSetId };
            var handlerGetQS = new GetQSHandler(formsEngineContext, mapper);
            var actualGetQS = await handlerGetQS.Handle(cmdGetQSet, System.Threading.CancellationToken.None);
            Assert.NotNull(actualGetQS.Definition);

            // Create Question Set Response
            var cmdCreate = new CreateQuestionSetResponse
            { ApplicationName = applicationName, ApplicationTypeId = applicationTypes.ElementAt(0).QSCollectionTypeId, Response = response, QuestionSetId = questionSetId, Country = "England" };
            var handlerCreate = new CreateQuestionSetResponseHandler(formsEngineContext);
            var actualCreate = await handlerCreate.Handle(cmdCreate, System.Threading.CancellationToken.None);
            questionSetResponseId = actualCreate;
            qscollectionId = actualCreate;

            Assert.IsType<Guid>(actualCreate);

            // Get Question Set Response
            var cmdGetQuestionSetResponse = new GetQuestionSetResponse { QSCollectionId = qscollectionId, ApplicationName = applicationName, QuestionSetId = questionSetId, Country = "England" };
            var handlerGetQuestionSetResponse = new GetQuestionSetResponseHandler(formsEngineContext);
            var actual = await handlerGetQuestionSetResponse.Handle(cmdGetQuestionSetResponse, System.Threading.CancellationToken.None);

            Assert.NotNull(actual.Response);

            // Validate Question Set Response
            var cmdValidate = new ValidateQuestionSetResponse { QSCollectionId = qscollectionId, ApplicationName = applicationName, QuestionSetId = questionSetId };
            var handlerValidate = new ValidateQuestionSetResponseHandler(formsEngineContext);
            var actualvalidate = await handlerValidate.Handle(cmdValidate, System.Threading.CancellationToken.None).ConfigureAwait(false);
            Assert.True(actualvalidate.Status);

            // Delete Question Set Response
            var cmdDelete = new DeleteQuestionSetResponse { Id = questionSetResponseId };
            var handlerDelete = new DeleteQuestionSetResponseHandler(formsEngineContext);
            var actualDelete = await handlerDelete.Handle(cmdDelete, System.Threading.CancellationToken.None);
            Assert.True(actualDelete);

            // Negative case

            // Create Question Set Response for invalid data
            var cmdCreateFail = new CreateQuestionSetResponse
            {
                ApplicationName = applicationName,
                ApplicationTypeId = applicationTypes.ElementAt(0).QSCollectionTypeId,
                Response = responseFail,
                QuestionSetId = questionSetIdFail,
                Country = "England"
            };
            var handlerCreateFail = new CreateQuestionSetResponseHandler(formsEngineContext);
            var actualCreateFail = await handlerCreateFail.Handle(cmdCreateFail, System.Threading.CancellationToken.None);
            questionSetResponseId = actualCreateFail;
            qscollectionId = actualCreateFail;
            Assert.IsType<Guid>(actualCreate);

            // Validate Question Set Response for invalid data
            var cmdValidateFail = new ValidateQuestionSetResponse { QSCollectionId = qscollectionId, ApplicationName = applicationName, QuestionSetId = questionSetIdFail };
            var handlerValidateFail = new ValidateQuestionSetResponseHandler(formsEngineContext);
            var actualvalidateFail = await handlerValidateFail.Handle(cmdValidateFail, System.Threading.CancellationToken.None).ConfigureAwait(false);
            Assert.False(actualvalidateFail.Status);

            // Delete Question Set Response
            var cmdDeleteFail = new DeleteQuestionSetResponse { Id = questionSetResponseId };
            var handlerDeleteFail = new DeleteQuestionSetResponseHandler(formsEngineContext);
            var actualDeleteFail = await handlerDeleteFail.Handle(cmdDeleteFail, System.Threading.CancellationToken.None);
            Assert.True(actualDeleteFail);
        }
    }
}