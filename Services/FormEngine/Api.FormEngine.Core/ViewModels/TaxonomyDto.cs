using CsvHelper.Configuration.Attributes;
using System.Collections.Generic;

namespace Api.FormEngine.Core.ViewModels
{
    /// <summary>
    /// Data transfer object for reading taxonomy csv
    /// </summary>
    public class TaxonomyDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaxonomyDto"/> class.
        /// </summary>
        public TaxonomyDto()
        {
            TaxonomyValues = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets or sets QsNo
        /// </summary>
        [Name("QsNo")]
        public int QsNo { get; set; }

        /// <summary>
        /// Gets or sets QsVersion
        /// </summary>
        [Name("QsVersion")]
        public string QsVersion { get; set; }

        /// <summary>
        /// Gets or sets LanguageTenant
        /// </summary>
        public string LanguageTenant { get; set; }

        /// <summary>
        /// Gets or sets taxonomyValues
        /// </summary>
        public Dictionary<string, string> TaxonomyValues { get; set; }
    }
}