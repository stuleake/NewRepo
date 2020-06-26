using Api.FormEngine.Core.Services.Parsers;
using Api.FormEngine.Core.Services.Parsers.Models;
using Api.FormEngine.Core.Services.Processors;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Core.Repository;
using Xunit;

namespace UnitTest.Api.FormEngine.Core.Services.Processors
{
    [ExcludeFromCodeCoverage]
    public class FieldAnswerGuideProcessorTests
    {
        private readonly IRepository<TQ.Data.FormEngine.Schemas.Forms.AnswerGuide> mockAnswerGuideRepository = Mock.Create<IRepository<TQ.Data.FormEngine.Schemas.Forms.AnswerGuide>>();

        private readonly IParser<FieldParserModel, IEnumerable<TQ.Data.FormEngine.Schemas.Forms.AnswerGuide>> mockAnswerGuideParser
            = Mock.Create<IParser<FieldParserModel, IEnumerable<TQ.Data.FormEngine.Schemas.Forms.AnswerGuide>>>();

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

        private FieldAnswerGuideProcessor CreateSut()
        {
            return new FieldAnswerGuideProcessor(mockAnswerGuideRepository, mockAnswerGuideParser);
        }

        private static FieldParserModel GetParseRequestData()
        {
            return new FieldParserModel();
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