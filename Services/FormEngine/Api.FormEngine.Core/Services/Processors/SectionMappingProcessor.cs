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
    /// Class to process question set section mappings.
    /// </summary>
    public class SectionMappingProcessor : IProcessor<SectionMappingParserModel>
    {
        private readonly IRepository<QSSectionMapping> questionSetSectionMappingRepository;
        private readonly IParser<SectionMappingParserModel, IEnumerable<QSSectionMapping>> questionSetSectionMappingParser;

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionMappingProcessor"/> class.
        /// </summary>
        /// <param name="questionSetSectionMappingRepository">Question set section mapping repository to query.</param>
        /// <param name="sectionRepository">Section repository to query.</param>
        /// <param name="questionSetSectionMappingParser">Parser of question set section mappings.</param>
        public SectionMappingProcessor(
            IRepository<QSSectionMapping> questionSetSectionMappingRepository,
            IRepository<Section> sectionRepository,
            IParser<SectionMappingParserModel, IEnumerable<QSSectionMapping>> questionSetSectionMappingParser)
        {
            this.questionSetSectionMappingRepository = questionSetSectionMappingRepository;
            this.questionSetSectionMappingParser = questionSetSectionMappingParser;
        }

        /// <inheritdoc/>
        public async Task ProcessAsync(SectionMappingParserModel entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            // Section Mappings
            var parsedSectionMappings = questionSetSectionMappingParser.Parse(entity);

            if (parsedSectionMappings.Added != null && parsedSectionMappings.Added.Any())
            {
                await questionSetSectionMappingRepository.AddRangeAsync(parsedSectionMappings.Added).ConfigureAwait(false);
            }

            if (parsedSectionMappings.Updated != null && parsedSectionMappings.Updated.Any())
            {
                await questionSetSectionMappingRepository.UpdateRangeAsync(parsedSectionMappings.Updated).ConfigureAwait(false);
            }

            if (parsedSectionMappings.Deleted != null && parsedSectionMappings.Deleted.Any())
            {
                await questionSetSectionMappingRepository.DeleteAllAsync(parsedSectionMappings.Deleted).ConfigureAwait(false);
            }
        }
    }
}