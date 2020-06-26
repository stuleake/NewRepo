using Api.FormEngine.Core.Constants;
using Api.FormEngine.Core.ViewModels.SheetModels;
using System.Threading.Tasks;
using TQ.Data.FormEngine;

namespace Api.FormEngine.Core.SheetValidators
{
    /// <summary>
    /// Interface to manage different sheets
    /// </summary>
    public abstract class SheetValidator
    {
        /// <summary>
        /// Execute Sheets
        /// </summary>
        /// <param name="sheetData">Sheet Details</param>
        /// <param name="formSturcutreTypes">form stuecture types list from database</param>
        /// <param name="formsEngineContext">form engine context</param>
        /// <returns>Sheet Response</returns>
        public abstract Task<string> ExecuteSheetAsync(ExcelSheetsData sheetData, FormStructureData formSturcutreTypes, FormsEngineContext formsEngineContext);

        /// <summary>
        /// Check value is number or not
        /// </summary>
        /// <param name="number">Number to check</param>
        /// <param name="type">Append some hardcoded message in error message</param>
        /// <returns>Return message with error if value is not number</returns>
        public static string CheckIsNumber(string number, string type)
        {
            if (!int.TryParse(number, out _))
            {
                if (!CheckIsNotNullOrEmpty(number))
                {
                    return string.Format(ApplicationConstants.EmptyOrNullValue, type);
                }
                return string.Format(ApplicationConstants.IsNotNumber, type, number);
            }

            return string.Empty;
        }

        /// <summary>
        /// check value is number or not
        /// </summary>
        /// <param name="number">number</param>
        /// <returns>return true if number else return false</returns>
        public static bool CheckIsNumber(string number)
        {
            return int.TryParse(number, out _);
        }

        /// <summary>
        /// Check value is not null empty and is number
        /// </summary>
        /// <param name="value">number to check is valid or not empty</param>
        /// <param name="message">message to append in error message</param>
        /// <returns>return error</returns>
        public static string CheckValueIsNumberAndNotEmpty(string value, string message)
        {
            if (CheckIsNotNullOrEmpty(value))
            {
                return CheckIsNumber(value, message);
            }
            return string.Empty;
        }

        /// <summary>
        /// Check value is not null empty and is number
        /// </summary>
        /// <param name="value">number to check is valid or not empty</param>
        /// <returns>return error</returns>
        public static bool CheckValueIsNumberAndNotEmpty(string value)
        {
            if (CheckIsNotNullOrEmpty(value))
            {
                return CheckIsNumber(value);
            }
            return false;
        }

        /// <summary>
        /// Check value is empty or not
        /// </summary>
        /// <param name="value">value to check is empty or not</param>
        /// <returns>if value is not null return true</returns>
        public static bool CheckIsNotNullOrEmpty(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Check value is bool if not return error message
        /// </summary>
        /// <param name="value">value to check is bool or not</param>
        /// <param name="message">message to append in error message</param>
        /// <returns>return error message if value is not bool</returns>
        public static string BoolErrorMessage(string value, string message)
        {
            if (CheckIsNotNullOrEmpty(value) && !CheckValueIsBool(value))
            {
                return string.Format(ApplicationConstants.ValueIsNotBoolErrorMessage, value, message);
            }
            return string.Empty;
        }

        /// <summary>
        /// Check string value is bool or not
        /// </summary>
        /// <param name="value">value to check is bool</param>
        /// <returns>if value is bool return true else return false</returns>
        public static bool CheckValueIsBool(string value)
        {
            if (bool.TryParse(value, out _))
            {
                return true;
            }
            return false;
        }
    }
}