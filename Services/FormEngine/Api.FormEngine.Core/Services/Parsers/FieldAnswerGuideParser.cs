using Api.FormEngine.Core.Constants;
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
    public class FieldAnswerGuideParser : IParser<FieldParserModel, IEnumerable<AnswerGuide>>
    {
        private readonly IRepository<AnswerType> answerTypeRepository;
        private readonly IMapper mapper;

        /// <inheritdoc/>
        public FieldAnswerGuideParser(IRepository<AnswerType> answerTypeRepository, IMapper mapper)
        {
            this.answerTypeRepository = answerTypeRepository;
            this.mapper = mapper;
        }

        /// <inheritdoc/>
        public ParseResult<IEnumerable<AnswerGuide>> Parse(FieldParserModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var answerGuides = new List<AnswerGuide>();

            foreach (var field in model.Fields)
            {
                var fieldDb = mapper.Map<ViewModels.SheetModels.Field, Field>(field);

                if (!string.IsNullOrEmpty(field.CopyfromQS) && !string.IsNullOrEmpty(field.CopyfromField))
                {
                    var dependencySheetData = model.Dependencies.FirstOrDefault(d => d.FieldNo == field.FieldNo && d.DependsOnAnsfromQS == field.CopyfromQS);
                    var answerTypeId = answerTypeRepository.GetQueryable()
                        .Where(a => a.AnswerTypes == FormStructureConstants.CopyFrom)
                        .Select(a => a.AnswerTypeId)
                        .FirstOrDefault();

                    var answerGuideDB = new AnswerGuide
                    {
                        FieldId = fieldDb.FieldId,
                        AnswerGuideNo = Convert.ToInt32(dependencySheetData.DependsOnAns),
                        AnswerTypeId = answerTypeId,
                        CopyFromFieldNo = Convert.ToInt32(field.CopyfromField),
                        CopyFromQSNo = Convert.ToInt32(field.CopyfromQS)
                    };

                    answerGuides.Add(answerGuideDB);
                }
            }

            return new ParseResult<IEnumerable<AnswerGuide>>
            {
                Added = answerGuides
            };
        }
    }
}