using Api.FormEngine.Core.Services.Parsers;
using Api.FormEngine.Core.Services.Parsers.Models;
using Api.FormEngine.Core.Services.Processors;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Core.Repository;
using TQ.Data.FormEngine.Schemas.Forms;
using Xunit;

namespace UnitTest.Api.FormEngine.Core.Services.Processors
{
    [ExcludeFromCodeCoverage]
    public class FieldProcessorTests
    {
        private readonly IRepository<Field> mockFieldRepository = Mock.Create<IRepository<Field>>();

        private readonly IParser<FieldParserModel, IEnumerable<Field>> mockFieldParser
            = Mock.Create<IParser<FieldParserModel, IEnumerable<Field>>>();

        [Fact]
        public async Task ProcessorUpdatesDatabaseWithAddedEntities()
        {
            // Arrange
            var processor = CreateSut();
            var result = GetParseResults(ParseResultTypes.Added);

            Mock.Arrange(() => mockFieldParser.Parse(null))
                .IgnoreArguments()
                .Returns(result)
                .OccursOnce();

            // Act
            await processor.ProcessAsync(GetParseRequestData());

            // Assert
            Mock.Assert(mockFieldParser);
            Mock.Assert(() => mockFieldRepository.AddRangeAsync(result.Added), Occurs.Once());
            Mock.Assert(() => mockFieldRepository.UpdateRangeAsync(result.Updated), Occurs.Never());
            Mock.Assert(() => mockFieldRepository.DeleteAllAsync(result.Deleted), Occurs.Never());
        }

        [Fact]
        public async Task ProcessorUpdatesDatabaseWithUpdatedEntities()
        {
            // Arrange
            var processor = CreateSut();
            var result = GetParseResults(ParseResultTypes.Updated);

            Mock.Arrange(() => mockFieldParser.Parse(null))
                .IgnoreArguments()
                .Returns(result)
                .OccursOnce();

            // Act
            await processor.ProcessAsync(GetParseRequestData());

            // Assert
            Mock.Assert(mockFieldParser);
            Mock.Assert(() => mockFieldRepository.AddRangeAsync(result.Added), Occurs.Never());
            Mock.Assert(() => mockFieldRepository.UpdateRangeAsync(result.Updated), Occurs.Once());
            Mock.Assert(() => mockFieldRepository.DeleteAllAsync(result.Deleted), Occurs.Never());
        }

        [Fact]
        public async Task ProcessorUpdatesDatabaseWithDeletedEntities()
        {
            // Arrange
            var processor = CreateSut();
            var result = GetParseResults(ParseResultTypes.Deleted);

            Mock.Arrange(() => mockFieldParser.Parse(null))
                .IgnoreArguments()
                .Returns(result)
                .OccursOnce();

            // Act
            await processor.ProcessAsync(GetParseRequestData());

            // Assert
            Mock.Assert(mockFieldParser);
            Mock.Assert(() => mockFieldRepository.AddRangeAsync(result.Added), Occurs.Never());
            Mock.Assert(() => mockFieldRepository.UpdateRangeAsync(result.Updated), Occurs.Never());
            Mock.Assert(() => mockFieldRepository.DeleteAllAsync(result.Deleted), Occurs.Once());
        }

        private FieldProcessor CreateSut()
        {
            return new FieldProcessor(mockFieldRepository, mockFieldParser);
        }

        private static FieldParserModel GetParseRequestData()
        {
            return new FieldParserModel();
        }

        private ParseResult<IEnumerable<Field>> GetParseResults(ParseResultTypes type)
        {
            var results = new List<Field>
            {
                new Field(),
                new Field(),
                new Field()
            };

            return new ParseResult<IEnumerable<Field>>
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