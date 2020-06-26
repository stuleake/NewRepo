using System;
using System.Collections.Generic;

namespace Api.FeeCalculator.Core.ViewModels
{
    /// <summary>
    /// A model class containing response for fee calculator
    /// </summary>
    public class FeeCalculatorResponseModel
    {
        /// <summary>
        /// Gets or Sets the Session Type
        /// </summary>
        public string SessionType { get; set; }

        /// <summary>
        /// Gets or Sets the Session Id
        /// </summary>
        public Guid SessionId { get; set; }

        /// <summary>
        /// Gets or Sets the CalculationSteps
        /// </summary>
        public IEnumerable<CalculationStepModel> CalculationSteps { get; set; }
    }
}