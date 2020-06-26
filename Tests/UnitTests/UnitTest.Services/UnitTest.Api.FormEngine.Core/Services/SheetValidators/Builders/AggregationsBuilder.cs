using Api.FormEngine.Core.ViewModels.SheetModels;

namespace UnitTest.Api.FormEngine.Core.Services.SheetValidators.Builders
{
    /// <summary>
    /// Aggregations Builder implementation.
    /// </summary>
    public class AggregationsBuilder
    {
        private readonly Aggregations aggregations;

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregationsBuilder"/> class.
        /// </summary>
        public AggregationsBuilder()
        {
            aggregations = new Aggregations
            {
                AggregatedFieldNo = "2",
                FieldNo = "1",
                Function = "function",
                Priority = "1"
            };
        }

        /// <summary>
        /// Method to add FieldNo property.
        /// </summary>
        /// <param name="fieldNo">The value to set.</param>
        /// <returns>An instance of <see cref="AggregationsBuilder"/>.</returns>
        public AggregationsBuilder WithFieldNo(string fieldNo)
        {
            aggregations.FieldNo = fieldNo;
            return this;
        }

        /// <summary>
        /// Method to add AggregatedFieldNo property.
        /// </summary>
        /// <param name="aggregatedFieldNo">The value to set.</param>
        /// <returns>An instance of <see cref="AggregationsBuilder"/>.</returns>
        public AggregationsBuilder WithAggregatedFieldNo(string aggregatedFieldNo)
        {
            aggregations.AggregatedFieldNo = aggregatedFieldNo;
            return this;
        }

        /// <summary>
        /// Method to add Function property.
        /// </summary>
        /// <param name="function">The value to set.</param>
        /// <returns>An instance of <see cref="AggregationsBuilder"/>.</returns>
        public AggregationsBuilder WithFunction(string function)
        {
            aggregations.Function = function;
            return this;
        }

        /// <summary>
        /// Method to add Priority property.
        /// </summary>
        /// <param name="priority">The value to set.</param>
        /// <returns>An instance of <see cref="AggregationsBuilder"/>.</returns>
        public AggregationsBuilder WithPriority(string priority)
        {
            aggregations.Priority = priority;
            return this;
        }

        /// <summary>
        /// Builds an instance of <see cref="Aggregations"/>.
        /// </summary>
        /// <returns>An instance of <see cref="Aggregations"/>.</returns>
        public Aggregations Build()
        {
            return aggregations;
        }
    }
}
