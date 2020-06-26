using System;

namespace Api.FormEngine.Core.ViewModels.QSR
{
    /// <summary>
    /// QsrAnswer Model
    /// </summary>
    public class QsrAnswerModel
    {
        /// <summary>
        /// Gets or Sets the Field Id
        /// </summary>
        public Guid FieldId { get; set; }

        /// <summary>
        /// Gets or Sets the Answer
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// Gets or Sets the field no
        /// </summary>
        public int FieldNo { get; set; }
    }
}