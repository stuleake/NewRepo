using Api.FormEngine.Core.Services.Parsers.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using TQ.Core.Repository;
using TQ.Data.FormEngine.Schemas.Forms;

namespace Api.FormEngine.Core.Services.Parsers
{
    /// <inheritdoc/>
    public class SectionMappingParser : IParser<SectionMappingParserModel, IEnumerable<QSSectionMapping>>
    {
        private readonly IReadOnlyRepository<QS> questionSetRepository;
        private readonly IReadOnlyRepository<Section> sectionRepository;
        private readonly IMapper mapper;

        /// <inheritdoc/>
        public SectionMappingParser(
            IReadOnlyRepository<QS> questionSetRepository,
            IReadOnlyRepository<Section> sectionRepository,
            IMapper mapper) {

            this.questionSetRepository = questionSetRepository;
            this.sectionRepository = sectionRepository;
            this.mapper = mapper;
        }

        /// <inheritdoc/>
        public ParseResult<IEnumerable<QSSectionMapping>> Parse(SectionMappingParserModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var sectionMappings = new List<QSSectionMapping>();
            var questionSet = questionSetRepository.GetQueryable().FirstOrDefault(qs => qs.QSNo == Convert.ToInt32(model.QuestionSet.QsNo));

            foreach (var section in model.QuestionSet.Sections)
            {
                var sectionDb = sectionRepository.GetQueryable().FirstOrDefault(sectionRepo => sectionRepo.SectionNo.ToString() == section.SectionNo);

                sectionMappings.Add(new QSSectionMapping
                {
                    QSId = questionSet.QSId,
                    SectionId = sectionDb.SectionId,
                });
            }

            return new ParseResult<IEnumerable<QSSectionMapping>>
            {
                Added = sectionMappings
            };
        }
    }
}