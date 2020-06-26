namespace Api.FormEngine.Core.ViewModels.SheetModels
{
    /// <summary>
    /// Aggregation sheet model
    /// </summary>
    public class Aggregations
    {
        /// <summary>
        /// Gets or Sets Field Number
        /// </summary>
        public string FieldNo { get; set; }

        /// <summary>
        /// Gets or Sets Aggregation Field Number
        /// </summary>
        public string AggregatedFieldNo { get; set; }

        /// <summary>
        /// Gets or Sets Function
        /// </summary>
        public string Function { get; set; }

        /// <summary>
        /// Gets or Sets Priority of field
        /// </summary>
        public string Priority { get; set; }
    }
}