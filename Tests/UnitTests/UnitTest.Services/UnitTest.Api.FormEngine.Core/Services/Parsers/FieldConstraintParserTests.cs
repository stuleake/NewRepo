using System;
using System.Collections.Generic;
using System.Linq;
using Api.FormEngine.Core.Services.Parsers;
using Api.FormEngine.Core.Services.Parsers.Models;
using Api.FormEngine.Core.ViewModels.SheetModels;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using FluentAssertions;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;
using TQ.Core.Repository;
using UnitTest.Api.FormEngine.Core.Services.SheetValidators.Builders;
using Xunit;
using AnswerGuide = TQ.Data.FormEngine.Schemas.Forms.AnswerGuide;
using Field = TQ.Data.FormEngine.Schemas.Forms.Field;
using Section = TQ.Data.FormEngine.Schemas.Forms.Section;

namespace UnitTest.Api.FormEngine.Core.Services.Parsers
{
    /// <summary>
    /// FieldConstraintParser  Tests
    /// </summary>
    public class FieldConstraintParserTests
    {
        private readonly IReadOnlyRepository<Field> mockFieldRepository;
        private readonly IReadOnlyRepository<AnswerGuide> mockAnswerGuideRepository;
        private readonly IReadOnlyRepository<Section> mockSectionRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldConstraintParserTests"/> class.
        /// </summary>
        public FieldConstraintParserTests()
        {
            mockFieldRepository = Mock.Create<IRepository<Field>>();
            mockAnswerGuideRepository = Mock.Create<IRepository<AnswerGuide>>();
            mockSectionRepository = Mock.Create<IRepository<Section>>();
        }

        /// <summary>
        /// Test to Assert an ArgumentNullException is thrown.
        /// </summary>
        [Fact]
        public void Parse_Throws_ArgumentNullException()
        {
            // Arrange
            var sut = CreateSut();

            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => sut.Parse(null));
        }

        /// <summary>
        /// Test to Assert return collection
        /// </summary>
        [Fact]
        public void Parse_Return_Collection()
        {
            // Arrange
            const int notestItems = 5;
            const int questionSetNo = 10;

            ArrangeMock(CreateFieldsData(notestItems, out var validFieldIds), CreateAnswerGuideData(notestItems), CreateSectionData(notestItems));

            var dependencies = CreateDependenciesData(notestItems, questionSetNo);
            var model = new FieldConstraintParserModel
            {
                Dependencies = dependencies,
                QuestionSet = CreateQuestionSet(questionSetNo),
            };

            var sut = CreateSut();

            // Act
            var res = sut.Parse(model);

            // Assert
            mockFieldRepository.Assert(mock => mock.GetLocalQueryable(), Occurs.Exactly(10));
            mockAnswerGuideRepository.Assert(mock => mock.GetLocalQueryable(), Occurs.Exactly(10));
            mockSectionRepository.Assert(mock => mock.GetLocalQueryable(), Occurs.Exactly(5));

            res.Added.Should().HaveCount(cnt => cnt == notestItems);

        }

        /// <summary>
        /// Test to Assert when Dependency FileNo does not exists in FieldRepoisitory.FieldNo
        /// </summary>
        [Fact]
        public void Parse_Return_Collection_When_FileNo_NotExists()
        {
            // Arrange
            const int notestItems = 5;
            const int questionSetNo = 10;
            const int nodependencies = 3;

            ArrangeMock(CreateFieldsData(notestItems, out var validFieldIds), CreateAnswerGuideData(notestItems), CreateSectionData(notestItems));

            var dependencies = CreateDependenciesData(3, questionSetNo);
            var model = new FieldConstraintParserModel
            {
                Dependencies = dependencies,
                QuestionSet = CreateQuestionSet(questionSetNo),
            };

            var sut = CreateSut();

            // Act
            var res = sut.Parse(model);

            // Assert
            mockFieldRepository.Assert(mock => mock.GetLocalQueryable(), Occurs.Exactly(6));
            mockAnswerGuideRepository.Assert(mock => mock.GetLocalQueryable(), Occurs.Exactly(6));
            mockSectionRepository.Assert(mock => mock.GetLocalQueryable(), Occurs.Exactly(3));

            res.Added.Should().HaveCount(cnt => cnt == nodependencies);
        }

