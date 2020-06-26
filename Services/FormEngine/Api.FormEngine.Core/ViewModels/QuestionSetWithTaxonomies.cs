using System.Collections.Generic;

namespace Api.FormEngine.Core.ViewModels
{
    /// <summary>
    /// Class for Question Set With Taxonomies
    /// </summary>
    public class QuestionSetWithTaxonomies
    {
        /// <summary>
        /// Gets or sets QSNo
        /// </summary>
        public int QSNo { get; set; }

        /// <summary>
        /// Gets or sets QSName
        /// </summary>
        public string QSName { get; set; }

        /// <summary>
        /// Gets or sets QSVersion
        /// </summary>
        public string QSVersion { get; set; }

        /// <summary>
        /// Gets or sets QSTaxonomy
        /// </summary>
        public Dictionary<string, string> QSTaxonomy { get; set; }
    }
}