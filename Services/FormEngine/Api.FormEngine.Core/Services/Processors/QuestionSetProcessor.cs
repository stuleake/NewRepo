using Api.FormEngine.Core.Services.Parsers;
using Api.FormEngine.Core.Services.Parsers.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using TQ.Core.Repository;
using TQ.Data.FormEngine.Schemas.Forms;

namespace Api.FormEngine.Core.Services.Processors
{
    /// <summary>
    /// Class to process incoming question set data.
    /// </summary>
    public class QuestionSetProcessor : IProcessor<QuestionSetParserModel>
    {
        private readonly IRepository<QS> questionSetRepository;
        private readonly IRepository<QSSectionMapping> questionSetSectionMappingRepository;
        private readonly IRepository<SectionFieldMapping> sectionFieldMappingRepository;
        private readonly IRepository<Section> sectionRepository;
        private readonly IRepository<Field> fieldRepository;
        private readonly IRepository<FieldConstraint> fieldConstraintsRepository;
        private readonly IParser<QuestionSetParserModel, QS> parser;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionSetProcessor"/> class.
        /// </summary>
        /// <param name="questionSetRepository">Question set repository to query.</param>
        /// <param name="questionSetSectionMappingRepository">Question set section mapping repository to query.</param>
        /// <param name="sectionFieldMappingRepository">Section field mapping repository to query.</param>
        /// <param name="sectionRepository">Section repository to query.</param>
        /// <param name="fieldRepository">Field repository to query.</param>
        /// <param name="fieldConstraintsRepository">Field constraints repository to query.</param>
        /// <param name="parser">Parser to convert the incoming excel data.</param>
        public QuestionSetProcessor(
            IRepository<QS> questionSetRepository,
            IRepository<QSSectionMapping> questionSetSectionMappingRepository,
            IRepository<SectionFieldMapping> sectionFieldMappingRepository,
            IRepository<Section> sectionRepository,
            IRepository<Field> fieldRepository,
            IRepository<FieldConstraint> fieldConstraintsRepository,
            IParser<QuestionSetParserModel, QS> parser)
        {
            this.questionSetRepository = questionSetRepository;
            this.questionSetSectionMappingRepository = questionSetSectionMappingRepository;
            this.sectionFieldMappingRepository = sectionFieldMappingRepository;
            this.sectionRepository = sectionRepository;
            this.fieldRepository = fieldRepository;
            this.fieldConstraintsRepository = fieldConstraintsRepository;
            this.parser = parser;
        }

        /// <inheritdoc/>
        public async Task ProcessAsync(QuestionSetParserModel entity)
        {
            var parsed = parser.Parse(entity);

            if (parsed.Deleted != null)
            {
                await DeleteDraftFieldsAsync(parsed.Deleted.QSId).ConfigureAwait(false);
            }

            if (parsed.Added != null)
            {
                await questionSetRepository.AddAsync(parsed.Added).ConfigureAwait(false);
            }

            if (parsed.Updated != null)
            {
                await questionSetRepository.UpdateAsync(parsed.Updated).ConfigureAwait(false);
            }
        }

        private async Task DeleteDraftFieldsAsync(Guid questionSetId)
        {
            var sectionId = questionSetSectionMappingRepository.GetQueryable()
                .Where(qssection => qssection.QSId == questionSetId)
                .Select(qssection => qssection.SectionId)
                .ToList();

            var fieldId = sectionFieldMappingRepository.GetQueryable()
                .Where(sectionField => sectionId.Contains(sectionField.SectionId))
                .Select(sectionField => sectionField.FieldId)
                .ToList();

            var deleteQsSectionMapping = questionSetSectionMappingRepository.GetQueryable()
                .Where(qssection => sectionId.Contains(qssection.SectionId)).ToList();

            var deleteSections = sectionRepository.GetQueryable()
                .Where(s => sectionId.Contains(s.SectionId)).ToList();

            var deleteSectionFieldMapping = sectionFieldMappingRepository.GetQueryable()
                .Where(sectionField => sectionId.Contains(sectionField.SectionId)).ToList();

            var deleteFields = fieldRepository.GetQueryable()
                .Where(field => fieldId.Contains(field.FieldId)).ToList();

            await questionSetSectionMappingRepository.DeleteAllAsync(deleteQsSectionMapping).ConfigureAwait(false);
            await sectionRepository.DeleteAllAsync(deleteSections).ConfigureAwait(false);
            await sectionFieldMappingRepository.DeleteAllAsync(deleteSectionFieldMapping).ConfigureAwait(false);

            foreach (var field in fieldId)
            {
                var deleteFieldConstraints = fieldConstraintsRepository.GetQueryable()
                    .Where(fieldConst => fieldConst.FieldId == field).ToList();

                await fieldConstraintsRepository.DeleteAllAsync(deleteFieldConstraints).ConfigureAwait(false);
            }

            await fieldRepository.DeleteAllAsync(deleteFields).ConfigureAwait(false);
        }
    }
}