        /// <summary>
        /// Test to Assert  when Dependency DependsOnAns does not exists in AnswerGuideRepository.AnswerGuideNo
        /// </summary>
        [Fact]
        public void Parse_Return_Collection_When_DependsOnAns_NotExists()
        {
            // Arrange
            const int notestItems = 8;
            const int answerItems = 4;
            const int questionSetNo = 10;

            ArrangeMock(CreateFieldsData(notestItems, out var validFieldIds), CreateAnswerGuideData(answerItems), CreateSectionData(notestItems));

            var dependencies = CreateDependenciesData(notestItems, questionSetNo);
            var model = new FieldConstraintParserModel
            {
                Dependencies = dependencies,
                QuestionSet = CreateQuestionSet(questionSetNo),
            };

            var sut = CreateSut();

            // Act
            var res = sut.Parse(model);

            // Assert
            mockFieldRepository.Assert(mock => mock.GetLocalQueryable(), Occurs.Exactly(12));
            mockAnswerGuideRepository.Assert(mock => mock.GetLocalQueryable(), Occurs.Exactly(12));
            mockSectionRepository.Assert(mock => mock.GetLocalQueryable(), Occurs.Exactly(4));

            res.Added.Should().HaveCount(cnt => cnt == answerItems);
        }

        /// <summary>
        /// Test to Assert  when Dependacy FieldNo is not given a value
        /// </summary>
        [Fact]
        public void Parse_With_No_Dependency_FieldNo_Return_Empty()
        {
            // Arrange
            const int notestItems = 4;
            const int answerItems = 4;
            const int questionSetNo = 10;

            ArrangeMock(CreateFieldsData(notestItems, out var validFieldIds), CreateAnswerGuideData(answerItems), CreateSectionData(notestItems));

            var dependencies = new List<Dependencies>();
            var dependency = new DependencyBuilder()
                                .WithFieldNo(string.Empty)
                                .WithSectionNo("1")
                                .WithDependsOnAnsfromQS(questionSetNo.ToString())
                                .WithDependsOnAns("1")
                                .Build();
            dependencies.Add(dependency);
 
            var model = new FieldConstraintParserModel
            {
                Dependencies = dependencies,
                QuestionSet = CreateQuestionSet(questionSetNo),
            };
            var sut = CreateSut();

            // Act
            var res = sut.Parse(model);

            // Assert
            mockFieldRepository.Assert(mock => mock.GetLocalQueryable(), Occurs.Never());
            mockAnswerGuideRepository.Assert(mock => mock.GetLocalQueryable(), Occurs.Never());
            mockSectionRepository.Assert(mock => mock.GetLocalQueryable(), Occurs.Never());

            res.Added.Should().BeEmpty();
        }

        /// <summary>
        ///  Test to Assert when Dependency Section does not have a value
        /// </summary>
        [Fact]
        public void Parse_With_No_Dependency_SectionNo_Return_SectionId_EmptyGuid()
        {
            // Arrange
            var notestItems = 4;
            var answerItems = 4;
            var questionSetNo = 10;

            ArrangeMock(CreateFieldsData(notestItems, out var validFieldIds), CreateAnswerGuideData(answerItems), CreateSectionData(notestItems));

            var dependencies = new List<Dependencies>
            {
                {
                    new DependencyBuilder()
                        .WithFieldNo("1")
                        .WithSectionNo(string.Empty)
                        .WithDependsOnAnsfromQS(questionSetNo.ToString())
                        .WithDependsOnAns("1").Build()
                },
            };

            var model = new FieldConstraintParserModel
            {
                Dependencies = dependencies,
                QuestionSet = CreateQuestionSet(questionSetNo),
            };
            var sut = CreateSut();

            // Act
            var res = sut.Parse(model);

            // Assert
            mockFieldRepository.Assert(mock => mock.GetLocalQueryable(), Occurs.Exactly(2));
            mockAnswerGuideRepository.Assert(mock => mock.GetLocalQueryable(), Occurs.Exactly(2));
            mockSectionRepository.Assert(mock => mock.GetLocalQueryable(), Occurs.Never());

            res.Added.Should().HaveCount(cnt => cnt == 1);
            res.Added.First().SectionId.Should().Be(Guid.Empty);
        }

