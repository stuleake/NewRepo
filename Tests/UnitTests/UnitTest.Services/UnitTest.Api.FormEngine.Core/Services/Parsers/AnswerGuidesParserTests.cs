using Api.FormEngine.Core.Services.Parsers;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;
using TQ.Core.Repository;
using TQ.Data.FormEngine.Schemas.Forms;
using UnitTest.Api.FormEngine.Core.Services.SheetValidators.Builders;
using Xunit;

namespace UnitTest.Api.FormEngine.Core.Services.Parsers
{
    /// <summary>
    /// Answer Guides Parser Tests.
    /// </summary>
    public class AnswerGuidesParserTests
    {
        private readonly IReadOnlyRepository<Field> fieldMock;
        private readonly IReadOnlyRepository<AnswerType> answerTypeMock;
        private const int AnswerTypeId = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnswerGuidesParserTests"/> class.
        /// </summary>
        public AnswerGuidesParserTests()
        {
            fieldMock = Mock.Create<IReadOnlyRepository<Field>>();
            answerTypeMock = Mock.Create<IReadOnlyRepository<AnswerType>>();
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
        /// Test to Assert empty collection for the Added property of the result when FieldNo isn't present in the repository.
        /// </summary>
        [Fact]
        public void Parse_Returns_Empty_Collection_When_FieldNo_NotFoundInFieldRepository()
        {
            // Arrange
            var sut = CreateSut();

            fieldMock.Arrange(repository => repository.GetLocalQueryable())
                .Returns(new EnumerableQuery<Field>(new List<Field>()));

            var answerGuides = new List<global::Api.FormEngine.Core.ViewModels.SheetModels.AnswerGuide>
            {
                new AnswerGuidesBuilder().Build()
            };

            // Act
            var result = sut.Parse(answerGuides);

            // Assert
            result.Added.Should().BeEmpty();
        }

        /// <summary>
        /// Test to Assert range data is added to the Added Collection.
        /// </summary>
        [Fact]
        public void Parse_Returns_Collection_WithRangeData()
        {
            // Arrange
            var sut = CreateSut();

            var fieldId = Guid.NewGuid();

            ArrangeFieldMock(fieldId);
            ArrangeAnswerTypeMock("Range");

            var answerGuides = new List<global::Api.FormEngine.Core.ViewModels.SheetModels.AnswerGuide>
            {
                new AnswerGuidesBuilder()
                    .WithMinRange("1")
                    .WithMaxRange("10")
                    .WithErrorLabel("This is an error label.")
                    .Build()
            };

            // Act
            var result = sut.Parse(answerGuides);

            // Assert
            result.Added.Should().BeEquivalentTo(
                new List<AnswerGuide>
                {
                    new AnswerGuide
                    {
                        FieldId = fieldId,
                        Min = "1",
                        Max = "10",
                        ErrLabel = "This is an error label.",
                        AnswerGuideNo = 1,
                        AnswerTypeId = AnswerTypeId
                    }
                }, options => options.ExcludingMissingMembers());
        }

        /// <summary>
        /// Test to Assert length data is added to the Added Collection.
        /// </summary>
        [Fact]
        public void Parse_Returns_Collection_WithLengthData()
        {
            // Arrange
            var sut = CreateSut();

            var fieldId = Guid.NewGuid();

            ArrangeFieldMock(fieldId);
            ArrangeAnswerTypeMock("Length");

            var answerGuides = new List<global::Api.FormEngine.Core.ViewModels.SheetModels.AnswerGuide>
            {
                new AnswerGuidesBuilder()
                    .WithMinLength("1")
                    .WithMaxLength("100")
                    .WithErrorLabel("This is an error label.")
                    .Build()
            };

            // Act
            var result = sut.Parse(answerGuides);

            // Assert
            result.Added.Should().BeEquivalentTo(
                new List<AnswerGuide>
                {
                    new AnswerGuide
                    {
                        FieldId = fieldId,
                        Min = "1",
                        Max = "100",
                        ErrLabel = "This is an error label.",
                        AnswerGuideNo = 1,
                        AnswerTypeId = AnswerTypeId
                    }
                }, options => options.ExcludingMissingMembers());
        }

        /// <summary>
        /// Test to Assert date data is added to the Added Collection.
        /// </summary>
        [Fact]
        public void Parse_Returns_Collection_WithDateData()
        {
            // Arrange
            var sut = CreateSut();

            var fieldId = Guid.NewGuid();
            var minDate = DateTime.UtcNow;
            var maxDate = DateTime.UtcNow.AddDays(10);

            ArrangeFieldMock(fieldId);
            ArrangeAnswerTypeMock("Date");

            var answerGuides = new List<global::Api.FormEngine.Core.ViewModels.SheetModels.AnswerGuide>
            {
                new AnswerGuidesBuilder()
                    .WithMinDate(minDate.ToString(CultureInfo.InvariantCulture))
                    .WithMaxDate(maxDate.ToString(CultureInfo.InvariantCulture))
                    .WithErrorLabel("This is an error label.")
                    .Build()
            };

            // Act
            var result = sut.Parse(answerGuides);

            // Assert
            result.Added.Should().BeEquivalentTo(
                new List<AnswerGuide>
                {
                    new AnswerGuide
                    {
                        FieldId = fieldId,
                        Min = minDate.ToString(CultureInfo.InvariantCulture),
                        Max = maxDate.ToString(CultureInfo.InvariantCulture),
                        ErrLabel = "This is an error label.",
                        AnswerGuideNo = 1,
                        AnswerTypeId = AnswerTypeId
                    }
                }, options => options.ExcludingMissingMembers());
        }

        /// <summary>
        /// Test to Assert regex data is added to the Added Collection.
        /// </summary>
        [Fact]
        public void Parse_Returns_Collection_WithRegexData()
        {
            // Arrange
            var sut = CreateSut();

            var fieldId = Guid.NewGuid();

            ArrangeFieldMock(fieldId);
            ArrangeAnswerTypeMock("regex");

            var answerGuides = new List<global::Api.FormEngine.Core.ViewModels.SheetModels.AnswerGuide>
            {
                new AnswerGuidesBuilder()
                    .WithRegex("regexValue")
                    .WithErrorLabel("This is an error label.")
                    .Build()
            };

            // Act
            var result = sut.Parse(answerGuides);

            // Assert
            result.Added.Should().BeEquivalentTo(
                new List<AnswerGuide>
                {
                    new AnswerGuide
                    {
                        FieldId = fieldId,
                        ErrLabel = "This is an error label.",
                        AnswerGuideNo = 1,
                        AnswerTypeId = AnswerTypeId,
                        Value = "regexValue"
                    }
                }, options => options.ExcludingMissingMembers());
        }

        /// <summary>
        /// Test to Assert regexBE data is added to the Added Collection.
        /// </summary>
        [Fact]
        public void Parse_Returns_Collection_WithRegexBeData()
        {
            // Arrange
            var sut = CreateSut();

            var fieldId = Guid.NewGuid();

            ArrangeFieldMock(fieldId);
            ArrangeAnswerTypeMock("RegexBE");

            var answerGuides = new List<global::Api.FormEngine.Core.ViewModels.SheetModels.AnswerGuide>
            {
                new AnswerGuidesBuilder()
                    .WithRegexBe("regexBEValue")
                    .WithErrorLabel("This is an error label.")
                    .Build()
            };

            // Act
            var result = sut.Parse(answerGuides);

            // Assert
            result.Added.Should().BeEquivalentTo(
                new List<AnswerGuide>
                {
                    new AnswerGuide
                    {
                        FieldId = fieldId,
                        ErrLabel = "This is an error label.",
                        AnswerGuideNo = 1,
                        AnswerTypeId = AnswerTypeId,
                        Value = "regexBEValue"
                    }
                }, options => options.ExcludingMissingMembers());
        }

        /// <summary>
        /// Test to Assert multiple data is added to the Added Collection.
        /// </summary>
        [Fact]
        public void Parse_Returns_Collection_WithMultipleData()
        {
            // Arrange
            var sut = CreateSut();

            var fieldId = Guid.NewGuid();

            ArrangeFieldMock(fieldId);
            ArrangeAnswerTypeMock("Multiple");

            var answerGuides = new List<global::Api.FormEngine.Core.ViewModels.SheetModels.AnswerGuide>
            {
                new AnswerGuidesBuilder()
                    .WithMultiple("multipleData")
                    .WithErrorLabel("This is an error label.")
                    .Build()
            };

            // Act
            var result = sut.Parse(answerGuides);

            // Assert
            result.Added.Should().BeEquivalentTo(
                new List<AnswerGuide>
                {
                    new AnswerGuide
                    {
                        FieldId = fieldId,
                        ErrLabel = "This is an error label.",
                        AnswerGuideNo = 1,
                        AnswerTypeId = AnswerTypeId,
                        Value = "multipleData"
                    }
                }, options => options.ExcludingMissingMembers());
        }

        /// <summary>
        /// Test to Assert value data is added to the Added Collection.
        /// </summary>
        [Fact]
        public void Parse_Returns_Collection_WithValueData()
        {
            // Arrange
            var sut = CreateSut();

            var fieldId = Guid.NewGuid();

            ArrangeFieldMock(fieldId);
            ArrangeAnswerTypeMock("Value");

            var answerGuides = new List<global::Api.FormEngine.Core.ViewModels.SheetModels.AnswerGuide>
            {
                new AnswerGuidesBuilder()
                    .WithLabel("label")
                    .WithValue("value")
                    .WithIsDefault("true")
                    .WithErrorLabel("This is an error label.")
                    .Build()
            };

            // Act
            var result = sut.Parse(answerGuides);

            // Assert
            result.Added.Should().BeEquivalentTo(
                new List<AnswerGuide>
                {
                    new AnswerGuide
                    {
                        FieldId = fieldId,
                        ErrLabel = "This is an error label.",
                        AnswerGuideNo = 1,
                        AnswerTypeId = AnswerTypeId,
                        Label = "label",
                        Value = "value",
                        Sequence = 1,
                        IsDefault = "true"
                    }
                }, options => options.ExcludingMissingMembers());
        }

        /// <summary>
        /// Test to Assert api data is added to the Added Collection.
        /// </summary>
        [Fact]
        public void Parse_Returns_Collection_WithApiData()
        {
            // Arrange
            var sut = CreateSut();

            var fieldId = Guid.NewGuid();

            ArrangeFieldMock(fieldId);
            ArrangeAnswerTypeMock("API");

            var answerGuides = new List<global::Api.FormEngine.Core.ViewModels.SheetModels.AnswerGuide>
            {
                new AnswerGuidesBuilder()
                    .WithApi("apiData")
                    .WithErrorLabel("This is an error label.")
                    .Build()
            };

            // Act
            var result = sut.Parse(answerGuides);

            // Assert
            result.Added.Should().BeEquivalentTo(
                new List<AnswerGuide>
                {
                    new AnswerGuide
                    {
                        FieldId = fieldId,
                        ErrLabel = "This is an error label.",
                        AnswerGuideNo = 1,
                        AnswerTypeId = AnswerTypeId,
                        Value = "apiData"
                    }
                }, options => options.ExcludingMissingMembers());
        }

        private void ArrangeFieldMock(Guid fieldId)
        {
            fieldMock.Arrange(repository => repository.GetLocalQueryable())
                .Returns(new EnumerableQuery<Field>(new List<Field>
                {
                    new Field
                    {
                        FieldNo = 7,
                        FieldId = fieldId
                    }
                }));
        }

        private void ArrangeAnswerTypeMock(string answerType)
        {
            answerTypeMock.Arrange(repository => repository.GetQueryable())
                .Returns(new EnumerableQuery<AnswerType>(new List<AnswerType>
                {
                    new AnswerType
                    {
                        AnswerTypeId = AnswerTypeId,
                        AnswerTypes = answerType
                    }
                }));
        }

        private AnswerGuidesParser CreateSut()
        {
            return new AnswerGuidesParser(fieldMock, answerTypeMock);
        }
    }
}