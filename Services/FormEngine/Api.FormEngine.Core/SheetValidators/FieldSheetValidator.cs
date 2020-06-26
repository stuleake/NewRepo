using Api.FormEngine.Core.Constants;
using Api.FormEngine.Core.ViewModels.SheetModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TQ.Data.FormEngine;

namespace Api.FormEngine.Core.SheetValidators
{
    /// <summary>
    /// Validation of Field sheet
    /// </summary>
    public class FieldSheetValidator : SheetValidator
    {
        private static FormStructureData FormStructureDataList { get; set; }

        /// <summary>
        /// Execute question set sheet
        /// </summary>
        /// <param name="sheetData">Details of all sheets</param>
        /// <param name="formSturcutreTypes">form stuecture types list from database</param>
        /// <param name="formsEngineContext">form engine context</param>
        /// <returns>Response Message of Question set sheet</returns>
        public override async Task<string> ExecuteSheetAsync(ExcelSheetsData sheetData, FormStructureData formSturcutreTypes, FormsEngineContext formsEngineContext)
        {
            return await Task.Run(() =>
            {
                FormStructureDataList = formSturcutreTypes;
                StringBuilder errorMessage = new StringBuilder();
                errorMessage.AppendLine(ValidateQuestionSet(sheetData.QuestionSet));
                foreach (var questionSet in sheetData.QuestionSet)
                {
                    errorMessage.AppendLine(ValidateSection(questionSet, sheetData.Dependencies));
                }
                return Regex.Replace(errorMessage.ToString(), @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Question Set Validation
        /// </summary>
        /// <param name="questionSets">List of question set</param>
        /// <returns>Return Error message in question set</returns>
        private static string ValidateQuestionSet(IList<QuestionSet> questionSets)
        {
            StringBuilder questionSetErrorMessage = new StringBuilder();
            if (questionSets == null)
            {
                questionSetErrorMessage.AppendLine(ApplicationConstants.NoQuestionSetFound);
                return questionSetErrorMessage.ToString();
            }
            if (questionSets.Count > 1)
            {
                questionSetErrorMessage.AppendLine(ApplicationConstants.MultipleQuestionSetFound);
            }
            foreach (var questionSet in questionSets)
            {
                questionSetErrorMessage.AppendLine(CheckIsNumber(questionSet.QsNo, ApplicationConstants.QuestionSet));
            }
            return questionSetErrorMessage.ToString();
        }

        /// <summary>
        /// Section Validation
        /// </summary>
        /// <param name="questionSet">Question set to fetch all section details</param>
        /// <returns>Return Error message in sections</returns>
        private static string ValidateSection(QuestionSet questionSet, IList<Dependencies> dependencies)
        {
            StringBuilder sectionErrorMessage = new StringBuilder();

            var duplicateSectionNo = questionSet.Sections.GroupBy(x => x.SectionNo).Where(x => x.Skip(1).Any()).ToList();
            foreach (var duplicateSection in duplicateSectionNo)
            {
                sectionErrorMessage.AppendLine(string.Format(
                    ApplicationConstants.DuplicateFieldSectionFound, ApplicationConstants.SectionNo, duplicateSection.Key, ApplicationConstants.QuestionNo, questionSet.QsNo));
            }

            foreach (var section in questionSet.Sections)
            {
                if (CheckIsNotNullOrEmpty(section.SectionRule))
                {
                    var isValidRule = FormStructureDataList.Rules.Any(x => x.Rules == section.SectionRule);
                    if (isValidRule)
                    {
                        section.RuleId = Convert.ToInt32(FormStructureDataList.Rules.Where(
                            x => string.Equals(x.Rules, section.SectionRule, System.StringComparison.CurrentCultureIgnoreCase)).Select(y => y.RuleId).FirstOrDefault());
                    }
                    else
                    {
                        sectionErrorMessage.AppendLine(string.Format(ApplicationConstants.InvalidSectionRule, section.SectionRule));
                    }
                }
                var isValidSection = FormStructureDataList.SectionTypes.Any(x => string.Equals(x.SectionTypes, section.SectionType, System.StringComparison.CurrentCultureIgnoreCase));
                if (!isValidSection)
                {
                    sectionErrorMessage.AppendLine(string.Format(ApplicationConstants.InvalidSectionType, section.SectionType, section.SectionNo));
                }
                else
                {
                    section.SectionTypeId = FormStructureDataList.SectionTypes.Where(x => x.SectionTypes == section.SectionType).Select(y => y.SectionTypeId).FirstOrDefault();
                }
                sectionErrorMessage.AppendLine(CheckValueIsNumberAndNotEmpty(section.SectionRuleCount, ApplicationConstants.SectionRuleCount));
                sectionErrorMessage.AppendLine(CheckIsNumber(section.SectionNo, ApplicationConstants.Section));
                sectionErrorMessage.AppendLine(ValidateFields(section, dependencies));
            }
            return sectionErrorMessage.ToString();
        }

        /// <summary>
        /// Field validation
        /// </summary>
        /// <param name="section">Section to fetch all fields inside this section</param>
        /// <returns>Return Error message in Fields</returns>
        private static string ValidateFields(Section section, IList<Dependencies> dependencies)
        {
            StringBuilder fieldsErrorMessage = new StringBuilder();
            var duplicateFeildNo = section.Fields.GroupBy(x => x.FieldNo).Where(x => x.Skip(1).Any()).ToList();
            foreach (var duplicateField in duplicateFeildNo)
            {
                fieldsErrorMessage.AppendLine(string.Format(
                    ApplicationConstants.DuplicateFieldSectionFound, ApplicationConstants.FieldNumber, duplicateField.Key, ApplicationConstants.SectionNo, section.SectionNo));
            }
            foreach (var field in section.Fields)
            {
                var isValidFieldType = FormStructureDataList.FieldTypes.Any(x => string.Equals(x.FieldTypes, field.FieldType, System.StringComparison.InvariantCultureIgnoreCase));
                if (!isValidFieldType)
                {
                    fieldsErrorMessage.AppendLine(string.Format(ApplicationConstants.InvalidFieldType, field.FieldType, field.FieldNo));
                }
                else
                {
                    field.FieldTypeId = FormStructureDataList.FieldTypes.Where(x => string.Equals(x.FieldTypes, field.FieldType, StringComparison.InvariantCultureIgnoreCase))
                        .Select(y => y.FieldTypeId)
                        .FirstOrDefault();
                }
                var isValidFieldDisplay = FormStructureDataList.Displays.Any(x => string.Equals(x.Displays, field.FieldDisplay, System.StringComparison.CurrentCultureIgnoreCase));
                if (!isValidFieldDisplay)
                {
                    fieldsErrorMessage.AppendLine(string.Format(ApplicationConstants.InvalidFieldDisplay, field.FieldDisplay, field.FieldNo));
                }
                else
                {
                    field.DisplayId = FormStructureDataList.Displays.Where(x => x.Displays == field.FieldDisplay).Select(y => y.DisplayId).FirstOrDefault();
                }
                fieldsErrorMessage.AppendLine(CheckIsNumber(field.FieldNo, ApplicationConstants.Field));
                fieldsErrorMessage.AppendLine(BoolErrorMessage(field.ToBeRedacted, ApplicationConstants.ToBeRedacted));
                fieldsErrorMessage.AppendLine(CheckValueIsNumberAndNotEmpty(field.CopyfromQS, ApplicationConstants.CopyFromQS));
                fieldsErrorMessage.AppendLine(CheckValueIsNumberAndNotEmpty(field.CopyfromField, ApplicationConstants.CopyFromField));

                if (!string.IsNullOrWhiteSpace(field.CopyfromQS))
                {
                    var copyFromQsExists = dependencies.Any(d => d.FieldNo == field.FieldNo && d.DependsOnAnsfromQS == field.CopyfromQS);
                    if (!copyFromQsExists)
                    {
                        fieldsErrorMessage.AppendLine(string.Format(ApplicationConstants.FieldAndCopyFromQsNotExists, field.FieldNo, field.CopyfromQS));
                    }
                }
            }
            return fieldsErrorMessage.ToString();
        }
    }
}