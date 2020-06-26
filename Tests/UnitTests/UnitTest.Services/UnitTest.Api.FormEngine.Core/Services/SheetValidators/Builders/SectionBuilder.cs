using Api.FormEngine.Core.ViewModels.SheetModels;
using System.Collections.Generic;

namespace UnitTest.Api.FormEngine.Core.Services.SheetValidators.Builders
{
    /// <summary>
    /// Section Builder Implementation.
    /// </summary>
    public class SectionBuilder
    {
        private readonly Section section;

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionBuilder"/> class.
        /// </summary>
        public SectionBuilder()
        {
            section = new Section
            {
                SectionNo = "2",
                Fields = new List<Field>()
            };
        }

        /// <summary>
        /// Method to set Field.
        /// </summary>
        /// <param name="field">The value to set.</param>
        /// <returns>An instance of <see cref="SectionBuilder"/>.</returns>
        public SectionBuilder WithField(Field field)
        {
            section.Fields.Add(field);
            return this;
        }

        /// <summary>
        /// Builds an instance of <see cref="Section"/>.
        /// </summary>
        /// <returns>An instance of <see cref="Section"/>.</returns>
        public Section Build()
        {
            return section;
        }
    }
}
