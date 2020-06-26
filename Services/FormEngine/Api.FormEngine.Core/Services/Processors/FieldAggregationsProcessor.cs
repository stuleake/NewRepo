using Api.FormEngine.Core.Services.Parsers;
using Api.FormEngine.Core.ViewModels.SheetModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TQ.Core.Repository;
using TQ.Data.FormEngine.Schemas.Forms;

namespace Api.FormEngine.Core.Services.Processors
{
    /// <summary>
    /// Class to process a collection of aggregations.
    /// </summary>
    public class FieldAggregationsProcessor : IProcessor<IEnumerable<Aggregations>>
    {
        private readonly IRepository<FieldAggregation> fieldAggregationRepository;
        private readonly IParser<IEnumerable<Aggregations>, IEnumerable<FieldAggregation>> parser;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldAggregationsProcessor"/> class.
        /// </summary>
        /// <param name="fieldAggregationRepository">Field aggregation repository to query.</param>
        /// <param name="parser">Aggregation parser.</param>
        public FieldAggregationsProcessor(
            IRepository<FieldAggregation> fieldAggregationRepository,
            IParser<IEnumerable<Aggregations>, IEnumerable<FieldAggregation>> parser)
        {
            this.fieldAggregationRepository = fieldAggregationRepository;
            this.parser = parser;
        }

        /// <inheritdoc/>
        public async Task ProcessAsync(IEnumerable<Aggregations> entity)
        {
            var parsed = parser.Parse(entity);

            if (parsed.Added != null && parsed.Added.Any())
            {
                await fieldAggregationRepository.AddRangeAsync(parsed.Added).ConfigureAwait(false);
            }

            if (parsed.Updated != null && parsed.Updated.Any())
            {
                await fieldAggregationRepository.UpdateRangeAsync(parsed.Updated).ConfigureAwait(false);
            }

            if (parsed.Deleted != null && parsed.Deleted.Any())
            {
                await fieldAggregationRepository.DeleteAllAsync(parsed.Deleted).ConfigureAwait(false);
            }
        }
    }
}