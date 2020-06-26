using Api.FormEngine.Core.ViewModels.Enum;
using Api.FormEngine.Core.ViewModels.SheetModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TQ.Core.Extensions;
using TQ.Data.FormEngine;

namespace Api.FormEngine.Core.Managers
{
    /// <summary>
    /// Excel Sheet Manager
    /// </summary>
    public class ExcelSheetManager
    {
        /// <summary>
        /// Execute Each excel sheet to get data
        /// </summary>
        /// <param name="sheets">Sheet value</param>
        /// <param name="formSturcutreTypes">form stuecture types list from database</param>
        /// <param name="formsEngineContext">form engine context</param>
        /// <returns>This will return response of excel sheet</returns>
        public static async Task<ExcelResponse> ExecuteAllExcelSheetsAsync(DataTableCollection sheets, FormStructureData formSturcutreTypes, FormsEngineContext formsEngineContext)
        {
            var excelResponse = new ExcelResponse();
            var sheetNotFoundErrorMessage = await ValidateExcelTemplate.ValidateSheetAsync(sheets).ConfigureAwait(false);
            if (!string.IsNullOrWhiteSpace(sheetNotFoundErrorMessage))
            {
                excelResponse.ErrorMessage = sheetNotFoundErrorMessage;
                return excelResponse;
            }
            var headerErrorMessage = await ValidateExcelTemplate.ValidateHeaderAsync(sheets).ConfigureAwait(false);
            if (!string.IsNullOrWhiteSpace(headerErrorMessage))
            {
                excelResponse.ErrorMessage = headerErrorMessage;
                return excelResponse;
            }
            var questionSetFields = new List<ViewModels.SheetModels.QuestionSet>();
            var answerGuide = new List<ViewModels.SheetModels.AnswerGuide>();
            var dependencies = new List<Dependencies>();
            var aggregations = new List<Aggregations>();

            if (sheets != null)
            {
                foreach (DataTable data in sheets)
                {
                    int rowCount = 0;
                    List<object> headers = new List<object>();
                    var allRowsDetails = new List<Dictionary<string, string>>();
                    foreach (DataRow row in data.Rows)
                    {
                        if (rowCount == 0)
                        {
                            headers = row.ItemArray.ToList();
                            rowCount++;
                            continue;
                        }
                        rowCount++;
                        var singleRowDetails = row.ItemArray;
                        int currentRowNumber = 0;
                        var eachFieldDetails = new Dictionary<string, string>();
                        foreach (var item in singleRowDetails)
                        {
                            eachFieldDetails.Add(headers[currentRowNumber].ToString().Replace(" ", string.Empty, StringComparison.InvariantCulture), item.ToString());
                            currentRowNumber++;
                        }
                        allRowsDetails.Add(eachFieldDetails);
                    }
                    var sheetNameEnum = GetSheetsDisplayName(data.ToString());
                    var sheetJson = CreateSheetData(allRowsDetails, sheetNameEnum);
                    switch (sheetNameEnum)
                    {
                        case Sheets.Fields:
                            questionSetFields = JsonConvert.DeserializeObject<List<QuestionSet>>(sheetJson);
                            break;

                        case Sheets.AnswerGuide:
                            answerGuide = JsonConvert.DeserializeObject<List<ViewModels.SheetModels.AnswerGuide>>(sheetJson);
                            break;

                        case Sheets.Dependencies:
                            dependencies = JsonConvert.DeserializeObject<List<Dependencies>>(sheetJson);
                            break;

                        case Sheets.Aggregations:
                            aggregations = JsonConvert.DeserializeObject<List<Aggregations>>(sheetJson);
                            break;
                    }
                }
            }
            var sheetData = new ExcelSheetsData
            {
                QuestionSet = questionSetFields,
                AnswerGuides = answerGuide,
                Dependencies = dependencies,
                Aggregations = aggregations
            };

            var fieldsData = await SheetValidationIdentifierAsync(Sheets.Fields, sheetData, formSturcutreTypes, formsEngineContext).ConfigureAwait(false);
            var aggregationsData = await SheetValidationIdentifierAsync(Sheets.Aggregations, sheetData, formSturcutreTypes, formsEngineContext).ConfigureAwait(false);
            var answerGuideData = await SheetValidationIdentifierAsync(Sheets.AnswerGuide, sheetData, formSturcutreTypes, formsEngineContext).ConfigureAwait(false);
            var dependenciesData = await SheetValidationIdentifierAsync(Sheets.Dependencies, sheetData, formSturcutreTypes, formsEngineContext).ConfigureAwait(false);
            var sheetsErrorMessage = $"{fieldsData} {aggregationsData} {answerGuideData} {dependenciesData}";
            sheetsErrorMessage = sheetsErrorMessage.Trim();
            excelResponse.FormattedExcelSheetData = sheetData;
            excelResponse.ErrorMessage = sheetsErrorMessage;
            return excelResponse;
        }

