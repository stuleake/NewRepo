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
    /// Class to process field objects.
    /// </summary>
    public class FieldProcessor : IProcessor<FieldParserModel>
    {
        private readonly IRepository<Field> fieldRepository;
        private readonly IParser<FieldParserModel, IEnumerable<Field>> parser;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldProcessor"/> class.
        /// </summary>
        /// <param name="fieldRepository">Field repository to query.</param>
        /// <param name="parser">Field parser.</param>
        public FieldProcessor(
            IRepository<Field> fieldRepository,
            IParser<FieldParserModel, IEnumerable<Field>> parser)
        {
            this.fieldRepository = fieldRepository;
            this.parser = parser;
        }

        /// <inheritdoc/>
        public async Task ProcessAsync(FieldParserModel entity)
        {
            var parsed = parser.Parse(entity);

            if (parsed.Added != null && parsed.Added.Any())
            {
                await fieldRepository.AddRangeAsync(parsed.Added).ConfigureAwait(false);
            }

            if (parsed.Updated != null && parsed.Updated.Any())
            {
                await fieldRepository.UpdateRangeAsync(parsed.Updated).ConfigureAwait(false);
            }

            if (parsed.Deleted != null && parsed.Deleted.Any())
            {
                await fieldRepository.DeleteAllAsync(parsed.Deleted).ConfigureAwait(false);
            }
        }
    }
}