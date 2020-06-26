using Api.FormEngine.Core.Services.Parsers;
using Api.FormEngine.Core.Services.Parsers.Models;
using Api.FormEngine.Core.Services.Processors;
using Api.FormEngine.Core.ViewModels.SheetModels;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Core.Repository;
using Xunit;

namespace UnitTest.Api.FormEngine.Core.Services.Processors
{
    /// <summary>
    /// Answer Guide Processor Tests
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AnswerGuideProcessorTests
    {
        private readonly IRepository<TQ.Data.FormEngine.Schemas.Forms.AnswerGuide> mockAnswerGuideRepository = Mock.Create<IRepository<TQ.Data.FormEngine.Schemas.Forms.AnswerGuide>>();

        private readonly IParser<IEnumerable<AnswerGuide>, IEnumerable<TQ.Data.FormEngine.Schemas.Forms.AnswerGuide>> mockAnswerGuideParser
            = Mock.Create<IParser<IEnumerable<AnswerGuide>, IEnumerable<TQ.Data.FormEngine.Schemas.Forms.AnswerGuide>>>();

        /// <summary>
        /// Test to ensure that a parsed result is Added
        /// </summary>
        /// <returns>Assertion that an answer Guid is added</returns>
        [Fact]
        public async Task ProcessorUpdatesDatabaseWithAddedEntities()
        {
            // Arrange
            var processor = CreateSut();
            var result = GetParseResults(ParseResultTypes.Added);

            Mock.Arrange(() => mockAnswerGuideParser.Parse(null))
                .IgnoreArguments()
                .Returns(result)
                .OccursOnce();

            // Act
            await processor.ProcessAsync(GetParseRequestData());

            // Assert
            Mock.Assert(mockAnswerGuideParser);
            Mock.Assert(() => mockAnswerGuideRepository.AddRangeAsync(result.Added), Occurs.Once());
            Mock.Assert(() => mockAnswerGuideRepository.UpdateRangeAsync(result.Updated), Occurs.Never());
            Mock.Assert(() => mockAnswerGuideRepository.DeleteAllAsync(result.Deleted), Occurs.Never());
        }

        /// <summary>
        /// Test to ensure that a parsed result is updated
        /// </summary>
        /// <returns>Assertion that an answer Guid is updated</returns>
        [Fact]
        public async Task ProcessorUpdatesDatabaseWithUpdatedEntities()
        {
            // Arrange
            var processor = CreateSut();
            var result = GetParseResults(ParseResultTypes.Updated);

            Mock.Arrange(() => mockAnswerGuideParser.Parse(null))
                .IgnoreArguments()
                .Returns(result)
                .OccursOnce();

            // Act
            await processor.ProcessAsync(GetParseRequestData());

            // Assert
            Mock.Assert(mockAnswerGuideParser);
            Mock.Assert(() => mockAnswerGuideRepository.AddRangeAsync(result.Added), Occurs.Never());
            Mock.Assert(() => mockAnswerGuideRepository.UpdateRangeAsync(result.Updated), Occurs.Once());
            Mock.Assert(() => mockAnswerGuideRepository.DeleteAllAsync(result.Deleted), Occurs.Never());
        }

        /// <summary>
        /// Test to ensure that a parsed result is Deleted
        /// </summary>
        /// <returns>Assertion that an answer Guid is deleted</returns>
        [Fact]
        public async Task ProcessorUpdatesDatabaseWithDeletedEntities()
        {
            // Arrange
            var processor = CreateSut();
            var result = GetParseResults(ParseResultTypes.Deleted);

            Mock.Arrange(() => mockAnswerGuideParser.Parse(null))
                .IgnoreArguments()
                .Returns(result)
                .OccursOnce();

            // Act
            await processor.ProcessAsync(GetParseRequestData());

            // Assert
            Mock.Assert(mockAnswerGuideParser);
            Mock.Assert(() => mockAnswerGuideRepository.AddRangeAsync(result.Added), Occurs.Never());
            Mock.Assert(() => mockAnswerGuideRepository.UpdateRangeAsync(result.Updated), Occurs.Never());
            Mock.Assert(() => mockAnswerGuideRepository.DeleteAllAsync(result.Deleted), Occurs.Once());
        }

        private AnswerGuideProcessor CreateSut()
        {
            return new AnswerGuideProcessor(mockAnswerGuideRepository, mockAnswerGuideParser);
        }

        private static IEnumerable<AnswerGuide> GetParseRequestData()
        {
            return new List<AnswerGuide>
            {
                new AnswerGuide(),
                new AnswerGuide(),
                new AnswerGuide()
            };
        }

        private ParseResult<IEnumerable<TQ.Data.FormEngine.Schemas.Forms.AnswerGuide>> GetParseResults(ParseResultTypes type)
        {
            var results = new List<TQ.Data.FormEngine.Schemas.Forms.AnswerGuide>
            {
                new TQ.Data.FormEngine.Schemas.Forms.AnswerGuide(),
                new TQ.Data.FormEngine.Schemas.Forms.AnswerGuide(),
                new TQ.Data.FormEngine.Schemas.Forms.AnswerGuide()
            };

            return new ParseResult<IEnumerable<TQ.Data.FormEngine.Schemas.Forms.AnswerGuide>>
            {
                Added = type == ParseResultTypes.Added ? results : default,
                Updated = type == ParseResultTypes.Updated ? results : default,
                Deleted = type == ParseResultTypes.Deleted ? results : default
            };
        }

        private enum ParseResultTypes
        {
            Added,
            Updated,
            Deleted
        }
    }
}