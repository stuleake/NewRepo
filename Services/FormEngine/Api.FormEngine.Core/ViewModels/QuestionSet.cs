using System;

namespace Api.FormEngine.Core.ViewModels
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
        public decimal QSVersion { get; set; }

        /// <summary>
        /// Gets or Sets Question Set id
        /// </summary>
        public Guid QuestionSetId { get; set; }

        /// <summary>
        /// Gets or Sets Application Type id
        /// </summary>
        public Guid ApplicationTypeId { get; set; }

        /// <summary>
        /// Gets or Sets Defination
        /// </summary>
        public string Definition { get; set; }

        /// <summary>
        /// Gets or Sets Language id
        /// </summary>
        public Guid LanguageId { get; set; }
    }
}