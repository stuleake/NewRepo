using Api.FormEngine.Core.Services.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TQ.Core.Repository;
using TQ.Data.FormEngine.Schemas.Forms;

namespace Api.FormEngine.Core.Services.Processors
{
    /// <summary>
    /// Class to process section field mappings.
    /// </summary>
    public class SectionFieldMappingProcessor : IProcessor<IEnumerable<ViewModels.SheetModels.Section>>
    {
        private readonly IRepository<SectionFieldMapping> sectionFieldMappingRepository;
        private readonly IParser<ViewModels.SheetModels.Section, IEnumerable<SectionFieldMapping>> parser;

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionFieldMappingProcessor"/> class.
        /// </summary>
        /// <param name="sectionFieldMappingRepository">Mapping field repository to query.</param>
        /// <param name="parser">Section field mapping parser.</param>
        public SectionFieldMappingProcessor(
            IRepository<SectionFieldMapping> sectionFieldMappingRepository,
            IParser<ViewModels.SheetModels.Section, IEnumerable<SectionFieldMapping>> parser)
        {
            this.sectionFieldMappingRepository = sectionFieldMappingRepository;
            this.parser = parser;
        }

        /// <inheritdoc/>
        public async Task ProcessAsync(IEnumerable<ViewModels.SheetModels.Section> entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            foreach (var section in entity)
            {
                var parsed = parser.Parse(section);

                if (parsed.Added != null && parsed.Added.Any())
                {
                    await sectionFieldMappingRepository.AddRangeAsync(parsed.Added).ConfigureAwait(false);
                }

                if (parsed.Updated != null && parsed.Updated.Any())
                {
                    await sectionFieldMappingRepository.UpdateRangeAsync(parsed.Updated).ConfigureAwait(false);
                }

                if (parsed.Deleted != null && parsed.Deleted.Any())
                {
                    await sectionFieldMappingRepository.DeleteAllAsync(parsed.Deleted).ConfigureAwait(false);
                }
            }
        }
    }
}