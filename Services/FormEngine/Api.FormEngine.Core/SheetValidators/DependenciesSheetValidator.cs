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
    /// Validation of dependency sheet
    /// </summary>
    public class DependenciesSheetValidator : SheetValidator
    {
        /// <summary>
        /// Execute dependency sheet
        /// </summary>
        /// <param name="sheetData">Excel sheet details</param>
        /// <param name="formSturcutreTypes">form stuecture types list from database</param>
        /// <param name="formsEngineContext">form engine context</param>
        /// <returns>Response Message of dependency sheet</returns>
        public override async Task<string> ExecuteSheetAsync(ExcelSheetsData sheetData, FormStructureData formSturcutreTypes, FormsEngineContext formsEngineContext)
        {
            return await Task.Run(() =>
            {
                StringBuilder dependenciesErrorMessage = new StringBuilder();
                var fieldDisplayOptions = formSturcutreTypes.Displays.ToList();
                var groupedDependencies = sheetData.Dependencies.GroupBy(f => f.FieldNo).ToList();

                foreach (var dependency in sheetData.Dependencies)
                {
                    // Check if FieldNo, DependsOnAns, DependsCount, DecidesSection is an number or not
                    dependenciesErrorMessage.AppendLine(CheckValueIsNumberAndNotEmpty(dependency.FieldNo, ApplicationConstants.Field));
                    dependenciesErrorMessage.AppendLine(CheckValueIsNumberAndNotEmpty(dependency.SectionNo, ApplicationConstants.Section));
                    dependenciesErrorMessage.AppendLine(CheckIsNumber(dependency.DependsOnAns, ApplicationConstants.DependsOnAns));
                    dependenciesErrorMessage.AppendLine(CheckValueIsNumberAndNotEmpty(dependency.DependsOnAnsfromQS, ApplicationConstants.DependsOnAns));
                    dependenciesErrorMessage.AppendLine(CheckIsNumber(dependency.DependsCount, ApplicationConstants.DependsCount));
                    dependenciesErrorMessage.AppendLine(CheckValueIsNumberAndNotEmpty(dependency.DecidesSection, ApplicationConstants.DecidesSection));

                    // Check if Field display is valid or not
                    var validFieldDisplay = fieldDisplayOptions.Any(option => option.Displays == dependency.FieldDisplay);
                    if (!validFieldDisplay)
                    {
                        dependenciesErrorMessage.AppendLine(
                            $"{ApplicationConstants.Dependencies} {string.Format(ApplicationConstants.InvalidFieldDisplay, dependency.FieldDisplay, dependency.FieldNo)}");
                    }

                    if (!CheckValueIsNumberAndNotEmpty(dependency.DependsOnAnsfromQS))
                    {
                        var dependsOnAnsExists = sheetData.AnswerGuides.Any(x => x.AnsNo == dependency.DependsOnAns);
                        if (!dependsOnAnsExists)
                        {
                            dependenciesErrorMessage.AppendLine(
                                $"{ApplicationConstants.Dependencies} {string.Format(ApplicationConstants.DependsOnAnsDoesNotExist, ApplicationConstants.DependsOnAns, dependency.DependsOnAns)}");
                        }
                    }

                    if (CheckValueIsNumberAndNotEmpty(dependency.FieldNo))
                    {
                        // check if FieldNo exists in question set
                        var fieldExists = sheetData.QuestionSet.Any(
                            x => x.Sections.Any(
                                y => y.Fields.Any(
                                    z => z.FieldNo == dependency.FieldNo)));
                        if (!fieldExists)
                        {
                            dependenciesErrorMessage.AppendLine(
                                $"{ApplicationConstants.Dependencies} {string.Format(ApplicationConstants.FieldNotExistErrorMessage, ApplicationConstants.FieldNumber, dependency.FieldNo)}");
                        }
                    }

                    if (CheckValueIsNumberAndNotEmpty(dependency.SectionNo))
                    {
                        // check if section exists in question set
                        var sectionExist = sheetData.QuestionSet.Any(x => x.Sections.Any(y => y.SectionNo == dependency.SectionNo));
                        if (!sectionExist)
                        {
                            dependenciesErrorMessage.AppendLine(ApplicationConstants.Dependencies +
                                string.Format(ApplicationConstants.FieldNotExistErrorMessage, ApplicationConstants.SectionNo, dependency.SectionNo));
                        }
                    }

                    if (!CheckIsNotNullOrEmpty(dependency.FieldNo) && !CheckIsNotNullOrEmpty(dependency.SectionNo))
                    {
                        dependenciesErrorMessage.AppendLine($"{ApplicationConstants.Dependencies} {ApplicationConstants.BothFieldAndSectionEmpty}");
                    }
                    else if (CheckIsNotNullOrEmpty(dependency.FieldNo) && CheckIsNotNullOrEmpty(dependency.SectionNo))
                    {
                        dependenciesErrorMessage.AppendLine($"{ApplicationConstants.Dependencies} {ApplicationConstants.BothFieldAndSectionIsNotEmpty}");
                    }

                    // Check if decides section exists in question set
                    if (CheckValueIsNumberAndNotEmpty(dependency.DecidesSection))
                    {
                        var decidesSectionExists = sheetData.QuestionSet.Any(x => x.Sections.Any(y => y.SectionNo == dependency.DecidesSection));
                        if (!decidesSectionExists)
                        {
                            dependenciesErrorMessage.AppendLine(
                                $"{ApplicationConstants.Dependencies} {string.Format(ApplicationConstants.FieldNotExistErrorMessage, ApplicationConstants.DecidesSection, dependency.DecidesSection)}");
                        }
                    }
                }

                foreach (var groupedDependency in groupedDependencies)
                {
                    int dependsOnCount = groupedDependency.Select(c => c.DependsOnAns).Count();
                    if (!groupedDependency.Any(a => !CheckIsNumber(a.DependsCount)))
                    {
                        var noOfDependsCount = groupedDependency.Any(a => (a.DependsCount != string.Empty && (Convert.ToInt32(a.DependsCount) > dependsOnCount)));
                        if (noOfDependsCount)
                        {
                            dependenciesErrorMessage.AppendLine(string.Format(ApplicationConstants.DependCountMoreThanDependsOnAns, groupedDependency.Key));
                        }
                    }

                    // Check duplicate entries for depends on ans
                    var duplicateDependsOn = groupedDependency.GroupBy(x => x.DependsOnAns).Where(x => x.Skip(1).Any()).ToList();
                    foreach (var duplicateDepends in duplicateDependsOn)
                    {
                        dependenciesErrorMessage.AppendLine(string.Format(ApplicationConstants.DuplicateDependsOnAnsFound, groupedDependency.Key));
                    }

                    // Check duplicate entries for decides section
                    var duplicateDecidesSections = groupedDependency.Where(x => CheckIsNotNullOrEmpty(x.DecidesSection)).GroupBy(x => x.DecidesSection).Where(x => x.Skip(1).Any()).ToList();
                    foreach (var duplicateDecidesSection in duplicateDecidesSections)
                    {
                        dependenciesErrorMessage.AppendLine(string.Format(ApplicationConstants.DuplicateDecidesSectionFound, groupedDependency.Key));
                    }
                }

                return Regex.Replace(dependenciesErrorMessage.ToString(), @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
            }).ConfigureAwait(false);
        }
    }
}