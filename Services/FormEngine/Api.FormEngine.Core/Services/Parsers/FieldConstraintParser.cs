using Api.FormEngine.Core.Services.Parsers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using TQ.Core.Repository;
using TQ.Data.FormEngine.Schemas.Forms;

namespace Api.FormEngine.Core.Services.Parsers
{
    /// <inheritdoc/>
    public class FieldConstraintParser : IParser<FieldConstraintParserModel, IEnumerable<FieldConstraint>>
    {
        private readonly IReadOnlyRepository<TQ.Data.FormEngine.Schemas.Forms.Field> fieldRepository;
        private readonly IReadOnlyRepository<TQ.Data.FormEngine.Schemas.Forms.AnswerGuide> answerGuideRepository;
        private readonly IReadOnlyRepository<TQ.Data.FormEngine.Schemas.Forms.Section> sectionRepository;

        /// <inheritdoc/>
        public FieldConstraintParser(
            IReadOnlyRepository<TQ.Data.FormEngine.Schemas.Forms.Field> fieldRepository,
            IReadOnlyRepository<TQ.Data.FormEngine.Schemas.Forms.AnswerGuide> answerGuideRepository,
            IReadOnlyRepository<TQ.Data.FormEngine.Schemas.Forms.Section> sectionRepository)
        {
            this.fieldRepository = fieldRepository;
            this.answerGuideRepository = answerGuideRepository;
            this.sectionRepository = sectionRepository;
        }

        /// <inheritdoc/>
        public ParseResult<IEnumerable<FieldConstraint>> Parse(FieldConstraintParserModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var fieldConstraints = new List<FieldConstraint>();

            foreach (var dependency in model.Dependencies)
            {
                if (string.IsNullOrEmpty(dependency.FieldNo)
                    ||
                    !fieldRepository.GetLocalQueryable()
                    .Any(localField => localField.FieldNo == Convert.ToInt32(dependency.FieldNo))
                    ||
                    !answerGuideRepository.GetLocalQueryable()
                    .Any(localAnswerGuide => localAnswerGuide.AnswerGuideNo.ToString() == dependency.DependsOnAns))
                {
                    continue;
                }

                var currentQuestionSet = model.QuestionSet.QsNo;
                var fieldId = Guid.Empty;
                var sectionId = Guid.Empty;
                var dependantAnswerGuideId = Guid.Empty;

                if (!string.IsNullOrEmpty(dependency.FieldNo))
                {
                    fieldId = fieldRepository.GetLocalQueryable()
                        .FirstOrDefault(localField => localField.FieldNo == Convert.ToInt32(dependency.FieldNo)).FieldId;
                }

                if (!string.IsNullOrEmpty(dependency.SectionNo))
                {
                    sectionId = sectionRepository.GetLocalQueryable()
                        .FirstOrDefault(localSection => localSection.SectionNo == Convert.ToInt32(dependency.SectionNo)).SectionId;
                }

                if (string.IsNullOrWhiteSpace(dependency.DependsOnAnsfromQS) || dependency.DependsOnAnsfromQS == currentQuestionSet)
                {
                    dependantAnswerGuideId = answerGuideRepository.GetLocalQueryable()
                        .FirstOrDefault(localAnswerGuide => localAnswerGuide.AnswerGuideNo == Convert.ToInt32(dependency.DependsOnAns)).AnswerGuideId;
                }

                fieldConstraints.Add(new FieldConstraint
                {
                    FieldId = fieldId,
                    DependantAnswerGuideId = dependantAnswerGuideId,
                    DependantAnswerQSNo = int.TryParse(dependency.DependsOnAnsfromQS, out var dependsOnAnsFromQS) ? dependsOnAnsFromQS : (int?)null,
                    DependantAnswerGuideNo = int.TryParse(dependency.DependsOnAns, out var dependsOnAns) ? dependsOnAns : (int?)null,
                    SectionId = sectionId,
                    SectionNo = int.TryParse(dependency.SectionNo, out var secNo) ? secNo : (int?)null,
                });
            }

            return new ParseResult<IEnumerable<FieldConstraint>>
            {
                Added = fieldConstraints
            };
        }
    }
}