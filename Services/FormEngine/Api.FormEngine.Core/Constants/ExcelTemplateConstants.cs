namespace Api.FormEngine.Core.Constants
{
    /// <summary>
    /// Excel template constants
    /// </summary>
    internal class ExcelTemplateConstants
    {
        /// <summary>
        /// Question Set Headers
        /// </summary>
        public const string FieldHeaders = "Qs No;Tenant;Language;QS Label;QS Helptext;QS Desc;Section No;Section Label;Section helptext;" +
            "Section Desc;Section Type;Section Rule;Section Rule Count;Field No;Field Label;Field helptext;Field Desc;Field Type;Field Display;ToBeRedacted;Copy from QS;Copy from Field;Action";

        /// <summary>
        /// Answer Guide Headers
        /// </summary>
        public const string AnswerGuideHeaders = "Field No;Ans No;Range Min;Range Max;Length Min;Length Max;DateMin;DateMax;regex;RegexBE;Multiple;Label;Value;Error Label;API;isDefault";

        /// <summary>
        /// Dependency Headers
        /// </summary>
        public const string DependenciesHeaders = "Field No;Section No;Field Display;Depends On Ans;Depends On Ans from QS;Depends Count;Decides Section";

        /// <summary>
        /// Aggregatin Headers
        /// </summary>
        public const string AggregationHeaders = "Field No;Aggregated Field No;Function;Priority";

        /// <summary>
        /// Header Not Found Error Message
        /// </summary>
        public const string ErrorMessage = "Headers Not Found in {0} Sheet : {1}";

        /// <summary>
        /// Sheet not found error message
        /// </summary>
        public const string SheetNotFoundErrorMessage = "Excel sheet : {0} not found";

        /// <summary>
        /// Remove last two character from header
        /// </summary>
        public const int RemoveHeaderCharacter = 2;

        /// <summary>
        /// fields
        /// </summary>
        public const string Fields = "Fields";

        /// <summary>
        /// answerGuide
        /// </summary>
        public const string AnswerGuide = "AnswerGuide";

        /// <summary>
        /// dependencies
        /// </summary>
        public const string Dependencies = "Dependencies";

        /// <summary>
        /// aggregations
        /// </summary>
        public const string Aggregations = "Aggregations";
    }
}