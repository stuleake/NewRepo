using Api.FormEngine.Core.Constants;
using Api.FormEngine.Core.Services.Parsers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using TQ.Core.Repository;
using TQ.Data.FormEngine.Schemas.Forms;

namespace Api.FormEngine.Core.Services.Parsers
{
    /// <inheritdoc />
    public class AnswerGuidesParser : IParser<IEnumerable<ViewModels.SheetModels.AnswerGuide>, IEnumerable<AnswerGuide>>
    {
        private readonly IReadOnlyRepository<Field> fieldRepository;
        private readonly IReadOnlyRepository<AnswerType> answerTypeRepository;

        /// <inheritdoc/>
        public AnswerGuidesParser(
            IReadOnlyRepository<Field> fieldRepository,
            IReadOnlyRepository<AnswerType> answerTypeRepository)
        {
            this.fieldRepository = fieldRepository;
            this.answerTypeRepository = answerTypeRepository;
        }

        /// <inheritdoc/>
        public ParseResult<IEnumerable<AnswerGuide>> Parse(IEnumerable<ViewModels.SheetModels.AnswerGuide> model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var guides = new List<AnswerGuide>();
            foreach (var answerGuide in model)
            {
                var fieldNumber = Convert.ToInt32(answerGuide.FieldNo);

                if (!fieldRepository.GetLocalQueryable().Any(f => f.FieldNo == fieldNumber))
                {
                    continue;
                }

                var fieldId = fieldRepository.GetLocalQueryable()
                    .Where(f => f.FieldNo == fieldNumber)
                    .Select(f => f.FieldId)
                    .FirstOrDefault();

                if (fieldId == null)
                {
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(answerGuide.RangeMin) || !string.IsNullOrWhiteSpace(answerGuide.RangeMax))
                {
                    guides.Add(ParseRangeGuide(answerGuide, fieldId));
                }

                if (!string.IsNullOrWhiteSpace(answerGuide.LengthMin) || !string.IsNullOrWhiteSpace(answerGuide.LengthMax))
                {
                    guides.Add(ParseLengthGuide(answerGuide, fieldId));
                }

                if (!string.IsNullOrWhiteSpace(answerGuide.DateMin) || !string.IsNullOrWhiteSpace(answerGuide.DateMax))
                {
                    guides.Add(ParseDateGuide(answerGuide, fieldId));
                }

                if (!string.IsNullOrWhiteSpace(answerGuide.Regex))
                {
                    guides.Add(ParseFrontEndRegexGuide(answerGuide, fieldId));
                }

                if (!string.IsNullOrWhiteSpace(answerGuide.RegexBE))
                {
                    guides.Add(ParseBackEndRegexGuide(answerGuide, fieldId));
                }

                if (!string.IsNullOrWhiteSpace(answerGuide.Multiple))
                {
                    guides.Add(ParseMultipleGuide(answerGuide, fieldId));
                }

                if (!string.IsNullOrWhiteSpace(answerGuide.Label) || !string.IsNullOrWhiteSpace(answerGuide.Value))
                {
                    guides.Add(ParseValueGuide(answerGuide, fieldId));
                }

                if (!string.IsNullOrWhiteSpace(answerGuide.Api))
                {
                    guides.Add(ParseApiGuide(answerGuide, fieldId));
                }
            }

            return new ParseResult<IEnumerable<AnswerGuide>>
            {
                Added = guides
            };
        }

        private AnswerGuide ParseRangeGuide(ViewModels.SheetModels.AnswerGuide answerGuide, Guid fieldId)
        {
            var guide = GetAnswerGuide(answerGuide, FormStructureConstants.Range, fieldId);
            guide.Min = answerGuide.RangeMin;
            guide.Max = answerGuide.RangeMax;

            return guide;
        }

        private AnswerGuide ParseLengthGuide(ViewModels.SheetModels.AnswerGuide answerGuide, Guid fieldId)
        {
            var guide = GetAnswerGuide(answerGuide, FormStructureConstants.Length, fieldId);
            guide.Min = answerGuide.LengthMin;
            guide.Max = answerGuide.LengthMax;

            return guide;
        }

        private AnswerGuide ParseDateGuide(ViewModels.SheetModels.AnswerGuide answerGuide, Guid fieldId)
        {
            var guide = GetAnswerGuide(answerGuide, FormStructureConstants.Date, fieldId);
            guide.Min = answerGuide.DateMin;
            guide.Max = answerGuide.DateMax;

            return guide;
        }

        private AnswerGuide ParseFrontEndRegexGuide(ViewModels.SheetModels.AnswerGuide answerGuide, Guid fieldId)
        {
            var guide = GetAnswerGuide(answerGuide, FormStructureConstants.Regex, fieldId);
            guide.Value = answerGuide.Regex;

            return guide;
        }

        private AnswerGuide ParseBackEndRegexGuide(ViewModels.SheetModels.AnswerGuide answerGuide, Guid fieldId)
        {
            var guide = GetAnswerGuide(answerGuide, FormStructureConstants.RegexBE, fieldId);
            guide.Value = answerGuide.RegexBE;

            return guide;
        }

        private AnswerGuide ParseMultipleGuide(ViewModels.SheetModels.AnswerGuide answerGuide, Guid fieldId)
        {
            var guide = GetAnswerGuide(answerGuide, FormStructureConstants.Multiple, fieldId);
            guide.Value = answerGuide.Multiple;

            return guide;
        }

        private AnswerGuide ParseValueGuide(ViewModels.SheetModels.AnswerGuide answerGuide, Guid fieldId)
        {
            var guide = GetAnswerGuide(answerGuide, FormStructureConstants.Value, fieldId);

            guide.Label = answerGuide.Label;
            guide.Value = answerGuide.Value;
            guide.Sequence = Convert.ToInt32(answerGuide.AnsNo);
            guide.IsDefault = answerGuide.IsDefault;

            return guide;
        }

        private AnswerGuide ParseApiGuide(ViewModels.SheetModels.AnswerGuide answerGuide, Guid fieldId)
        {
            var guide = GetAnswerGuide(answerGuide, FormStructureConstants.API, fieldId);
            guide.Value = answerGuide.Api;

            return guide;
        }

        private AnswerGuide GetAnswerGuide(ViewModels.SheetModels.AnswerGuide answerGuide, string formStructure, Guid fieldId)
        {
            var answerTypeId = answerTypeRepository.GetQueryable()
               .Where(answerType => answerType.AnswerTypes == formStructure)
               .Select(answerType => answerType.AnswerTypeId)
               .FirstOrDefault();

            return new AnswerGuide
            {
                FieldId = fieldId,
                AnswerTypeId = answerTypeId,
                ErrLabel = answerGuide.ErrorLabel,
                AnswerGuideNo = Convert.ToInt32(answerGuide.AnsNo)
            };
        }
    }
}