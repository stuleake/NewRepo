using Api.FormEngine.Core.Services.Parsers;
using Api.FormEngine.Core.Services.Parsers.Models;
using Api.FormEngine.Core.Services.Processors;
using Api.FormEngine.Core.ViewModels.SheetModels;
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
    public class FieldAggregationsProcessorTests
    {
        private readonly IRepository<FieldAggregation> mockFieldAggregationRepository = Mock.Create<IRepository<FieldAggregation>>();

        private readonly IParser<IEnumerable<Aggregations>, IEnumerable<FieldAggregation>> mockAggregationParser
            = Mock.Create<IParser<IEnumerable<Aggregations>, IEnumerable<FieldAggregation>>>();

        [Fact]
        public async Task ProcessorUpdatesDatabaseWithAddedEntities()
        {
            // Arrange
            var processor = CreateSut();
            var result = GetParseResults(ParseResultTypes.Added);

            Mock.Arrange(() => mockAggregationParser.Parse(null))
                .IgnoreArguments()
                .Returns(result)
                .OccursOnce();

            // Act
            await processor.ProcessAsync(GetParseRequestData());

            // Assert
            Mock.Assert(mockAggregationParser);
            Mock.Assert(() => mockFieldAggregationRepository.AddRangeAsync(result.Added), Occurs.Once());
            Mock.Assert(() => mockFieldAggregationRepository.UpdateRangeAsync(result.Updated), Occurs.Never());
            Mock.Assert(() => mockFieldAggregationRepository.DeleteAllAsync(result.Deleted), Occurs.Never());
        }

        [Fact]
        public async Task ProcessorUpdatesDatabaseWithUpdatedEntities()
        {
            // Arrange
            var processor = CreateSut();
            var result = GetParseResults(ParseResultTypes.Updated);

            Mock.Arrange(() => mockAggregationParser.Parse(null))
                .IgnoreArguments()
                .Returns(result)
                .OccursOnce();

            // Act
            await processor.ProcessAsync(GetParseRequestData());

            // Assert
            Mock.Assert(mockAggregationParser);
            Mock.Assert(() => mockFieldAggregationRepository.AddRangeAsync(result.Added), Occurs.Never());
            Mock.Assert(() => mockFieldAggregationRepository.UpdateRangeAsync(result.Updated), Occurs.Once());
            Mock.Assert(() => mockFieldAggregationRepository.DeleteAllAsync(result.Deleted), Occurs.Never());
        }

        [Fact]
        public async Task ProcessorUpdatesDatabaseWithDeletedEntities()
        {
            // Arrange
            var processor = CreateSut();
            var result = GetParseResults(ParseResultTypes.Deleted);

            Mock.Arrange(() => mockAggregationParser.Parse(null))
                .IgnoreArguments()
                .Returns(result)
                .OccursOnce();

            // Act
            await processor.ProcessAsync(GetParseRequestData());

            // Assert
            Mock.Assert(mockAggregationParser);
            Mock.Assert(() => mockFieldAggregationRepository.AddRangeAsync(result.Added), Occurs.Never());
            Mock.Assert(() => mockFieldAggregationRepository.UpdateRangeAsync(result.Updated), Occurs.Never());
            Mock.Assert(() => mockFieldAggregationRepository.DeleteAllAsync(result.Deleted), Occurs.Once());
        }

        private FieldAggregationsProcessor CreateSut()
        {
            return new FieldAggregationsProcessor(mockFieldAggregationRepository, mockAggregationParser);
        }

        private static IEnumerable<Aggregations> GetParseRequestData()
        {
            return new List<Aggregations>
            {
                new Aggregations(),
                new Aggregations(),
                new Aggregations()
            };
        }

        private ParseResult<IEnumerable<FieldAggregation>> GetParseResults(ParseResultTypes type)
        {
            var results = new List<FieldAggregation>
            {
                new FieldAggregation(),
                new FieldAggregation(),
                new FieldAggregation()
            };

            return new ParseResult<IEnumerable<FieldAggregation>>
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