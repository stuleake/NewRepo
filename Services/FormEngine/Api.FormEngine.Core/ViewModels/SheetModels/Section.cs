using System.Collections.Generic;

namespace Api.FormEngine.Core.ViewModels.SheetModels
{
    /// <summary>
    /// List of Sections
    /// </summary>
    public class Section
    {
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
        /// Gets or Sets Section Type Id
        /// </summary>
        public int SectionTypeId { get; set; }

        /// <summary>
        /// Gets or Sets Section Type
        /// </summary>
        public string SectionType { get; set; }

        /// <summary>
        /// Gets or Sets the rule id
        /// </summary>
        public int? RuleId { get; set; }

        /// <summary>
        /// Gets or Sets Section Rule
        /// </summary>
        public string SectionRule { get; set; }

        /// <summary>
        /// Gets or Sets Section Rule Count
        /// </summary>
        public string SectionRuleCount { get; set; }

        /// <summary>
        /// Gets or Sets List of fields
        /// </summary>
        public IList<Field> Fields { get; set; }
    }
}