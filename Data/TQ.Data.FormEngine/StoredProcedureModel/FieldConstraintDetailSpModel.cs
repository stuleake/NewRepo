using System;
using System.ComponentModel.DataAnnotations;

namespace TQ.Data.FormEngine.StoredProcedureModel
{
    public class FieldConstraintDetailSpModel
    {
        /// <summary>
        /// Gets or Sets Field Constraint Id
        /// </summary>
        [Key]
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
        /// Gets or Sets Dependant Answer Field Id
        /// </summary>
        public string DependantAnswerFieldId { get; set; }

        /// <summary>
        /// Gets or Sets Answer Value
        /// </summary>
        public string AnswerValue { get; set; }
    }
}
