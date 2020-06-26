using System.Collections.Generic;

namespace Api.FormEngine.Core.ViewModels.SheetModels
{
    /// <summary>
    /// List of sheet model
    /// </summary>
    public class QuestionSet
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
        /// Gets or Sets List of sections
        /// </summary>
        public IList<Section> Sections { get; set; }
    }
}