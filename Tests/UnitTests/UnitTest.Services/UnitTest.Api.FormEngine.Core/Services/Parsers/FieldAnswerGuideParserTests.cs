using System;
using System.Collections.Generic;
using System.Linq;
using Api.FormEngine.Core;
using Api.FormEngine.Core.Services.Parsers;
using Api.FormEngine.Core.Services.Parsers.Models;
using Api.FormEngine.Core.ViewModels.SheetModels;
using AutoMapper;
using FluentAssertions;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;
using TQ.Core.Repository;
using TQ.Data.FormEngine.Schemas.Forms;
using UnitTest.Api.FormEngine.Core.Services.SheetValidators.Builders;
using Xunit;
using Field = Api.FormEngine.Core.ViewModels.SheetModels.Field;

namespace UnitTest.Api.FormEngine.Core.Services.Parsers
{
    /// <summary>
    /// FieldAnswerGuideParser  Tests
    /// </summary>
    public class FieldAnswerGuideParserTests
    {
        /// <summary>
        /// CopyFrom
        /// </summary>
        private const string CopyFrom = "copyFrom";
        private readonly IRepository<AnswerType> mockAnswerTypeRepository;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldAnswerGuideParserTests"/> class.
        /// </summary>
        public FieldAnswerGuideParserTests()
        {
            mockAnswerTypeRepository = Mock.Create<IRepository<AnswerType>>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            mapper = config.CreateMapper();
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
        /// Test to Assert empty collection is returned when CopyfromQS is not supplied
        /// </summary>
        [Fact]
        public void Parse_Return_EmptyCollection_When_CopyfromQS_IsEmpty()
        {
            // Arrange
            var fields = new List<Field>
                {
                    new FieldBuilder()
                            .WithFieldNo("1")
                            .WithCopyfromQS("1")
                            .WithCopyfromField(string.Empty)
                            .Build(),
                };
            var model = new FieldParserModel
            {
                Fields = fields,
                Dependencies = null,
                QuestionSet = null,
                UserId = Guid.NewGuid(),
            };

            var sut = CreateSut();

            // Act
            var res = sut.Parse(model);

            // Assert
            res.Added.Should().BeEmpty();
        }

        /// <summary>
        /// Test to Assert empty collection is returned when CopyfromQS is not supplied
        /// </summary>
        [Fact]
        public void Parse_Return_EmptyCollection_When_CopyfromField_isEmpty()
        {
            // Arrange
            var fields = new List<Field>
                {
                    new FieldBuilder()
                            .WithFieldNo("2")
                            .WithCopyfromQS(string.Empty)
                            .WithCopyfromField("2")
                            .Build(),
                };
            var model = new FieldParserModel
            {
                Fields = fields,
                Dependencies = null,
                QuestionSet = null,
                UserId = Guid.NewGuid(),
            };

            var sut = CreateSut();

            // Act
            var res = sut.Parse(model);

            // Assert
            res.Added.Should().BeEmpty();
        }

        /// <summary>
        /// Test to Assert that fields are all returned
        /// </summary>
        [Fact]
        public void Parse_Return_AnswerGuide_Collection()
        {
            // Arrange
            var answerTypeData = new List<AnswerType>
                {
                    new AnswerType
                        {
                           AnswerTypeId = 100,
                           AnswerTypes = CopyFrom,
                        },
                };
            ArrangeMock(answerTypeData);

            const int noTestItems = 3;
            var fields = CreateFieldsData(noTestItems);
            var dependencies = CreateDependenciesData(noTestItems);

            var model = new FieldParserModel
            {
                Fields = fields,
                Dependencies = dependencies,
                QuestionSet = null,
                UserId = Guid.NewGuid(),
            };

            var sut = CreateSut();

            // Act
            var res = sut.Parse(model);

            // Assert
            mockAnswerTypeRepository.Assert(mock => mock.GetQueryable(), Occurs.Exactly(noTestItems));
            
            res.Added.Should().NotBeNull()
                    .And.HaveCount(c => c == fields.Count);
        }

        /// <summary>
        /// Test to Assert that valid fields are all returned
        /// </summary>
        [Fact]
        public void Parse_Return_Collection_Of_Valid_Fields()
        {
            // Arrange
            var answerTypeData = new List<AnswerType>
                {
                    new AnswerType
                        {
                           AnswerTypeId = 100,
                           AnswerTypes = CopyFrom,
                        },
                };

            ArrangeMock(answerTypeData);

            const int noTestItems = 5;

            var fields = CreateFieldsData(noTestItems);
            var dependencies = CreateDependenciesData(noTestItems);

            // Invalid field
            fields.Add(
                    new Field
                    {
                        FieldNo = "65",
                        CopyfromQS = string.Empty,
                        CopyfromField = "65",
                    });

            var model = new FieldParserModel
            {
                Fields = fields,
                Dependencies = dependencies,
                QuestionSet = null,
                UserId = Guid.NewGuid(),
            };

            var sut = CreateSut();

            // Act
            var res = sut.Parse(model);

            // Assert
            mockAnswerTypeRepository.Assert(mock => mock.GetQueryable(), Occurs.Exactly(noTestItems));
            res.Added.Should().NotBeNull()
                    .And.HaveCount(c => c == noTestItems);
        }

        private FieldAnswerGuideParser CreateSut()
        {
            return new FieldAnswerGuideParser(mockAnswerTypeRepository, mapper);
        }

        /// <summary>
        /// Create valid Fields data
        /// </summary>
        /// <param name="noItems">Number of items to create</param>
        /// <returns>A <see cref="List{Field}"/></returns>
        private static List<Field> CreateFieldsData(int noItems)
        {
            var fields = new List<Field>();

            for (int i = 1; i <= noItems; i++)
            {
                fields.Add(
                    new FieldBuilder()
                            .WithFieldNo(i.ToString())
                            .WithCopyfromQS(i.ToString())
                            .WithCopyfromField(i.ToString())
                            .Build());
            }
            return fields;
        }

        /// <summary>
        /// Create valid Dependencies data
        /// </summary>
        /// <param name="noItems">Number of items to create</param>
        /// <returns>A <see cref="List{Dependencies}"/></returns>
        private static List<Dependencies> CreateDependenciesData(int noItems)
        {
            var dependencies = new List<Dependencies>();

            for (int i = 1; i <= noItems; i++)
            {
                dependencies.Add(
                    new DependencyBuilder()
                        .WithFieldNo(i.ToString())
                        .WithDependsOnAnsfromQS(i.ToString())
                        .Build());
            }
            return dependencies;
        }

        private void ArrangeMock(List<AnswerType> answerTypeData)
        {
            mockAnswerTypeRepository.Arrange(repository => repository.GetQueryable())
                .Returns(new EnumerableQuery<AnswerType>(answerTypeData));
        }
    }
}
