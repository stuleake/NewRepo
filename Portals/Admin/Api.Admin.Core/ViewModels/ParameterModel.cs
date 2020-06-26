namespace Api.Admin.Core.ViewModels
{
    /// <summary>
    /// A model class for rule paramter
    /// </summary>
    public class ParameterModel
    {
        /// <summary>
        /// Gets or Sets the Name of Parameter
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets the datatype of paramter
        /// </summary>
        public string Datatype { get; set; }

        /// <summary>
        /// Gets or Sets the Parameter type
        /// </summary>
        public string ParameterType { get; set; }

        /// <summary>
        /// Gets or Sets the Output operation to be perfomed on output
        /// </summary>
        public string OutputOperation { get; set; }
    }
}