using Api.FormEngine.Core.Services.Parsers;
using Api.FormEngine.Core.Services.Parsers.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TQ.Core.Repository;
using TQ.Data.FormEngine.Schemas.Forms;

namespace Api.FormEngine.Core.Services.Processors
{
    /// <summary>
    /// Class to process taxonomies.
    /// </summary>
    public class TaxonomyProcessor : IProcessor<TaxonomyParserModel>
    {
        private readonly IParser<TaxonomyParserModel, IEnumerable<Taxonomy>> taxonomyParser;
        private readonly IRepository<Taxonomy> taxonomyRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaxonomyProcessor"/> class.
        /// </summary>
        /// <param name="taxonomyParser">Parser of taxonomies.</param>
        /// <param name="taxonomyRepository">Taxonomy repository to query.</param>
        public TaxonomyProcessor(
            IParser<TaxonomyParserModel, IEnumerable<Taxonomy>> taxonomyParser,
            IRepository<Taxonomy> taxonomyRepository)
        {
            this.taxonomyParser = taxonomyParser;
            this.taxonomyRepository = taxonomyRepository;
        }

        /// <inheritdoc/>
        public async Task ProcessAsync(TaxonomyParserModel entity)
        {
            var parsed = taxonomyParser.Parse(entity);

            if (parsed.Added != null && parsed.Added.Any())
            {
                await taxonomyRepository.AddRangeAsync(parsed.Added).ConfigureAwait(false);
            }

            if (parsed.Updated != null && parsed.Updated.Any())
            {
                await taxonomyRepository.UpdateRangeAsync(parsed.Updated).ConfigureAwait(false);
            }

            if (parsed.Deleted != null && parsed.Deleted.Any())
            {
                await taxonomyRepository.DeleteAllAsync(parsed.Deleted).ConfigureAwait(false);
            }
        }
    }
}