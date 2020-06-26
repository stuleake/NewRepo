using Api.FormEngine.Core.Services.Parsers;
using Api.FormEngine.Core.ViewModels.SheetModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TQ.Core.Repository;

namespace Api.FormEngine.Core.Services.Processors
{
    /// <summary>
    /// Class to process a collection of answer guides.
    /// </summary>
    public class AnswerGuideProcessor : IProcessor<IEnumerable<AnswerGuide>>
    {
        private readonly IRepository<TQ.Data.FormEngine.Schemas.Forms.AnswerGuide> answerGuideRepository;
        private readonly IParser<IEnumerable<AnswerGuide>, IEnumerable<TQ.Data.FormEngine.Schemas.Forms.AnswerGuide>> parser;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnswerGuideProcessor"/> class.
        /// </summary>
        /// <param name="answerGuideRepository">Answer guide data access repository.</param>
        /// <param name="parser">Answer guide parser.</param>
        public AnswerGuideProcessor(
            IRepository<TQ.Data.FormEngine.Schemas.Forms.AnswerGuide> answerGuideRepository,
            IParser<IEnumerable<AnswerGuide>, IEnumerable<TQ.Data.FormEngine.Schemas.Forms.AnswerGuide>> parser)
        {
            this.answerGuideRepository = answerGuideRepository;
            this.parser = parser;
        }

        /// <inheritdoc/>
        public async Task ProcessAsync(IEnumerable<AnswerGuide> entity)
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