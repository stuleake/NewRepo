using Api.FormEngine.Core.ViewModels.SheetModels;
using System.Collections.Generic;

namespace UnitTest.Api.FormEngine.Core.Services.SheetValidators.Builders
{
    /// <summary>
    /// ExcelSheetsData Builder implementation.
    /// </summary>
    public class ExcelSheetsDataBuilder
    {
        private readonly ExcelSheetsData excelSheetsData;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelSheetsDataBuilder"/> class.
        /// </summary>
        public ExcelSheetsDataBuilder()
        {
            excelSheetsData = new ExcelSheetsData
            {
                Aggregations = new List<Aggregations>(),
                AnswerGuides = new List<AnswerGuide>(),
                Dependencies = new List<Dependencies>(),
                QuestionSet = new List<QuestionSet>()
            };
        }

        /// <summary>
        /// Method to set Dependencies property.
        /// </summary>
        /// <param name="dependencies">The value to set.</param>
        /// <returns>An instance of <see cref="ExcelSheetsDataBuilder"/>.</returns>
        public ExcelSheetsDataBuilder WithDependency(Dependencies dependencies)
        {
            excelSheetsData.Dependencies.Add(dependencies);
            return this;
        }

        /// <summary>
        /// Method to set QuestionSet property.
        /// </summary>
        /// <param name="questionSet">The value to set.</param>
        /// <returns>An instance of <see cref="ExcelSheetsDataBuilder"/>.</returns>
        public ExcelSheetsDataBuilder WithQuestionSet(QuestionSet questionSet)
        {
            excelSheetsData.QuestionSet.Add(questionSet);
            return this;
        }

        /// <summary>
        /// Method to set AnswerGuides property.
        /// </summary>
        /// <param name="answerGuide">The value to set.</param>
        /// <returns>An instance of <see cref="ExcelSheetsDataBuilder"/>.</returns>
        public ExcelSheetsDataBuilder WithAnswerGuide(AnswerGuide answerGuide)
        {
            excelSheetsData.AnswerGuides.Add(answerGuide);
            return this;
        }

        /// <summary>
        /// Method to set Aggregations.
        /// </summary>
        /// <param name="aggregations">The value to set.</param>
        /// <returns>An instance of <see cref="ExcelSheetsDataBuilder"/>.</returns>
        public ExcelSheetsDataBuilder WithAggregations(Aggregations aggregations)
        {
            excelSheetsData.Aggregations.Add(aggregations);
            return this;
        }

        /// <summary>
        /// Builds an instance of <see cref="ExcelSheetsData"/>.
        /// </summary>
        /// <returns>An instance of <see cref="ExcelSheetsData"/>.</returns>
        public ExcelSheetsData Build()
        {
            return excelSheetsData;
        }
    }
}
