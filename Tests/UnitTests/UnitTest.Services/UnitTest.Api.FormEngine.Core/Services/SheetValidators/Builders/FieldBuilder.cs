using Api.FormEngine.Core.ViewModels.SheetModels;

namespace UnitTest.Api.FormEngine.Core.Services.SheetValidators.Builders
{
    /// <summary>
    /// Field Builder implementation.
    /// </summary>
    public class FieldBuilder
    {
        private readonly Field field;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldBuilder"/> class.
        /// </summary>
        public FieldBuilder()
        {
            field = new Field
            {
                FieldNo = "1",
                FieldType = "number"
            };
        }

        /// <summary>
        /// Method to set FieldNo.
        /// </summary>
        /// <param name="fieldNo">The value to set.</param>
        /// <returns>An instance of <see cref="FieldBuilder"/>.</returns>
        public FieldBuilder WithFieldNo(string fieldNo)
        {
            field.FieldNo = fieldNo;
            return this;
        }

        /// <summary>
        /// Method to set FieldType.
        /// </summary>
        /// <param name="fieldType">The value to set.</param>
        /// <returns>An instance of <see cref="FieldBuilder"/>.</returns>
        public FieldBuilder WithFieldType(string fieldType)
        {
            field.FieldType = fieldType;
            return this;
        }

        /// <summary>
        /// Method to set CopyfromQS.
        /// </summary>
        /// <param name="copyfromQS">The value to set.</param>
        /// <returns>An instance of <see cref="FieldBuilder"/>.</returns>
        public FieldBuilder WithCopyfromQS(string copyfromQS)
        {
            field.CopyfromQS = copyfromQS;
            return this;
        }

        /// <summary>
        /// Method to set CopyfromField.
        /// </summary>
        /// <param name="copyfromField">The value to set.</param>
        /// <returns>An instance of <see cref="FieldBuilder"/>.</returns>
        public FieldBuilder WithCopyfromField(string copyfromField)
        {
            field.CopyfromField = copyfromField;
            return this;
        }

        /// <summary>
        /// Builds an instance of <see cref="Field"/>.
        /// </summary>
        /// <returns>An instance of <see cref="Field"/>.</returns>
        public Field Build()
        {
            return field;
        }
    }
}
