using Api.FormEngine.Core.Constants;
using Api.FormEngine.Core.ViewModels.SheetModels;
using System;
using System.Collections.Generic;
using System.Linq;
using TQ.Data.FormEngine;

namespace Api.FormEngine.Core.Services.Validators
{
    /// <inheritdoc/>
    public class QuestionSetValidator : IValidator<ExcelSheetsData>
    {
        private readonly FormsEngineContext formsEngineContext;

        /// <inheritdoc/>
        public QuestionSetValidator(FormsEngineContext formsEngineContext)
        {
            this.formsEngineContext = formsEngineContext;
        }

        /// <inheritdoc/>
        public ValidationResult<ExcelSheetsData> Validate(ExcelSheetsData model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var warningMessages = new List<string>();
            var questionSet = model?.QuestionSet?.First() ?? throw new ArgumentNullException(nameof(model));

            var availableQsDetails = formsEngineContext.QS.Where(x => x.QSNo == Convert.ToInt32(questionSet.QsNo) && x.StatusId == FormStructureConstants.ActiveQsStatusNumber)
                .OrderByDescending(y => y.QSVersion)
                .FirstOrDefault();

            if (availableQsDetails != null)
            {
                warningMessages.AddRange(CompareToExistingQuestionSet(model, availableQsDetails.QSId));
            }

            var deletedFieldNumber = model.QuestionSet.SelectMany(quesset => quesset.Sections)
                .SelectMany(field => field.Fields)
                .Where(field => field.Action == ApplicationConstants.Delete)
                .Select(field => field.FieldNo)
                .ToList();

            var deletedFieldAggregation = model.Aggregations.Where(aggregation => deletedFieldNumber.Contains(aggregation.FieldNo)).ToList();
            var deletedFieldAnswerGuides = model.AnswerGuides.Where(answerGuide => deletedFieldNumber.Contains(answerGuide.FieldNo)).ToList();
            var answerNo = deletedFieldAnswerGuides.Select(deletedAnswerGuide => deletedAnswerGuide.AnsNo).ToList();

            var deletedFieldDependencies = model.Dependencies
                .Where(dependencies =>
                (deletedFieldNumber.Contains(dependencies.FieldNo) || answerNo.Contains(dependencies.DependsOnAns)) &&
                (string.IsNullOrEmpty(dependencies.DependsOnAnsfromQS) || dependencies.DependsOnAnsfromQS == model.QuestionSet[0].QsNo))
                .ToList();

            // Temporarily hard coding messages as to not break dependencies on current placeholder strings.
            deletedFieldNumber.ForEach(fieldNumber => warningMessages.Add($"Field number {fieldNumber} deleted"));
            deletedFieldAggregation.ForEach(fieldAggregation => warningMessages.Add($"You are trying to create aggregations of deleted field number {fieldAggregation.FieldNo}"));
            deletedFieldAnswerGuides.ForEach(fieldAnswerGuide => warningMessages.Add($"You are trying to create answer guides of deleted field number {fieldAnswerGuide.FieldNo}"));
            deletedFieldDependencies.ForEach(fieldDependencies => warningMessages.Add($"You are trying to create dependencies of deleted field number {fieldDependencies.FieldNo} " +
                $"on deleted answer guide number {fieldDependencies.DependsOnAns}"));

            if (warningMessages.Any())
            {
                return new ValidationResult<ExcelSheetsData>(model, warningMessages);
            }

            return new ValidationResult<ExcelSheetsData>(model);
        }

        /// <summary>
        /// Compares the new question set to the existing question set in the database.
        /// </summary>
        /// <param name="sheetsData">Question data to compare with.</param>
        /// <param name="questionSetId">ID of the question set to compare to.</param>
        /// <returns>A collection of warnings, if any.</returns>
        private IEnumerable<string> CompareToExistingQuestionSet(ExcelSheetsData sheetsData, Guid questionSetId)
        {
            var warningMessages = new List<string>();

            var fieldInDb = (from field in formsEngineContext.Field
                             join sectionFieldMapping in formsEngineContext.SectionFieldMapping on field.FieldId equals sectionFieldMapping.FieldId
                             join qssectionMapping in formsEngineContext.QSSectionMapping
                             .Where(qssection => qssection.QSId == questionSetId) on sectionFieldMapping.SectionId equals qssectionMapping.SectionId
                             select field)
                             .ToList();

            var fieldsInSheet = sheetsData.QuestionSet.SelectMany(qs => qs.Sections).SelectMany(qs => qs.Fields).ToList();

            var deletedFields = fieldInDb.Where(fdb => !fieldsInSheet.Any(fsh => fsh.FieldNo == fdb.FieldNo.ToString())).ToList();
            var newFields = fieldsInSheet.Where(fsh => !fieldInDb.Any(fdb => fdb.FieldNo.ToString() == fsh.FieldNo) && fsh.Action != ApplicationConstants.Delete).ToList();
            var updateFields = (from fieldExcel in fieldsInSheet
                                join fieldDB in fieldInDb on fieldExcel.FieldNo equals fieldDB.FieldNo.ToString()
                                where fieldExcel.FieldType != fieldDB.FieldType.FieldTypes
                                select new { fieldDB.FieldNo, OldFieldType = fieldDB.FieldType.FieldTypes, NewFieldType = fieldExcel.FieldType, FieldAction = fieldExcel.Action }).ToList();

            // Added
            newFields.ForEach(newField =>
            {
                if (newField.Action != ApplicationConstants.Add && !string.IsNullOrEmpty(newField.Action))
                {
                    warningMessages.Add($"Action of field number {newField.FieldNo} is Add not {newField.Action}");
                }

                warningMessages.Add($"Field number {newField.FieldNo} added");
            });

            // Updated
            updateFields.ForEach(updateField =>
            {
                if (updateField.FieldAction != ApplicationConstants.Update && !string.IsNullOrEmpty(updateField.FieldAction))
                {
                    warningMessages.Add($"Action of Field number {updateField.FieldNo} is Update not {updateField.FieldAction}");
                }

                warningMessages.Add($"Field number {updateField.FieldNo} updated form Field Type {updateField.OldFieldType} to Field Type {updateField.NewFieldType}");
            });

            // Deleted
            deletedFields.ForEach(deletedField => warningMessages.Add($"Field number {deletedField.FieldNo} deleted"));

            return warningMessages;
        }
    }
}