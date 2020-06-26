using Api.FormEngine.Core.Constants;
using Api.FormEngine.Core.SheetValidators;
using Api.FormEngine.Core.ViewModels.SheetModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TQ.Data.FormEngine;

namespace Api.FormEngine.Core.SheetValidation
{
    /// <summary>
    /// Validation of aggregation sheet
    /// </summary>
    public class AggregationSheetValidator : SheetValidator
    {
        private static FormStructureData FormStructureDataList { get; set; }

        /// <summary>
        /// Execute aggregation sheet
        /// </summary>
        /// <param name="sheetData">Excel sheet details</param>
        /// <param name="formSturcutreTypes">form structure types list from database</param>
        /// <param name="formsEngineContext">form engine context</param>
        /// <returns>Response Message of aggregation sheet</returns>
        public override async Task<string> ExecuteSheetAsync(ExcelSheetsData sheetData, FormStructureData formSturcutreTypes, FormsEngineContext formsEngineContext)
        {
            return await Task.Run(() =>
            {
                var errorMessage = new StringBuilder();
                FormStructureDataList = formSturcutreTypes;
                var groupedAggregations = sheetData.Aggregations.GroupBy(e => e.FieldNo).ToList();
                var fieldsDetails = sheetData.QuestionSet.SelectMany(x => x.Sections).SelectMany(x => x.Fields).ToList();
                foreach (var groupAggregation in groupedAggregations)
                {
                    var fieldExistInQuestionSet = CheckFieldExistInQuestionSet(groupAggregation.Key, sheetData.QuestionSet, ApplicationConstants.FieldNumber);
                    var fieldDetails = fieldsDetails.FirstOrDefault(x => x.FieldNo == groupAggregation.Key);
                    if (fieldDetails != null && !fieldDetails.FieldType.Equals(ApplicationConstants.NUMBER, StringComparison.OrdinalIgnoreCase))
                    {
                        errorMessage.AppendLine(string.Format(ApplicationConstants.FieldTypeNotNumber, fieldDetails.FieldType, groupAggregation.Key));
                        continue;
                    }
                    if (fieldExistInQuestionSet.Length > 0)
                    {
                        errorMessage.AppendLine(fieldExistInQuestionSet + ApplicationConstants.CanNotProcessAggregation);
                    }
                    var invalidFieldNumber = CheckValueIsNumberAndNotEmpty(groupAggregation.Key, ApplicationConstants.FieldNumber);
                    var isFieldAndAggregationNumberSame = groupAggregation.Any(x => x.AggregatedFieldNo == groupAggregation.Key);
                    var isDuplicateAggregation = groupAggregation.GroupBy(x => x.AggregatedFieldNo).Any(y => y.Count() > 1);
                    var isDuplicatePriority = groupAggregation.GroupBy(x => x.Priority).Any(y => y.Count() > 1);
                    if (invalidFieldNumber.Length > 0)
                    {
                        errorMessage.AppendLine($"{invalidFieldNumber} {ApplicationConstants.CanNotProcessAggregation}");
                        continue;
                    }

                    if (isFieldAndAggregationNumberSame)
                    {
                        errorMessage.AppendLine(string.Format(ApplicationConstants.SameFieldNoAndAggregationNo, groupAggregation.Key));
                        continue;
                    }

                    if (isDuplicateAggregation)
                    {
                        errorMessage.AppendLine(string.Format(ApplicationConstants.DuplicateAggregationFieldNumberFound, groupAggregation.Key));
                        continue;
                    }

                    if (isDuplicatePriority)
                    {
                        errorMessage.AppendLine(string.Format(ApplicationConstants.DuplicatePriorityFound, groupAggregation.Key));
                        continue;
                    }

                    foreach (var aggregation in groupAggregation.ToList())
                    {
                        var aggregationFieldDetails = fieldsDetails.FirstOrDefault(x => x.FieldNo == aggregation.AggregatedFieldNo);
                        if (aggregationFieldDetails != null && !aggregationFieldDetails.FieldType.Equals(ApplicationConstants.NUMBER, StringComparison.InvariantCultureIgnoreCase))
                        {
                            errorMessage.AppendLine(string.Format(ApplicationConstants.AggregationFieldTypeNotNumber, aggregationFieldDetails.FieldType, aggregation.AggregatedFieldNo));
                            break;
                        }
                        var fieldNotExist = CheckFieldExistInQuestionSet(aggregation.AggregatedFieldNo, sheetData.QuestionSet, ApplicationConstants.AggregationFieldNumber);
                        var invalidAggregationFieldNumber = CheckValueIsNumberAndNotEmpty(aggregation.AggregatedFieldNo, ApplicationConstants.AggregationFieldNumber);
                        var invalidFunction = InvalidFunction(aggregation.Function);
                        var invalidPriority = CheckIsNumber(aggregation.Priority, ApplicationConstants.Priority);

                        if (fieldNotExist.Length > 0)
                        {
                            errorMessage.AppendLine(fieldNotExist);
                            break;
                        }

                        if (invalidAggregationFieldNumber.Length > 0)
                        {
                            errorMessage.AppendLine(invalidAggregationFieldNumber);
                            break;
                        }

                        if (invalidFunction.Length > 0)
                        {
                            errorMessage.AppendLine(invalidFunction);
                            break;
                        }

                        if (invalidPriority.Length > 0)
                        {
                            errorMessage.AppendLine(invalidPriority);
                            break;
                        }
                    }
                }
                return errorMessage.ToString();
            }).ConfigureAwait(false);
        }

        private static string InvalidFunction(string function)
        {
            var isValidFunction = FormStructureDataList.Functions.Any(x => string.Equals(x.Functions, function, StringComparison.InvariantCultureIgnoreCase));
            if (!isValidFunction)
            {
                return string.Format(ApplicationConstants.InvalidFunction, function);
            }
            return string.Empty;
        }

        private static string CheckFieldExistInQuestionSet(string fieldNumber, IList<QuestionSet> questionSet, string columnName = null)
        {
            var isFieldExistInQuestionSet = questionSet.Any(x => x.Sections.Any(y => y.Fields.Any(z => z.FieldNo == fieldNumber)));
            if (!isFieldExistInQuestionSet)
            {
                return string.Format(ApplicationConstants.FieldNotExistErrorMessage, columnName, fieldNumber);
            }
            return string.Empty;
        }
    }
}