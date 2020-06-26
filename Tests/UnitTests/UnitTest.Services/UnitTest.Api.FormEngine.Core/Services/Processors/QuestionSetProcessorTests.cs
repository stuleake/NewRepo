using Api.FormEngine.Core.Services.Parsers;
using Api.FormEngine.Core.Services.Parsers.Models;
using Api.FormEngine.Core.Services.Processors;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Telerik.JustMock;
using TQ.Core.Repository;
using TQ.Data.FormEngine.Schemas.Forms;
using Xunit;

namespace UnitTest.Api.FormEngine.Core.Services.Processors
{
    [ExcludeFromCodeCoverage]
    public class QuestionSetProcessorTests
    {
        private readonly IRepository<QS> mockQuestionSetRepository = Mock.Create<IRepository<QS>>();
        private readonly IRepository<QSSectionMapping> mockQuestionSetSectionMappingRepository = Mock.Create<IRepository<QSSectionMapping>>();
        private readonly IRepository<SectionFieldMapping> mockSectionFieldMappingRepository = Mock.Create<IRepository<SectionFieldMapping>>();
        private readonly IRepository<Section> mockSectionRepository = Mock.Create<IRepository<Section>>();
        private readonly IRepository<Field> mockFieldRepository = Mock.Create<IRepository<Field>>();
        private readonly IRepository<FieldConstraint> mockFieldConstraintsRepository = Mock.Create<IRepository<FieldConstraint>>();
        private readonly IParser<QuestionSetParserModel, QS> mockQuestionSetParser = Mock.Create<IParser<QuestionSetParserModel, QS>>();

        [Fact]
        public async Task ProcessorUpdatesDatabaseWithAddedEntities()
        {
            // Arrange
            var processor = CreateSut();
            var result = GetParseResults(ParseResultTypes.Added);

            Mock.Arrange(() => mockQuestionSetParser.Parse(null))
                .IgnoreArguments()
                .Returns(result)
                .OccursOnce();

            // Act
            await processor.ProcessAsync(GetParseRequestData());

            // Assert
            Mock.Assert(mockQuestionSetParser);
            Mock.Assert(() => mockQuestionSetRepository.AddAsync(result.Added), Occurs.Once());
            Mock.Assert(() => mockQuestionSetRepository.UpdateAsync(result.Updated), Occurs.Never());
            Mock.Assert(() => mockQuestionSetRepository.DeleteAsync(result.Deleted), Occurs.Never());
        }

        [Fact]
        public async Task ProcessorUpdatesDatabaseWithUpdatedEntities()
        {
            // Arrange
            var processor = CreateSut();
            var result = GetParseResults(ParseResultTypes.Updated);

            Mock.Arrange(() => mockQuestionSetParser.Parse(null))
                .IgnoreArguments()
                .Returns(result)
                .OccursOnce();

            // Act
            await processor.ProcessAsync(GetParseRequestData());

            // Assert
            Mock.Assert(mockQuestionSetParser);
            Mock.Assert(() => mockQuestionSetRepository.AddAsync(result.Added), Occurs.Never());
            Mock.Assert(() => mockQuestionSetRepository.UpdateAsync(result.Updated), Occurs.Once());
            Mock.Assert(() => mockQuestionSetRepository.DeleteAsync(result.Deleted), Occurs.Never());
        }

        [Fact]
        public async Task ProcessorUpdatesDatabaseWithDeletedEntities()
        {
            // Arrange
            var processor = CreateSut();
            var result = GetParseResults(ParseResultTypes.Deleted);

            Mock.Arrange(() => mockQuestionSetParser.Parse(null))
                .IgnoreArguments()
                .Returns(result)
                .OccursOnce();

            // Act
            await processor.ProcessAsync(GetParseRequestData());

            // Assert
            Mock.Assert(mockQuestionSetParser);
            Mock.Assert(() => mockQuestionSetRepository.AddAsync(result.Added), Occurs.Never());
            Mock.Assert(() => mockQuestionSetRepository.UpdateAsync(result.Updated), Occurs.Never());
        }

        private QuestionSetProcessor CreateSut()
        {
            return new QuestionSetProcessor(
                mockQuestionSetRepository,
                mockQuestionSetSectionMappingRepository,
                mockSectionFieldMappingRepository,
                mockSectionRepository,
                mockFieldRepository,
                mockFieldConstraintsRepository,
                mockQuestionSetParser);
        }

        private static QuestionSetParserModel GetParseRequestData()
        {
            return new QuestionSetParserModel();
        }

        private ParseResult<QS> GetParseResults(ParseResultTypes type)
        {
            var results = new QS
            {
                QSId = Guid.NewGuid()
            };

            return new ParseResult<QS>
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