using Api.FormEngine.Core.Commands.Forms;
using Api.FormEngine.Core.Handlers.Forms;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using TQ.Core.Constants;
using TQ.Core.Exceptions;
using TQ.Data.FormEngine;
using TQ.Data.FormEngine.Schemas.Forms;
using UnitTest.Helpers;
using Xunit;

namespace UnitTest.Api.FormEngine.Core.Handlers.Forms
{
    /// <summary>
    /// Unit tests for CreateQuestionSetResponseHandler
    /// </summary>
    public class CreateQuestionSetResponseHandlerTests
    {
        private readonly FormsEngineContext formsEngineContext;
        private readonly CancellationToken cancellationToken = CancellationToken.None;
        private readonly Field number;
        private readonly Field date;
        private readonly Field checkBox;
        private readonly Field numberSelector;
        private readonly QSCollectionType qscollectionType;
        private readonly QS qs;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateQuestionSetResponseHandlerTests"/> class.
        /// </summary>
        public CreateQuestionSetResponseHandlerTests()
        {
            formsEngineContext = UnitTestHelper.GiveServiceProvider().GetRequiredService<FormsEngineContext>();

            // configure database
            formsEngineContext.SetupFieldTypes();
            number = formsEngineContext.AddField(TQ.Core.Enums.FieldTypes.Number);
            date = formsEngineContext.AddField(TQ.Core.Enums.FieldTypes.Date);
            checkBox = formsEngineContext.AddField(TQ.Core.Enums.FieldTypes.CheckBox);
            numberSelector = formsEngineContext.AddField(TQ.Core.Enums.FieldTypes.NumberSelector);
            qscollectionType = formsEngineContext.AddQSCollectionType(CountryConstants.England);
            qs = formsEngineContext.AddQS(1);
            formsEngineContext.AddQSCollectionMapping(1, qs.QSId, qscollectionType.QSCollectionTypeId);
            formsEngineContext.SaveChanges();
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
        /// Handle throws a <see cref="ArgumentNullException"/> when response in request is null
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task HandleThrowsArgumentNullExceptionWhenResponseInRequestIsNull()
        {
            // Arrange
            var sut = CreateSut();
            var req = new CreateQuestionSetResponse { Response = null };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Handle(req, this.cancellationToken));
        }

        /// <summary>
        /// Handle throws a <see cref="TQException"/> when question in database set is null
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task HandleThrowsTQExceptionWhenQuestionSetDoesNotExistInDatabase()
        {
            // Arrange
            var sut = CreateSut();
            var req = new CreateQuestionSetResponse
            {
                ApplicationName = "Test Application",
                Response = string.Empty,
                Country = CountryConstants.England
            };

            // Act
            var ex = await Assert.ThrowsAsync<TQException>(() => sut.Handle(req, this.cancellationToken));

            // Assert
            ex.Message.Should().Be("Invalid question set Id.");
        }

        /// <summary>
        /// Handle throws a <see cref="TQException"/> when response for number field is not valid
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task HandleThrowsTQExceptionWhenResponseForNumberFieldIsNotValid()
        {
            // Arrange
            var sut = CreateSut();
            var response = $"{{\"{number.FieldId}-1\" : \"xyz\"}}";
            var request = new CreateQuestionSetResponse
            {
                ApplicationTypeId = qscollectionType.QSCollectionTypeId,
                ApplicationName = "Test Application",
                Response = response,
                QuestionSetId = qs.QSId,
                Country = CountryConstants.England
            };

            // Act
            var ex = await Assert.ThrowsAsync<TQException>(() => sut.Handle(request, this.cancellationToken));

            // Assert
            ex.Message.Should().Contain(ex.Message).And.Contain(number.FieldId.ToString());
        }

        /// <summary>
        /// Handle throws a <see cref="TQException"/> when response for date field is not valid
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task HandleThrowsTQExceptionWhenResponseForDateFieldIsNotValid()
        {
            // Arrange
            var sut = CreateSut();
            var response = $"{{\"{date.FieldId}-1\" : \"31/02/2020\"}}";
            var request = new CreateQuestionSetResponse
            {
                ApplicationTypeId = qscollectionType.QSCollectionTypeId,
                ApplicationName = "Test Application",
                Response = response,
                QuestionSetId = qs.QSId,
                Country = CountryConstants.England
            };

            // Act
            var ex = await Assert.ThrowsAsync<TQException>(() => sut.Handle(request, this.cancellationToken));

            // Assert
            ex.Message.Should().Contain(ex.Message).And.Contain(date.FieldId.ToString());
        }

