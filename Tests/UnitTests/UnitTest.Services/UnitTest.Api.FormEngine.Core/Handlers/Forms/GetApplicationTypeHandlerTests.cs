using Api.FormEngine.Core.Commands.Forms;
using Api.FormEngine.Core.Handlers.Forms;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using TQ.Core.Constants;
using TQ.Data.FormEngine;
using UnitTest.Helpers;
using Xunit;

namespace UnitTest.Api.FormEngine.Core.Handlers.Forms
{
    /// <summary>
    /// Unit tests for GetApplicationTypeHandler
    /// </summary>
    public class GetApplicationTypeHandlerTests
    {
        private readonly FormsEngineContext formsEngineContext;
        private readonly IMapper mapper;
        private readonly CancellationToken cancellationToken = CancellationToken.None;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetApplicationTypeHandlerTests"/> class.
        /// </summary>
        public GetApplicationTypeHandlerTests()
        {
            formsEngineContext = UnitTestHelper.GiveServiceProvider().GetRequiredService<FormsEngineContext>();
            mapper = UnitTestHelper.GiveServiceProvider().GetRequiredService<IMapper>();
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
        /// Handle throws an <see cref="Exception"/> when data not available for country in request
        /// </summary>
        /// <param name="country">country for the request</param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Wales")]
        public void HandleThrowsExceptionWhenDataNotAvailableForCountryInRequest(string country)
        {
            // Arrange
            var data = formsEngineContext.AddQSCollectionType(CountryConstants.England);
            formsEngineContext.QSCollectionType.Add(data);
            formsEngineContext.SaveChanges();

            var sut = CreateSut();
            var req = new GetApplicationType { Country = country };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => sut.Handle(req, this.cancellationToken));
        }

        /// <summary>
        /// Handle returns list of <see cref="<ApplicationTypes>"/> when data is available for country in request
        /// </summary>
        [Fact]
        public void HandleReturnsApplicationTypesWhenDataAvailableForCountryInRequest()
        {
            // Arrange
            formsEngineContext.AddQSCollectionType(CountryConstants.Wales);
            var data = formsEngineContext.AddQSCollectionType(CountryConstants.England);
            formsEngineContext.SaveChanges();

            var sut = CreateSut();
            var req = new GetApplicationType { Country = CountryConstants.England, Product = ProductConstants.PP2 };

            // Act & Assert
            var result = sut.Handle(req, this.cancellationToken).Result;
            result.Should().OnlyContain(x => x.QSCollectionTypeId == data.QSCollectionTypeId);
        }

        private GetApplicationTypeHandler CreateSut()
        {
            return new GetApplicationTypeHandler(formsEngineContext, mapper);
        }
    }
}