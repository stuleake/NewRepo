using System;

namespace Api.Planner.Core.ViewModels
{
    /// <summary>
    /// Question Set Data
    /// </summary>
    public class QuestionSet
    {
        /// <summary>
        /// Gets or Sets QS Id
        /// </summary>
        public Guid QSId { get; set; }

        /// <summary>
        /// Gets or Sets QSName
        /// </summary>
        public string QSName { get; set; }

        /// <summary>
        /// Gets or Sets QSNo
        /// </summary>
        public int QSNo { get; set; }

        /// <summary>
        /// Gets or Sets Sequence
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// Gets or Sets QSVersion
        /// </summary>
        public string QSVersion { get; set; }

        /// <summary>
        /// Gets or Sets the Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets the Status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or Sets Question Set id
        /// </summary>
        public Guid QuestionSetId { get; set; }

        /// <summary>
        /// Gets or Sets Definition of question set
        /// </summary>
        public string Definition { get; set; }
    }
}