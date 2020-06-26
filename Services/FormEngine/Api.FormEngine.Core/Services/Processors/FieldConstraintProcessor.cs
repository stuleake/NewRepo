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
    /// Class to process field constraints.
    /// </summary>
    public class FieldConstraintProcessor : IProcessor<FieldConstraintParserModel>
    {
        private readonly IRepository<FieldConstraint> fieldConstraintRepository;
        private readonly IParser<FieldConstraintParserModel, IEnumerable<FieldConstraint>> parser;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldConstraintProcessor"/> class.
        /// </summary>
        /// <param name="fieldConstraintRepository">Field constraint repository to query.</param>
        /// <param name="parser">Field constraint parser.</param>
        public FieldConstraintProcessor(
            IRepository<FieldConstraint> fieldConstraintRepository,
            IParser<FieldConstraintParserModel, IEnumerable<FieldConstraint>> parser)
        {
            this.fieldConstraintRepository = fieldConstraintRepository;
            this.parser = parser;
        }

        /// <inheritdoc/>
        public async Task ProcessAsync(FieldConstraintParserModel entity)
        {
            var parsed = parser.Parse(entity);

            if (parsed.Added != null && parsed.Added.Any())
            {
                await fieldConstraintRepository.AddRangeAsync(parsed.Added).ConfigureAwait(false);
            }

            if (parsed.Updated != null && parsed.Updated.Any())
            {
                await fieldConstraintRepository.UpdateRangeAsync(parsed.Updated).ConfigureAwait(false);
            }

            if (parsed.Deleted != null && parsed.Deleted.Any())
            {
                await fieldConstraintRepository.DeleteAllAsync(parsed.Deleted).ConfigureAwait(false);
            }
        }
    }
}