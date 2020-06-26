using Api.FormEngine.Core.ViewModels.SheetModels;
using System.Collections.Generic;

namespace Api.FormEngine.Core.Managers
{
    /// <summary>
    /// Taxanomy helper
    /// </summary>
    public class TaxonomyHelper
    {
        /// <summary>
        /// Execute excel sheet data and return taxanomy dictionary
        /// </summary>
        /// <param name="data">send excel sheet data</param>
        /// <returns>return taxanomy csv file steam data</returns>
        public static Dictionary<string, string> GenerateTaxonomy(ExcelSheetsData data)
        {
            Dictionary<string, string> taxonomies = new Dictionary<string, string>();
            string questionSetNo = string.Empty;
            foreach (var question in data?.QuestionSet)
            {
                questionSetNo = question.QsNo;
                question.QSDesc = AddTaxonomy(ref taxonomies, $"qs_desc_{question.QsNo}", question.QSDesc);
                question.QSHelptext = AddTaxonomy(ref taxonomies, $"qs_help_{question.QsNo}", question.QSHelptext);
                question.QSLabel = AddTaxonomy(ref taxonomies, $"qs_lb_{question.QsNo}", question.QSLabel);

                foreach (var section in question.Sections)
                {
                    section.SectionDesc = AddTaxonomy(ref taxonomies, $"sec_desc_{question.QsNo}_{section.SectionNo}", section.SectionDesc);
                    section.Sectionhelptext = AddTaxonomy(ref taxonomies, $"sec_help_{question.QsNo}_{section.SectionNo}", section.Sectionhelptext);
                    section.SectionLabel = AddTaxonomy(ref taxonomies, $"sec_lb_{question.QsNo}_{section.SectionNo}", section.SectionLabel);

                    foreach (var field in section.Fields)
                    {
                        field.FieldDesc = AddTaxonomy(ref taxonomies, $"fld_desc_{question.QsNo}_{field.FieldNo}", field.FieldDesc);
                        field.Fieldhelptext = AddTaxonomy(ref taxonomies, $"fld_help_{question.QsNo}_{field.FieldNo}", field.Fieldhelptext);
                        field.FieldLabel = AddTaxonomy(ref taxonomies, $"fld_lb_{question.QsNo}_{field.FieldNo}", field.FieldLabel);
                    }
                }
            }

            foreach (var guide in data?.AnswerGuides)
            {
                if (!taxonomies.ContainsKey($"ans_lbl_{questionSetNo}_{guide.FieldNo}_{guide.AnsNo}"))
                {
                    guide.Label = AddTaxonomy(ref taxonomies, $"ans_lbl_{questionSetNo}_{guide.FieldNo}_{guide.AnsNo}", guide.Label);
                }
                if (!taxonomies.ContainsKey($"err_lbl_{questionSetNo}_{guide.FieldNo}"))
                {
                    guide.ErrorLabel = AddTaxonomy(ref taxonomies, $"err_lbl_{questionSetNo}_{guide.FieldNo}", guide.ErrorLabel);
                }
            }
            return taxonomies;
        }

        private static string AddTaxonomy(ref Dictionary<string, string> taxonomyDictionary, string labelKey, string labelValue)
        {
            if (string.IsNullOrWhiteSpace(labelValue))
            {
                return null;
            }
            else
            {
                taxonomyDictionary.Add(labelKey, labelValue);
                return labelKey;
            }
        }
    }
}