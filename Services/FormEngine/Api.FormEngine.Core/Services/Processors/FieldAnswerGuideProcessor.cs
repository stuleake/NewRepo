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
    /// Class to process a FieldParserModel into answer guides.
    /// </summary>
    public class FieldAnswerGuideProcessor : IProcessor<FieldParserModel>
    {
        private readonly IRepository<AnswerGuide> answerGuideRepository;
        private readonly IParser<FieldParserModel, IEnumerable<AnswerGuide>> parser;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldAnswerGuideProcessor"/> class.
        /// </summary>
        /// <param name="answerGuideRepository">Answer guide repository to query.</param>
        /// <param name="parser">Field parser model parser.</param>
        public FieldAnswerGuideProcessor(
            IRepository<AnswerGuide> answerGuideRepository,
            IParser<FieldParserModel, IEnumerable<AnswerGuide>> parser)
        {
            this.answerGuideRepository = answerGuideRepository;
            this.parser = parser;
        }

        /// <inheritdoc/>
        public async Task ProcessAsync(FieldParserModel entity)
        {
            var parsed = parser.Parse(entity);

            if (parsed.Added != null && parsed.Added.Any())
            {
                await answerGuideRepository.AddRangeAsync(parsed.Added).ConfigureAwait(false);
            }

            if (parsed.Updated != null && parsed.Updated.Any())
            {
                await answerGuideRepository.UpdateRangeAsync(parsed.Updated).ConfigureAwait(false);
            }

            if (parsed.Deleted != null && parsed.Deleted.Any())
            {
                await answerGuideRepository.DeleteAllAsync(parsed.Deleted).ConfigureAwait(false);
            }
        }
    }
}