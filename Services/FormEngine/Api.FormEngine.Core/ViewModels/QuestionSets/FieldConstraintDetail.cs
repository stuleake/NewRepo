using System;

namespace Api.FormEngine.Core.ViewModels.QuestionSets
{
    /// <summary>
    /// class to manage Field Constraint Detail
    /// </summary>
    public class FieldConstraintDetail
    {
        /// <summary>
        /// Gets or Sets Field Constraint Id
        /// </summary>
        public Guid FieldConstraintId { get; set; }

        /// <summary>
        /// Gets or Sets Field  Id
        /// </summary>
        public Guid? FieldId { get; set; }

        /// <summary>
        /// Gets or Sets the section id
        /// </summary>
        public Guid? SectionId { get; set; }

        /// <summary>
        /// Gets or Sets Section No
        /// </summary>
        public int? SectionNo { get; set; }

        /// <summary>
        /// Gets or Sets Dependant Answer Guide No
        /// </summary>
        public int? DependantAnswerGuideNo { get; set; }

        /// <summary>
        /// Gets or Sets Dependant Answer Guide QS No
        /// </summary>
        public int? DependantAnswerQSNo { get; set; }

        /// <summary>
        /// Gets or Sets Dependant Answer Field Id
        /// </summary>
        public string DependantAnswerFieldId { get; set; }

        /// <summary>
        /// Gets or Sets Answer Value
        /// </summary>
        public string AnswerValue { get; set; }

        /// <summary>
        /// Gets or Sets Constraint Rule Count
        /// </summary>
        public string ConstraintRuleCount { get; set; }
    }
}