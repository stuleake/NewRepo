using Api.FormEngine.Core.ViewModels.SheetModels;

namespace UnitTest.Api.FormEngine.Core.Services.SheetValidators.Builders
{
    /// <summary>
    /// Answer Guides Builder implementation.
    /// </summary>
    public class AnswerGuidesBuilder
    {
        private readonly AnswerGuide answerGuide;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnswerGuidesBuilder"/> class.
        /// </summary>
        public AnswerGuidesBuilder()
        {
            answerGuide = new AnswerGuide
            {
                AnsNo = "1",
                FieldNo = "7",
                DateMax = string.Empty,
                DateMin = string.Empty,
            };
        }

        /// <summary>
        /// Method to set AnsNo property.
        /// </summary>
        /// <param name="ansNo">Value to set.</param>
        /// <returns>An instance of <see cref="AnswerGuidesBuilder"/>.</returns>
        public AnswerGuidesBuilder WithAnsNo(string ansNo)
        {
            answerGuide.AnsNo = ansNo;
            return this;
        }

        /// <summary>
        /// Method to set FieldNo property.
        /// </summary>
        /// <param name="fieldNo">Value to set.</param>
        /// <returns>An instance of <see cref="AnswerGuidesBuilder"/>.</returns>
        public AnswerGuidesBuilder WithFieldNo(string fieldNo)
        {
            answerGuide.FieldNo = fieldNo;
            return this;
        }

        /// <summary>
        /// Method to set Multiple property.
        /// </summary>
        /// <param name="multiple">Value to set.</param>
        /// <returns>An instance of <see cref="AnswerGuidesBuilder"/>.</returns>
        public AnswerGuidesBuilder WithMultiple(string multiple)
        {
            answerGuide.Multiple = multiple;
            return this;
        }

        /// <summary>
        /// Method to set Range Min Range property.
        /// </summary>
        /// <param name="rangeMin">Min value to set.</param>
        /// <returns>An instance of <see cref="AnswerGuidesBuilder"/>.</returns>
        public AnswerGuidesBuilder WithMinRange(string rangeMin)
        {
            answerGuide.RangeMin = rangeMin;
            return this;
        }

        /// <summary>
        /// Method to set Range Max property.
        /// </summary>
        /// <param name="rangeMax">Max value to set.</param>
        /// <returns>An instance of <see cref="AnswerGuidesBuilder"/>.</returns>
        public AnswerGuidesBuilder WithMaxRange(string rangeMax)
        {
            answerGuide.RangeMax = rangeMax;
            return this;
        }

        /// <summary>
        /// Method to set Length Min property.
        /// </summary>
        /// <param name="lengthMin">Min value to set.</param>
        /// <returns>An instance of <see cref="AnswerGuidesBuilder"/>.</returns>
        public AnswerGuidesBuilder WithMinLength(string lengthMin)
        {
            answerGuide.LengthMin = lengthMin;
            return this;
        }

        /// <summary>
        /// Method to set Length Max property.
        /// </summary>
        /// <param name="lengthMax">Max value to set.</param>
        /// <returns>An instance of <see cref="AnswerGuidesBuilder"/>.</returns>
        public AnswerGuidesBuilder WithMaxLength(string lengthMax)
        {
            answerGuide.LengthMax = lengthMax;
            return this;
        }

        /// <summary>
        /// Method to set Date Min Range property.
        /// </summary>
        /// <param name="dateMin">Min value to set.</param>
        /// <returns>An instance of <see cref="AnswerGuidesBuilder"/>.</returns>
        public AnswerGuidesBuilder WithMinDate(string dateMin)
        {
            answerGuide.DateMin = dateMin;
            return this;
        }

        /// <summary>
        /// Method to set Date Max property.
        /// </summary>
        /// <param name="dateMax">Max value to set.</param>
        /// <returns>An instance of <see cref="AnswerGuidesBuilder"/>.</returns>
        public AnswerGuidesBuilder WithMaxDate(string dateMax)
        {
            answerGuide.DateMax = dateMax;
            return this;
        }

        /// <summary>
        /// Method to set Regex property.
        /// </summary>
        /// <param name="expression">value to set.</param>
        /// <returns>An instance of <see cref="AnswerGuidesBuilder"/>.</returns>
        public AnswerGuidesBuilder WithRegex(string expression)
        {
            answerGuide.Regex = expression;
            return this;
        }

        /// <summary>
        /// Method to set RegexBE property.
        /// </summary>
        /// <param name="expression">value to set.</param>
        /// <returns>An instance of <see cref="AnswerGuidesBuilder"/>.</returns>
        public AnswerGuidesBuilder WithRegexBe(string expression)
        {
            answerGuide.RegexBE = expression;
            return this;
        }

        /// <summary>
        /// Method to set IsDefault property.
        /// </summary>
        /// <param name="defaultValue">value to set.</param>
        /// <returns>An instance of <see cref="AnswerGuidesBuilder"/>.</returns>
        public AnswerGuidesBuilder WithIsDefault(string defaultValue)
        {
            answerGuide.IsDefault = defaultValue;
            return this;
        }

        /// <summary>
        /// Method to set Api property.
        /// </summary>
        /// <param name="api">value to set.</param>
        /// <returns>An instance of <see cref="AnswerGuidesBuilder"/>.</returns>
        public AnswerGuidesBuilder WithApi(string api)
        {
            answerGuide.Api = api;
            return this;
        }

        /// <summary>
        /// Method to set ErrorLabel property.
        /// </summary>
        /// <param name="errorLabel">value to set.</param>
        /// <returns>An instance of <see cref="AnswerGuidesBuilder"/>.</returns>
        public AnswerGuidesBuilder WithErrorLabel(string errorLabel)
        {
            answerGuide.ErrorLabel = errorLabel;
            return this;
        }

        /// <summary>
        /// Method to set Label property.
        /// </summary>
        /// <param name="label">value to set.</param>
        /// <returns>An instance of <see cref="AnswerGuidesBuilder"/>.</returns>
        public AnswerGuidesBuilder WithLabel(string label)
        {
            answerGuide.Label = label;
            return this;
        }

        /// <summary>
        /// Method to set Value property.
        /// </summary>
        /// <param name="value">value to set.</param>
        /// <returns>An instance of <see cref="AnswerGuidesBuilder"/>.</returns>
        public AnswerGuidesBuilder WithValue(string value)
        {
            answerGuide.Value = value;
            return this;
        }

        /// <summary>
        /// Builds an instance of <see cref="AnswerGuide"/>.
        /// </summary>
        /// <returns>An instance of <see cref="AnswerGuide"/>.</returns>
        public AnswerGuide Build()
        {
            return answerGuide;
        }
    }
}
