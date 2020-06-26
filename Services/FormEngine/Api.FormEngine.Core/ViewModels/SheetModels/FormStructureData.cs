using System.Collections.Generic;
using TQ.Data.FormEngine.Schemas.Forms;

namespace Api.FormEngine.Core.ViewModels.SheetModels
{
    /// <summary>
    /// List of all types in form structure
    /// </summary>
    public class FormStructureData
    {
        /// <summary>
        /// Gets or Sets the list of section type
        /// </summary>
        public IList<SectionType> SectionTypes { get; set; }

        /// <summary>
        /// Gets or Sets the list of field type
        /// </summary>
        public IList<FieldType> FieldTypes { get; set; }

        /// <summary>
        /// Gets or Sets the list of displays
        /// </summary>
        public IList<Display> Displays { get; set; }

        /// <summary>
        /// Gets or Sets the list of constraints
        /// </summary>
        public IList<Constraint> Constraints { get; set; }

        /// <summary>
        /// Gets or Sets the list of answerTypes
        /// </summary>
        public IList<AnswerType> AnswerTypes { get; set; }

        /// <summary>
        /// Gets or Sets the list of rules
        /// </summary>
        public IList<Rule> Rules { get; set; }

        /// <summary>
        /// Gets or sets the list of functions
        /// </summary>
        public IList<Function> Functions { get; set; }
    }
}