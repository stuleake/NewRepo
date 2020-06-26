using Api.FormEngine.Core.ViewModels.SheetModels;

namespace UnitTest.Api.FormEngine.Core.Services.SheetValidators.Builders
{
    /// <summary>
    /// Dependencies Builder implementation.
    /// </summary>
    public class DependencyBuilder
    {
        private readonly Dependencies dependencies;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyBuilder"/> class.
        /// </summary>
        public DependencyBuilder()
        {
            dependencies = new Dependencies
            {
                DecidesSection = "2",
                DependsCount = "1",
                DependsOnAns = "1",
                DependsOnAnsfromQS = "1",
                FieldNo = "1",
                FieldDisplay = "1"
            };
        }

        /// <summary>
        /// Method to set DecidesSection property.
        /// </summary>
        /// <param name="decidesSection">The value to set.</param>
        /// <returns>An instance of <see cref="DependencyBuilder"/>.</returns>
        public DependencyBuilder WithDecidesSection(string decidesSection)
        {
            dependencies.DecidesSection = decidesSection;
            return this;
        }

        /// <summary>
        /// Method to set FieldDisplay property.
        /// </summary>
        /// <param name="fieldDisplay">The value to set.</param>
        /// <returns>An instance of <see cref="DependencyBuilder"/>.</returns>
        public DependencyBuilder WithFieldDisplay(string fieldDisplay)
        {
            dependencies.FieldDisplay = fieldDisplay;
            return this;
        }

        /// <summary>
        /// Method to set SectionNo property.
        /// </summary>
        /// <param name="sectionNo">The value to set.</param>
        /// <returns>An instance of <see cref="DependencyBuilder"/>.</returns>
        public DependencyBuilder WithSectionNo(string sectionNo)
        {
            dependencies.SectionNo = sectionNo;
            return this;
        }

        /// <summary>
        /// Method to set DependsCount property.
        /// </summary>
        /// <param name="dependsCount">The value to set.</param>
        /// <returns>An instance of <see cref="DependencyBuilder"/>.</returns>
        public DependencyBuilder WithDependsCount(string dependsCount)
        {
            dependencies.DependsCount = dependsCount;
            return this;
        }

        /// <summary>
        /// Method to set DependsOnAns property.
        /// </summary>
        /// <param name="dependsOnAns">The value to set.</param>
        /// <returns>An instance of <see cref="DependencyBuilder"/>.</returns>
        public DependencyBuilder WithDependsOnAns(string dependsOnAns)
        {
            dependencies.DependsOnAns = dependsOnAns;
            return this;
        }

        /// <summary>
        /// Method to set DependsOnAnsfromQS property.
        /// </summary>
        /// <param name="dependsOnAnsfromQS">The value to set.</param>
        /// <returns>An instance of <see cref="DependencyBuilder"/>.</returns>
        public DependencyBuilder WithDependsOnAnsfromQS(string dependsOnAnsfromQS)
        {
            dependencies.DependsOnAnsfromQS = dependsOnAnsfromQS;
            return this;
        }

        /// <summary>
        /// Method to set FieldNo property.
        /// </summary>
        /// <param name="fieldNo">The value to set.</param>
        /// <returns>An instance of <see cref="DependencyBuilder"/>.</returns>
        public DependencyBuilder WithFieldNo(string fieldNo)
        {
            dependencies.FieldNo = fieldNo;
            return this;
        }

        /// <summary>
        /// Builds an instance of <see cref="Dependencies"/>.
        /// </summary>
        /// <returns>An instance of <see cref="Dependencies"/>.</returns>
        public Dependencies Build()
        {
            return dependencies;
        }
    }
}
