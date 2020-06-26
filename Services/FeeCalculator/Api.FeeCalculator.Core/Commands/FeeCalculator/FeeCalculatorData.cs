using Api.FeeCalculator.Core.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;

namespace Api.FeeCalculator.Core.Commands.FeeCalculator
{
    /// <summary>
    /// A Command to calculate the fee for answers
    /// </summary>
    public class FeeCalculatorData : BaseCommand, IRequest<FeeCalculatorResponseModel>
    {
        /// <summary>
        /// Gets or Sets the session type
        /// </summary>
        public string SessionType { get; set; }

        /// <summary>
        /// Gets or Sets the session Id
        /// </summary>
        public Guid SessionId { get; set; }

        /// <summary>
        /// Gets or Sets the QS Collection Id
        /// </summary>
        public Guid? QsCollectionId { get; set; }

        /// <summary>
        /// Gets or Sets the Region
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Gets or Sets the Language
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Gets or Sets the list of answers for each parameter
        /// </summary>
        public IEnumerable<AnswerModel> Answers { get; set; }
    }
}