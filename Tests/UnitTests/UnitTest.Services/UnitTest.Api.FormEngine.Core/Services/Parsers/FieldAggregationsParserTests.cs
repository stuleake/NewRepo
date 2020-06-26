using Api.FormEngine.Core.Services.Parsers;
using Api.FormEngine.Core.ViewModels.SheetModels;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;
using TQ.Core.Repository;
using TQ.Data.FormEngine.Schemas.Forms;
using UnitTest.Api.FormEngine.Core.Services.SheetValidators.Builders;
using Xunit;
using Field = TQ.Data.FormEngine.Schemas.Forms.Field;

namespace UnitTest.Api.FormEngine.Core.Services.Parsers
{
    /// <summary>
    /// FieldAggregationsParser Tests
    /// </summary>
    public class FieldAggregationsParserTests
    {
        private readonly IReadOnlyRepository<Field> mockFieldRepository;
        private readonly IReadOnlyRepository<Function> mockFunctionRepository;
        private const int NoParseFieldRepoCall = 3;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldAggregationsParserTests"/> class.
        /// </summary>
        public FieldAggregationsParserTests()
        {
            mockFieldRepository = Mock.Create<IReadOnlyRepository<Field>>();
            mockFunctionRepository = Mock.Create<IReadOnlyRepository<Function>>();
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
        /// Test that only FieldAggregation are included when Field No exists
        /// </summary>
        [Fact]
        public void Parse_Return_EmptyCollection_When_FieldNo_NotExists()
        {
            // Arrange
            var validFieldId = Guid.NewGuid();
            var fieldData = new List<Field>
                {
                    new Field
                        {
                            FieldNo = 2,
                            FieldId = validFieldId,
                        },
                };
            var functionData = new List<Function>
                {
                    new Function
                        {
                           FunctionsId = 2,
                           Functions = "Function 2",
                        },
                };

            ArrangeMocks(fieldData, functionData);

            var sut = CreateSut();
            var model = new List<Aggregations>
                {
                    new AggregationsBuilder()
                            .WithFieldNo("10")
                            .WithAggregatedFieldNo("1")
                            .WithFunction("111")
                            .WithPriority("1111")
                            .Build(),
                    new AggregationsBuilder()
                            .WithFieldNo("20")
                            .WithAggregatedFieldNo("2")
                            .WithFunction("222")
                            .WithPriority("2222")
                            .Build(),
                    new AggregationsBuilder()
                            .WithFieldNo("30")
                            .WithAggregatedFieldNo("3")
                            .WithFunction("333")
                            .WithPriority("3333")
                            .Build(),
                };

            // Act
            var res = sut.Parse(model);

            // Assert
            mockFieldRepository.Assert(mock => mock.GetLocalQueryable(), Occurs.Exactly(model.Count));
            mockFunctionRepository.Assert(mock => mock.GetQueryable(), Occurs.Never());

            res.Added.Should().BeEmpty();
        }

        /// <summary>
        /// Test Parse returns all
        /// </summary>
        [Fact]
        public void Parse_Return_Collection()
        {
            // Arrange
            const int noItems = 10;
            ArrangeMocks(CreateFieldData(noItems, out var validFieldIds), CreateFunctionsData(noItems));

            var sut = CreateSut();
            var model = CreateAggregationsData(3);

            // Act
            var res = sut.Parse(model);

            // Assert
            mockFieldRepository.Assert(mock => mock.GetLocalQueryable(), Occurs.AtLeast(model.Count * NoParseFieldRepoCall));
            mockFunctionRepository.Assert(mock => mock.GetQueryable(), Occurs.AtLeast(model.Count));

            res.Added.Should().HaveSameCount(model)
                .And.OnlyContain(c => validFieldIds.Contains(c.FieldId));
        }


        /// <summary>
        /// Test that only FieldAggregation are included when Field No exists
        /// </summary>
        [Fact]
        public void Parse_Return_Collection_Only_When_FieldNo_Exists()
        {
            // Arrange
            const int noItems = 3;
            ArrangeMocks(CreateFieldData(noItems, out var validFieldIds), CreateFunctionsData(noItems));

            var sut = CreateSut();
            var model = CreateAggregationsData(noItems);

            // invalid Aggregations, FieldNo not exist
            model.Add(
                 new AggregationsBuilder()
                        .WithFieldNo("123")
                        .WithAggregatedFieldNo("1231")
                        .WithFunction("123111")
                        .WithPriority("1231111")
                        .Build());

            // Act
            var res = sut.Parse(model);

            // Assert
            var expectFieldRepoCalls = (noItems * NoParseFieldRepoCall) + 1;
            mockFieldRepository.Assert(mock => mock.GetLocalQueryable(), Occurs.AtLeast(expectFieldRepoCalls));
            mockFunctionRepository.Assert(mock => mock.GetQueryable(), Occurs.AtLeast(noItems));

            res.Added.Should().HaveCount(noItems)
                .And.OnlyContain(c => validFieldIds.Contains(c.FieldId));
        }

        private FieldAggregationsParser CreateSut()
        {
            return new FieldAggregationsParser(mockFieldRepository, mockFunctionRepository);
        }

        private void ArrangeMocks(List<Field> fieldData, List<Function> functionData) 
        {
            mockFieldRepository.Arrange(repository => repository.GetLocalQueryable())
                  .Returns(new EnumerableQuery<Field>(fieldData));
            mockFunctionRepository.Arrange(repository => repository.GetQueryable())
                .Returns(new EnumerableQuery<Function>(functionData));
        }

        /// <summary>
        ///  Create valid Fields data
        /// </summary>
        /// <param name="noItems">number of items to create</param>
        /// <param name="fieldIds"> List of Id created</param>
        /// <returns>A <see cref="List{Field}"/></returns>
        private List<Field> CreateFieldData(int noItems, out List<Guid> fieldIds)
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
                        FieldNo = i,
                        FieldId = newId,
                    });
            }
            return fields;
        }

        /// <summary>
        /// Create valid Functions data
        /// </summary>
        /// <param name="noItems">number of items to create</param>
        /// <returns>A <see cref="List{Function}"/></returns>
        private static List<Function> CreateFunctionsData(int noItems)
        {
            var functions = new List<Function>();

            for (int i = 1; i <= noItems; i++)
            {
                functions.Add(
                      new Function
                      {
                          FunctionsId = i,
                          Functions = $"Function {i}",
                      });
            }
            return functions;
        }

        /// <summary>
        ///  Create valid Aggregations data
        /// </summary>
        /// <param name="noItems">number of items to create</param>
        /// <returns>A <see cref="List{Aggregations}"/></returns>
        private static List<Aggregations> CreateAggregationsData(int noItems)
        {
            var aggregations = new List<Aggregations>();

            for (int i = 1; i <= noItems; i++)
            {
                aggregations.Add(
                        new AggregationsBuilder()
                                .WithFieldNo(i.ToString())
                                .WithAggregatedFieldNo(i.ToString())
                                .WithFunction($"Function {i}")
                                .WithPriority(i.ToString())
                                .Build());
            }
            return aggregations;
        }
    }
}
