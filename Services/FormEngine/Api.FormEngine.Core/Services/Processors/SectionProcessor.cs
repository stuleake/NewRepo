using Api.FormEngine.Core.Services.Parsers;
using Api.FormEngine.Core.Services.Parsers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TQ.Core.Repository;
using TQ.Data.FormEngine.Schemas.Forms;

namespace Api.FormEngine.Core.Services.Processors
{
    /// <summary>
    /// Class to process question set sections.
    /// </summary>
    public class SectionProcessor : IProcessor<SectionParserModel>
    {
        private readonly IRepository<Section> sectionRepository;
        private readonly IRepository<QSSectionMapping> questionSetSectionMappingRepository;
        private readonly IParser<SectionMappingParserModel, IEnumerable<QSSectionMapping>> questionSetSectionMappingParser;
        private readonly IParser<SectionParserModel, IEnumerable<Section>> sectionParser;

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionProcessor"/> class.
        /// </summary>
        /// <param name="questionSetSectionMappingRepository">Question set section mapping repository to query.</param>
        /// <param name="sectionRepository">Section repository to query.</param>
        /// <param name="questionSetSectionMappingParser">Parser of question set section mappings.</param>
        /// <param name="sectionParser">Parser of sections.</param>
        public SectionProcessor(
            IRepository<QSSectionMapping> questionSetSectionMappingRepository,
            IRepository<Section> sectionRepository,
            IParser<SectionMappingParserModel, IEnumerable<QSSectionMapping>> questionSetSectionMappingParser,
            IParser<SectionParserModel, IEnumerable<Section>> sectionParser)
        {
            this.sectionRepository = sectionRepository;
            this.questionSetSectionMappingRepository = questionSetSectionMappingRepository;
            this.questionSetSectionMappingParser = questionSetSectionMappingParser;
            this.sectionParser = sectionParser;
        }

        /// <inheritdoc/>
        public async Task ProcessAsync(SectionParserModel entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            foreach (var section in entity.QuestionSet.Sections)
            {
                var parsedSections = sectionParser.Parse(entity);

                // Sections
                if (parsedSections.Added != null && parsedSections.Added.Any())
                {
                    await sectionRepository.AddRangeAsync(parsedSections.Added).ConfigureAwait(false);
                }

                if (parsedSections.Updated != null && parsedSections.Updated.Any())
                {
                    await sectionRepository.UpdateRangeAsync(parsedSections.Updated).ConfigureAwait(false);
                }

                if (parsedSections.Deleted != null && parsedSections.Deleted.Any())
                {
                    await sectionRepository.DeleteAllAsync(parsedSections.Deleted).ConfigureAwait(false);
                }
            }
        }
    }
}