        /// <summary>
        /// This method will call all sheet executor method
        /// </summary>
        /// <param name="sheet">Sheet Type</param>
        /// <param name="sheetData">Sheet Data to execute</param>
        /// <param name="formStructureData">form structure types list from database</param>
        /// <returns>Error Response of Sheet</returns>
        private static async Task<string> SheetValidationIdentifierAsync(Sheets sheet, ExcelSheetsData sheetData, FormStructureData formStructureData, FormsEngineContext formsEngineContext)
        {
            return await Task.Run(() =>
            {
                var sheetValidator = SheetValidatorFactory.GetSheetValidator(sheet);
                return sheetValidator.ExecuteSheetAsync(sheetData, formStructureData, formsEngineContext);
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Get display name of sheet value
        /// </summary>
        /// <param name="sheetValue">Sheet value</param>
        /// <returns>Return sheet enum value</returns>
        private static Sheets GetSheetsDisplayName(string sheetValue)
        {
            return EnumExtensions<Sheets>.ParseDisplayName(sheetValue);
        }

        /// <summary>
        /// Manage excel sheet data
        /// </summary>
        /// <param name="rowsDetails">Sheet each rows</param>
        /// <param name="sheetName">Sheet name to identify excel sheet type</param>
        /// <returns>Return sheet data in string format</returns>
        private static string CreateSheetData(List<Dictionary<string, string>> rowsDetails, Sheets sheetName)
        {
            switch (sheetName)
            {
                case Sheets.Fields:
                    return CreateQuestionSetDetails(rowsDetails);

                case Sheets.Aggregations:
                case Sheets.AnswerGuide:
                case Sheets.Dependencies:
                    return JsonConvert.SerializeObject(rowsDetails, Newtonsoft.Json.Formatting.Indented);

                default: return "Invalid Sheet Name";
            }
        }

        /// <summary>
        /// Manage question set details
        /// </summary>
        /// <param name="questionSetValue">Question set value</param>
        /// <returns>Return question set value in string format with question set list</returns>
        private static string CreateQuestionSetDetails(List<Dictionary<string, string>> questionSetValue)
        {
            var serializeSheetValue = JsonConvert.SerializeObject(questionSetValue, Newtonsoft.Json.Formatting.Indented);
            var questionSetSheetObject = JsonConvert.DeserializeObject<List<QuestionSetSheet>>(serializeSheetValue);
            var result = questionSetSheetObject.GroupBy(x => new { x.QsNo, x.QSLabel, x.QSDesc, x.QSHelptext, x.Tenant, x.Language })
                 .Select(b => new ViewModels.SheetModels.QuestionSet
                 {
                     QsNo = b.Key.QsNo,
                     Tenant = b.Key.Tenant,
                     Language = b.Key.Language,
                     QSLabel = b.Key.QSLabel,
                     QSDesc = b.Key.QSDesc,
                     QSHelptext = b.Key.QSHelptext,
                     Sections = b.GroupBy(y => new { y.SectionNo, y.SectionLabel, y.Sectionhelptext, y.SectionDesc, y.SectionType, y.SectionRule, y.SectionRuleCount })
                     .Select(section => new ViewModels.SheetModels.Section
                     {
                         SectionNo = section.Key.SectionNo,
                         SectionLabel = section.Key.SectionLabel,
                         Sectionhelptext = section.Key.Sectionhelptext,
                         SectionDesc = section.Key.SectionDesc,
                         SectionType = section.Key.SectionType,
                         SectionRule = section.Key.SectionRule,
                         SectionRuleCount = section.Key.SectionRuleCount,
                         Fields = section.
                         Select(fields => new ViewModels.SheetModels.Field
                         {
                             FieldNo = fields.FieldNo,
                             FieldLabel = fields.FieldLabel,
                             Fieldhelptext = fields.Fieldhelptext,
                             FieldDesc = fields.FieldDesc,
                             FieldType = fields.FieldType,
                             FieldDisplay = fields.FieldDisplay,
                             ToBeRedacted = fields.ToBeRedacted,
                             CopyfromField = fields.CopyfromField,
                             CopyfromQS = fields.CopyfromQS,
                             Action = fields.Action
                         }).ToList()
                     }).ToList(),
                 }).ToList();
            var questionSet = JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);
            return questionSet;
        }
    }
}