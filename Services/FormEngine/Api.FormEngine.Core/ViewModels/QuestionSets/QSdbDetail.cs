using System;

namespace Api.FormEngine.Core.ViewModels.QuestionSets
{
    /// <summary>
    /// Model for Question set DB result
    /// </summary>
    public class QSdbDetail
    {
        /// <summary>
        /// Gets or Sets QS id
        /// </summary>
        public Guid QSId { get; set; }

        /// <summary>
        /// Gets or Sets QS no
        /// </summary>
        public int QSNo { get; set; }

        /// <summary>
        /// Gets or Sets QS version
        /// </summary>
        public string QSVersion { get; set; }

        /// <summary>
        /// Gets or Sets QS id
        /// </summary>
        public string QSName { get; set; }

        /// <summary>
        /// Gets or Sets QS label
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or Sets QS helptext
        /// </summary>
        public string Helptext { get; set; }

        /// <summary>
        /// Gets or Sets QS description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets QS status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or Sets QS created date
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or Sets QS Last Modified Date
        /// </summary>
        public DateTime LastModifiedDate { get; set; }

        /// <summary>
        /// Gets or Sets QS Last Modified by
        /// </summary>
        public Guid LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or Sets Section Sequence
        /// </summary>
        public int SectionSequence { get; set; }

        /// <summary>
        /// Gets or Sets Section id
        /// </summary>
        public Guid SectionId { get; set; }

        /// <summary>
        /// Gets or Sets Section Label
        /// </summary>
        public string SectionLabel { get; set; }

        /// <summary>
        /// Gets or Sets Section Helptext
        /// </summary>
        public string SectionHelpText { get; set; }

        /// <summary>
        /// Gets or Sets Section Description
        /// </summary>
        public string SectionDescription { get; set; }

        /// <summary>
        /// Gets or Sets Section Type
        /// </summary>
        public string SectionType { get; set; }

        /// <summary>
        /// Gets or Sets Section Rule
        /// </summary>
        public string SectionRule { get; set; }

        /// <summary>
        /// Gets or Sets SectionRule count
        /// </summary>
        public int SectionRuleCount { get; set; }

        /// <summary>
        /// Gets or Sets FieldSequence
        /// </summary>
        public int FieldSequence { get; set; }

        /// <summary>
        /// Gets or Sets Field id
        /// </summary>
        public Guid FieldId { get; set; }

        /// <summary>
        /// Gets or Sets FieldNo
        /// </summary>
        public int FieldNo { get; set; }

        /// <summary>
        /// Gets or Sets Field version
        /// </summary>
        public string FieldVersion { get; set; }

        /// <summary>
        /// Gets or Sets QS id
        /// </summary>
        public string FieldLabel { get; set; }

        /// <summary>
        /// Gets or Sets Field Helptext
        /// </summary>
        public string FieldHelptext { get; set; }

        /// <summary>
        /// Gets or Sets Field Description
        /// </summary>
        public string FieldDescription { get; set; }

        /// <summary>
        /// Gets or Sets Field Type
        /// </summary>
        public string FieldType { get; set; }

        /// <summary>
        /// Gets or Sets Display
        /// </summary>
        public string Display { get; set; }

        /// <summary>
        /// Gets or Sets DisplayConstraint
        /// </summary>
        public string DisplayConstraint { get; set; }

        /// <summary>
        /// Gets or Sets ConstraintRuleId
        /// </summary>
        public string ConstraintRule { get; set; }

        /// <summary>
        /// Gets or Sets ConstraintRuleCount
        /// </summary>
        public int ConstraintRuleCount { get; set; }

        /// <summary>
        /// Gets or Sets AnswerRule
        /// </summary>
        public string AnswerRule { get; set; }

        /// <summary>
        /// Gets or Sets AnswerRuleCount
        /// </summary>
        public int AnswerRuleCount { get; set; }

        /// <summary>
        /// Gets or Sets AnswerGuide Id
        /// </summary>

        public Guid AnswerGuideId { get; set; }

        /// <summary>
        /// Gets or Sets AnswerType
        /// </summary>

        public string AnswerType { get; set; }

        /// <summary>
        /// Gets or Sets Min
        /// </summary>
        public string Min { get; set; }

        /// <summary>
        /// Gets or Sets Max
        /// </summary>
        public string Max { get; set; }

        /// <summary>
        /// Gets or Sets AnswerGuideLabel
        /// </summary>
        public string AnswerGuideLabel { get; set; }

        /// <summary>
        /// Gets or Sets AnswerGuide value
        /// </summary>
        public string AnswerGuideValue { get; set; }

        /// <summary>
        /// Gets or Sets AnswerGuide Error
        /// </summary>
        public string AnswerGuideError { get; set; }

        /// <summary>
        /// Gets or Sets IsDefault
        /// </summary>
        public string IsDefault { get; set; }

        /// <summary>
        /// Gets or Sets AnswerSequence
        /// </summary>
        public int AnswerSequence { get; set; }

        /// <summary>
        /// Gets or Sets the Answer Guide Pattern
        /// </summary>
        public string AnswerGuidePattern { get; set; }
    }
}