using Api.FormEngine.Core.Services.Parsers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using TQ.Core.Repository;
using TQ.Data.FormEngine.Schemas.Forms;

namespace Api.FormEngine.Core.Services.Parsers
{
    /// <inheritdoc/>
    public class SectionFieldMappingParser : IParser<ViewModels.SheetModels.Section, IEnumerable<SectionFieldMapping>>
    {
        private readonly IReadOnlyRepository<Section> sectionRepository;
        private readonly IReadOnlyRepository<Field> fieldRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionFieldMappingParser"/> class.
        /// </summary>
        /// <param name="sectionRepository">Section repository to query.</param>
        /// <param name="fieldRepository">Field repository to query.</param>
        public SectionFieldMappingParser(
            IReadOnlyRepository<Section> sectionRepository,
            IReadOnlyRepository<Field> fieldRepository)
        {
            this.sectionRepository = sectionRepository;
            this.fieldRepository = fieldRepository;
        }

        /// <inheritdoc/>
        public ParseResult<IEnumerable<SectionFieldMapping>> Parse(ViewModels.SheetModels.Section model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var fieldMappings = new List<SectionFieldMapping>();
            var section = sectionRepository.GetQueryable().FirstOrDefault(section => section.SectionNo == Convert.ToInt32(model.SectionNo));
            var fields = model.Fields.Select(item => fieldRepository.GetQueryable()?.FirstOrDefault(field => field.FieldNo == Convert.ToInt32(item.FieldNo))).ToList();

            foreach (var field in fields)
            {
                fieldMappings.Add(new SectionFieldMapping
                {
                    SectionId = section.SectionId,
                    FieldId = field.FieldId,
                    FieldNo = field.FieldNo
                });
            }

            return new ParseResult<IEnumerable<SectionFieldMapping>>
            {
                Added = fieldMappings
            };
        }
    }
}