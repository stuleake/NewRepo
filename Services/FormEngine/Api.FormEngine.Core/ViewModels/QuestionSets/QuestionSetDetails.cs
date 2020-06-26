using System;

namespace Api.FormEngine.Core.ViewModels.QuestionSets
{
    /// <summary>
    /// Model for QuestionSet
    /// </summary>
    public class QuestionSetDetails
    {
        /// <summary>
        /// Gets or Sets QuestionSet Id
        /// </summary>
        public Guid QSId { get; set; }

        /// <summary>
        /// Gets or Sets QuestionSet Number
        /// </summary>
        public int QSNo { get; set; }

        /// <summary>
        /// Gets or Sets the QuestionSet Version
        /// </summary>
        public string QSVersion { get; set; }

        /// <summary>
        /// Gets or Sets the QuestionSet Name
        /// </summary>
        public string QSName { get; set; }

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
        /// Gets or Sets the Status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or Sets Defination
        /// </summary>
        public string Definition { get; set; }
    }
}