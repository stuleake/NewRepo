using Api.FormEngine.Core.Constants;
using Api.FormEngine.Core.ViewModels.SheetModels;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TQ.Data.FormEngine;

namespace Api.FormEngine.Core.SheetValidators
{
    /// <summary>
    /// Validation of answer guide sheet
    /// </summary>
    public class AnswerGuideSheetValidator : SheetValidator
    {
        /// <summary>
        /// Execute answer guide sheet
        /// </summary>
        /// <param name="sheetData">Excel sheet details</param>
        /// <param name="formSturcutreTypes">form stuecture types list from database</param>
        /// <param name="formsEngineContext">form engine context</param>
        /// <returns>Response Message of answer guide sheet</returns>
        public override async Task<string> ExecuteSheetAsync(ExcelSheetsData sheetData, FormStructureData formSturcutreTypes, FormsEngineContext formsEngineContext)
        {
            return await Task.Run(() =>
            {
                StringBuilder answerGuideErrorMessage = new StringBuilder();
                foreach (var answerGuide in sheetData.AnswerGuides)
                {
                    answerGuideErrorMessage.AppendLine(CheckIsNumber(answerGuide.FieldNo, ApplicationConstants.Field));
                    answerGuideErrorMessage.AppendLine(CheckIsNumber(answerGuide.AnsNo, ApplicationConstants.Answer));

                    var isFieldExistInQuestionSet = sheetData.QuestionSet
                        .Any(questionSet => questionSet.Sections
                        .Any(section => section.Fields
                        .Any(field => field.FieldNo == answerGuide.FieldNo)));

                    if (!isFieldExistInQuestionSet)
                    {
                        answerGuideErrorMessage.AppendLine(string.Format(ApplicationConstants.FieldNotExistErrorMessage, ApplicationConstants.FieldNumber, answerGuide.FieldNo));
                    }
                    answerGuideErrorMessage.AppendLine(CheckValueIsNumberAndNotEmpty(answerGuide.RangeMin, ApplicationConstants.RangeMin));
                    answerGuideErrorMessage.AppendLine(CheckValueIsNumberAndNotEmpty(answerGuide.RangeMax, ApplicationConstants.RangeMax));
                    answerGuideErrorMessage.AppendLine(CheckValueIsNumberAndNotEmpty(answerGuide.LengthMin, ApplicationConstants.LengthMin));
                    answerGuideErrorMessage.AppendLine(CheckValueIsNumberAndNotEmpty(answerGuide.LengthMax, ApplicationConstants.LengthMax));
                    answerGuideErrorMessage.AppendLine(CheckValueIsNumberAndNotEmpty(answerGuide.Multiple, ApplicationConstants.Multiple));
                    answerGuideErrorMessage.AppendLine(BoolErrorMessage(answerGuide.IsDefault, ApplicationConstants.IsDefault));
                    if (!answerGuide.DateMin.Equals(ApplicationConstants.Today, System.StringComparison.InvariantCultureIgnoreCase)
                    && CheckIsNotNullOrEmpty(answerGuide.DateMin)
                    && !CheckIsNumber(answerGuide.DateMin))
                    {
                        answerGuideErrorMessage.AppendLine(ApplicationConstants.MinDateNotNumberError);
                    }
                    if (!answerGuide.DateMax.Equals(ApplicationConstants.Today, System.StringComparison.InvariantCultureIgnoreCase)
                        && CheckIsNotNullOrEmpty(answerGuide.DateMax)
                        && !CheckIsNumber(answerGuide.DateMax))
                    {
                        answerGuideErrorMessage.AppendLine(ApplicationConstants.MaxDateNotNumberError);
                    }

                    if (isFieldExistInQuestionSet)
                    {
                        var field = sheetData.QuestionSet
                            .SelectMany(questionSet => questionSet.Sections)
                            .SelectMany(section => section.Fields)
                            .FirstOrDefault(field => field.FieldNo == answerGuide.FieldNo);

                        switch (field.FieldType.ToLower())
                        {
                            case "number":
                                answerGuideErrorMessage.AppendLine(NumberFiledType(answerGuide));
                                break;

                            case "dropdown":
                            case "radiogroup":
                                answerGuideErrorMessage.AppendLine(DropdownFieldType(answerGuide));
                                break;

                            case "date":
                                answerGuideErrorMessage.AppendLine(DateFieldType(answerGuide));
                                break;

                            case "text":
                                answerGuideErrorMessage.AppendLine(TextFieldType(answerGuide));
                                break;
                        }
                    }
                }
                return Regex.Replace(answerGuideErrorMessage.ToString(), @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
            }).ConfigureAwait(false);
        }

        private static string NumberFiledType(AnswerGuide answerGuideData)
        {
            var numberFieldErrorMessage = new StringBuilder();
            if (CheckIsNotNullOrEmpty(answerGuideData.DateMin)
                || CheckIsNotNullOrEmpty(answerGuideData.DateMax)
                || CheckIsNotNullOrEmpty(answerGuideData.Api))
            {
                numberFieldErrorMessage.AppendLine(string.Format(ApplicationConstants.FieldTypeInputErrorMessage, answerGuideData.FieldNo));
            }
            numberFieldErrorMessage.AppendLine(CheckMaxValueIsGreaterThanMinValue(answerGuideData.RangeMin, answerGuideData.RangeMax, answerGuideData.FieldNo, ApplicationConstants.Range));
            numberFieldErrorMessage.AppendLine(CheckMaxValueIsGreaterThanMinValue(answerGuideData.LengthMin, answerGuideData.LengthMax, answerGuideData.FieldNo, ApplicationConstants.Length));
            return numberFieldErrorMessage.ToString();
        }

        private static string CheckMaxValueIsGreaterThanMinValue(string minValue, string maxValue, string fieldNo, string columnName)
        {
            var isValidMinNumber = CheckValueIsNumberAndNotEmpty(minValue);
            var isValidMaxNumber = CheckValueIsNumberAndNotEmpty(maxValue);
            if (isValidMinNumber && isValidMaxNumber)
            {
                var rangeMin = Convert.ToInt32(minValue);
                var rangeMax = Convert.ToInt32(maxValue);
                if (rangeMin > rangeMax)
                {
                    return string.Format(ApplicationConstants.MinValueShouldNotGreaterThanMaxValue, columnName, fieldNo);
                }
            }
            return string.Empty;
        }

        private static string DropdownFieldType(AnswerGuide answerGuideData)
        {
            if (CheckIsNotNullOrEmpty(answerGuideData.RangeMin)
                || CheckIsNotNullOrEmpty(answerGuideData.RangeMax)
                || CheckIsNotNullOrEmpty(answerGuideData.LengthMin)
                || CheckIsNotNullOrEmpty(answerGuideData.LengthMax)
                || CheckIsNotNullOrEmpty(answerGuideData.DateMin)
                || CheckIsNotNullOrEmpty(answerGuideData.DateMax)
                || CheckIsNotNullOrEmpty(answerGuideData.Multiple))
            {
                return string.Format(ApplicationConstants.FieldTypeDropdownErrorMessage, answerGuideData.FieldNo);
            }
            return string.Empty;
        }

        private static string DateFieldType(AnswerGuide answerGuideData)
        {
            if (CheckIsNotNullOrEmpty(answerGuideData.RangeMin)
                || CheckIsNotNullOrEmpty(answerGuideData.RangeMax)
                || CheckIsNotNullOrEmpty(answerGuideData.LengthMin)
                || CheckIsNotNullOrEmpty(answerGuideData.LengthMax)
                || CheckIsNotNullOrEmpty(answerGuideData.Multiple)
                || CheckIsNotNullOrEmpty(answerGuideData.Api))
            {
                return string.Format(ApplicationConstants.FieldTypeDateErrorMessage, answerGuideData.FieldNo);
            }
            return string.Empty;
        }

        private static string TextFieldType(AnswerGuide answerGuideData)
        {
            var textFieldErrorMessage = new StringBuilder();
            if (CheckIsNotNullOrEmpty(answerGuideData.RangeMin)
                || CheckIsNotNullOrEmpty(answerGuideData.RangeMax)
                || CheckIsNotNullOrEmpty(answerGuideData.DateMin)
                || CheckIsNotNullOrEmpty(answerGuideData.DateMax)
                || CheckIsNotNullOrEmpty(answerGuideData.Multiple))
            {
                textFieldErrorMessage.AppendLine(string.Format(ApplicationConstants.FieldTypeTextErrorMessage, answerGuideData.FieldNo));
            }
            textFieldErrorMessage.AppendLine(CheckMaxValueIsGreaterThanMinValue(answerGuideData.LengthMin, answerGuideData.LengthMax, answerGuideData.FieldNo, ApplicationConstants.Length));
            return textFieldErrorMessage.ToString();
        }
    }
}