        /// <summary>
        /// Handle throws a <see cref="TQException"/> when response for checkbox field is not valid
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task HandleThrowsTQExceptionWhenResponseForNumberSelectorFieldIsNotValid()
        {
            // Arrange
            var sut = CreateSut();
            var response = $"{{\"{numberSelector.FieldId}-1\" : \"x\"}}";
            var request = new CreateQuestionSetResponse
            {
                ApplicationTypeId = qscollectionType.QSCollectionTypeId,
                ApplicationName = "Test Application",
                Response = response,
                QuestionSetId = qs.QSId,
                Country = CountryConstants.England
            };

            // Act
            var ex = await Assert.ThrowsAsync<TQException>(() => sut.Handle(request, this.cancellationToken));

            // Assert
            ex.Message.Should().Contain(ex.Message).And.Contain(numberSelector.FieldId.ToString());
        }

        /// <summary>
        /// Handle throws a <see cref="TQException"/> when application and collection type is null
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task HandleThrowsTQExceptionWhenApplicationAndCollectionTypeIsNull()
        {
            // Arrange
            var sut = CreateSut();
            var response = $"{{\"{checkBox.FieldId}-1\" : \"1\"}}";
            var request = new CreateQuestionSetResponse
            {
                ApplicationName = "Test Application",
                Response = response,
                QuestionSetId = qs.QSId,
                Country = CountryConstants.England
            };

            // Act
            var ex = await Assert.ThrowsAsync<TQException>(() => sut.Handle(request, this.cancellationToken));

            // Assert
            ex.Message.Should().Be("Invalid Application type.");
        }

        /// <summary>
        /// Handle saves a new response set
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task HandleSavesNewResponseWhenResponseSetIsValid()
        {
            // Arrange
            var sut = CreateSut();
            var response = $"{{\"{number.FieldId}-1\" : \"123\",\"{date.FieldId}-2\":\"16/12/2001\",\"{checkBox.FieldId}-3\":\"1\",\"{numberSelector.FieldId}-4\":\"20\"}}";
            var request = new CreateQuestionSetResponse
            {
                ApplicationTypeId = qscollectionType.QSCollectionTypeId,
                ApplicationName = "Test Application",
                Response = response,
                QuestionSetId = qs.QSId,
                Country = CountryConstants.England
            };

            // Act
            var result = await sut.Handle(request, this.cancellationToken);

            // Assert
            Assert.IsType<Guid>(result);
        }

        /// <summary>
        /// Handle update an existing response set
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task HandleUpdateResponseWhenResponseSetAlreadyExists()
        {
            // Arrange
            var sut = CreateSut();
            var response = $"{{\"{number.FieldId}-1\" : \"123\",\"{date.FieldId}-2\":\"16/12/2001\",\"{checkBox.FieldId}-3\":\"1\",\"{numberSelector.FieldId}-4\":\"10\"}}";
            var request = new CreateQuestionSetResponse
            {
                ApplicationTypeId = qscollectionType.QSCollectionTypeId,
                ApplicationName = "Test Application",
                Response = response,
                QuestionSetId = qs.QSId,
                Country = CountryConstants.England
            };
            var result = await sut.Handle(request, this.cancellationToken);

            // prepare update request
            var updateResponse = $"{{\"{number.FieldId}-1\" : \"456\",\"{date.FieldId}-2\":\"31/12/2001\",\"{checkBox.FieldId}-3\":\"10\",\"{numberSelector.FieldId}-4\":\"100\"}}";
            var updateRequest = new CreateQuestionSetResponse
            {
                ApplicationTypeId = qscollectionType.QSCollectionTypeId,
                ApplicationName = "Test Application",
                Response = updateResponse,
                QuestionSetId = qs.QSId,
                Country = CountryConstants.England
            };

            // Act
            var updateResult = await sut.Handle(updateRequest, this.cancellationToken);

            // Assert
            Assert.IsType<Guid>(updateResult).Equals(result);
        }

        private CreateQuestionSetResponseHandler CreateSut()
        {
            return new CreateQuestionSetResponseHandler(formsEngineContext);
        }
    }
}