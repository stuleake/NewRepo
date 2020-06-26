using Api.FormEngine.Core.SheetValidation;
using Api.FormEngine.Core.SheetValidators;
using Api.FormEngine.Core.ViewModels.Enum;
using System;

namespace Api.FormEngine.Core.Managers
{
    /// <summary>
    /// Execute sheets in excel file
    /// </summary>
    public class SheetValidatorFactory
    {
        /// <summary>
        /// Interface to manage excel sheet to execute
        /// </summary>
        /// <param name="sheet">Sheet type</param>
        /// <returns>Return repsonse of excel sheet</returns>
        public static SheetValidator GetSheetValidator(Sheets sheet)
        {
            switch (sheet)
            {
                case Sheets.Fields: return new FieldSheetValidator();
                case Sheets.AnswerGuide: return new AnswerGuideSheetValidator();
                case Sheets.Dependencies: return new DependenciesSheetValidator();
                case Sheets.Aggregations: return new AggregationSheetValidator();
                default: throw new InvalidOperationException($"{sheet} sheet is invalid");
            }
        }
    }
}