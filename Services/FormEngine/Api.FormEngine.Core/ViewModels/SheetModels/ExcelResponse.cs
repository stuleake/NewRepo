using System.IO;

namespace Api.FormEngine.Core.ViewModels.SheetModels
{
    /// <summary>
    /// excel response with error message and formatted excel sheet data
    /// </summary>
    public class ExcelResponse
    {
        /// <summary>
        /// Gets or Sets the excel error message
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or Sets the formatted excel sheet
        /// </summary>
        public ExcelSheetsData FormattedExcelSheetData { get; set; }

        /// <summary>
        /// Gets or Sets the Excel file stream
        /// </summary>
        public Stream ExcelFileStream { get; set; }

        /// <summary>
        /// Gets or Sets the file name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or Sets the excel file name that we will store in blob
        /// </summary>
        public string ExcelBlobFileName { get; set; }
    }
}