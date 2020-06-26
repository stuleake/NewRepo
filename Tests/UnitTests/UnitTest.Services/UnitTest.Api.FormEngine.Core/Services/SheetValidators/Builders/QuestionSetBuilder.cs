using Api.FormEngine.Core.ViewModels.SheetModels;
using System.Collections.Generic;

namespace UnitTest.Api.FormEngine.Core.Services.SheetValidators.Builders
{
    /// <summary>
    /// QuestionSet Builder implementation.
    /// </summary>
    public class QuestionSetBuilder
    {
        private readonly QuestionSet questionSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionSetBuilder"/> class.
        /// </summary>
        public QuestionSetBuilder()
        {
            // set any other default values
            questionSet = new QuestionSet
            {
                Sections = new List<Section>()
            };
        }

        /// <summary>
        /// Method to set Sections property.
        /// </summary>
        /// <param name="sections">The value to set.</param>
        /// <returns>An instance of <see cref="QuestionSetBuilder"/>.</returns>
        public QuestionSetBuilder WithSections(List<Section> sections)
        {
            questionSet.Sections = sections;
            return this;
        }

        /// <summary>
        /// Method to add Section to Sections property.
        /// </summary>
        /// <param name="section">The value to set.</param>
        /// <returns>An instance of <see cref="QuestionSetBuilder"/>.</returns>
        public QuestionSetBuilder WithSection(Section section)
        {
            questionSet.Sections.Add(section);
            return this;
        }


        /// <summary>
        /// Method to set QsNo.
        /// </summary>
        /// <param name="qsno">The value to set.</param>
        /// <returns>An instance of <see cref="QuestionSetBuilder"/>.</returns>
        public QuestionSetBuilder WithQsNo(string qsno)
        {
            questionSet.QsNo = qsno;
            return this;
        }


        /// <summary>
        /// Method to set QSDesc.
        /// </summary>
        /// <param name="qsdesc">The value to set.</param>
        /// <returns>An instance of <see cref="QuestionSetBuilder"/>.</returns>
        public QuestionSetBuilder WithQSDesc(string qsdesc)
        {
            questionSet.QSDesc = qsdesc;
            return this;
        }


        /// <summary>
        /// Method to set QSLabel.
        /// </summary>
        /// <param name="qslabel">The value to set.</param>
        /// <returns>An instance of <see cref="QuestionSetBuilder"/>.</returns>
        public QuestionSetBuilder WithQSLabel(string qslabel)
        {
            questionSet.QSLabel = qslabel;
            return this;
        }

        /// <summary>
        /// Method to set QSHelptext.
        /// </summary>
        /// <param name="qshelptext">The value to set.</param>
        /// <returns>An instance of <see cref="QuestionSetBuilder"/>.</returns>
        public QuestionSetBuilder WithQSHelptext(string qshelptext)
        {
            questionSet.QSHelptext = qshelptext;
            return this;
        }

        /// <summary>
        /// Builds an instance of <see cref="QuestionSet"/>.
        /// </summary>
        /// <returns>An instance of <see cref="QuestionSet"/>.</returns>
        public QuestionSet Build()
        {
            return questionSet;
        }
    }
}
