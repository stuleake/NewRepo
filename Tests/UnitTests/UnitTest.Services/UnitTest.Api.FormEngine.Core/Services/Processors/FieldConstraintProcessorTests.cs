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
    public class FieldConstraintProcessorTests
    {
        private readonly IRepository<FieldConstraint> mockFieldConstraintRepository = Mock.Create<IRepository<FieldConstraint>>();

        private readonly IParser<FieldConstraintParserModel, IEnumerable<FieldConstraint>> mockFieldConstraintParser
            = Mock.Create<IParser<FieldConstraintParserModel, IEnumerable<FieldConstraint>>>();

        [Fact]
        public async Task ProcessorUpdatesDatabaseWithAddedEntities()
        {
            // Arrange
            var processor = CreateSut();
            var result = GetParseResults(ParseResultTypes.Added);

            Mock.Arrange(() => mockFieldConstraintParser.Parse(null))
                .IgnoreArguments()
                .Returns(result)
                .OccursOnce();

            // Act
            await processor.ProcessAsync(GetParseRequestData());

            // Assert
            Mock.Assert(mockFieldConstraintParser);
            Mock.Assert(() => mockFieldConstraintRepository.AddRangeAsync(result.Added), Occurs.Once());
            Mock.Assert(() => mockFieldConstraintRepository.UpdateRangeAsync(result.Updated), Occurs.Never());
            Mock.Assert(() => mockFieldConstraintRepository.DeleteAllAsync(result.Deleted), Occurs.Never());
        }

        [Fact]
        public async Task ProcessorUpdatesDatabaseWithUpdatedEntities()
        {
            // Arrange
            var processor = CreateSut();
            var result = GetParseResults(ParseResultTypes.Updated);

            Mock.Arrange(() => mockFieldConstraintParser.Parse(null))
                .IgnoreArguments()
                .Returns(result)
                .OccursOnce();

            // Act
            await processor.ProcessAsync(GetParseRequestData());

            // Assert
            Mock.Assert(mockFieldConstraintParser);
            Mock.Assert(() => mockFieldConstraintRepository.AddRangeAsync(result.Added), Occurs.Never());
            Mock.Assert(() => mockFieldConstraintRepository.UpdateRangeAsync(result.Updated), Occurs.Once());
            Mock.Assert(() => mockFieldConstraintRepository.DeleteAllAsync(result.Deleted), Occurs.Never());
        }

        [Fact]
        public async Task ProcessorUpdatesDatabaseWithDeletedEntities()
        {
            // Arrange
            var processor = CreateSut();
            var result = GetParseResults(ParseResultTypes.Deleted);

            Mock.Arrange(() => mockFieldConstraintParser.Parse(null))
                .IgnoreArguments()
                .Returns(result)
                .OccursOnce();

            // Act
            await processor.ProcessAsync(GetParseRequestData());

            // Assert
            Mock.Assert(mockFieldConstraintParser);
            Mock.Assert(() => mockFieldConstraintRepository.AddRangeAsync(result.Added), Occurs.Never());
            Mock.Assert(() => mockFieldConstraintRepository.UpdateRangeAsync(result.Updated), Occurs.Never());
            Mock.Assert(() => mockFieldConstraintRepository.DeleteAllAsync(result.Deleted), Occurs.Once());
        }

        private FieldConstraintProcessor CreateSut()
        {
            return new FieldConstraintProcessor(mockFieldConstraintRepository, mockFieldConstraintParser);
        }

        private static FieldConstraintParserModel GetParseRequestData()
        {
            return new FieldConstraintParserModel();
        }

        private ParseResult<IEnumerable<FieldConstraint>> GetParseResults(ParseResultTypes type)
        {
            var results = new List<FieldConstraint>
            {
                new FieldConstraint(),
                new FieldConstraint(),
                new FieldConstraint()
            };

            return new ParseResult<IEnumerable<FieldConstraint>>
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