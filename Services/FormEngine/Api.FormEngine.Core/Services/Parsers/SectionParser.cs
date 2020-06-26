using Api.FormEngine.Core.Services.Parsers.Models;
using Api.FormEngine.Core.ViewModels.SheetModels;
using AutoMapper;
using System;
using System.Collections.Generic;

namespace Api.FormEngine.Core.Services.Parsers
{
    /// <inheritdoc/>
    public class SectionParser : IParser<SectionParserModel, IEnumerable<TQ.Data.FormEngine.Schemas.Forms.Section>>
    {
        private readonly IMapper mapper;

        /// <inheritdoc/>
        public SectionParser(IMapper mapper)
        {
            this.mapper = mapper;
        }

        /// <inheritdoc/>
        public ParseResult<IEnumerable<TQ.Data.FormEngine.Schemas.Forms.Section>> Parse(SectionParserModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var dateTimeNow = DateTime.UtcNow;
            var sections = new List<TQ.Data.FormEngine.Schemas.Forms.Section>();

            foreach (var section in model.QuestionSet.Sections)
            {
                var sectionDB = mapper.Map<Section, TQ.Data.FormEngine.Schemas.Forms.Section>(section);
                sectionDB.RuleCount = int.TryParse(section.SectionRuleCount, out var countValue) ? countValue : (int?)null;
                sectionDB.CreatedDate = dateTimeNow;
                sectionDB.LastModifiedDate = dateTimeNow;
                sectionDB.CreatedBy = model.UserId;
                sectionDB.LastModifiedBy = model.UserId;

                sections.Add(sectionDB);
            }

            return new ParseResult<IEnumerable<TQ.Data.FormEngine.Schemas.Forms.Section>>
            {
                Added = sections
            };
        }
    }
}