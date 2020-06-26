using Api.FormEngine.Core.Commands.Forms;
using Api.FormEngine.Core.Handlers.Forms;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using TQ.Data.FormEngine;
using TQ.Data.FormEngine.Schemas.Forms;
using UnitTest.Helpers;
using Xunit;

namespace UnitTest.Api.FormEngine.Core.Handlers.Forms
{
    /// <summary>
    /// Unit tests for GetSectionTypeHandler
    /// </summary>
    public class GetSectionTypeHandlerTests
    {
        private readonly FormsEngineContext formsEngineContext;
        private readonly CancellationToken cancellationToken = CancellationToken.None;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetSectionTypeHandlerTests"/> class.
        /// </summary>
        public GetSectionTypeHandlerTests()
        {
            formsEngineContext = UnitTestHelper.GiveServiceProvider().GetRequiredService<FormsEngineContext>();
        }

        /// <summary>
        /// Handle throws a <see cref="ArgumentNullException"/> when request is null
        /// </summary>
        [Fact]
        public void HandleThrowsArgumentNullExceptionWhenRequestIsNull()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => sut.Handle(null, this.cancellationToken));
        }

        /// <summary>
        /// Handle returns <see cref="List<SectionType>"/> when request is not null
        /// </summary>
        [Fact]
        public void HandleReturnsSectionTypesWhenRequestIsNotNull()
        {
            // Arrange
            var data = new List<SectionType>();
            for (int i = 0; i < 2; i++)
            {
                data.Add(formsEngineContext.AddSectionTypes());
            }
            formsEngineContext.SaveChanges();

            var sut = CreateSut();
            var req = new GetSectionType();

            // Act & Assert
            var result = sut.Handle(req, this.cancellationToken).Result;
            result.Should().Equal(data);
        }

        private GetSectionTypeHandler CreateSut()
        {
            return new GetSectionTypeHandler(formsEngineContext);
        }
    }
}
