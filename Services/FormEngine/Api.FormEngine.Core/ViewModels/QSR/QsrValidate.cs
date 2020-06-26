using System;

namespace Api.FormEngine.Core.ViewModels.QSR
{
    /// <summary>
    /// QSRValidate Model
    /// </summary>
    public class QsrValidate
    {
        /// <summary>
        /// Gets or Sets the Field Id
        /// </summary>
        public Guid FieldId { get; set; }

        /// <summary>
        /// Gets or Sets the Field No
        /// </summary>
        public int FieldNo { get; set; }

        /// <summary>
        /// Gets or Sets the QSR Answer Id
        /// </summary>
        public Guid QsrAnswerId { get; set; }

        /// <summary>
        /// Gets or Sets the QSR ID
        /// </summary>
        public Guid QsrId { get; set; }

        /// <summary>
        /// Gets or Sets the Answer
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// Gets or Sets the Field Type
        /// </summary>
        public string FieldType { get; set; }

        /// <summary>
        /// Gets or Sets the Display
        /// </summary>
        public string Display { get; set; }

        /// <summary>
        /// Gets or Sets the Display Constraint
        /// </summary>
        public string DisplayConstraint { get; set; }

        /// <summary>
        /// Gets or Sets the Constraint Rule
        /// </summary>
        public string ConstraintRule { get; set; }

        /// <summary>
        /// Gets or Sets the Constraint Rule Count
        /// </summary>
        public int ConstraintRuleCount { get; set; }

        /// <summary>
        /// Gets or Sets the Answer Rule
        /// </summary>
        public string AnswerRule { get; set; }

        /// <summary>
        /// Gets or Sets the Answer Rule Count
        /// </summary>
        public int AnswerRuleCount { get; set; }

        /// <summary>
        /// Gets or Sets the Answer Type
        /// </summary>
        public string AnswerType { get; set; }

        /// <summary>
        /// Gets or Sets the err label
        /// </summary>
        public string Errlabel { get; set; }

        /// <summary>
        /// Gets or Sets the Min
        /// </summary>
        public string Min { get; set; }

        /// <summary>
        /// Gets or Sets the Max
        /// </summary>
        public string Max { get; set; }

        /// <summary>
        /// Gets or Sets the Field Count
        /// </summary>
        public int FieldCount { get; set; }

        /// <summary>
        /// Gets or Sets the Field Answer Match Count
        /// </summary>
        public int FieldAnswerMatchCount { get; set; }

        /// <summary>
        /// Gets or Sets the Regex
        /// </summary>
        public string Regex { get; set; }
    }
}