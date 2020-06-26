using Api.FormEngine.Core.Commands.Forms;
using Api.FormEngine.Core.Handlers.Forms;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using TQ.Core.Constants;
using TQ.Data.FormEngine;
using UnitTest.Helpers;
using Xunit;

namespace UnitTest.Api.FormEngine.Core.Handlers.Forms
{
    /// <summary>
    /// Unit tests for CreateQuestionSetResponseHandler
    /// </summary>
    public class DeleteQuestionSetResponseHandlerTests
    {
        private readonly FormsEngineContext formsEngineContext;
        private readonly CancellationToken cancellationToken = CancellationToken.None;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteQuestionSetResponseHandlerTests"/> class.
        /// </summary>
        public DeleteQuestionSetResponseHandlerTests()
        {
            formsEngineContext = UnitTestHelper.GiveServiceProvider().GetRequiredService<FormsEngineContext>();
        }

        /// <summary>
        /// Handle throws a <see cref="ArgumentNullException"/> when request is null
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task HandleThrowsArgumentNullExceptionWhenRequestIsNull()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Handle(null, this.cancellationToken));
        }


        /// <summary>
        /// Handle delete QuestionSetResponse when request is not null
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task HandleDeleteQuestionSetResponseWhenRequestIsNotNull()
        {
            // Arrange
            var sut = CreateSut();

            var createHandler = new CreateQuestionSetResponseHandler(formsEngineContext);
            formsEngineContext.SetupFieldTypes();
            var number = formsEngineContext.AddField(TQ.Core.Enums.FieldTypes.Number);
            var date = formsEngineContext.AddField(TQ.Core.Enums.FieldTypes.Date);
            var checkBox = formsEngineContext.AddField(TQ.Core.Enums.FieldTypes.CheckBox);
            var numberSelector = formsEngineContext.AddField(TQ.Core.Enums.FieldTypes.NumberSelector);
            var response = $"{{\"{number.FieldId}-1\" : \"123\",\"{date.FieldId}-2\":\"16/12/2001\",\"{checkBox.FieldId}-3\":\"1\",\"{numberSelector.FieldId}-4\":\"10\"}}";

            var qscollectionType = formsEngineContext.AddQSCollectionType(CountryConstants.England);
            var qs = formsEngineContext.AddQS(1);
            formsEngineContext.AddQSCollectionMapping(1, qs.QSId, qscollectionType.QSCollectionTypeId);
            formsEngineContext.SaveChanges();

            var request = new CreateQuestionSetResponse
            {
                ApplicationTypeId = qscollectionType.QSCollectionTypeId,
                ApplicationName = "Test Application 1",
                Response = response,
                QuestionSetId = qs.QSId,
                Country = CountryConstants.England
            };
            var result = createHandler.Handle(request, this.cancellationToken).Result;

            var cmdDelete = new DeleteQuestionSetResponse { Id = result };

            // Act
            var actualDelete = await sut.Handle(cmdDelete, cancellationToken);

            // Assert
            actualDelete.Should().BeTrue();
        }

        private DeleteQuestionSetResponseHandler CreateSut()
        {
            return new DeleteQuestionSetResponseHandler(formsEngineContext);
        }
    }
}