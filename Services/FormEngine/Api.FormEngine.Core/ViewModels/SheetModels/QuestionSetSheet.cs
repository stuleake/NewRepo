namespace Api.FormEngine.Core.ViewModels.SheetModels
{
    /// <summary>
    /// List of Question Set Sheet
    /// </summary>
    public class QuestionSetSheet
    {
        /// <summary>
        /// Gets or Sets Question Number
        /// </summary>
        public string QsNo { get; set; }

        /// <summary>
        /// Gets or Sets Tenant
        /// </summary>
        public string Tenant { get; set; }

        /// <summary>
        /// Gets or Sets Language
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Gets or Sets Question Set Label
        /// </summary>
        public string QSLabel { get; set; }

        /// <summary>
        /// Gets or Sets Question Set Helptext
        /// </summary>
        public string QSHelptext { get; set; }

        /// <summary>
        /// Gets or Sets Question Set Description
        /// </summary>
        public string QSDesc { get; set; }

        /// <summary>
        /// Gets or Sets Section Number
        /// </summary>
        public string SectionNo { get; set; }

        /// <summary>
        /// Gets or Sets Section Label
        /// </summary>
        public string SectionLabel { get; set; }

        /// <summary>
        /// Gets or Sets Section Helptext
        /// </summary>
        public string Sectionhelptext { get; set; }

        /// <summary>
        /// Gets or Sets Section Description
        /// </summary>
        public string SectionDesc { get; set; }

        /// <summary>
        /// Gets or Sets Section Type
        /// </summary>
        public string SectionType { get; set; }

        /// <summary>
        /// Gets or Sets Section Rule
        /// </summary>
        public string SectionRule { get; set; }

        /// <summary>
        /// Gets or Sets Section Rule Count
        /// </summary>
        public string SectionRuleCount { get; set; }

        /// <summary>
        /// Gets or Sets Field Number
        /// </summary>
        public string FieldNo { get; set; }

        /// <summary>
        /// Gets or Sets Field Label
        /// </summary>
        public string FieldLabel { get; set; }

        /// <summary>
        /// Gets or Sets Field Help Text
        /// </summary>
        public string Fieldhelptext { get; set; }

        /// <summary>
        /// Gets or Sets Field Description
        /// </summary>
        public string FieldDesc { get; set; }

        /// <summary>
        /// Gets or Sets Field Type
        /// </summary>
        public string FieldType { get; set; }

        /// <summary>
        /// Gets or Sets Field Display
        /// </summary>
        public string FieldDisplay { get; set; }

        /// <summary>
        /// Gets or Sets ToBeRedacted
        /// </summary>
        public string ToBeRedacted { get; set; }

        /// <summary>
        /// Gets or Sets the copy from qs
        /// </summary>
        public string CopyfromQS { get; set; }

        /// <summary>
        /// Gets or Sets the Copy from Field
        /// </summary>
        public string CopyfromField { get; set; }

        /// <summary>
        /// Gets or Sets the action of field
        /// </summary>
        public string Action { get; set; }
    }
}