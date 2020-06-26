using Api.FormEngine.Core.Constants;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.FormEngine.Core.Managers
{
    /// <summary>
    /// Validate Headers in excel sheet
    /// </summary>
    public class ValidateExcelTemplate
    {
        /// <summary>
        /// Gets all sheets header list
        /// </summary>
        private static Dictionary<string, string> SheetsHeaders { get; }

        static ValidateExcelTemplate()
        {
            SheetsHeaders = new Dictionary<string, string>
            {
                { ExcelTemplateConstants.Fields, ExcelTemplateConstants.FieldHeaders },
                { ExcelTemplateConstants.AnswerGuide, ExcelTemplateConstants.AnswerGuideHeaders },
                { ExcelTemplateConstants.Dependencies, ExcelTemplateConstants.DependenciesHeaders },
                { ExcelTemplateConstants.Aggregations, ExcelTemplateConstants.AggregationHeaders }
            };
        }

        /// <summary>
        /// Header error message method
        /// </summary>
        /// <param name="sheets">Send all sheet value</param>
        /// <returns>Return error message if header is invalid or not found</returns>
        public static async Task<string> ValidateHeaderAsync(DataTableCollection sheets)
        {
            return await Task.Run(() =>
            {
                var headerErrorMessage = new StringBuilder();

                for (int sheet = 0; sheet < sheets.Count; sheet++)
                {
                    var headers = sheets[sheet].Rows[0].ItemArray;
                    var sheetName = sheets[sheet].ToString();
                    var invalidHeaders = CheckHeaders(SheetsHeaders[sheetName], headers);
                    if (invalidHeaders.Length > 0)
                    {
                        headerErrorMessage.AppendLine(string.Format(ExcelTemplateConstants.ErrorMessage, sheetName, invalidHeaders));
                    }
                }
                return headerErrorMessage.ToString();
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Find invalid or empty headers
        /// </summary>
        /// <param name="header">List of headers</param>
        /// <param name="excelSheetHeader">Headers in excel sheet</param>
        /// <returns>If header not found send error message</returns>
        public static string CheckHeaders(string header, object[] excelSheetHeader)
        {
            StringBuilder missingColumns = new StringBuilder();
            var headerList = header?.Split(';');
            var missingColumnList = headerList.Except(excelSheetHeader).ToList();
            foreach (var missingColumn in missingColumnList)
            {
                missingColumns.Append(missingColumn + ", ");
            }
            if (missingColumns.Length > 1)
            {
                missingColumns.Remove(missingColumns.Length - ExcelTemplateConstants.RemoveHeaderCharacter, ExcelTemplateConstants.RemoveHeaderCharacter);
            }
            return missingColumns.ToString();
        }

        /// <summary>
        /// error message if sheet not found
        /// </summary>
        /// <param name="sheets">Send all sheet value</param>
        /// <returns>return error message if sheet not found</returns>
        public static async Task<string> ValidateSheetAsync(DataTableCollection sheets)
        {
            return await Task.Run(() =>
            {
                var sheetErrorMessage = new StringBuilder();
                var sheetList = new List<string>();
                for (int sheet = 0; sheet < sheets.Count; sheet++)
                {
                    sheetList.Add(sheets[sheet].ToString());
                }
                var missingSheetList = SheetsHeaders.Keys.Except(sheetList).ToList();
                if (missingSheetList.Any())
                {
                    foreach (var sheetName in missingSheetList)
                    {
                        sheetErrorMessage.AppendLine($"{string.Format(ExcelTemplateConstants.SheetNotFoundErrorMessage, sheetName)}");
                    }
                }
                return sheetErrorMessage.ToString();
            }).ConfigureAwait(false);
        }
    }
}