        /// <summary>
        /// Test to Assert when Dependacy DependsOnAnsfromQS does not match questionset
        /// </summary>
        [Fact]
        public void Parse_With_NoMatch_Dependency_DependsOnAnsfromQS()
        {
            // Arrange
            const int notestItems = 5;
            const int answerItems = 5;

            ArrangeMock(CreateFieldsData(notestItems, out var validFieldIds), CreateAnswerGuideData(answerItems), CreateSectionData(notestItems));

            var dependencies = new List<Dependencies>
            {
                {
                    new DependencyBuilder()
                        .WithFieldNo("1")
                        .WithSectionNo("1")
                        .WithDependsOnAnsfromQS("22")
                        .WithDependsOnAns("1").Build()
                },
            };

            var model = new FieldConstraintParserModel
            {
                Dependencies = dependencies,
                QuestionSet = CreateQuestionSet(11),
            };
            var sut = CreateSut();

            // Act
            var res = sut.Parse(model);

            // Assert
            mockFieldRepository.Assert(mock => mock.GetLocalQueryable(), Occurs.Exactly(2));
            mockAnswerGuideRepository.Assert(mock => mock.GetLocalQueryable(), Occurs.Exactly(1));
            mockSectionRepository.Assert(mock => mock.GetLocalQueryable(), Occurs.Exactly(1));

            res.Added.Should().HaveCount(cnt => cnt == 1);
            res.Added.First().DependantAnswerGuideId.Should().Be(Guid.Empty);
        }

        private FieldConstraintParser CreateSut()
        {
            return new FieldConstraintParser(mockFieldRepository, mockAnswerGuideRepository, mockSectionRepository);
        }

        private void ArrangeMock(List<Field> fields, List<AnswerGuide> answerGuides, List<Section> sections) 
        {
            mockFieldRepository.Arrange(repository => repository.GetLocalQueryable())
                .Returns(new EnumerableQuery<Field>(fields));
            mockAnswerGuideRepository.Arrange(repository => repository.GetLocalQueryable())
                .Returns(new EnumerableQuery<AnswerGuide>(answerGuides));
            mockSectionRepository.Arrange(repository => repository.GetLocalQueryable())
                .Returns(new EnumerableQuery<Section>(sections));
        }

        private static QuestionSet CreateQuestionSet(int id)
        {
            return new QuestionSetBuilder()
                            .WithQsNo(id.ToString())
                            .WithQSDesc($"Test Question Set {id}")
                            .WithQSLabel($"Test Question Set Label {id}")
                            .WithQSHelptext($"Test Question Text Helper Text {id}")
                            .Build();
        }

        /// <summary>
        ///  Create valid Field data
        /// </summary>
        /// <param name="noItems">Number of items to create</param>
        /// <param name="fieldIds">List of Guid</param>
        /// <returns>A <see cref="List{Field}"/></returns>
        private static List<Field> CreateFieldsData(int noItems, out List<Guid> fieldIds)
        {
            var fields = new List<Field>();
            fieldIds = new List<Guid>();
            for (int i = 1; i <= noItems; i++)
            {
                var newId = Guid.NewGuid();
                fieldIds.Add(newId);
                fields.Add(
                        new Field
                        {
                            FieldId = newId,
                            FieldNo = i,
                        });
            }
            return fields;
        }

        /// <summary>
        /// Create valid Section data
        /// </summary>
        /// <param name="noItems">Number of items to create</param>
        /// <returns>A <see cref="List{Section}"/></returns>
        private static List<Section> CreateSectionData(int noItems)
        {
            var sections = new List<Section>();

            for (int i = 1; i <= noItems; i++)
            {
                sections.Add(
                        new Section
                        {
                            SectionId = Guid.NewGuid(),
                            SectionNo = i,
                        });
            }
            return sections;
        }

        /// <summary>
        ///  Create valid Dependencies data
        /// </summary>
        /// <param name="noItems">Number of items to create</param>
        /// <param name="questionSetNo">Set DependsOnAnsfromQS</param>
        /// <returns>A <see cref="List{Dependencies}"/></returns>
        private static List<Dependencies> CreateDependenciesData(int noItems, int questionSetNo)
        {
            var dependencies = new List<Dependencies>();

            for (int i = 1; i <= noItems; i++)
            {
                dependencies.Add(new DependencyBuilder()
                                    .WithFieldNo(i.ToString())
                                    .WithSectionNo(i.ToString())
                                    .WithDependsOnAnsfromQS(questionSetNo.ToString())
                                    .WithDependsOnAns(i.ToString())
                                    .Build());
            }
            return dependencies;
        }

        /// <summary>
        /// Create valid AnswerGuide data
        /// </summary>
        /// <param name="noItems">Number of items to create</param>
        /// <returns>A <see cref="List{AnswerGuide}"/></returns>
        private static List<AnswerGuide> CreateAnswerGuideData(int noItems)
        {
            var answerGuides = new List<AnswerGuide>();
            for (int i = 1; i <= noItems; i++)
            {
                answerGuides.Add(
                        new AnswerGuide
                        {
                            AnswerGuideId = Guid.NewGuid(),
                            AnswerGuideNo = i,
                        });
            }
            return answerGuides;
        }
    }
}
