using Api.FormEngine.Core.Handlers.Forms;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using TQ.Data.FormEngine;
using UnitTest.Helpers;
using Xunit;

namespace UnitTest.Api.FormEngine.Core.Handlers.Forms
{
    /// <summary>
    /// Unit tests for GetQuestionSetByApplicationTypeHandler
    /// </summary>
    public class GetQuestionSetByApplicationTypeHandlerTests
    {
        private readonly FormsEngineContext formsEngineContext;
        private readonly CancellationToken cancellationToken = CancellationToken.None;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetQuestionSetByApplicationTypeHandlerTests"/> class.
        /// </summary>
        public GetQuestionSetByApplicationTypeHandlerTests()
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
            Exception ex = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Handle(null, this.cancellationToken));
            ex.Message.Should().Contain("Parameter 'request'");
        }

        private GetQuestionSetByApplicationTypeHandler CreateSut()
        {
            return new GetQuestionSetByApplicationTypeHandler(formsEngineContext);
        }
    }
}