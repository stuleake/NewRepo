namespace Api.FormEngine.Core.ViewModels.SheetModels
{
    /// <summary>
    /// List of sheet model
    /// </summary>
    public class Field
    {
        /// <summary>
        /// Gets or Sets Field Number
        /// </summary>
        public string FieldNo { get; set; }

        /// <summary>
        /// Gets or Sets Field Label
        /// </summary>
        public string FieldLabel { get; set; }

        /// <summary>
        /// Gets or Sets Field Help Text
        /// </summary>
        public string Fieldhelptext { get; set; }

        /// <summary>
        /// Gets or Sets Field Description
        /// </summary>
        public string FieldDesc { get; set; }

        /// <summary>
        /// Gets or Sets the field type id
        /// </summary>
        public int FieldTypeId { get; set; }

        /// <summary>
        /// Gets or Sets Field Type
        /// </summary>
        public string FieldType { get; set; }

        /// <summary>
        /// Gets or Sets the DIsplay id
        /// </summary>
        public int? DisplayId { get; set; }

        /// <summary>
        /// Gets or Sets Field Display
        /// </summary>
        public string FieldDisplay { get; set; }

        /// <summary>
        /// Gets or Sets ToBeRedacted
        /// </summary>
        public string ToBeRedacted { get; set; }

        /// <summary>
        /// Gets or Sets the copy from qs
        /// </summary>
        public string CopyfromQS { get; set; }

        /// <summary>
        /// Gets or Sets the Copy from Field
        /// </summary>
        public string CopyfromField { get; set; }

        /// <summary>
        /// Gets or Sets the action of field
        /// </summary>
        public string Action { get; set; }
    }
}