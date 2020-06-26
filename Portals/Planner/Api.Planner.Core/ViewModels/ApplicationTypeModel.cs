using System;

namespace Api.Planner.Core.ViewModels
{
    /// <summary>
    /// Model for Application Types
    /// </summary>
    public class ApplicationTypeModel
    {
        /// <summary>
        /// Gets or Sets the QS Collection Type Id
        /// </summary>
        public Guid QSCollectionTypeId { get; set; }

        /// <summary>
        /// Gets or Sets the Label
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or Sets the Helptext
        /// </summary>
        public string Helptext { get; set; }

        /// <summary>
        /// Gets or Sets the Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets Country Code
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// Gets or sets ApplicationTypeRefNo
        /// </summary>
        public int ApplicationTypeRefNo { get; set; }
    }
}